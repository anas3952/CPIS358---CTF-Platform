using Microsoft.AspNetCore.Mvc;
using CTF_Platform_V2.Data;
using CTF_Platform_V2.Models;
using System.Linq;

namespace CTF_Platform_V2.Controllers
{
    public class ChallengesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChallengesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");

            var challenges = _context.Challenges.ToList();

            var solvedChallengeIds = _context.Submissions
                .Where(s => s.UserId == userId)
                .Select(s => s.ChallengeId)
                .ToList();

            ViewBag.SolvedIds = solvedChallengeIds; 

            return View(challenges);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Challenge challenge)
        {
            if (ModelState.IsValid)
            {
                _context.Challenges.Add(challenge); 
                _context.SaveChanges(); 
                return RedirectToAction(nameof(Index)); 
            }
            return View(challenge);
        }
        public IActionResult Solve(int id)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var challenge = _context.Challenges.Find(id);
            if (challenge == null) return NotFound();

            return View(challenge);
        }

        [HttpPost]
        public IActionResult SubmitFlag(int id, string flagAttempt)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");

            var challenge = _context.Challenges.Find(id);

            if (challenge != null && challenge.Flag == flagAttempt)
            {
                bool alreadySolved = _context.Submissions.Any(s => s.UserId == userId && s.ChallengeId == id);

                if (!alreadySolved)
                {
                    var user = _context.Users.Find(userId);
                    user.Score += challenge.Points;

                    _context.Submissions.Add(new Submission { UserId = (int)userId, ChallengeId = id, SubmissionTime = DateTime.Now });

                    _context.SaveChanges();
                }

                TempData["Message"] = "Correct Flag! Points Added.";
                TempData["Type"] = "success"; 
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Message"] = "Wrong Flag! Try Again.";
                TempData["Type"] = "danger"; 
                return RedirectToAction("Solve", new { id = id });
            }

        }

        public IActionResult Edit(int id)
        {
            var challenge = _context.Challenges.Find(id);
            if (challenge == null) return NotFound();
            return View(challenge);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Challenge challenge)
        {
            if (id != challenge.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(challenge);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(challenge);
        }

        public IActionResult Delete(int id)
        {
            var challenge = _context.Challenges.Find(id);
            if (challenge == null) return NotFound();
            return View(challenge);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var challenge = _context.Challenges.Find(id);
            if (challenge != null)
            {
                _context.Challenges.Remove(challenge);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}