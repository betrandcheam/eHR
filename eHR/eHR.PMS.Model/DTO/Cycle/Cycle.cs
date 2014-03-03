using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eHR.PMS.Model.DTO.Cycle
{
    public class Cycle
    {
        private int int_id;
        private string str_name;
        private PMS.Model.DTO.Master.Stage obj_stage;
        private PMS.Model.DTO.Master.Status obj_status;
        private PMS.Model.DTO.Core.Employee obj_starter;
        private DateTime? dt_started_timestamp;
        private List<PMS.Model.DTO.Cycle.Stage> lst_cycle_stages;
        private List<PMS.Model.DTO.Appraisal.Appraisal> lst_appraisals;
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

        public PMS.Model.DTO.Master.Stage Stage
        {
            get { return obj_stage; }
            set { obj_stage = value; }
        }

        public PMS.Model.DTO.Master.Status Status
        {
            get { return obj_status; }
            set { obj_status = value; }
        }

        public PMS.Model.DTO.Core.Employee Starter
        {
            get { return obj_starter; }
            set { obj_starter = value; }
        }

        public DateTime? StartedTimestamp
        {
            get { return dt_started_timestamp; }
            set { dt_started_timestamp = value; }
        }

        public List<PMS.Model.DTO.Cycle.Stage> CycleStages 
        {
            get { return lst_cycle_stages; }
            set { lst_cycle_stages = value; }
        }

        public void AddCycleStage(PMS.Model.DTO.Cycle.Stage cycleStage)
        {
            if (cycleStage != null)
            {
                if (Lib.Utility.Common.IsNullOrEmptyList(lst_cycle_stages))
                {
                    lst_cycle_stages = new List<Model.DTO.Cycle.Stage>();
                }
                lst_cycle_stages.Add(cycleStage);
            }
        }

        public List<PMS.Model.DTO.Appraisal.Appraisal> Appriasals
        {
            get { return lst_appraisals; }
            set { lst_appraisals = value; }
        }

        public void AddAppraisal(PMS.Model.DTO.Appraisal.Appraisal appraisal)
        {
            if (appraisal != null)
            {
                if (Lib.Utility.Common.IsNullOrEmptyList(lst_appraisals))
                {
                    lst_appraisals = new List<Appraisal.Appraisal>();
                }
                lst_appraisals.Add(appraisal);
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
