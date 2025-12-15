using Microsoft.AspNetCore.Mvc;
using CTF_Platform_V2.Data;
using CTF_Platform_V2.Models;
using Microsoft.AspNetCore.Http; 
using System.Linq;

namespace CTF_Platform_V2.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                if (_context.Users.Any(u => u.Email == user.Email))
                {
                    ModelState.AddModelError("Email", "This email is already taken.");
                    return View(user);
                }

                user.Role = "Contestant"; 
                user.Score = 0; 

                _context.Users.Add(user);
                _context.SaveChanges();

                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("Username", user.Username);

                return RedirectToAction("Index", "Home");
            }
            return View(user);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);

            if (user != null)
            {
                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetString("Role", user.Role);

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Invalid Email or Password";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); 
            return RedirectToAction("Login");
        }

        public IActionResult Profile()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login");

            var user = _context.Users
                .Where(u => u.Id == userId)
                .Select(u => new
                {
                    u.Username,
                    u.Email,
                    u.Score,
                    u.Role,

                    SolvedChallenges = _context.Submissions
                        .Where(s => s.UserId == u.Id)
                        .Select(s => new { s.Challenge.Title, s.Challenge.Points, s.SubmissionTime })
                        .OrderByDescending(s => s.SubmissionTime)
                        .ToList()
                })
                .FirstOrDefault();

            if (user == null) return RedirectToAction("Login");

            return View(user);
        }
    }
}