using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Team7ADProject.Database;
using Team7ADProject.Models;

namespace Team7ADProject.Service
{
    public class UserService
    {
        public UserService() { }
        public User GetUserFromCookie(HttpCookie cookie)
        {
            Team7ADProjectDbContext db = new Team7ADProjectDbContext();
            if(cookie == null) return null;
            if (cookie["sessionId"] == null) return null;
            string sessionId = cookie["sessionId"].ToString();
            Session session = db.Session.Where(x => x.SessionId == sessionId).FirstOrDefault();
            if (session == null) return null;
            return session.User;
        }
        public User GetUserFromSession(string sessionId)
        {
            Team7ADProjectDbContext db = new Team7ADProjectDbContext();
            Session session = db.Session.Where(x => x.SessionId == sessionId).FirstOrDefault();
            if (session != null) return session.User;
            return null;
        }
        public User Login(string username, string password)
        {
            Team7ADProjectDbContext db = new Team7ADProjectDbContext();
            User user = db.User.Where(x => x.Username == username).FirstOrDefault();
            if (user == null) return null;
            if(user.Username == username && user.Password == password)
            {
                return user;
            }
            return null;
        }
        public void LogoutWithCookie(HttpCookie cookie)
        {
            Team7ADProjectDbContext db = new Team7ADProjectDbContext();
            string sessionId = cookie["sessionId"].ToString();
            Session session = db.Session.Where(x => x.SessionId == sessionId).FirstOrDefault();
            db.Session.Remove(session);
            db.SaveChanges();
        }
        public string CreateSession(int userId)
        {
            Team7ADProjectDbContext db = new Team7ADProjectDbContext();
            User user = db.User.Where(x => x.UserId == userId).FirstOrDefault();
            Session session = new Session(user);
            db.Session.Add(session);
            db.SaveChanges();
            return session.SessionId;
        }
        public List<DepartmentStaff> GetDepartmentStaffsByDepartment(int departmentId)
        {
            Team7ADProjectDbContext db = new Team7ADProjectDbContext();
            List<User> allUsers = db.User.ToList();
            List<DepartmentStaff> departmentStaffs = new List<DepartmentStaff>();
            foreach(var user in allUsers)
            {
                if (user.UserType != "departmentStaff") continue;
                if(((DepartmentStaff) user).Department.DepartmentId == departmentId)
                {
                    departmentStaffs.Add((DepartmentStaff)user);
                }
            }
            return departmentStaffs;
        }
    }
}