using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eHR.PMS.Model.DTO.Appraisal
{
    public class CoreValueComment : Model.DTO.Appraisal.Comment
    {
        private Model.DTO.Appraisal.CoreValue obj_appraisal_core_value;

        public Model.DTO.Appraisal.CoreValue AppraisalCoreValue
        {
            get { return obj_appraisal_core_value; }
            set { obj_appraisal_core_value = value; }
        }
    }
}
