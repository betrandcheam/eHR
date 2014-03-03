using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eHR.PMS.Model.DTO.Appraisal
{
    public class PerformanceCoachingComment : PMS.Model.DTO.Appraisal.Comment
    {
        private Model.DTO.Appraisal.PerformanceCoaching obj_appraisal_performance_coaching;
        private int? int_item_id;

        public Model.DTO.Appraisal.PerformanceCoaching AppraisalPerformanceCoaching
        {
            get { return obj_appraisal_performance_coaching; }
            set { obj_appraisal_performance_coaching = value; }
        }

        public int? ItemId
        {
            get { return int_item_id; }
            set { int_item_id = value; }
        }
    }
}
