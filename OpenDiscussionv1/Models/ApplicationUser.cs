using Microsoft.AspNetCore.Identity;

namespace OpenDiscussionv1.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<IdentityRole> ApplicationUserRoles { get; set; }
    }
}
