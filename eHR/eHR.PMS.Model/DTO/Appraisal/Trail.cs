using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eHR.PMS.Model.DTO.Appraisal
{
    public class Trail
    {
        private int int_id;
        private PMS.Model.DTO.Appraisal.Appraisal obj_appraisal;
        private PMS.Model.DTO.Master.Stage obj_stage;
        private PMS.Model.DTO.Master.Action obj_action;
        private PMS.Model.DTO.Core.Employee obj_actioner;
        private DateTime? dt_action_timestamp;

        public int Id
        {
            get { return int_id; }
            set { int_id = value; }
        }

        public PMS.Model.DTO.Appraisal.Appraisal Appraisal
        {
            get { return obj_appraisal; }
            set { obj_appraisal = value; }
        }

        public PMS.Model.DTO.Master.Stage Stage
        {
            get { return obj_stage; }
            set { obj_stage = value; }
        }

        public PMS.Model.DTO.Master.Action Action
        {
            get { return obj_action; }
            set { obj_action = value; }  
        }

        public PMS.Model.DTO.Core.Employee Actioner
        {
            get { return obj_actioner; }
            set { obj_actioner = value; }
        }

        public DateTime? ActionTimestamp
        {
            get { return dt_action_timestamp; }
            set { dt_action_timestamp = value; }
        }
        
    }
}
