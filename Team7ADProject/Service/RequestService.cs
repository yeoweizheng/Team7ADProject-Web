﻿using Newtonsoft.Json;
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
        private NotificationService notificationService;
        private Team7ADProjectDbContext db;
        public RequestService()
        {
            this.notificationService = new NotificationService();
        }
        public List<DepartmentRequest> GetDepartmentRequests()
        {
            db = new Team7ADProjectDbContext();
            return db.DepartmentRequest.ToList();
        }
        public DepartmentRequest GetDepartmentRequestById(int departmentrequestId)
        {
            db = new Team7ADProjectDbContext();
            return db.DepartmentRequest.Where(x => x.DepartmentRequestId == departmentrequestId).FirstOrDefault();
        }
        public List<StationeryRequest> GetStationeryRequests()
        {
            db = new Team7ADProjectDbContext();
            return db.StationeryRequest.ToList();
        }
        public StationeryRequest GetStationeryRequestById(int stationeryrequestId)
        {
            db = new Team7ADProjectDbContext();
            return db.StationeryRequest.Where(x => x.StationeryRequestId == stationeryrequestId).FirstOrDefault();
        }
        public List<StationeryQuantity> GetStationeryQuantitiesByDepartment(int departmentrequestId)
        {
            db = new Team7ADProjectDbContext();
            Dictionary<Stationery, int> stationeryQtyMap = new Dictionary<Stationery, int>();
            DepartmentRequest departmentRequest = db.DepartmentRequest.Where(x => x.DepartmentRequestId == departmentrequestId).FirstOrDefault();
            foreach (var stationeryRequest in departmentRequest.StationeryRequests)
            {
                foreach (var stationeryQuantity in stationeryRequest.StationeryQuantities)
                {
                    if (stationeryQtyMap.ContainsKey(stationeryQuantity.Stationery))
                    {
                        stationeryQtyMap[stationeryQuantity.Stationery] += stationeryQuantity.QuantityRequested;
                    }
                    else
                    {
                        stationeryQtyMap[stationeryQuantity.Stationery] = stationeryQuantity.QuantityRequested;
                    }
                }
            }

            List<StationeryQuantity> stationeryQuantities = new List<StationeryQuantity>();
            foreach (var mapItem in stationeryQtyMap)
            {
                StationeryQuantity stationeryQuantity = new StationeryQuantity(mapItem.Key);
                stationeryQuantity.QuantityRequested = mapItem.Value;
                stationeryQuantities.Add(stationeryQuantity);
            }
            return stationeryQuantities;
        }
        public void AddStationeryRequest(int departmentStaffId, string stationeryQuantitiesJSON)
        {
            db = new Team7ADProjectDbContext();
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
            db = new Team7ADProjectDbContext();
            List<StationeryRequest> stationeryRequests = new List<StationeryRequest>();
            List<StationeryRequest> allStationeryRequests = db.StationeryRequest.ToList();
            foreach (var stationeryRequest in allStationeryRequests)
            {
                if (stationeryRequest.DepartmentStaff.Department.DepartmentId == departmentId)
                    stationeryRequests.Add(stationeryRequest);
            }
            return stationeryRequests;
        }
        public List<StationeryRequest> GetStationeryRequestsByStaffId(int staffId)
        {
            db = new Team7ADProjectDbContext();
            List<StationeryRequest> stationeryRequests = new List<StationeryRequest>();
            List<StationeryRequest> allStationeryRequests = db.StationeryRequest.ToList();
            foreach (var stationeryRequest in allStationeryRequests)
            {
                if (stationeryRequest.DepartmentStaff.UserId == staffId)
                    stationeryRequests.Add(stationeryRequest);
            }
            return stationeryRequests;
        }
        public void AddToRetrieval(int storeClerkId, int departmentRequestId)
        {
            db = new Team7ADProjectDbContext();
            StoreClerk storeClerk = (StoreClerk)db.User.Where(x => x.UserId == storeClerkId).FirstOrDefault();
            DepartmentRequest departmentRequest = db.DepartmentRequest.Where(x => x.DepartmentRequestId == departmentRequestId).FirstOrDefault();
            storeClerk.RetrievalList.DepartmentRequests.Add(departmentRequest);
            departmentRequest.Status = "Added to Retrieval";
            db.SaveChanges();
        }
        public void ApproveStationeryRequest(int stationeryRequestId, string remarks)
        {
            db = new Team7ADProjectDbContext();
            StationeryRequest stationeryRequest = db.StationeryRequest.Where(x => x.StationeryRequestId == stationeryRequestId).FirstOrDefault();
            stationeryRequest.Status = "Approved";
            stationeryRequest.Remarks = remarks;
            int departmentStaffId = stationeryRequest.DepartmentStaff.UserId;
            notificationService.SendNotificationToUser(departmentStaffId, DateTime.Today.ToString("dd-MMM-yy"), "Department Head", "Stationery Request Approved", "Your stationry request has been approved");
            db.SaveChanges();
        }
        public void RejectStationeryRequest(int stationeryRequestId, string remarks)
        {
            db = new Team7ADProjectDbContext();
            StationeryRequest stationeryRequest = db.StationeryRequest.Where(x => x.StationeryRequestId == stationeryRequestId).FirstOrDefault();
            stationeryRequest.Status = "Rejected";
            stationeryRequest.Remarks = remarks;
            int departmentStaffId = stationeryRequest.DepartmentStaff.UserId;
            notificationService.SendNotificationToUser(departmentStaffId, DateTime.Today.ToString("dd-MMM-yy"), "Department Head", "Stationery Request Rejected", "The Stationery Request you made has been Rejected, kindly review your request again");
            db.SaveChanges();
        }
        public void RemoveFromRetrieval(int storeClerkId, int departmentRequestId)
        {
            db = new Team7ADProjectDbContext();
            StoreClerk storeClerk = (StoreClerk)db.User.Where(x => x.UserId == storeClerkId).FirstOrDefault();
            DepartmentRequest departmentRequest = db.DepartmentRequest.Where(x => x.DepartmentRequestId == departmentRequestId).FirstOrDefault();
            storeClerk.RetrievalList.DepartmentRequests.Remove(departmentRequest);
            departmentRequest.Status = "Not Retrieved";
            db.SaveChanges();
        }
        public void MarkAsRetrieved(int storeClerkId)
        {
            db = new Team7ADProjectDbContext();
            StoreClerk storeClerk = (StoreClerk)db.User.Where(x => x.UserId == storeClerkId).FirstOrDefault();
            List<DepartmentRequest> departmentRequests = (List<DepartmentRequest>)storeClerk.RetrievalList.DepartmentRequests;
            foreach (var departmentRequest in departmentRequests)
            {
                departmentRequest.Status = "Retrieved";
            }
            departmentRequests.Clear();
            db.SaveChanges();
        }
        public RetrievalList GetRetrievalListByStoreClerk(int storeClerkId)
        {
            db = new Team7ADProjectDbContext();
            StoreClerk storeClerk = (StoreClerk)db.User.Where(x => x.UserId == storeClerkId).FirstOrDefault();
            return storeClerk.RetrievalList;
        }
        public List<StationeryQuantity> GetStationeryQuantitiesFromRetrieval(int retrievalListId)
        {
            db = new Team7ADProjectDbContext();
            Dictionary<Stationery, int> stationeryQtyMap = new Dictionary<Stationery, int>();
            RetrievalList retrievalList = db.RetrievalList.Where(x => x.RetrievalListId == retrievalListId).FirstOrDefault();
            List<DepartmentRequest> departmentRequests = (List<DepartmentRequest>)retrievalList.DepartmentRequests;
            foreach (var departmentRequest in departmentRequests)
            {
                foreach (var stationeryRequest in departmentRequest.StationeryRequests)
                {
                    foreach (var stationeryQuantity in stationeryRequest.StationeryQuantities)
                    {
                        if (stationeryQtyMap.ContainsKey(stationeryQuantity.Stationery))
                        {
                            stationeryQtyMap[stationeryQuantity.Stationery] += stationeryQuantity.QuantityRequested;
                        }
                        else
                        {
                            stationeryQtyMap[stationeryQuantity.Stationery] = stationeryQuantity.QuantityRequested;
                        }
                    }
                }
            }
            List<StationeryQuantity> stationeryQuantities = new List<StationeryQuantity>();
            foreach (var mapItem in stationeryQtyMap)
            {
                StationeryQuantity stationeryQuantity = new StationeryQuantity(mapItem.Key);
                stationeryQuantity.QuantityRequested = mapItem.Value;
                stationeryQuantities.Add(stationeryQuantity);
            }
            return stationeryQuantities;
        }
        public void AddToDisbursement(int storeClerkId, int departmentRequestId)
        {
            db = new Team7ADProjectDbContext();
            StoreClerk storeClerk = (StoreClerk)db.User.Where(x => x.UserId == storeClerkId).FirstOrDefault();
            DepartmentRequest departmentRequest = db.DepartmentRequest.Where(x => x.DepartmentRequestId == departmentRequestId).FirstOrDefault();
            storeClerk.DisbursementList.DepartmentRequests.Add(departmentRequest);
            departmentRequest.Status = "Added to Disbursement";
            db.SaveChanges();
        }
        public void RemoveDisbursement(int storeClerkId, int departmentRequestId)
        {
            db = new Team7ADProjectDbContext();
            StoreClerk storeClerk = (StoreClerk)db.User.Where(x => x.UserId == storeClerkId).FirstOrDefault();
            DepartmentRequest departmentRequest = db.DepartmentRequest.Where(x => x.DepartmentRequestId == departmentRequestId).FirstOrDefault();
            storeClerk.DisbursementList.DepartmentRequests.Remove(departmentRequest);
            departmentRequest.Status = "Retrieved";
            db.SaveChanges();
        }
        public void MarkAsDisbursed(int storeClerkId)
        {
            db = new Team7ADProjectDbContext();
            StoreClerk storeClerk = (StoreClerk)db.User.Where(x => x.UserId == storeClerkId).FirstOrDefault();
            List<DepartmentRequest> departmentRequests = (List<DepartmentRequest>)storeClerk.DisbursementList.DepartmentRequests;
            foreach (var departmentRequest in departmentRequests)
            {
                departmentRequest.Status = "Disbursed";
            }
            departmentRequests.Clear();
            db.SaveChanges();
        }
        public DisbursementList GetDisbursementListByStoreClerk(int storeClerkId)
        {
            db = new Team7ADProjectDbContext();
            db = new Team7ADProjectDbContext();
            StoreClerk storeClerk = (StoreClerk)db.User.Where(x => x.UserId == storeClerkId).FirstOrDefault();
            return storeClerk.DisbursementList;
        }
        public List<StationeryQuantity> GetStationeryQuantitiesFromDisbursement(int disbursementListId)
        {
            db = new Team7ADProjectDbContext();
            Dictionary<Stationery, int> stationeryQtyMap = new Dictionary<Stationery, int>();
            DisbursementList disbursementList = db.DisbursementList.Where(x => x.DisbursementListId == disbursementListId).FirstOrDefault();
            List<DepartmentRequest> departmentRequests = (List<DepartmentRequest>)disbursementList.DepartmentRequests;
            foreach (var departmentRequest in departmentRequests)
            {
                foreach (var stationeryRequest in departmentRequest.StationeryRequests)
                {
                    foreach (var stationeryQuantity in stationeryRequest.StationeryQuantities)
                    {
                        if (stationeryQtyMap.ContainsKey(stationeryQuantity.Stationery))
                        {
                            stationeryQtyMap[stationeryQuantity.Stationery] += stationeryQuantity.QuantityRequested;
                        }
                        else
                        {
                            stationeryQtyMap[stationeryQuantity.Stationery] = stationeryQuantity.QuantityRequested;
                        }
                    }
                }
            }
            List<StationeryQuantity> stationeryQuantities = new List<StationeryQuantity>();
            foreach (var mapItem in stationeryQtyMap)
            {
                StationeryQuantity stationeryQuantity = new StationeryQuantity(mapItem.Key);
                stationeryQuantity.QuantityRequested = mapItem.Value;
                stationeryQuantities.Add(stationeryQuantity);
            }
            return stationeryQuantities;
        }
        public DepartmentRequest GetDepartmentRequestsByDepartmentStaff(int departmentStaffId)
        {
            db = new Team7ADProjectDbContext();
            DepartmentStaff departmentStaff = (DepartmentStaff)db.User.Where(x => x.UserId == departmentStaffId).FirstOrDefault();
            return departmentStaff.DepartmentRequest;
        }
        public List<DepartmentRequest> GetDepartmentRequestsByDepartment(int departmentId, int disbursementListId)
        {
            db = new Team7ADProjectDbContext();
            Dictionary<Department, int> DepartmentMap = new Dictionary<Department, int>();
            Department department = db.Department.Where(x => x.DepartmentId == departmentId).FirstOrDefault();
            DisbursementList disbursementList = db.DisbursementList.Where(x => x.DisbursementListId == disbursementListId).FirstOrDefault();
            List<DepartmentRequest> departmentRequests = (List<DepartmentRequest>)disbursementList.DepartmentRequests;
            foreach (var departmentRequest in departmentRequests)
            {
                if (departmentRequest.Status == "Disbursed" && DepartmentMap.ContainsKey(departmentRequest.Department))
                {
                    departmentRequests.Add(departmentRequest);
                }
            }
            return departmentRequests;
        }
        public List<AuthorizeForm> GetAuthorizeForms()
        {
            db = new Team7ADProjectDbContext();
            return db.AuthorizeForm.ToList();
        }
        public AuthorizeForm GetAuthorizeFormById(int authorizeFormId)
        {
            db = new Team7ADProjectDbContext();
            return db.AuthorizeForm.Where(x => x.AuthorizeFormId == authorizeFormId).FirstOrDefault();
        }
        public List<AuthorizeForm> GetAuthorizeFormsByDepartment(int departmentId)
        {
            db = new Team7ADProjectDbContext();
            List<AuthorizeForm> authorizeForms = new List<AuthorizeForm>();
            List<AuthorizeForm> allAuthorizeForms = db.AuthorizeForm.ToList();
            foreach (var authorizeForm in allAuthorizeForms)
            {
                if (authorizeForm.DepartmentStaff.Department.DepartmentId == departmentId)
                    authorizeForms.Add(authorizeForm);
            }
            return authorizeForms;
        }
        public void AddAuthorizeStaff(int departmentStaffId, string startDate, string endDate)
        {
            db = new Team7ADProjectDbContext();
            DepartmentStaff departmentStaff = (DepartmentStaff)db.User.Where(x => x.UserId == departmentStaffId).FirstOrDefault();
            db.AuthorizeForm.Add(new AuthorizeForm(departmentStaff, startDate, endDate));
        }
        public void CancelAuthorizeStaff(int departmentHeadId, int authorizeFormId)
        {
            db = new Team7ADProjectDbContext();
            DepartmentHead departmentHead = (DepartmentHead)db.User.Where(x => x.UserId == departmentHeadId).FirstOrDefault();
            AuthorizeForm authorizeForm = db.AuthorizeForm.Where(x => x.AuthorizeFormId == authorizeFormId).FirstOrDefault();
            departmentHead.AuthorizeForm.DepartmentStaff.AuthorizeForms.Remove(authorizeForm);
            db.SaveChanges();
        }
        public List<AssignForm> GetAssignForms()
        {
            db = new Team7ADProjectDbContext();
            return db.AssignForm.ToList();
        }
        public AssignForm GetAssignFormById(int assignFormId)
        {
            return db.AssignForm.Where(x => x.AssignFormId == assignFormId).FirstOrDefault();
        }
        public List<AssignForm> GetAssignFormsByDepartment(int departmentId)
        {
            db = new Team7ADProjectDbContext();
            List<AssignForm> assignForms = new List<AssignForm>();
            List<AssignForm> allAssignForms = db.AssignForm.ToList();
            foreach (var assignForm in allAssignForms)
            {
                if (assignForm.DepartmentStaff.Department.DepartmentId == departmentId)
                    assignForms.Add(assignForm);
            }
            return assignForms;
        }
        public void AddAssignRepresentative(int departmentStaffId, string startDate, string endDate)
        {
            db = new Team7ADProjectDbContext();
            DepartmentStaff departmentStaff = (DepartmentStaff)db.User.Where(x => x.UserId == departmentStaffId).FirstOrDefault();
            db.AssignForm.Add(new AssignForm(departmentStaff, startDate, endDate));
            db.SaveChanges();
        }
        public void CancelAssignRepresentative(int departmentHeadId, int assignFormId)
        {
            db = new Team7ADProjectDbContext();
            DepartmentHead departmentHead = (DepartmentHead)db.User.Where(x => x.UserId == departmentHeadId).FirstOrDefault();
            AssignForm assignForm = db.AssignForm.Where(x => x.AssignFormId == assignFormId).FirstOrDefault();
            departmentHead.AssignForm.DepartmentStaff.AssignForms.Remove(assignForm);
            db.SaveChanges();
        }
        public void GenerateDepartmentRequests()
        {
            db = new Team7ADProjectDbContext();
            List<StationeryRequest> allStationeryRequests = db.StationeryRequest.ToList();
            List<StationeryRequest> stationeryRequestsToAdd = new List<StationeryRequest>();
            foreach(var sr in allStationeryRequests)
            {
                if (sr.IncludedInDeptRequest == false && sr.Status == "Approved") stationeryRequestsToAdd.Add(sr);
            }
            Dictionary<Department, List<StationeryRequest>> deptReqDict = new Dictionary<Department, List<StationeryRequest>>();
            foreach(var sr in stationeryRequestsToAdd)
            {
                if (!deptReqDict.ContainsKey(sr.DepartmentStaff.Department))
                {
                    deptReqDict[sr.DepartmentStaff.Department] = new List<StationeryRequest>();
                }
                deptReqDict[sr.DepartmentStaff.Department].Add(sr);
                sr.IncludedInDeptRequest = true;
            }
            foreach(var d in deptReqDict)
            {
                DepartmentRequest departmentRequest = new DepartmentRequest(d.Key, DateTime.Today.ToString("dd-MMM-yy"), "");
                departmentRequest.StationeryRequests = d.Value;
                db.DepartmentRequest.Add(departmentRequest);
            }
            db.SaveChanges();
        }
    }
}
