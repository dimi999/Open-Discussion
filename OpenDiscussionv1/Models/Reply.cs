using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace OpenDiscussionv1.Models
{
    public class Reply
    {
        [Key]
        public int ReplyId { get; set; }
        public String ?UserId { get; set; }

        [Required(ErrorMessage = "Reply content is mandatory!")]
        [MinLength(3, ErrorMessage = "Reply content has to be at least 3 characters long!")]
        public String Content { get; set; }

        public DateTime CreatedAt { get; set; }

        public int ?DiscussionId { get; set; }
        public virtual Discussion? Discussion { get; set; }
        public virtual ApplicationUser? User { get; set; }
    }
}
