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
        private Team7ADProjectDbContext db;
        public UserService()
        {
            this.db = new Team7ADProjectDbContext();
        }
        public User GetUserFromCookie(HttpCookie cookie)
        {
            if(cookie == null) return null;
            if (cookie["sessionId"] == null) return null;
            string sessionId = cookie["sessionId"].ToString();
            Session session = db.Session.Where(x => x.SessionId == sessionId).FirstOrDefault();
            if (session == null) return null;
            return session.User;
        }
        public User Login(string username, string password)
        {
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
            string sessionId = cookie["sessionId"].ToString();
            Session session = db.Session.Where(x => x.SessionId == sessionId).FirstOrDefault();
            db.Session.Remove(session);
            db.SaveChanges();
        }
        public string CreateSession(User user)
        {
            Session session = new Session(user);
            db.Session.Add(session);
            db.SaveChanges();
            return session.SessionId;
        }
    }
}