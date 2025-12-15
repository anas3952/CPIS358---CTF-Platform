using Microsoft.EntityFrameworkCore;
using CTF_Platform_V2.Models; // تأكد أن هذا يطابق اسم مشروعك

namespace CTF_Platform_V2.Data
{
    // لاحظ: ورثنا من DbContext بدلاً من IdentityDbContext للتبسيط والسرعة
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // هذه هي الجداول التي ستحمل بيانات مشروعك
        public DbSet<User> Users { get; set; }
        public DbSet<Challenge> Challenges { get; set; }
        public DbSet<Submission> Submissions { get; set; }
    }
}