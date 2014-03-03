using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eHR.PMS.Model.DTO.Core.Task
{
    public class Owner : PMS.Model.DTO.Core.Employee
    {
        private int int_id;
        private PMS.Model.DTO.Core.Task.Task obj_task;
        private int? int_employee_id;
        private PMS.Model.DTO.Core.Employee obj_modifier;
        private DateTime? dt_modified_timestamp;

        public new int Id
        {
            get { return int_id; }
            set { int_id = value; }
        }

        public PMS.Model.DTO.Core.Task.Task Task
        {
            get { return obj_task; }
            set { obj_task = value; }
        }

        public int? EmployeeId
        {
            get { return int_employee_id; }
            set { int_employee_id = value; }
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
