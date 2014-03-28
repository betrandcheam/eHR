using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eHR.PMS.Web.Helpers
{
    public class CustomAuthorizationAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool boo_is_authorized = false;

            if (HttpContext.Current.User != null)
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    string str_domain_id = Business.SecurityManager.GetUserLoginIDFromPrincipal(System.Web.HttpContext.Current.User);
                    Model.DTO.Core.Security.User obj_user = Model.PMSModel.GetUserByDomainId(str_domain_id);
                    if (obj_user != null && Business.SecurityManager.IsValidUser(obj_user))
                    {
                        System.Web.HttpContext.Current.Session.Add(str_domain_id, obj_user);
                        boo_is_authorized = true;
                    }
                }
            }

            return boo_is_authorized;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("~/Error/AccessDenied");
        }
    }
}