using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DavistaArchitect.Models
{
    public class UserLevel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        public int? Level { get; set; }
        public string? AuthorityLevel { get; set; }


    }
}
