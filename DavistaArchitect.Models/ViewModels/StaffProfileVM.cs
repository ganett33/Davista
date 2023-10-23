using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DavistaArchitect.Models.ViewModels
{
    public class StaffProfileVM
    {

        public StaffProfile StaffProfile { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> CategoryList { get; set; }


    }
}
