using System.ComponentModel.DataAnnotations;

namespace OpenDiscussionv1.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Category name is mandatory!")]
        public string CategoryName { get; set; }

        public ICollection<Discussion> Discussions { get; set; }
    }
}
