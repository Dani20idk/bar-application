
using BarProject.App_Code;
using BarProject.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BarProject.Orm
{
    public class DrinkRepository
    {
        
        private BarEntities objbarEntities;
        public DrinkRepository()
        {
            objbarEntities = new BarEntities();
        }
        public IEnumerable<SelectListItem>GetAllDrinks()
        {
         var  objselectListItems = new List<SelectListItem>();
            objselectListItems = (from obj in objbarEntities.Drinks
                                  select new SelectListItem()
                                  {
                                      Text = obj.Name,
                                      Value = obj.Drink_id.ToString(),
                                      Selected = false
                                  }).ToList();
            return objselectListItems;
        }
      

    }
}