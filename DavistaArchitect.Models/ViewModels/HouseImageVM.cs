using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DavistaArchitect.Models.ViewModels
{
    public class HouseImageVM
    {
        public List<IFormFile> Image { get; set; }
        public House House { get; set; }
    }
}
