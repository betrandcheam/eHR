using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eHR.PMS.Model.DTO.Appraisal
{
    public class Appraisal
    {
        private int int_id;
        private PMS.Model.DTO.Cycle.Cycle obj_cycle;
        private PMS.Model.DTO.Master.Stage obj_stage;
        private List<PMS.Model.DTO.Appraisal.Stage> lst_appraisal_stages;
        private List<PMS.Model.DTO.Appraisal.Section> lst_appraisal_sections;
        private PMS.Model.DTO.Master.Status obj_status;
        private PMS.Model.DTO.Core.Employee obj_employee;
        private PMS.Model.DTO.Core.Master.Department obj_department;
        private List<PMS.Model.DTO.Appraisal.Approver> lst_approvers;
        private List<PMS.Model.DTO.Appraisal.Reviewer> lst_reviewers;
        private List<PMS.Model.DTO.Appraisal.KPI> lst_kpi;
        private List<PMS.Model.DTO.Appraisal.CoreValue> lst_core_values;
        private List<PMS.Model.DTO.Appraisal.PerformanceCoaching> lst_performance_coachings;
        private List<PMS.Model.DTO.Appraisal.CareerDevelopment> lst_career_developments;
        private List<PMS.Model.DTO.Appraisal.Trail> lst_appraisal_trails;
        private PMS.Model.DTO.Core.Employee obj_modifier;
        private DateTime? dt_modified_timestamp;
        private bool? boo_locked;
        private PMS.Model.DTO.Core.Task.Task obj_task;

        public int Id
        {
            get { return int_id; }
            set { int_id = value; }
        }

        public PMS.Model.DTO.Cycle.Cycle Cycle
        {
            get { return obj_cycle; }
            set { obj_cycle = value; }
        }

        public PMS.Model.DTO.Master.Stage Stage
        {
            get { return obj_stage; }
            set { obj_stage = value; }
        }

        public List<PMS.Model.DTO.Appraisal.Stage> AppraisalStages
        {
            get { return lst_appraisal_stages; }
            set { lst_appraisal_stages = value; }
        }

        public void AddAppraisalStage(PMS.Model.DTO.Appraisal.Stage appraisalStage)
        {
            if (appraisalStage != null)
            {
                if (Lib.Utility.Common.IsNullOrEmptyList(lst_appraisal_stages))
                {
                    lst_appraisal_stages = new List<Stage>();
                }
                lst_appraisal_stages.Add(appraisalStage);
            }
        }

        public List<PMS.Model.DTO.Appraisal.Section> AppraisalSections
        {
            get { return lst_appraisal_sections; }
            set { lst_appraisal_sections = value; }
        }

        public void AddAppraisalSection(PMS.Model.DTO.Appraisal.Section appraisalSection)
        {
            if (appraisalSection != null)
            {
                if (Lib.Utility.Common.IsNullOrEmptyList(lst_appraisal_stages))
                {
                    lst_appraisal_sections = new List<Section>();
                }
                lst_appraisal_sections.Add(appraisalSection);
            }
        }

        public DateTime? ReviewPeriodStartDate
        {
            get 
            {
                if (!Lib.Utility.Common.IsNullOrEmptyList(lst_appraisal_stages))
                {
                    return lst_appraisal_stages.OrderBy(b => b.StartDate).First().StartDate;
                }
                else
                {
                    return null;
                }
            }
        }

        public DateTime? ReviewPeriodEndDate
        {
            get
            {
                if (!Lib.Utility.Common.IsNullOrEmptyList(lst_appraisal_stages))
                {
                    return lst_appraisal_stages.OrderByDescending(b => b.EndDate).First().EndDate;
                }
                else
                {
                    return null;
                }
            }
        }

        public PMS.Model.DTO.Master.Status Status
        {
            get { return obj_status; }
            set { obj_status = value; }
        }

        public PMS.Model.DTO.Core.Employee Employee
        {
            get { return obj_employee; }
            set { obj_employee = value; }
        }

        public PMS.Model.DTO.Core.Master.Department Department
        {
            get { return obj_department; }
            set { obj_department = value; }
        }

        public List<PMS.Model.DTO.Appraisal.Approver> Approvers
        {
            get { return lst_approvers; }
            set { lst_approvers = value; }
        }

        public void AddApprover(PMS.Model.DTO.Appraisal.Approver approver)
        {
            if (approver != null)
            {
                if (Lib.Utility.Common.IsNullOrEmptyList(lst_approvers))
                {
                    lst_approvers = new List<Approver>();
                }
                lst_approvers.Add(approver);
            }
        }

        public PMS.Model.DTO.Appraisal.Approver GetApproverByLevel(int level)
        {
            PMS.Model.DTO.Appraisal.Approver obj_approver = null;
            if (!Lib.Utility.Common.IsNullOrEmptyList(lst_approvers))
            {
                var approvers = lst_approvers.Where(a => a.ApprovalLevel == level);
                if(!Lib.Utility.Common.IsNullOrEmptyList(approvers))
                {
                    obj_approver = approvers.First();
                }
            }
            return obj_approver;
        }

        public string ApproverLevelInString(int approverEmployeeId)
        {
            StringBuilder sb_levels = new StringBuilder();
            int int_counter = 0;
            if (!Lib.Utility.Common.IsNullOrEmptyList(lst_approvers))
            {
                List<Int32> lst_levels = (from appr in lst_approvers
                                         where appr.EmployeeId == approverEmployeeId
                                         orderby appr.ApprovalLevel
                                         select Convert.ToInt32(appr.ApprovalLevel)).ToList();

                foreach (int level in lst_levels)
                {
                    sb_levels.Append(level.ToString());
                    if (int_counter != lst_levels.Count() - 1)
                    {
                        sb_levels.Append(", ");
                    }
                    int_counter++;
                }
            }
            return sb_levels.ToString();
        }

        public List<PMS.Model.DTO.Appraisal.Reviewer> Reviewers
        {
            get { return lst_reviewers; }
            set { lst_reviewers = value; }
        }

        public string ReviewersNamesInString
        {
            get
            {
                StringBuilder sb_names = new StringBuilder();
                int int_counter = 0;
                if (!Lib.Utility.Common.IsNullOrEmptyList(lst_reviewers))
                {
                    foreach (PMS.Model.DTO.Appraisal.Reviewer obj_reviewer in lst_reviewers)
                    {
                        sb_names.Append(obj_reviewer.PreferredName);
                        if (int_counter != lst_reviewers.Count() - 1)
                        {
                            sb_names.Append(", ");
                        }
                        int_counter++;
                    }
                }
                return sb_names.ToString();
            }

        }

        public void AddCycleStage(PMS.Model.DTO.Appraisal.Reviewer reviewer)
        {
            if (reviewer != null)
            {
                if (Lib.Utility.Common.IsNullOrEmptyList(lst_reviewers))
                {
                    lst_reviewers = new List<Reviewer>();
                }
                lst_reviewers.Add(reviewer);
            }
        }

        public List<PMS.Model.DTO.Appraisal.KPI> KPIs
        {
            get { return lst_kpi; }
            set { lst_kpi = value; }
        }

        public void AddKPI(PMS.Model.DTO.Appraisal.KPI kpi)
        {
            if (kpi != null)
            {
                if (Lib.Utility.Common.IsNullOrEmptyList(lst_kpi))
                {
                    lst_kpi = new List<KPI>();
                }
                lst_kpi.Add(kpi);
            }
        }

        public List<PMS.Model.DTO.Appraisal.CoreValue> CoreValues
        {
            get { return lst_core_values; }
            set { lst_core_values = value; }
        }

        public void AddCoreValue(PMS.Model.DTO.Appraisal.CoreValue coreValue)
        {
            if (coreValue != null)
            {
                if (Lib.Utility.Common.IsNullOrEmptyList(lst_core_values))
                {
                    lst_core_values = new List<CoreValue>();
                }
                lst_core_values.Add(coreValue);
            }
        }

        public List<PMS.Model.DTO.Appraisal.PerformanceCoaching> PerformanceCoachings
        {
            get { return lst_performance_coachings; }
            set { lst_performance_coachings = value; }
        }

        public PMS.Model.DTO.Appraisal.PerformanceCoaching PerformanceCoaching
        {
            get 
            {
                if (!Lib.Utility.Common.IsNullOrEmptyList(lst_performance_coachings))
                {
                    return lst_performance_coachings.First();
                }
                else 
                {
                    return null;
                }
            }
        }

        public List<PMS.Model.DTO.Appraisal.CareerDevelopment> CareerDevelopments
        {
            get { return lst_career_developments; }
            set { lst_career_developments = value; }
        }

        public PMS.Model.DTO.Appraisal.CareerDevelopment CareerDevelopment
        {
            get
            {
                if (!Lib.Utility.Common.IsNullOrEmptyList(lst_career_developments))
                {
                    return lst_career_developments.First();
                }
                else
                {
                    return null;
                }
            }
        }

        public List<PMS.Model.DTO.Appraisal.Trail> Trails
        {
            get { return lst_appraisal_trails; }
            set { lst_appraisal_trails = value; }
        }

        public void AddTrail(PMS.Model.DTO.Appraisal.Trail trail)
        {
            if (trail != null)
            {
                if (Lib.Utility.Common.IsNullOrEmptyList(lst_appraisal_trails))
                {
                    lst_appraisal_trails = new List<Trail>();
                }
                lst_appraisal_trails.Add(trail);
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

        public bool? Locked
        {
            get { return boo_locked; }
            set { boo_locked = value; }
        }

        public Model.DTO.Core.Task.Task Task
        {
            get { return obj_task; }
            set { obj_task = value; }
        }
    }
}
