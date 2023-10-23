using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DavistaArchitect.Models
{

    public class House
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter name.")]
        [StringLength(50)]
        public string HouseName { get; set; }

        [Required(ErrorMessage = "Please enter description")]
        public string HouseDescription { get; set; }

        [ValidateNever]
        public string HouseImage { get; set; }

        [Required]
        [Display(Name = "Project Type")]
        public int ProjectTypeId { get; set; }

        [ForeignKey("ProjectTypeId")]
        [ValidateNever]
        public ProjectType? ProjectType { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category Category { get; set; }

    }
}
