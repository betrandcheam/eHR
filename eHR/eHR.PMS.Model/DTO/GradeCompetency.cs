using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eHR.PMS.Model.DTO
{
    public class GradeCompetency : PMS.Model.DTO.Master.CoreValueCompetency
    {
        private int int_id;
        private int? int_competency_id;
        private PMS.Model.DTO.Master.Block obj_block;
        private PMS.Model.DTO.Core.Master.Grade obj_grade;
        private PMS.Model.DTO.Core.Employee obj_modifier;
        private DateTime? dt_modified_timestamp;

        public new int Id
        {
            get { return int_id; }
            set { int_id = value; }
        }

        public int? CoreValueCompetencyId
        {
            get { return int_competency_id; }
            set { int_competency_id = value; }
        }

        public PMS.Model.DTO.Master.Block Block
        {
            get { return obj_block; }
            set { obj_block = value; }
        }

        public PMS.Model.DTO.Core.Master.Grade Grade
        {
            get { return obj_grade; }
            set { obj_grade = value; }
        }

        public new PMS.Model.DTO.Core.Employee Modifier
        {
            get { return obj_modifier; }
            set { obj_modifier = value; }
        }

        public new DateTime? ModifiedTimestamp
        {
            get { return dt_modified_timestamp; }
            set { dt_modified_timestamp = value; }
        }
    }
}
