using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eHR.PMS.Model.DTO.Appraisal
{
    public class SectionComment : PMS.Model.DTO.Appraisal.Comment
    {

        private Model.DTO.Appraisal.Section obj_appraisal_section;
        private Model.DTO.Master.Stage obj_appraisal_stage;

        public Model.DTO.Appraisal.Section AppraisalSection
        {
            get { return obj_appraisal_section; }
            set { obj_appraisal_section = value; }
        }

        public Model.DTO.Master.Stage Stage
        {
            get { return obj_appraisal_stage; }
            set { obj_appraisal_stage = value; }
        }
    }
}
