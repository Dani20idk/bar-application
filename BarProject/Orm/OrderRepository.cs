using BarProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BarProject.Orm
{
    public class OrderRepository
    {
        private BarEntities objbarEntities;
        public OrderRepository()
        {
            objbarEntities = new BarEntities();
        }
        public bool AddOrder(Orders ObjorderModel)
        {
            try
            {
                Order objOrder = new Order();
                {
                    objOrder.Customer_id = ObjorderModel.Customer_id;
                    objOrder.TotalAmount = ObjorderModel.TotalAmount;
                    objOrder.OrderDate = DateTime.Now;
                    objOrder.TableNumber = ObjorderModel.TableNumber;
                };
                objbarEntities.Orders.Add(objOrder);
                objbarEntities.SaveChanges();
                int Order_id = objOrder.Order_id;

                foreach (var drink in ObjorderModel.ListorderItems)
                {
                    OrderItem objorderItem = new OrderItem();
                    {
                        objorderItem.Order_id = Order_id;
                        objorderItem.Customer_id = drink.Customer_id;
                        objorderItem.Quantity = drink.Quantity;
                        objorderItem.Drink_id = drink.Drink_id;
                        objorderItem.Subtotal = drink.Subtotal;
                        objorderItem.UnitPrice= drink.UnitPrice;

                    };
                    objbarEntities.OrderItems.Add(objorderItem);
                    objbarEntities.SaveChanges();
                    int OrderItem_id = objorderItem.OrderItem_id;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}