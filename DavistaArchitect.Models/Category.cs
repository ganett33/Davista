using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace DavistaArchitect.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Category Name value must be over 3 characters. ")]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }

        [Display(Name = "Display Order")]
        [Range(1, 100, ErrorMessage = "Display order must be in range of 1-100!!!")]
        public int DisplayOrder { get; set; }

        public DateTime CreatedDateTime { get; set; } = DateTime.Now;

    }
}
