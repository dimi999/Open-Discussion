using Microsoft.AspNetCore.Identity;

namespace OpenDiscussionv1.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string ?Description { get; set; }
        public string ?Title { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual ICollection<IdentityRole> ApplicationUserRoles { get; set; }
    }
}
