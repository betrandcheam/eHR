using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eHR.PMS.Model.DTO.Appraisal
{
    public class KPIComment : Model.DTO.Appraisal.Comment
    {
        private Model.DTO.Appraisal.KPI obj_appraisal_kpi;

        public Model.DTO.Appraisal.KPI AppraisalKPI
        {
            get { return obj_appraisal_kpi; }
            set { obj_appraisal_kpi = value; }
        }
    }
}
