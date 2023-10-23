using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DavistaArchitect.Models
{
    public class House_Image
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter image name.")]
        public string ImageName { get; set; }

        [Required(ErrorMessage = "Please enter Description")]
        [StringLength(500)]
        public string Description { get; set; }

        [Display(Name = "House")]
        public int HouseId { get; set; }
        public House House { get; set; }
    }
}
