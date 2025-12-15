using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CTF_Platform_V2.Models
{
    public class Submission
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }

        [ForeignKey("Challenge")]
        public int ChallengeId { get; set; }
        public Challenge Challenge { get; set; }

        public DateTime SubmissionTime { get; set; } = DateTime.Now;
    }
}