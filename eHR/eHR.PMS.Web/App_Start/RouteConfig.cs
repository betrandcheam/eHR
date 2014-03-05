using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace eHR.PMS.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //routes.IgnoreRoute("Scripts/{pathInfo}/*.js");

            routes.MapRoute(
                name: "HomeUrl",
                url: "Home/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = 0 }
            );
            routes.MapRoute(
                name: "ReviewUrl",
                url: "Review/{action}/{id}",
                defaults: new { controller = "Review", action = "Index", id = 0 }
            );

            routes.MapRoute(
                name: "HRUrl",
                url: "HRManage/{action}/{cycleDateRangeStart}/{cycleDateRangeEnd}/{cycleId}",
                defaults: new { controller = "HRManage", action = "Index", cycleDateRangeStart = UrlParameter.Optional, cycleDateRangeEnd = UrlParameter.Optional, cycleId = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "TaskUrl",
                url: "{controller}/{action}/{taskid}/{id}",
                defaults: new { controller = "Home", action = "Index", taskid = 0, id = 0 }
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            
        }
    }
}