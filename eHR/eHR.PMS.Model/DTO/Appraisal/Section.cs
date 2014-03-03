using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eHR.PMS.Model.DTO.Appraisal
{
    public class Section : PMS.Model.DTO.Master.Section
    {
        private int int_id;
        private int? int_section_id;
        private PMS.Model.DTO.Appraisal.Appraisal obj_appraisal;
        private int? int_self_score;
        private int? int_level_1_score;
        private int? int_level_2_score;
        private int? int_final_score;
        private List<PMS.Model.DTO.Appraisal.SectionComment> lst_comments;
        private PMS.Model.DTO.Core.Employee obj_modifier;
        private DateTime? dt_modified_timestamp;

        public new int Id
        {
            get { return int_id; }
            set { int_id = value; }
        }

        public PMS.Model.DTO.Appraisal.Appraisal Appraisal
        {
            get { return obj_appraisal; }
            set { obj_appraisal = value; }
        }

        public int? SectionId
        {
            get { return int_section_id; }
            set { int_section_id = value; }
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

        public List<PMS.Model.DTO.Appraisal.SectionComment> Comments
        {
            get { return lst_comments; }
            set { lst_comments = value; }
        }

        public void AddComment(PMS.Model.DTO.Appraisal.SectionComment comment)
        {
            if (comment != null)
            {
                if (Lib.Utility.Common.IsNullOrEmptyList(lst_comments))
                {
                    lst_comments = new List<SectionComment>();
                }
                lst_comments.Add(comment);
            }
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
