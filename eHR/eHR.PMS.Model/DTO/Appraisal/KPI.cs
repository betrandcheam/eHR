using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eHR.PMS.Model.DTO.Appraisal
{
    public class KPI
    {
        private int int_id;
        private PMS.Model.DTO.Appraisal.Appraisal obj_appraisal;
        private PMS.Model.DTO.Master.Section obj_section;
        private PMS.Model.DTO.Master.Block obj_block;
        private string str_description;
        private string str_target;
        private PMS.Model.DTO.Master.Priority obj_priority;
        private int? int_self_score;
        private int? int_level_1_score;
        private int? int_level_2_score;
        private int? int_final_score;
        private List<PMS.Model.DTO.Appraisal.KPIComment> lst_comments;
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

        public Model.DTO.Master.Block Block
        {
            get { return obj_block; }
            set { obj_block = value; }
        }

        public string Description
        {
            get { return str_description; }
            set { str_description = value; }
        }

        public string Target
        {
            get { return str_target; }
            set { str_target = value; }
        }

        public Model.DTO.Master.Priority Priority
        {
            get { return obj_priority; }
            set { obj_priority = value; }
        }

        public int? SelfScore
        {
            get { return int_self_score; }
            set { int_self_score = value; }
        }

        public int? Level1Score
        {
            get { return int_level_1_score; }
            set { int_level_1_score = value; }
        }

        public int? Level2Score
        {
            get { return int_level_2_score; }
            set { int_level_2_score = value; }
        }

        public int? FinalScore
        {
            get { return int_final_score; }
            set { int_final_score = value; }
        }

        public List<PMS.Model.DTO.Appraisal.KPIComment> Comments
        {
            get { return lst_comments; }
            set { lst_comments = value; }
        }

        public void AddComment(PMS.Model.DTO.Appraisal.KPIComment comment)
        {
            if (comment != null)
            {
                if (Lib.Utility.Common.IsNullOrEmptyList(lst_comments))
                {
                    lst_comments = new List<KPIComment>();
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
