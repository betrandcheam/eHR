using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eHR.PMS.Web.Filters
{
    public class CustomAppFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            int stageid = 0;
            string stagename = string.Empty;

           // var abc = filterContext.RouteData.Values["controller"].ToString();

            //filterContext.Result = new RedirectResult("~/");

        }
    }
}