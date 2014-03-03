using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eHR.PMS.Model.DTO.Core.Master
{
    public class Grade
    {
        private int int_id;
        private string str_name;
        private string str_description;
        private bool? boo_active;
        private PMS.Model.DTO.Core.Employee obj_modifier;
        private DateTime? dt_modified_timestamp;

        public int Id
        {
            get { return int_id; }
            set { int_id = value; }
        }

        public string Name
        {
            get { return str_name; }
            set { str_name = value; }
        }

        public string Description
        {
            get { return str_description; }
            set { str_description = value; }
        }

        public bool? Active
        {
            get { return boo_active; }
            set { boo_active = value; }
        }

        public PMS.Model.DTO.Core.Employee Modifier
        {
            get { return obj_modifier; }
            set { obj_modifier = value; }
        }

        public DateTime? ModifiedTimestamp
        {
            get { return dt_modified_timestamp; }
            set { dt_modified_timestamp = value; }
        }
    }
}
