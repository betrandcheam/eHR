using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eHR.PMS.Model.DTO.Appraisal
{
    public class Comment
    {
        private int int_id;
        private string str_comment;
        private bool? boo_form_save_only;
        private PMS.Model.DTO.Core.Employee obj_commentor;
        private DateTime? dt_commented_timestamp;
        private PMS.Model.DTO.Core.Employee obj_modifier;
        private DateTime? dt_modified_timestamp;

        public int Id
        {
            get { return int_id; }
            set { int_id = value; }
        }

        public string Comments
        {
            get { return str_comment; }
            set { str_comment = value; }
        }

        public bool? FormSaveOnly
        {
            get { return boo_form_save_only; }
            set { boo_form_save_only = value; }
        }

        public PMS.Model.DTO.Core.Employee Commentor
        {
            get { return obj_commentor; }
            set { obj_commentor = value; }
        }

        public DateTime? CommentedTimestamp
        {
            get { return dt_commented_timestamp; }
            set { dt_commented_timestamp = value; }
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
