using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OpenDiscussionv1.Models
{
    public class Reply
    {
        [Key]
        public int ReplyId { get; set; }
        [ForeignKey("Discussions")]
        public int DiscussionsId { get; set; }

        [ForeignKey("AspNetUsers")]
        public String AuthorId { get; set; }

        public String Content { get; set; }

        public DateTime CreatedAt { get; set; }

        public virtual Discussion Discussion { get; set; }
    }
}
