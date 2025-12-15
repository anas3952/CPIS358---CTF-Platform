using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient; 
using System.Collections.Generic;
using CTF_Platform_V2.Models;

namespace CTF_Platform_V2.Controllers
{
    public class ChallengesController : Controller
    {
        private readonly IConfiguration _configuration;

        public ChallengesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        
        public IActionResult Index(string searchString)
        {
            
            if (User.Identity.IsAuthenticated)
            {
                HttpContext.Session.SetString("User", User.Identity.Name);
                Response.Cookies.Append("UserCookie", User.Identity.Name, new CookieOptions { Expires = DateTime.Now.AddDays(1) });
            }
            

            List<Challenge> challenges = new List<Challenge>();
            string connStr = _configuration.GetConnectionString("DefaultConnection");

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    
                    string sql = "SELECT * FROM Challenges";

                    
                    if (!string.IsNullOrEmpty(searchString))
                    {
                        
                        sql += " WHERE Title LIKE @search";
                    }

                    SqlCommand cmd = new SqlCommand(sql, conn);

                    
                    if (!string.IsNullOrEmpty(searchString))
                    {
                        cmd.Parameters.AddWithValue("@search", "%" + searchString + "%");
                    }

                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        challenges.Add(new Challenge
                        {
                            Id = (int)rdr["Id"],
                            Title = rdr["Title"].ToString(),
                            Points = (int)rdr["Points"]
                        });
                    }
                }
            }
            catch
            {

            }

            
            return View(challenges);
        }


        [HttpPost]
        public IActionResult Create(string title, int points)
        {
            string connStr = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                
                string sql = "INSERT INTO Challenges (Title, Points, Owner) VALUES (@t, @p, @o)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@t", title);
                cmd.Parameters.AddWithValue("@p", points);
                cmd.Parameters.AddWithValue("@o", User.Identity.Name); 
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            string connStr = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM Challenges WHERE Id=@id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }
    }
}