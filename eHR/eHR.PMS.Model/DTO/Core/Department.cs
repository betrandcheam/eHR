using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eHR.PMS.Model.DTO.Core
{
    public class Department : Master.Department
    {
        private int int_id;
        private int? int_master_department_id;

        public new int Id
        {
            get { return int_id; }
            set { int_id = value; }
        }

        public int? DepartmentId
        {
            get { return int_master_department_id; }
            set { int_master_department_id = value; }
        }
    }
}
