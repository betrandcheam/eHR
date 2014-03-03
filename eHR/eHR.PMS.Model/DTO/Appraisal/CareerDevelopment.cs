using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eHR.PMS.Model.DTO.Appraisal
{
    public class CareerDevelopment
    {
        private int int_id;
        private PMS.Model.DTO.Appraisal.Appraisal obj_appraisal;
        private PMS.Model.DTO.Master.Section obj_section;
        private string str_short_term_goals;
        private string str_career_plans;
        private string str_learning_plans;
        private List<PMS.Model.DTO.Appraisal.CareerDevelopmentComment> lst_comments;
        private PMS.Model.DTO.Core.Employee obj_modifier;
        private DateTime? dt_modified_timestamp;

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

        public Model.DTO.Master.Section Section
        {
            get { return obj_section; }
            set { obj_section = value; }
        }

        public string ShortTermGoals
        {
            get { return str_short_term_goals; }
            set { str_short_term_goals = value; }
        }

        public string CareerPlans
        {
            get { return str_career_plans; }
            set { str_career_plans = value; }
        }

        public string LearningPlans
        {
            get { return str_learning_plans; }
            set { str_learning_plans = value; }
        }

        public List<PMS.Model.DTO.Appraisal.CareerDevelopmentComment> Comments
        {
            get { return lst_comments; }
            set { lst_comments = value; }
        }

        public List<PMS.Model.DTO.Appraisal.CareerDevelopmentComment> ShortTermGoalsPlanComments
        {
            get
            {
                List<PMS.Model.DTO.Appraisal.CareerDevelopmentComment> lst_tmp_comments = null;
                if (!Lib.Utility.Common.IsNullOrEmptyList(lst_comments.Where(rec => rec.ItemId == PMSConstants.CAREER_DEVELOPMENT_ITEM_SHORT_TERM_GOAL_ID)))
                {
                    lst_tmp_comments = lst_comments.Where(rec => rec.ItemId == PMSConstants.CAREER_DEVELOPMENT_ITEM_SHORT_TERM_GOAL_ID).ToList();
                }

                return lst_tmp_comments;
            }
        }

        public List<PMS.Model.DTO.Appraisal.CareerDevelopmentComment> CareerPlanComments
        {
            get
            {
                List<PMS.Model.DTO.Appraisal.CareerDevelopmentComment> lst_tmp_comments = null;
                if (!Lib.Utility.Common.IsNullOrEmptyList(lst_comments.Where(rec => rec.ItemId == PMSConstants.CAREER_DEVELOPMENT_ITEM_CAREER_PLAN_ID)))
                {
                    lst_tmp_comments = lst_comments.Where(rec => rec.ItemId == PMSConstants.CAREER_DEVELOPMENT_ITEM_CAREER_PLAN_ID).ToList();
                }

                return lst_tmp_comments;
            }
        }

        public List<PMS.Model.DTO.Appraisal.CareerDevelopmentComment> LearningPlanComments
        {
            get
            {
                List<PMS.Model.DTO.Appraisal.CareerDevelopmentComment> lst_tmp_comments = null;
                if (!Lib.Utility.Common.IsNullOrEmptyList(lst_comments.Where(rec => rec.ItemId == PMSConstants.CAREER_DEVELOPMENT_ITEM_LEARNING_PLAN_ID)))
                {
                    lst_tmp_comments = lst_comments.Where(rec => rec.ItemId == PMSConstants.CAREER_DEVELOPMENT_ITEM_LEARNING_PLAN_ID).ToList();
                }

                return lst_tmp_comments;
            }
        }

        public void AddComment(PMS.Model.DTO.Appraisal.CareerDevelopmentComment comment)
        {
            if (comment != null)
            {
                if (Lib.Utility.Common.IsNullOrEmptyList(lst_comments))
                {
                    lst_comments = new List<CareerDevelopmentComment>();
                }
                lst_comments.Add(comment);
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
