using BarProject.Models;
using BarProject.Orm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace BarProject.Controllers
{
    public class HomeController : Controller
    {
        private  BarEntities objbarEntities;
        public HomeController() {
            objbarEntities = new BarEntities();
        }
        //GET : Home
        public ActionResult Index()
        {
            CustomerRepository objcustomerRepository = new CustomerRepository();
            DrinkRepository objdrinkRepositoy = new DrinkRepository();
            //  Tuple<IEnumerable<SelectListItem>, IEnumerable<SelectListItem>>
            var objCustomerRepository = new CustomerRepository();
            var objDrinkRepository = new DrinkRepository();
           var  objMultipleModels = new Tuple<IEnumerable<SelectListItem>, IEnumerable<SelectListItem>>
                (objcustomerRepository.GetAllCustomers(),objdrinkRepositoy.GetAllDrinks());
                return View(objMultipleModels);
        }

        [HttpGet]
       public JsonResult getItemUnitPrice(int drink_id)
        {
            decimal UnitPrice = (decimal)objbarEntities.Drinks.Single(model => model.Drink_id == drink_id).Price;
            return Json(UnitPrice, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult getCategory(int drink_id)
        {
            string  Category = objbarEntities.Drinks.Single(model => model.Drink_id == drink_id).Category;
            return Json(Category, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public JsonResult getDescription(int drink_id)
        {
            string Description = objbarEntities.Drinks.Single(model => model.Drink_id == drink_id).Description;
            return Json(Description, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult Index(Orders ObjorderModel)
        {
            OrderRepository objOrderRepository = new OrderRepository();
            objOrderRepository.AddOrder(ObjorderModel);

            return Json("Order Succesfully  created", JsonRequestBehavior.AllowGet);
        }

    }

    
}