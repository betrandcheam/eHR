using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eHR.PMS.Model.DTO.Core.Security
{
    public class User : PMS.Model.DTO.Core.Employee
    {
        private int int_id;
        private List<PMS.Model.DTO.Core.Security.UserRole> lst_roles;
        private PMS.Model.DTO.Core.Employee obj_modifier;
        private DateTime? dt_modified_timestamp;

        public new int Id
        {
            get { return int_id; }
            set { int_id = value; }
        }

        public List<PMS.Model.DTO.Core.Security.UserRole> Roles
        {
            get { return lst_roles; }
            set { lst_roles = value; }
        }

        public void AddRole(PMS.Model.DTO.Core.Security.UserRole role)
        {
            if (role != null)
            {
                if (Lib.Utility.Common.IsNullOrEmptyList(lst_roles))
                {
                    lst_roles = new List<PMS.Model.DTO.Core.Security.UserRole>();
                }
                lst_roles.Add(role);
            }
        }

        public new PMS.Model.DTO.Core.Employee Modifier
        {
            get { return obj_modifier; }
            set { obj_modifier = value; }
        }

        public new DateTime? ModifiedTimestamp
        {
            get { return dt_modified_timestamp; }
            set { dt_modified_timestamp = value; }
        }
    }
}
