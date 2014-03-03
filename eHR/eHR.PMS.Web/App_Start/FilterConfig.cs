using System.Web;
using System.Web.Mvc;

namespace eHR.PMS.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new eHR.PMS.Web.Helpers.CustomAuthorizationAttribute());
        }
    }
}