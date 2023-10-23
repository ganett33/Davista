using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace DavistaArchitect.Models
{
    public class ProjectType
    {
        private string _projectTypeName="";

        [Key]
        public int Id { get; set; }

        [StringLength(100, MinimumLength = 3)]
        [Required(ErrorMessage = "Please enter name.")]
        [Display(Name = "Name")]
        public string ProjectTypeName
        {
            get { return _projectTypeName; }
            set
            {
                _projectTypeName = value;
                // Automatically generate ProjectTypeUrl based on ProjectTypeName
                ProjectTypeUrl = value?.ToLower().Replace(" ", "-");
            }
        }

        [ValidateNever]
        public string ProjectTypeUrl { get; private set; }


        [Required(ErrorMessage = "Please enter description.")]
        [Display(Name = "Description")]
        public string ProjectTypeDescription { get; set; }


        [ValidateNever]
        [Display(Name = "Image")]
        public string ProjectTypeImage { get; set; }

        public DateTime CreatedDateTime { get; set; } = DateTime.Now;

    }
}
