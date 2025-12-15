using System.ComponentModel.DataAnnotations;

namespace CTF_Platform_V2.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Username required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Email required")]
        [EmailAddress]
        public string Email { get; set; }

        public string Role { get; set; } = "Contestant";

        public int Score { get; set; } = 0;
    }
}