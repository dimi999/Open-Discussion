using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace OpenDiscussionv1.Models
{
    public class Reply
    {
        [Key]
        public int ReplyId { get; set; }

        [Required(ErrorMessage = "Reply content is mandatory!")]
        public String Content { get; set; }

        public DateTime CreatedAt { get; set; }

        [ForeignKey("Discussions")]
        public int DiscussionId { get; set; }
        public virtual Discussion Discussion { get; set; }
    }
}
