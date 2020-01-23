using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Team7ADProject.Database;
using Team7ADProject.Models;

namespace Team7ADProject.Service
{
    public class RequestService
    {
        private Team7ADProjectDbContext db;
        public RequestService()
        {
            this.db = new Team7ADProjectDbContext();
        }
        public List<DepartmentRequest> GetDepartmentRequests()
        {
            return db.DepartmentRequest.ToList();
        }
        public DepartmentRequest GetDepartmentRequestById(int departmentrequestId)
        {
            return db.DepartmentRequest.Where(x => x.DepartmentRequestId == departmentrequestId).FirstOrDefault();
        }
        public List<StationeryRequest> GetStationeryRequests()
        {
            return db.StationeryRequest.ToList();
        }
        public StationeryRequest GetStationeryRequestById(int stationeryrequestId)
        {
            return db.StationeryRequest.Where(x => x.StationeryRequestId == stationeryrequestId).FirstOrDefault();
        }
        public List<StationeryQuantity>GetStationeryQuantitiesByDepartment(int departmentrequestId)
        {
            Dictionary<Stationery, int> stationeryQtyMap = new Dictionary<Stationery, int>();
            DepartmentRequest departmentRequest = db.DepartmentRequest.Where(x => x.DepartmentRequestId == departmentrequestId).FirstOrDefault();
             foreach (var stationeryRequest in departmentRequest.StationeryRequests)
            {
                foreach (var stationeryQuantity in stationeryRequest.StationeryQuantities)
                {
                    if (stationeryQtyMap.ContainsKey(stationeryQuantity.Stationery))
                    {
                        stationeryQtyMap[stationeryQuantity.Stationery] += stationeryQuantity.QuantityRequested;
                    } else
                    {
                        stationeryQtyMap[stationeryQuantity.Stationery] = stationeryQuantity.QuantityRequested;
                    }
                }
            }

            List<StationeryQuantity> stationeryQuantities = new List<StationeryQuantity>();
            foreach(var mapItem in stationeryQtyMap)
            {
                StationeryQuantity stationeryQuantity = new StationeryQuantity(mapItem.Key);
                stationeryQuantity.QuantityRequested = mapItem.Value;
                stationeryQuantities.Add(stationeryQuantity);
            }
                return stationeryQuantities;
        }
        public void AddStationeryRequest(int departmentStaffId, string stationeryQuantitiesJSON)
        {
            dynamic stationeryQuantities = JsonConvert.DeserializeObject(stationeryQuantitiesJSON);
            StationeryRequest stationeryRequest = new StationeryRequest(DateTime.Today.ToString("dd-MMM-yy"));
            foreach (var s in stationeryQuantities)
            {
                int stationeryId = Convert.ToInt32(s.stationeryId.ToString());
                int quantity = Convert.ToInt32(s.quantity.ToString());
                Stationery stationery = db.Stationery.Where(x => x.StationeryId == stationeryId).FirstOrDefault();
                StationeryQuantity stationeryQuantity = new StationeryQuantity(stationery);
                stationeryQuantity.QuantityRequested = quantity;
                stationeryRequest.StationeryQuantities.Add(stationeryQuantity);
            }
            DepartmentStaff departmentStaff = (DepartmentStaff)db.User.Where(x => x.UserId == departmentStaffId).FirstOrDefault();
            departmentStaff.StationeryRequests.Add(stationeryRequest);
            db.SaveChanges();
        }
        public List<StationeryRequest> GetStationeryRequestsByDepartment(int departmentId)
        {
            List<StationeryRequest> stationeryRequests = new List<StationeryRequest>();
            List<StationeryRequest> allStationeryRequests = db.StationeryRequest.ToList();
            foreach (var stationeryRequest in allStationeryRequests)
            {
                if (stationeryRequest.DepartmentStaff.Department.DepartmentId == departmentId)
                    stationeryRequests.Add(stationeryRequest);
            }
            return stationeryRequests;
        }
        public void AddToRetrieval(int storeClerkId, int departmentRequestId)
        {
            StoreClerk storeClerk = (StoreClerk) db.User.Where(x => x.UserId == storeClerkId).FirstOrDefault();
            DepartmentRequest departmentRequest = db.DepartmentRequest.Where(x => x.DepartmentRequestId == departmentRequestId).FirstOrDefault();
            storeClerk.RetrievalList.DepartmentRequests.Add(departmentRequest);
            departmentRequest.Status = "Added to Retrieval";
            db.SaveChanges();
        }
        public RetrievalList GetRetrievalListByStoreClerk(int storeClerkId)
        {
            StoreClerk storeClerk = (StoreClerk) db.User.Where(x => x.UserId == storeClerkId).FirstOrDefault();
            return storeClerk.RetrievalList;
        }
        public List<StationeryQuantity> GetStationeryQuantitiesFromRetrieval(int retrievalListId)
        {
            Dictionary<Stationery, int> stationeryQtyMap = new Dictionary<Stationery, int>();
            RetrievalList retrievalList = db.RetrievalList.Where(x => x.RetrievalListId == retrievalListId).FirstOrDefault();
            List<DepartmentRequest> departmentRequests = (List<DepartmentRequest>) retrievalList.DepartmentRequests;
            foreach(var departmentRequest in departmentRequests)
            {
                foreach(var stationeryRequest in departmentRequest.StationeryRequests)
                {
                    foreach(var stationeryQuantity in stationeryRequest.StationeryQuantities)
                    {
                        if (stationeryQtyMap.ContainsKey(stationeryQuantity.Stationery)) {
                            stationeryQtyMap[stationeryQuantity.Stationery] += stationeryQuantity.QuantityRequested;
                        } else
                        {
                            stationeryQtyMap[stationeryQuantity.Stationery] = stationeryQuantity.QuantityRequested;
                        }
                    }
                }
            }
            List<StationeryQuantity> stationeryQuantities = new List<StationeryQuantity>();
            foreach(var mapItem in stationeryQtyMap)
            {
                StationeryQuantity stationeryQuantity = new StationeryQuantity(mapItem.Key);
                stationeryQuantity.QuantityRequested = mapItem.Value;
                stationeryQuantities.Add(stationeryQuantity);
            }
            return stationeryQuantities;
        }
    }
}