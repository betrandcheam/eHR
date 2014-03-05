using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eHR.PMS.Web.Models.DTO
{
    public class NewCycleManagementPage
    {
        private PMS.Model.DTO.Cycle.Cycle obj_cycle;
        private List<PMS.Model.DTO.Master.Stage> lst_stages;
        private List<PMS.Model.DTO.Core.Employee> lst_participants;
        private string str_eligibility_range_start_date;
        private string str_eligibility_range_end_date;
        private List<PMS.Model.DTO.Cycle.Cycle> lst_cycles;

        public PMS.Model.DTO.Cycle.Cycle CurrentCycle
        {
            get { return obj_cycle; }
            set { obj_cycle = value; }
        }
        public List<PMS.Model.DTO.Cycle.Cycle> Cycles
        {
            get { return lst_cycles; }
            set { lst_cycles = value; }
        }

        public List<PMS.Model.DTO.Master.Stage> Stages
        {
            get { return lst_stages; }
            set { lst_stages = value; }
        }

        public List<PMS.Model.DTO.Core.Employee> Participants
        {
            get { return lst_participants; }
            set { lst_participants = value; }
        }

        public int GetNumberOfParticipants()
        {
            if (!Lib.Utility.Common.IsNullOrEmptyList(lst_participants))
            {
                return lst_participants.Count();
            }
            else
            {
                return 0;
            }
        }

        public int GetNumberOfParticipantsWithNoLevelOfApproval()
        {
            int int_approvers_count = 0;
            IEnumerable<PMS.Model.DTO.Core.Employee> lst_tmp_participants;
            if (!Lib.Utility.Common.IsNullOrEmptyList(lst_participants))
            {
                lst_tmp_participants = lst_participants.Where(rec => rec.Level1Approver == null && rec.Level2Approver == null);
                if (!Lib.Utility.Common.IsNullOrEmptyList(lst_tmp_participants))
                {
                    int_approvers_count = lst_tmp_participants.Count();
                }
            }
            return int_approvers_count;
        }

        public int GetNumberOfParticipantsWithOnlyOneLevelOfApproval()
        {
            int int_approvers_count = 0;
            IEnumerable<PMS.Model.DTO.Core.Employee> lst_tmp_participants;
            if (!Lib.Utility.Common.IsNullOrEmptyList(lst_participants))
            {
                lst_tmp_participants = lst_participants.Where(rec => rec.Level1Approver != null && rec.Level2Approver == null);
                if (!Lib.Utility.Common.IsNullOrEmptyList(lst_tmp_participants))
                {
                    int_approvers_count = lst_tmp_participants.Count();
                }
            }
            return int_approvers_count;
        }

        public int GetNumberOfParticipantsWithTwoLevelsOfApproval()
        {
            int int_approvers_count = 0;
            IEnumerable<PMS.Model.DTO.Core.Employee> lst_tmp_participants;
            if (!Lib.Utility.Common.IsNullOrEmptyList(lst_participants))
            {
                lst_tmp_participants = lst_participants.Where(rec => rec.Level2Approver != null && rec.Level2Approver != null);
                if (!Lib.Utility.Common.IsNullOrEmptyList(lst_tmp_participants))
                {
                    int_approvers_count = lst_tmp_participants.Count();
                }
            }
            return int_approvers_count;
        }

        public string EligibilityRangeStartDateString
        {
            get { return str_eligibility_range_start_date; }
            set { str_eligibility_range_start_date = value; }
        }

        public string EligibilityRangeEndDateString
        {
            get { return str_eligibility_range_end_date; }
            set { str_eligibility_range_end_date = value; }
        }
    }
    public class CycleManagementPage
    {
        private PMS.Model.DTO.Cycle.Cycle obj_cycle;
        private List<PMS.Model.DTO.Master.Stage> lst_stages;
        private List<PMS.Model.DTO.Appraisal.Appraisal> lst_participants;
        private string str_eligibility_range_start_date;
        private string str_eligibility_range_end_date;
        private List<PMS.Model.DTO.Cycle.Cycle> lst_cycles;

        public PMS.Model.DTO.Cycle.Cycle CurrentCycle
        {
            get { return obj_cycle; }
            set { obj_cycle = value; }
        }
        public List<PMS.Model.DTO.Cycle.Cycle> Cycles
        {
            get { return lst_cycles; }
            set { lst_cycles = value; }
        }

        public List<PMS.Model.DTO.Master.Stage> Stages
        {
            get { return lst_stages; }
            set { lst_stages = value; }
        }

        public List<PMS.Model.DTO.Appraisal.Appraisal> Participants
        {
            get { return lst_participants; }
            set { lst_participants = value; }
        }

        public int GetNumberOfParticipants()
        {
            if (!Lib.Utility.Common.IsNullOrEmptyList(lst_participants))
            {
                return lst_participants.Count();
            }
            else
            {
                return 0;
            }
        }

        public int GetNumberOfParticipantsWithNoLevelOfApproval()
        {
            int int_approvers_count = 0;
            IEnumerable<PMS.Model.DTO.Appraisal.Appraisal> lst_tmp_participants;
            if (!Lib.Utility.Common.IsNullOrEmptyList(lst_participants))
            {
                lst_tmp_participants = lst_participants.Where(rec => rec.Employee.Level1Approver == null && rec.Employee.Level2Approver == null);
                if (!Lib.Utility.Common.IsNullOrEmptyList(lst_tmp_participants))
                {
                    int_approvers_count = lst_tmp_participants.Count();
                }
            }
            return int_approvers_count;
        }

        public int GetNumberOfParticipantsWithOnlyOneLevelOfApproval()
        {
            int int_approvers_count = 0;
            IEnumerable<PMS.Model.DTO.Appraisal.Appraisal> lst_tmp_participants;
            if (!Lib.Utility.Common.IsNullOrEmptyList(lst_participants))
            {
                lst_tmp_participants = lst_participants.Where(rec => rec.Employee.Level1Approver != null && rec.Employee.Level2Approver == null);
                if (!Lib.Utility.Common.IsNullOrEmptyList(lst_tmp_participants))
                {
                    int_approvers_count = lst_tmp_participants.Count();
                }
            }
            return int_approvers_count;
        }

        public int GetNumberOfParticipantsWithTwoLevelsOfApproval()
        {
            int int_approvers_count = 0;
            IEnumerable<PMS.Model.DTO.Appraisal.Appraisal> lst_tmp_participants;
            if (!Lib.Utility.Common.IsNullOrEmptyList(lst_participants))
            {
                lst_tmp_participants = lst_participants.Where(rec => rec.Employee.Level2Approver != null && rec.Employee.Level2Approver != null);
                if (!Lib.Utility.Common.IsNullOrEmptyList(lst_tmp_participants))
                {
                    int_approvers_count = lst_tmp_participants.Count();
                }
            }
            return int_approvers_count;
        }

        public string EligibilityRangeStartDateString
        {
            get { return str_eligibility_range_start_date; }
            set { str_eligibility_range_start_date = value; }
        }

        public string EligibilityRangeEndDateString
        {
            get { return str_eligibility_range_end_date; }
            set { str_eligibility_range_end_date = value; }
        }
    }
}