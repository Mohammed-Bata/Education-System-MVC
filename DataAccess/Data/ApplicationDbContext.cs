using Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Data
{
    public class ApplicationDbContext:IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<ApplicationUser> applicationUsers { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Learner_Course> Learners_Courses { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Learner_Course>().HasKey(lc => new {lc.LearnerId, lc.CourseId});

            modelBuilder.Entity<Course>()
            .HasOne(lc => lc.category)
            .WithMany(c => c.Courses)
            .HasForeignKey(lc => lc.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Category>().HasData(new Category
            {
                Id = 1,Name="CPP"
            },new Category
            {
                Id=2,Name="Csharp"
            },new Category
            {
                Id=3,Name="DesignPattern"
            });

        }
    }
}
