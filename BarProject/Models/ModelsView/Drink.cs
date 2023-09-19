using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BarProject.ModelsView
{
    public class Drink
    {
        public int Drink_id { get; set; }
        public string Name { get; set; }
        public string  Description { get; set; }
        public float Price { get; set; }
        public string  Category { get; set; }
        public string IsAvailable   { get; set; }
    }
}