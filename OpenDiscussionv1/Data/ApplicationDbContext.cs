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
        public virtual DbSet<Discussion> Discussions { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Reply> Replies { get; set; }
    }
}