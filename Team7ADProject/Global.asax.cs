using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Team7ADProject.Controllers;
using Team7ADProject.Database;

namespace Team7ADProject
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Team7ADProjectDbContext db = new Team7ADProjectDbContext();
            db.Database.Initialize(force: true);
            HomeController.Init();
            DepartmentController.Init();
        }
    }
}
