﻿using Newtonsoft.Json;
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
        public double GetSupplierPrice(int supplierId, int stationeryId, Team7ADProjectDbContext dbInput)
        {
            db = dbInput == null ? new Team7ADProjectDbContext() : dbInput;
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
                stationeryQuantity.Price = GetSupplierPrice(supplierId, stationeryId, db);
                stationeryQuantity.Subtotal = (double)quantityOrdered * GetSupplierPrice(supplierId, stationeryId, db);
                if (ordersBySupplier.ContainsKey(supplierId))
                {
                    ordersBySupplier[supplierId].Add(stationeryQuantity);
                } else
                {
                    ordersBySupplier[supplierId] = new List<StationeryQuantity>();
                    ordersBySupplier[supplierId].Add(stationeryQuantity);
                }
            }
            List<int> suppliersCovered = new List<int>();
            foreach(var o in ordersBySupplier)
            {
                List<Order> existingOrders = db.Order.ToList();
                foreach(var existingOrder in existingOrders)
                {
                    if(existingOrder.Supplier.SupplierId == o.Key && existingOrder.Status == "Created")
                    {
                        Dictionary<Stationery, int> existingOrderQty = new Dictionary<Stationery, int>();
                        foreach(var existingSQ in existingOrder.StationeryQuantities)
                        {
                            existingOrderQty[existingSQ.Stationery] = existingSQ.QuantityOrdered;
                        }
                        foreach(var newSQ in o.Value)
                        {
                            if (existingOrderQty.ContainsKey(newSQ.Stationery))
                            {
                                existingOrderQty[newSQ.Stationery] += newSQ.QuantityOrdered;
                            } else
                            {
                                existingOrderQty[newSQ.Stationery] = newSQ.QuantityOrdered;
                            }
                        }
                        List<StationeryQuantity> updatedSQ = new List<StationeryQuantity>();
                        existingOrder.StationeryQuantities.Clear();
                        foreach(var s in existingOrderQty)
                        {
                            StationeryQuantity stationeryQuantity = new StationeryQuantity(s.Key);
                            stationeryQuantity.QuantityOrdered = s.Value;
                            stationeryQuantity.Price = GetSupplierPrice(existingOrder.Supplier.SupplierId, s.Key.StationeryId, db);
                            stationeryQuantity.Subtotal = stationeryQuantity.QuantityOrdered * stationeryQuantity.Price;
                            existingOrder.StationeryQuantities.Add(stationeryQuantity);
                        }
                        suppliersCovered.Add(existingOrder.Supplier.SupplierId);
                    }
                }
            }
            foreach(var o in ordersBySupplier)
            {
                if (suppliersCovered.Contains(o.Key)) continue;
                Supplier supplier = db.Supplier.Where(x => x.SupplierId == o.Key).FirstOrDefault();
                Order order = new Order(supplier, DateService.GetTodayDate());
                order.StationeryQuantities = o.Value;
                db.Order.Add(order);
            }
            db.SaveChanges();
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
        public void PlaceOrder(int orderId)
        {
            db = new Team7ADProjectDbContext();
            Order order = db.Order.Where(x => x.OrderId == orderId).FirstOrDefault();
            order.Status = "Ordered";
            db.SaveChanges();
        }
        public List<Supplier> GetSuppliers()
        {
            db = new Team7ADProjectDbContext();
            List<Supplier> suppliers = db.Supplier.ToList();
            return suppliers;
        }
    }
}