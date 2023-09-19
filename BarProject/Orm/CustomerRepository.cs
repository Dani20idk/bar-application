using BarProject.App_Code;
using BarProject.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace BarProject.Orm
{
    public class CustomerRepository
    {
       
        private BarEntities objbarEntities;
        public CustomerRepository()
        {
            objbarEntities = new BarEntities();

        }
        public IEnumerable<SelectListItem> GetAllCustomers()
        {
            var objselectListItems = new List<SelectListItem>();
            objselectListItems = (from obj in objbarEntities.Customers
                                  select new SelectListItem()
                                  {
                                      Text = obj.FirstName,
                                      Value = obj.Customer_id.ToString(),
                                      Selected = true
                                  }).ToList();
            return objselectListItems;
        }

    }

}