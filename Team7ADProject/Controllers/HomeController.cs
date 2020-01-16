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
            ViewData["showSidebar"] = false;
            return View();
        }
        public ActionResult Login(string username, string password)
        {
            ViewData["showSidebar"] = false;
            if (HttpContext.Request.HttpMethod == "POST") 
            {
                User user = db.User.Where(x => x.Username == username).FirstOrDefault();
                if (user == null) return RedirectToAction("Login");
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

    }
}