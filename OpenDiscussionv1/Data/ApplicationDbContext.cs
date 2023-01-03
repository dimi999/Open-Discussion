using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OpenDiscussionv1.Models;

namespace OpenDiscussionv1.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Reply>()
                .HasOne(r => r.Discussion)
                .WithMany(d => d.Replies)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Discussion>()
                 .HasOne(d => d.Category)
                 .WithMany(c => c.Discussions)
                 .OnDelete(DeleteBehavior.Cascade);
        }

        public virtual DbSet<Discussion> Discussions { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Reply> Replies { get; set; }
    }
}