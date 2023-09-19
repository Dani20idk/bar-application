using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BarProject.Models
{
    public class Users
    {
        public int User_id { get; set; }
        public string UserName { get; set; }
        public int Password { get; set; }
        public string Role { get; set; }
    }
}