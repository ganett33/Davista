using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DavistaArchitect.Models
{
    public class ApplicationUser: IdentityUser
    {

        [Display(Name = "About Me")]
        public string? AboutMe { get; set; }


        [Display(Name = "Job Title")]
        public string? JobTitle { get; set; }

        public string? Role { get; set; }

        public int UsernameChangeLimit { get; set; } = 10;
        public byte[]? ProfilePicture { get; set; }
    }
}
