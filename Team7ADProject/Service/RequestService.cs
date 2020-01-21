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
    }
}