using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenDiscussionv1.Models
{
    public class Discussion
    {
        [Key]
        public int DiscussionId { get; set; }
        [ForeignKey("Categories")]
        public int CategoryId { get; set; }

        [ForeignKey("AspNetUsers")]
        public String AuthorId { get; set; } 
        public String Title { get; set; }

        public String Content { get; set; }

        public DateTime CreatedAt { get; set; }

        public virtual Category Category { get; set; }
        public IList<Reply> Replies { get; set; }
    }
}
