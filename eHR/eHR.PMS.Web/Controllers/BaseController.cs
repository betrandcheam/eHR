using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eHR.PMS.Web.Controllers
{ 
    //[HandleError]
    [Helpers.CustomAuthorization]
    [Filters.CustomAppFilter]
    public class BaseController : Controller
    {
        private PMS.Model.DTO.Core.Security.User obj_user;
        private int int_latest_cycle_id;

        public BaseController()
        {
            InitialiseLatestCycleId();
            InitialiseUserObject();
            ViewData["userid"] = CurrentUser.Id;
            ViewData["moduleName"] = "Performance Appraisal";
            ViewData["hr_user"] = Business.SecurityManager.HasHRRole(CurrentUser) == true ? "Y" : "N";
            ViewBag.Title = "ACR/eHR";
        }

        public int LatestCycleId
        {
            get 
            {
                InitialiseLatestCycleId();
                return int_latest_cycle_id; 
            }
        }

        public PMS.Model.DTO.Core.Security.User CurrentUser
        {
            get { return obj_user; }
        }

        private void InitialiseLatestCycleId()
        {
            int_latest_cycle_id = PMS.Model.PMSModel.GetLatestActiveCycleId();
        }

        private void InitialiseUserObject()
        {
            string str_user_domain_id = Business.SecurityManager.GetUserLoginIDFromPrincipal(System.Web.HttpContext.Current.User);

            if (System.Web.HttpContext.Current.Session[str_user_domain_id] == null)
            {
                obj_user = PMS.Model.PMSModel.GetUserByDomainId(str_user_domain_id);
                System.Web.HttpContext.Current.Session.Add(str_user_domain_id, obj_user);
            }
            else 
            {
                obj_user = (PMS.Model.DTO.Core.Security.User)System.Web.HttpContext.Current.Session[str_user_domain_id];
            }
        }
    }
}
