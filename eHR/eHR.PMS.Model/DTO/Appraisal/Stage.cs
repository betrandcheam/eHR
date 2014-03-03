using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eHR.PMS.Model.DTO.Appraisal
{
    public class Stage : DTO.Master.Stage
    {
        private int int_id;
        private PMS.Model.DTO.Appraisal.Appraisal obj_appraisal;
        private int? int_stage_id;
        private DateTime? d_start_date;
        private DateTime? d_end_date;
        private PMS.Model.DTO.Core.Employee obj_modifier;
        private DateTime? dt_modified_timestamp;

        public new int Id
        {
            get { return int_id; }
            set { int_id = value; }
        }

        public PMS.Model.DTO.Appraisal.Appraisal Appraisal
        {
            get { return obj_appraisal; }
            set { obj_appraisal = value; }
        }

        public int? StageId
        {
            get { return int_stage_id; }
            set { int_stage_id = value; }
        }

        public DateTime? StartDate
        {
            get { return d_start_date; }
            set { d_start_date = value; }
        }

        public DateTime? EndDate
        {
            get { return d_end_date; }
            set { d_end_date = value; }
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
