using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BarProject.Models
{
    public class Orders
    {
        public int Order_id { get; set; }
        public int Customer_id { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime OrderDate { get; set; }
        public int TableNumber { get; set; }
        public float  TotalAmount { get; set; }
        public string Status { get; set; }
        public IEnumerable<OrderItems> ListorderItems { get; set; }


    }
}