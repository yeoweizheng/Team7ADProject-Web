using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Team7ADProject.Database;
using Team7ADProject.Models;

namespace Team7ADProject.Controllers
{
    public class HomeController : Controller
    {
        private static Team7ADProjectDbContext db;
        public static void Init()
        {
            db = new Team7ADProjectDbContext();
        }
        public ActionResult Index()
        {
            User user = HomeController.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null)
            {
                return RedirectToAction("Login");
            } else
            {
                if (user.UserType == "departmentStaff" || user.UserType == "departmentHead")
                {
                    return RedirectToAction("Index", "Department");
                } else
                {
                    return RedirectToAction("Index", "Store");
                }
            }
        }
        public ActionResult Login(string username, string password)
        {
            User user = HomeController.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user != null) return RedirectToAction("Index");
            if (HttpContext.Request.HttpMethod == "POST") 
            {
                user = db.User.Where(x => x.Username == username).FirstOrDefault();
                if(user == null) return RedirectToAction("Login");
                if(user.Username == username && user.Password == password)
                {
                    Session session = new Session(user);
                    Response.Cookies["Team7ADProject"]["sessionId"] = session.SessionId;
                    db.Session.Add(session);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Department");
                } else
                {
                    return RedirectToAction("Login");
                }
            }
            return View();
        }
        public ActionResult Logout()
        {
            HttpCookie cookie = Request.Cookies["Team7ADProject"];
            if (GetUserFromCookie(cookie) != null)
            {
                string sessionId = cookie["sessionId"].ToString();
                Session session = db.Session.Where(x => x.SessionId == sessionId).FirstOrDefault();
                db.Session.Remove(session);
                db.SaveChanges();
            }
            return RedirectToAction("Login");
        }
        public static User GetUserFromCookie(HttpCookie cookie)
        {
            if(cookie == null) return null;
            if (cookie["sessionId"] == null) return null;
            string sessionId = cookie["sessionId"].ToString();
            Session session = db.Session.Where(x => x.SessionId == sessionId).FirstOrDefault();
            if (session == null) return null;
            return session.User;
        }

    }
}