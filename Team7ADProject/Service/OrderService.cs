using Newtonsoft.Json;
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
        private StationeryService stationeryService;
        public OrderService()
        {
            this.db = new Team7ADProjectDbContext();
            this.stationeryService = new StationeryService();
        }
        public List<Order> GetOrders()
        {
            return db.Order.ToList();
        }
        public Order GetOrderById(int orderId)
        {
            return db.Order.Where(x => x.OrderId == orderId).FirstOrDefault();
        }
        public List<StationerySupplierPrice> GetSupplierPrices()
        {
            return db.StationerySupplierPrice.ToList();
        }
        public void AddOrders(string allOrdersJSON)
        {
            System.Diagnostics.Debug.WriteLine(allOrdersJSON);
            Dictionary<int, List<StationeryQuantity>> ordersBySupplier = new Dictionary<int, List<StationeryQuantity>>();
            dynamic allOrders = JsonConvert.DeserializeObject(allOrdersJSON);
            foreach(var o in allOrders)
            {
                int stationeryId = o.stationeryId;
                int quantityOrdered = o.quantity;
                int supplierId = o.supplierId;
                Stationery stationery = db.Stationery.Where(x => x.StationeryId == stationeryId).FirstOrDefault();
                StationeryQuantity stationeryQuantity = new StationeryQuantity(stationery);
                stationeryQuantity.QuantityOrdered = quantityOrdered;
                if (ordersBySupplier.ContainsKey(supplierId))
                {
                    ordersBySupplier[supplierId].Add(stationeryQuantity);
                } else
                {
                    ordersBySupplier[supplierId] = new List<StationeryQuantity>();
                    ordersBySupplier[supplierId].Add(stationeryQuantity);
                }
            }
            foreach(var o in ordersBySupplier)
            {
                Supplier supplier = db.Supplier.Where(x => x.SupplierId == o.Key).FirstOrDefault();
                Order order = new Order(supplier, DateTime.Now.ToString("dd-MMM-yy"));
                order.StationeryQuantities = o.Value;
                db.Order.Add(order);
                db.SaveChanges();
            }
        }
    }
}