using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenDiscussionv1.Models
{
    public class Discussion
    {
        [Key]
        public int DiscussionId { get; set; }
        public string? UserId { get; set; }

        [Required(ErrorMessage = "Discussion title is mandatory!")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Discussion content is mandatory!")]
        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? CategoryId { get; set; }
        public virtual Category? Category { get; set; }
        public virtual ApplicationUser? User { get; set; }
        public ICollection<Reply> Replies { get; set; }
    }
}
