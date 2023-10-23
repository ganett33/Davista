using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DavistaArchitect.Models.ViewModels
{
    public class ProjectHouseVM
    {
        public int ProjectTypeId { get; set; }
        public string ProjectTypeName { get; set; }
        public string ProjectTypeUrl { get; set; }
        public string ProjectTypeImage { get; set; }

        public IEnumerable<House> Houses { get; set; }

    }
}
