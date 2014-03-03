using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eHR.PMS.Model.DTO.Core.Task
{
    public class Task
    {
        private int int_id;
        private PMS.Model.DTO.Core.Master.Module obj_module;
        private int? int_record_id;
        private PMS.Model.DTO.Core.Master.Status obj_status;
        private string str_name;
        private string str_address;
        private List<PMS.Model.DTO.Core.Task.Owner> lst_owners;
        private PMS.Model.DTO.Core.Employee obj_modifier;
        private DateTime? dt_modified_timestamp;

        public int Id
        {
            get { return int_id; }
            set { int_id = value; }
        }

        public PMS.Model.DTO.Core.Master.Module Module
        {
            get { return obj_module; }
            set { obj_module = value; }
        }

        public int? RecordId
        {
            get { return int_record_id; }
            set { int_record_id = value; }
        }

        public PMS.Model.DTO.Core.Master.Status Status
        {
            get { return obj_status; }
            set { obj_status = value; }
        }

        public string Name
        {
            get { return str_name; }
            set { str_name = value; }
        }

        public string Address
        {
            get { return str_address; }
            set { str_address = value; }
        }

        public List<PMS.Model.DTO.Core.Task.Owner> Owners
        {
            get { return lst_owners; }
            set { lst_owners = value; }
        }

        public void AddOwner(PMS.Model.DTO.Core.Task.Owner owner)
        {
            if (owner != null)
            {
                if (Lib.Utility.Common.IsNullOrEmptyList(lst_owners))
                {
                    lst_owners = new List<PMS.Model.DTO.Core.Task.Owner>();
                }
                lst_owners.Add(owner);
            }
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
