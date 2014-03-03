using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eHR.PMS.Model.DTO.Core.Security
{
    public class UserRole : PMS.Model.DTO.Core.Master.Role
    {
        private int int_id;
        private int? int_role_id;
        private PMS.Model.DTO.Core.Security.User obj_user;
        private PMS.Model.DTO.Core.Employee obj_modifier;
        private DateTime? dt_modified_timestamp;

        public new int Id
        {
            get { return int_id; }
            set { int_id = value; }
        }

        public int? RoleId
        {
            get { return int_role_id; }
            set { int_role_id = value; }
        }

        public PMS.Model.DTO.Core.Security.User User
        {
            get { return obj_user; }
            set { obj_user = value; }
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
