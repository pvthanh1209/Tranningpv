using Microsoft.EntityFrameworkCore;

namespace Tranning.DataDBContext
{
    public class TranningDBContext : DbContext
    {
        public TranningDBContext(DbContextOptions<TranningDBContext> options) : base(options)
        {
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Trainee_course>trainee_Courses { get; set; }
        public DbSet<Trainner_topic>trainner_Topics { get; set; }
        public DbSet<Users>Users { get; set; }

    }
}
