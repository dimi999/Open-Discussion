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
        [MinLength(3, ErrorMessage = "Discussion title has to be at least 3 characters long!")]
        [MaxLength(25, ErrorMessage = "Discussion title has to be at most 25 characters long!")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Discussion content is mandatory!")]
        [MinLength(10, ErrorMessage = "Discussion content has to be at least 10 characters long!")]
        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? CategoryId { get; set; }
        public virtual Category? Category { get; set; }
        public virtual ApplicationUser? User { get; set; }
        public virtual ICollection<Reply>? Replies { get; set; }
    }
}
