using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eHR.PMS.Model.DTO.Core
{
    public class Employee
    {
        private int int_id;
        private string str_first_name;
        private string str_last_name;
        private string str_preferred_name;
        private string str_domain_id;
        private string str_office_email_address;
        private DTO.Core.Master.EmploymentType obj_employment_type;
        private DTO.Core.Department obj_department;
        private PMS.Model.DTO.Core.Master.Grade obj_grade;
        private bool boo_active;
        private DateTime? dt_date_of_hire;
        private DateTime? dt_date_of_departure;
        private PMS.Model.DTO.Core.Employee obj_pms_level_1_approver;
        private PMS.Model.DTO.Core.Employee obj_pms_level_2_approver;
        private PMS.Model.DTO.Core.Employee obj_modifier;
        private DateTime? dt_modified_timestamp;

        public int Id
        {
            get { return int_id; }
            set { int_id = value; }
        }

        public string FirstName
        {
            get { return str_first_name; }
            set { str_first_name = value; }
        }

        public string LastName
        {
            get { return str_last_name; }
            set { str_last_name = value; }
        }

        public string PreferredName
        {
            get 
            {
                if (string.IsNullOrEmpty(str_preferred_name))
                {
                    StringBuilder sb = new StringBuilder(str_first_name);
                    sb.Append(" ");
                    sb.Append(str_last_name);
                    str_preferred_name =  sb.ToString();
                }
                return str_preferred_name;
            }
            set { str_preferred_name = value; }
        }

        public string DomainId
        {
            get { return str_domain_id; }
            set { str_domain_id = value; }
        }

        public DTO.Core.Department Department
        {
            get { return obj_department; }
            set { obj_department = value; }
        }

        public PMS.Model.DTO.Core.Master.Grade ACRGrade
        {
            get { return obj_grade; }
            set { obj_grade = value; }
        }

        public string OfficeEmailAddress
        {
            get { return str_office_email_address; }
            set { str_office_email_address = value; }
        }

        public DTO.Core.Master.EmploymentType EmploymentType
        {
            get { return obj_employment_type; }
            set { obj_employment_type = value; }
        }

        public DateTime? DateOfHire
        {
            get { return dt_date_of_hire; }
            set { dt_date_of_hire = value; }
        }

        public DateTime? DateOfDeparture
        {
            get { return dt_date_of_departure; }
            set { dt_date_of_departure = value; }
        }

        public bool Active
        {
            get { return boo_active; }
            set { boo_active = value; }
        }

        public DTO.Core.Employee Level1Approver
        {
            get { return obj_pms_level_1_approver; }
            set { obj_pms_level_1_approver = value; }
        }

        public DTO.Core.Employee Level2Approver
        {
            get { return obj_pms_level_2_approver; }
            set { obj_pms_level_2_approver = value; }
        }

        public int GetNumberOfApprovers()
        {
            int int_approvers_count = 0;
            if (Level1Approver != null) { int_approvers_count++; }
            if (Level2Approver != null) { int_approvers_count++; }
            return int_approvers_count;
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
