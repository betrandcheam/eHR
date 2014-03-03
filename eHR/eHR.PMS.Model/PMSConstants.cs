using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eHR.PMS.Model
{
    public static class PMSConstants
    {
        public static readonly int MODULE_ID_PMS = 2;

        public static readonly int ROLE_ID_HR = 1;

        public static readonly int EMPLOYMENT_TYPE_ID_PERMANENT = 2;

        #region Stage

        public static readonly int STAGE_ID_PRE_CYCLE = 1;
        public static readonly int STAGE_ID_GOAL_SETTING = 2;
        public static readonly int STAGE_ID_PROGRESS_REVIEW = 3;
        public static readonly int STAGE_ID_FINAL_YEAR = 5;
        public static readonly int STAGE_ID_POST_CYCLE = 4;

        #endregion

        #region Status

        public static readonly int STATUS_ID_NEW = 1;
        public static readonly int STATUS_ID_DRAFT = 2;
        public static readonly int STATUS_ID_PENDING_LEVEL_1_APPROVAL = 3;
        public static readonly int STATUS_ID_PENDING_LEVEL_2_APPROVAL = 4;
        public static readonly int STATUS_ID_APPROVED = 5;
        public static readonly int STATUS_ID_REJECT = 6;
        public static readonly int STATUS_ID_OPEN = 7;
        public static readonly int STATUS_ID_CLOSE = 8;
        public static readonly int STATUS_ID_HR_ADMINISTERED = 9;

        public static readonly int STATUS_CORE_ID_OPEN = 1;
        public static readonly int STATUS_CORE_ID_COMPLETED = 2;

        #endregion

        #region Section

        public static readonly int SECTION_ID_KPI = 1;
        public static readonly int SECTION_ID_CORE_VALUES = 2;
        public static readonly int SECTION_ID_PERFORMANCE_COACHING = 3;
        public static readonly int SECTION_ID_CAREER_DEVELOPMENT = 4;
        
        #endregion

        #region Performance Coaching

        public static readonly int PERFORMANCE_COACHING_ITEM_STRENGTH_ID = 1;
        public static readonly int PERFORMANCE_COACHING_ITEM_IMPROVEMENT_ID = 2;

        #endregion Performance Coaching

        #region Career Development

        public static readonly int CAREER_DEVELOPMENT_ITEM_SHORT_TERM_GOAL_ID = 1;
        public static readonly int CAREER_DEVELOPMENT_ITEM_CAREER_PLAN_ID = 2;
        public static readonly int CAREER_DEVELOPMENT_ITEM_LEARNING_PLAN_ID = 3;

        #endregion Career Development

        #region Action

        public static readonly int ACTION_ID_APPRAISAL_CREATED = 1;
        public static readonly int ACTION_ID_APPRAISAL_SUBMITTED = 2;
        public static readonly int ACTION_ID_APPRAISAL_SAVED = 3;
        public static readonly int ACTION_ID_APPRAISAL_APPROVED_LEVEL_1 = 4;
        public static readonly int ACTION_ID_APPRAISAL_REJECTED_LEVEL_1 = 5;
        public static readonly int ACTION_ID_APPRAISAL_APPROVED_LEVEL_2 = 6;
        public static readonly int ACTION_ID_APPRAISAL_REJECTED_LEVEL_2 = 7;
        public static readonly int ACTION_ID_APPRAISAL_OPENED = 8;
        public static readonly int ACTION_ID_APPRAISAL_HR_ADMINISTERED = 9; 

        #endregion
    }
}
