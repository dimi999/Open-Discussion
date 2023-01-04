using System.ComponentModel.DataAnnotations;

namespace OpenDiscussionv1.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Category name is mandatory!")]
        [MinLength(3, ErrorMessage = "Category name has to be at least 3 characters long!")]
        [MaxLength(15, ErrorMessage = "Category name has to be at most 15 characters long!")]
        public string CategoryName { get; set; }

        public ICollection<Discussion>? Discussions { get; set; }
    }
}
