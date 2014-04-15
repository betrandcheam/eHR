using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace eHR.PMS.Web.Helpers
{
    public class Common
    {
        public static string GetLayout(RouteData routeData, string defaultLayout)
        {
            string str_default_layout = "";

            if (!string.IsNullOrEmpty(defaultLayout))
            {
                str_default_layout = defaultLayout;
            }
            string str_controller_name = routeData.Values["controller"].ToString().ToLower();

            if (str_controller_name.IndexOf("stage") >= 0 || str_controller_name.IndexOf("review") >= 0) 
            { 
                str_default_layout =  "~/Views/Shared/_LayoutForApprisal.cshtml";
            }

            return str_default_layout;
        }

        public static void LogToFile(string text)
        {
            string path = System.Web.HttpContext.Current.Server.MapPath("~/log/log.txt");
            Lib.Utility.Common.Log(path, text);
        }

        public static void LogException(Exception exc)
        {
            if (exc != null)
            {
                LogToFile(exc.Message);
                LogToFile(exc.StackTrace);

                if (exc.InnerException != null)
                {
                    LogToFile(exc.InnerException.Message);
                    LogToFile(exc.InnerException.StackTrace);
                }
            }
        }

    }
}