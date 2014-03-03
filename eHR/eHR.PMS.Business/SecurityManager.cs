using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eHR.PMS.Business
{
    public class SecurityManager
    {
        public static string GetUserLoginIDFromPrincipal(System.Security.Principal.IPrincipal userPrincipal)
        {
               return userPrincipal.Identity.Name.ToUpper().Replace(@"ASIACAPITALRE", "").Replace(@"\", "");
        }

        public static bool IsValidUser(PMS.Model.DTO.Core.Security.User user)
        {
            return user.Active;
        }

        public static bool HasHRRole(Model.DTO.Core.Security.User user)
        {
            bool boo_is_hr_user = false;
            if (!Lib.Utility.Common.IsNullOrEmptyList(user.Roles))
            {
                if (!Lib.Utility.Common.IsNullOrEmptyList(user.Roles.Where(rec => rec.RoleId == Model.PMSConstants.ROLE_ID_HR)))
                {
                    boo_is_hr_user = true;
                }
            }
            return boo_is_hr_user;
        }
    }
}
