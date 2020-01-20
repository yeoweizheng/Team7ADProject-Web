﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Team7ADProject;
using Team7ADProject.Models;
using Team7ADProject.Database;
using System.Data.Entity;
using Team7ADProject.Service;

namespace Team7ADProject.Controllers
{
    public class NotificationsController : Controller
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
            if ( user == null ) return RedirectToAction("Index", "Home");
            if ( user.UserType != "departmentStaff" && user.UserType != "departmentHead" ) return RedirectToAction("Index", "Home");
            return View();
        }

        public ActionResult ViewAll()
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if ( user == null ) return RedirectToAction("Index", "Home");
            if ( user.UserType != "departmentStaff" && user.UserType != "departmentHead" ) return RedirectToAction("Index", "Home");
            ViewData["user"] = user;
            return View();
        }

        //Create a Notification when Actions are done...after which an email is sent...then return X View
        public ActionResult CreateNotification()
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            String notificationType = "";
            NotificationService notificationService = new NotificationService();
            Notification notification = notificationService.CreateNotification(user, notificationType);
            //How to add notification to DB? <- Inside NotificationService
            return RedirectToAction("EmailToRecipient");
        }


        public ActionResult Notification()
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if ( user == null ) return RedirectToAction("Index", "Home");
            if ( user.UserType != "departmentStaff" && user.UserType != "departmentHead" ) return RedirectToAction("Index", "Home");
            //ViewData["notification"] = notification;
            return View();
        }

    }
}