using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Team7ADProject.Database;
using Team7ADProject.Models;
using Team7ADProject.Service;

namespace Team7ADProject.Controllers
{
    public class HomeController : Controller
    {
        private static Team7ADProjectDbContext db;
        private static UserService userService;
        public static void Init()
        {
            db = new Team7ADProjectDbContext();
            userService = new UserService();
        }
        public ActionResult Index()
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
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
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user != null) return RedirectToAction("Index");
            if (HttpContext.Request.HttpMethod == "POST")
            {
                user = userService.Login(username, password);
                if (user != null)
                {
                    Response.Cookies["Team7ADProject"]["sessionId"] = userService.CreateSession(user);
                }
                return RedirectToAction("Index");
            }
            return View();
        }
        public ActionResult Logout()
        {
            HttpCookie cookie = Request.Cookies["Team7ADProject"];
            userService.LogoutWithCookie(cookie);
            return RedirectToAction("Index", "Home");
        }
    }
}