using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BarProject.Models
{
    public class OrderItems
    {
        public int OrderItem_id { get; set; }
        public int Customer_id { get; set; }
        public int Order_id { get; set; }
        public int Drink_id { get; set; }
        public int Quantity { get; set; }
        public float Subtotal { get; set; }
        public float UnitPrice{ get; set; }
    }
}