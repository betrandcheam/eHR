using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eHR.PMS.Web.Controllers
{
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        //
        // GET: /Error/

        public ActionResult AccessDenied(string aspxerrorpath)
        {
            ViewData["hr_user"] = "N";
            ViewData["OldUrl"] = aspxerrorpath;
            return View();
        }

        public ActionResult PageNotFound()
        {
            ViewData["hr_user"] = "N";
            return View();
        }

        public ActionResult CycleNotStart(int? sid,string sname)
        {
            ViewData["hr_user"] = "N";
            string message=string.Empty;
            switch (sid.HasValue ? sid.Value : 0)
            {
                case 1:
                    message = "Sorry, the cycle has not started yet,please wait";
                    break;
                case 2:
                    message = "Sorry, the cycle current is <span class='label label-danger'>" + sname + "</span>, you can not go ahead of this stage";
                    break;
                case 3:
                    message = "Sorry, the cycle current is <span class='label label-danger'>" + sname + ", you can not go ahead of this stage";
                    break;
                default:
                    break;
            }
            ViewData["message"]=message;
            return View();
        }
    }
}
