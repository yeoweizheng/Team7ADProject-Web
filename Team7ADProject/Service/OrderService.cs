using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Team7ADProject.Database;
using Team7ADProject.Models;

namespace Team7ADProject.Service
{
    public class OrderService
    {

            private Team7ADProjectDbContext db;
            public OrderService()
            {
                this.db = new Team7ADProjectDbContext();
            }
            public List<Order> GetOrder()
            {
                return db.Order.ToList();
            }
    }
}