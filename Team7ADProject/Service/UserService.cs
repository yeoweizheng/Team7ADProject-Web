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
        Team7ADProjectDbContext db;
        public UserService() 
        {
        }
        public User GetUserFromCookie(HttpCookie cookie)
        {
            db = new Team7ADProjectDbContext();
            if(cookie == null) return null;
            if (cookie["sessionId"] == null) return null;
            string sessionId = cookie["sessionId"].ToString();
            Session session = db.Session.Where(x => x.SessionId == sessionId).FirstOrDefault();
            if (session == null) return null;
            return session.User;
        }
        public User GetUserFromSession(string sessionId)
        {
            db = new Team7ADProjectDbContext();
            Session session = db.Session.Where(x => x.SessionId == sessionId).FirstOrDefault();
            if (session != null) return session.User;
            return null;
        }
        public User GetUserById(int userId)
        {
            db = new Team7ADProjectDbContext();
            User user = db.User.Where(x => x.UserId == userId).FirstOrDefault();
            return user;
        }
        public User Login(string username, string password)
        {
            db = new Team7ADProjectDbContext();
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
            db = new Team7ADProjectDbContext();
            string sessionId = cookie["sessionId"].ToString();
            Session session = db.Session.Where(x => x.SessionId == sessionId).FirstOrDefault();
            db.Session.Remove(session);
            db.SaveChanges();
        }
        public string CreateSession(int userId)
        {
            db = new Team7ADProjectDbContext();
            User user = db.User.Where(x => x.UserId == userId).FirstOrDefault();
            Session session = new Session(user);
            db.Session.Add(session);
            db.SaveChanges();
            return session.SessionId;
        }
        public List<DepartmentStaff> GetDepartmentStaffsByDepartment(int departmentId)
        {
            db = new Team7ADProjectDbContext();
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
        public DepartmentStaff GetAssignedRepresentative(int departmentId)
        {
            db = new Team7ADProjectDbContext();
            List<User> allUsers = db.User.ToList();
            foreach(var user in allUsers)
            {
                if (user.UserType != "departmentStaff") continue;
                DepartmentStaff departmentStaff = (DepartmentStaff)user;
                if (departmentStaff.Representative && departmentStaff.Department.DepartmentId == departmentId) return departmentStaff;
            }
            return null;
        }
        public void AssignNewRepresentative(int departmentId, int staffId)
        {
            db = new Team7ADProjectDbContext();
            List<User> allUsers = db.User.ToList();
            foreach(var user in allUsers)
            {
                if (user.UserType != "departmentStaff") continue;
                DepartmentStaff departmentStaff = (DepartmentStaff)user;
                if(departmentStaff.Representative && departmentStaff.Department.DepartmentId == departmentId)
                {
                    departmentStaff.Representative = false;
                }
                if(departmentStaff.UserId == staffId)
                {
                    departmentStaff.Representative = true;
                }
            }
            db.SaveChanges();
        }
        public List<AuthorizeForm> GetAuthorizeFormsByDepartment(int departmentId)
        {
            db = new Team7ADProjectDbContext();
            List<AuthorizeForm> authorizeForms = new List<AuthorizeForm>();
            List<AuthorizeForm> allAuthorizeForms = db.AuthorizeForm.ToList();
            foreach (var authorizeForm in allAuthorizeForms)
            {
                if (authorizeForm.DepartmentStaff.Department.DepartmentId == departmentId)
                {
                    authorizeForms.Add(authorizeForm);
                }
            }
            return authorizeForms;
        }
        public bool AddAuthorizeStaff(int departmentStaffId, string startDate, string endDate)
        {
            db = new Team7ADProjectDbContext();
            DepartmentStaff departmentStaff = (DepartmentStaff)db.User.Where(x => x.UserId == departmentStaffId).FirstOrDefault();
            List<AuthorizeForm> allAuthorizeForms = db.AuthorizeForm.ToList();
            List<AuthorizeForm> existingDeptAuths = new List<AuthorizeForm>();
            foreach(var af in allAuthorizeForms)
            {
                if(af.DepartmentStaff.Department.DepartmentId == departmentStaff.Department.DepartmentId)
                {
                    existingDeptAuths.Add(af);
                }
            }
            foreach(var af in existingDeptAuths)
            {
                if (DateService.IsOverlap(af.StartDate, af.EndDate, startDate, endDate)) return false;
            }
            db.AuthorizeForm.Add(new AuthorizeForm(departmentStaff, startDate, endDate));
            db.SaveChanges();
            return true;
        }
        public void CancelAuthorizeStaff(int authorizeFormId)
        {
            db = new Team7ADProjectDbContext();
            AuthorizeForm authorizeForm = db.AuthorizeForm.Where(x => x.AuthorizeFormId == authorizeFormId).FirstOrDefault();
            db.AuthorizeForm.Remove(authorizeForm);
            db.SaveChanges();
        }
        public bool IsStaffAuthorized(int staffId)
        {
            db = new Team7ADProjectDbContext();
            List<AuthorizeForm> authorizeForms = db.AuthorizeForm.ToList();
            foreach(var authorizeForm in authorizeForms)
            {
                if(authorizeForm.DepartmentStaff.UserId == staffId && 
                    DateService.IsOverlap(DateService.GetTodayDate(), DateService.GetTodayDate(), authorizeForm.StartDate, authorizeForm.EndDate))
                {
                    return true;
                }
            }
            return false;
        }
        public bool IsAuthorityDelegated(int departmentId)
        {
            db = new Team7ADProjectDbContext();
            List<AuthorizeForm> authorizeForms = db.AuthorizeForm.ToList();
            foreach(var authorizeForm in authorizeForms)
            {
                if(authorizeForm.DepartmentStaff.Department.DepartmentId == departmentId &&
                    DateService.IsOverlap(DateService.GetTodayDate(), DateService.GetTodayDate(), authorizeForm.StartDate, authorizeForm.EndDate))
                {
                    return true;
                }
            }
            return false;
        }
    }
}