using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eHR.PMS.Model.DTO.Appraisal
{
    public class CareerDevelopmentComment : PMS.Model.DTO.Appraisal.Comment
    {
        private Model.DTO.Appraisal.CareerDevelopment obj_appraisal_career_development;
        private int? int_item_id;

        public Model.DTO.Appraisal.CareerDevelopment AppraisalCareerDevelopment
        {
            get { return obj_appraisal_career_development; }
            set { obj_appraisal_career_development = value; }
        }

        public int? ItemId
        {
            get { return int_item_id; }
            set { int_item_id = value; }
        }
    }
}
