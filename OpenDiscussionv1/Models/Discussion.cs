using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenDiscussionv1.Models
{
    public class Discussion
    {
        [Key]
        public int DiscussionId { get; set; }

        [Required(ErrorMessage = "Discussion title is mandatory!")]
        public String Title { get; set; }

        [Required(ErrorMessage = "Discussion content is mandatory!")]
        public String Content { get; set; }

        public DateTime CreatedAt { get; set; }

        [ForeignKey("Categories")]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public ICollection<Reply> Replies { get; set; }
    }
}
