﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DavistaArchitect.Models
{
    public class HouseImage
    {
        public int Id { get; set; }
        public string ImageUrl  { get; set; }
        public House House { get; set; }
    }
}