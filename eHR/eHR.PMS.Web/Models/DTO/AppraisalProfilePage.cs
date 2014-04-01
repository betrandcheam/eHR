using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eHR.PMS.Web.Models.DTO
{
    public class AppraisalProfilePage
    {
        private PMS.Model.DTO.Core.Security.User obj_user;
        private PMS.Model.DTO.Appraisal.Appraisal obj_appraisal;

        public PMS.Model.DTO.Core.Security.User User
        {
            get { return obj_user; }
            set { obj_user = value; }
        }

        public PMS.Model.DTO.Appraisal.Appraisal Appraisal
        {
            get { return obj_appraisal; }
            set { obj_appraisal = value; }
        }

        public string GetCycleStartDate()
        {
            Model.DTO.Cycle.Stage obj_stage = Appraisal.Cycle.CycleStages.Where(rec => rec.StageId == eHR.PMS.Model.PMSConstants.STAGE_ID_GOAL_SETTING).SingleOrDefault();
            return Convert.ToDateTime(obj_stage.StartDate).ToString("dd/MM/yyyy");
        }

        public string GetCycleEndDate()
        {
            Model.DTO.Cycle.Stage obj_stage = Appraisal.Cycle.CycleStages.Where(rec => rec.StageId == eHR.PMS.Model.PMSConstants.STAGE_ID_FINAL_YEAR).SingleOrDefault();
            return Convert.ToDateTime(obj_stage.StartDate).ToString("dd/MM/yyyy");
        }

        public string GetAppraisalStageStartDate(int stageId)
        {
            Model.DTO.Appraisal.Stage obj_stage = Appraisal.AppraisalStages.Where(rec => rec.StageId == stageId).SingleOrDefault();
            if (obj_stage == null)
            {
                return null;
            }
            else
            {
                return Convert.ToDateTime(obj_stage.StartDate).ToString("dd/MM/yyyy");
            } 
        }

        public string GetAppraisalStageEndDate(int stageId)
        {
            Model.DTO.Appraisal.Stage obj_stage = Appraisal.AppraisalStages.Where(rec => rec.StageId == stageId).SingleOrDefault();
            if (obj_stage == null)
            {
                return null;
            }
            else
            {
                return Convert.ToDateTime(obj_stage.EndDate).ToString("dd/MM/yyyy");
            }
        }

        private List<Model.DTO.Appraisal.Reviewer> GetReviewers()
        {
            List<Model.DTO.Appraisal.Reviewer> lst_reviewers = null;
            if (!Lib.Utility.Common.IsNullOrEmptyList(Appraisal.Reviewers))
            {
                var var_reviewers = Appraisal.Reviewers.Where(rec => rec.SMT == false);

                if (!Lib.Utility.Common.IsNullOrEmptyList(var_reviewers))
                {
                    lst_reviewers = var_reviewers.ToList();
                }
            }
            return lst_reviewers;
        }

        public string GetAppraisalFirstReviewerName()
        {
            string str_name = null;
            List<Model.DTO.Appraisal.Reviewer> lst_reviewers = GetReviewers();

            if (!Lib.Utility.Common.IsNullOrEmptyList(lst_reviewers))
            {
                if (lst_reviewers.Count() > 0)
                {
                    str_name = lst_reviewers[0].PreferredName;
                }
            }
            return str_name;
        }

        public string GetAppraisalFirstReviewerDomainId()
        {
            string str_domain_id = null;
            List<Model.DTO.Appraisal.Reviewer> lst_reviewers = GetReviewers();

            if (!Lib.Utility.Common.IsNullOrEmptyList(lst_reviewers))
            {
                if (lst_reviewers.Count() > 0)
                {
                    str_domain_id = lst_reviewers[0].DomainId;
                }
            }
            return str_domain_id;
        }
        public string GetAppraisalFirstReviewerId()
        {
            string str_id = "";
            List<Model.DTO.Appraisal.Reviewer> lst_reviewers = GetReviewers();
            if (!Lib.Utility.Common.IsNullOrEmptyList(lst_reviewers))
            {
                if (lst_reviewers.Count() > 0)
                {
                    str_id = lst_reviewers[0].EmployeeId.ToString();
                }
            }
            return str_id;
        }
        public string GetAppraisalSecondReviewerName()
        {
            string str_name = null;
            List<Model.DTO.Appraisal.Reviewer> lst_reviewers = GetReviewers();

            if (!Lib.Utility.Common.IsNullOrEmptyList(lst_reviewers))
            {
                if (lst_reviewers.Count() > 1)
                {
                    str_name = lst_reviewers[1].PreferredName;
                }
            }
            return str_name;
        }

        public string GetAppraisalSecondReviewerDomainId()
        {
            string str_domain_id = null;
            List<Model.DTO.Appraisal.Reviewer> lst_reviewers = GetReviewers();

            if (!Lib.Utility.Common.IsNullOrEmptyList(lst_reviewers))
            {
                if (lst_reviewers.Count() > 1)
                {
                    str_domain_id = lst_reviewers[1].DomainId;
                }
            }
            return str_domain_id;
        }

        public string GetAppraisalSecondReviewerId()
        {
            string str_id = "";
            List<Model.DTO.Appraisal.Reviewer> lst_reviewers = GetReviewers();
            if (!Lib.Utility.Common.IsNullOrEmptyList(lst_reviewers))
            {
                if (lst_reviewers.Count() > 1)
                {
                    str_id = lst_reviewers[1].EmployeeId.ToString();
                }
            }
            return str_id;
        }

        public string GetSeniorManagementTeamMemberName()
        {
            string str_name = null;
            if (!Lib.Utility.Common.IsNullOrEmptyList(Appraisal.Reviewers))
            {
                Model.DTO.Appraisal.Reviewer obj_reviewer = Appraisal.Reviewers.Where(rec => rec.SMT == true).SingleOrDefault();

                if (obj_reviewer != null)
                {
                    str_name = obj_reviewer.PreferredName;
                }
            }
            return str_name;
        }


        public string GetSeniorManagementTeamMemberDomainId()
        {
            string str_domain_id = null;
            if (!Lib.Utility.Common.IsNullOrEmptyList(Appraisal.Reviewers))
            {
                Model.DTO.Appraisal.Reviewer obj_reviewer = Appraisal.Reviewers.Where(rec => rec.SMT == true).SingleOrDefault();

                if (obj_reviewer != null)
                {
                    str_domain_id = obj_reviewer.DomainId;
                }
            }
            return str_domain_id;
        }

        public string GetSeniorManagementTeamMemberId()
        {
            string str_id = null;
            if (!Lib.Utility.Common.IsNullOrEmptyList(Appraisal.Reviewers))
            {
                Model.DTO.Appraisal.Reviewer obj_reviewer = Appraisal.Reviewers.Where(rec => rec.SMT == true).SingleOrDefault();

                if (obj_reviewer != null)
                {
                    str_id = Convert.ToString(obj_reviewer.EmployeeId);
                }
            }
            return str_id;
        }
    }
}