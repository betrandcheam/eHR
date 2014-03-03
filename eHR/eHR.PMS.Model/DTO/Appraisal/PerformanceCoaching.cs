using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eHR.PMS.Model.DTO.Appraisal
{
    public class PerformanceCoaching
    {
        private int int_id;
        private PMS.Model.DTO.Appraisal.Appraisal obj_appraisal;
        private PMS.Model.DTO.Master.Section obj_section;
        private string str_strength_area;
        private string str_improvement_area;
        private List<PMS.Model.DTO.Appraisal.PerformanceCoachingComment> lst_comments;
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

        public string AreasOfStrength
        {
            get { return str_strength_area; }
            set { str_strength_area = value; }
        }

        public string AreasOfImprovement
        {
            get { return str_improvement_area; }
            set { str_improvement_area = value; }
        }

        public List<PMS.Model.DTO.Appraisal.PerformanceCoachingComment> Comments
        {
            get { return lst_comments; }
            set { lst_comments = value; }
        }

        public List<PMS.Model.DTO.Appraisal.PerformanceCoachingComment> StrengthAreasComments
        {
            get 
            {
                List<PMS.Model.DTO.Appraisal.PerformanceCoachingComment> lst_tmp_comments = null;
                if (!Lib.Utility.Common.IsNullOrEmptyList(lst_comments.Where(rec => rec.ItemId == PMSConstants.PERFORMANCE_COACHING_ITEM_STRENGTH_ID)))
                {
                    lst_tmp_comments = lst_comments.Where(rec => rec.ItemId == PMSConstants.PERFORMANCE_COACHING_ITEM_STRENGTH_ID).ToList();
                }

                return lst_tmp_comments; 
            }
        }

        public List<PMS.Model.DTO.Appraisal.PerformanceCoachingComment> ImprovementAreasComments
        {
            get
            {
                List<PMS.Model.DTO.Appraisal.PerformanceCoachingComment> lst_tmp_comments = null;
                if (!Lib.Utility.Common.IsNullOrEmptyList(lst_comments.Where(rec => rec.ItemId == PMSConstants.PERFORMANCE_COACHING_ITEM_IMPROVEMENT_ID)))
                {
                    lst_tmp_comments = lst_comments.Where(rec => rec.ItemId == PMSConstants.PERFORMANCE_COACHING_ITEM_IMPROVEMENT_ID).ToList();
                }

                return lst_tmp_comments;
            }
        }

        public void AddComment(PMS.Model.DTO.Appraisal.PerformanceCoachingComment comment)
        {
            if (comment != null)
            {
                if (Lib.Utility.Common.IsNullOrEmptyList(lst_comments))
                {
                    lst_comments = new List<PerformanceCoachingComment>();
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
