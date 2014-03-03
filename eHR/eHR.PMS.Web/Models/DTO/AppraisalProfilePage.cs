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

        public string GetAppraisalFirstReviewerName()
        {
            string str_name = null;
            if (!Lib.Utility.Common.IsNullOrEmptyList(Appraisal.Reviewers))
            {
                if (Appraisal.Reviewers.Count() > 0)
                {
                    str_name = Appraisal.Reviewers[0].PreferredName;
                }
            }
            return str_name;
        }

        public string GetAppraisalSecondReviewerName()
        {
            string str_name = null;
            if (!Lib.Utility.Common.IsNullOrEmptyList(Appraisal.Reviewers))
            {
                if (Appraisal.Reviewers.Count() > 1)
                {
                    str_name = Appraisal.Reviewers[1].PreferredName;
                }
            }
            return str_name;
        }
    }
}