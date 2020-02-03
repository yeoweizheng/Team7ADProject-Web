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
        private StationeryService stationeryService;
        private Team7ADProjectDbContext db;
        public OrderService()
        {
            this.stationeryService = new StationeryService();
        }
        public List<Order> GetOrders()
        {
            db = new Team7ADProjectDbContext();
            return db.Order.ToList();
        }
        public Order GetOrderById(int orderId)
        {
            db = new Team7ADProjectDbContext();
            return db.Order.Where(x => x.OrderId == orderId).FirstOrDefault();
        }
        public List<StationerySupplierPrice> GetSupplierPrices()
        {
            db = new Team7ADProjectDbContext();
            return db.StationerySupplierPrice.ToList();
        }
        public double GetSupplierPrice(int supplierId, int stationeryId)
        {
            db = new Team7ADProjectDbContext();
            List<StationerySupplierPrice> allPrices = db.StationerySupplierPrice.ToList();
            foreach(var supplierPrice in allPrices)
            {
                if(supplierPrice.Stationery.StationeryId == stationeryId && supplierPrice.Supplier.SupplierId == supplierId)
                {
                    return supplierPrice.Price;
                }
            }
            return -1;
        }
        public void AddOrders(string allOrdersJSON)
        {
            db = new Team7ADProjectDbContext();
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
                stationeryQuantity.Price = GetSupplierPrice(stationeryId, stationeryId);
                stationeryQuantity.Subtotal = (double)quantityOrdered * GetSupplierPrice(stationeryId, stationeryId);
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
        public void UpdateOrder(int orderId, String quantitiesReceivedJSON)
        {
            db = new Team7ADProjectDbContext();
            Order order = db.Order.Where(x => x.OrderId == orderId).FirstOrDefault();
            dynamic quantitiesReceived = JsonConvert.DeserializeObject<List<int>>(quantitiesReceivedJSON);
            bool allReceived = true;
            bool partialReceived = false;
            for (int i = 0; i < quantitiesReceived.Count; i++)
            {
                StationeryQuantity stationeryQuantity = ((List<StationeryQuantity>)order.StationeryQuantities)[i];
                int diff = quantitiesReceived[i] - stationeryQuantity.QuantityReceived;
                stationeryService.ChangeStockLevel(stationeryQuantity.Stationery.StationeryId, diff);
                stationeryQuantity.QuantityReceived = quantitiesReceived[i];
                if (quantitiesReceived[i] > 0) partialReceived = true;
            }
            foreach(var stationeryQuantity in order.StationeryQuantities)
            {
                if (stationeryQuantity.QuantityReceived != stationeryQuantity.QuantityOrdered) allReceived = false;
            }
            if (allReceived)
            {
                order.Status = "Received";
            } else if (partialReceived)
            {
                order.Status = "Partially Received";
            }
            db.SaveChanges();
        }
    }
}