namespace WebServer.ByTheCakeApplication.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using WebServer.ByTheCakeApplication.Data;
    using WebServer.ByTheCakeApplication.Data.Models;

    public class ShoppingService : IShoppingService
    {
        public void CreateOrder(int userId, IEnumerable<int> productsIds)
        {
            using (var db = new ByTheCakeDbContext())
            {
                var order = new Order
                {
                    Userid = userId,
                    CreationDate = DateTime.UtcNow,
                    OrdersProducts = productsIds
                        .Select(id => new OrderProduct
                        {
                            ProductId = id
                        })
                        .ToList()
                };

                db.Add(order);
                db.SaveChanges();
            }
        }
    }
}
