﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eHR.PMS.Model.DTO.Master
{
    public class Block
    {
        private int int_id;
        private PMS.Model.DTO.Master.Section obj_section;
        private string str_name;
        private string str_description;
        private int? int_sort_order;
        private bool? boo_active;
        private PMS.Model.DTO.Core.Employee obj_modifier;
        private DateTime? dt_modified_timestamp;

        public int Id
        {
            get { return int_id; }
            set { int_id = value; }
        }

        public PMS.Model.DTO.Master.Section Section
        {
            get { return obj_section; }
            set { obj_section = value; }
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

        public int? SortOrder
        {
            get { return int_sort_order; }
            set { int_sort_order = value; }
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
