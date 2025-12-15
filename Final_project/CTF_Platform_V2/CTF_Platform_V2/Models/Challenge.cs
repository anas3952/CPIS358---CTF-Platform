using System.ComponentModel.DataAnnotations;

namespace CTF_Platform_V2.Models
{
    public class Challenge
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title required")]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public int Points { get; set; }

        [Required]
        public string Flag { get; set; }
    }
}