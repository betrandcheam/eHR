using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace eHR.PMS.Web.Models.DTO
{
    public class AppraisalPage
    {
        private PMS.Model.DTO.Core.Security.User obj_user;
        private PMS.Model.DTO.Core.Task.Task obj_task;
        private int? int_task_id;
        private PMS.Model.DTO.Master.Section obj_current_section;
        private List<PMS.Model.DTO.Master.Section> lst_sections;
        private List<Model.DTO.Master.Priority> lst_priorities;
        private List<Model.DTO.GradeCompetency> lst_core_value_competencies;
        private Model.DTO.Appraisal.Appraisal obj_appraisal;
        private bool boo_view_only;

        public PMS.Model.DTO.Core.Security.User User
        {
            get { return obj_user; }
            set { obj_user = value; }
        }

        public PMS.Model.DTO.Core.Task.Task Task
        {
            get { return obj_task; }
            set { obj_task = value; }
        }

        public int? CurrentTaskId
        {
            get { return int_task_id; }
            set { int_task_id = value; }
        }

        public PMS.Model.DTO.Master.Section CurrentSection
        {
            get { return obj_current_section; }
            set { obj_current_section = value; }
        }

        public List<PMS.Model.DTO.Master.Section> Sections
        {
            get { return lst_sections; }
            set { lst_sections = value; }
        }

        public List<Model.DTO.Master.Priority> Priorities
        {
            get { return lst_priorities; }
            set { lst_priorities = value; }
        }

        public List<Model.DTO.GradeCompetency> CoreValueCompetencies
        {
            get { return lst_core_value_competencies; }
            set { lst_core_value_competencies = value; }
        }

        public Model.DTO.Appraisal.Appraisal Appraisal
        {
            get { return obj_appraisal; }
            set { obj_appraisal = value; }
        }

        public bool ViewOnly
        {
            get { return boo_view_only; }
            set { boo_view_only = value; }
        }

        public string GetSectionURL(int stage, string sectionName, int taskId, int appraisalId)
        {
            StringBuilder sb = new StringBuilder();

            if (stage == 1) 
            {
                sb.Append("/Stage1/");
            }
            else if (stage == 2)
            {
                sb.Append("/Stage2/");
            }
            else if (stage == 3)
            {
                sb.Append("/Stage2/");
            }
            else 
            {
                return "#";
            }

            sb.Append(sectionName);
            sb.Append("/");
            sb.Append(taskId);
            sb.Append("/");
            sb.Append(appraisalId);
            sb.Append("#");

            return sb.ToString();
        }

        public string SubmissionDeadline
        {
            get 
            {
                if (obj_appraisal != null)
                {
                    return Convert.ToDateTime(obj_appraisal.AppraisalStages.Where(rec => rec.StageId == obj_appraisal.Stage.Id).Single().SubmissionDeadline).ToString("dd/MM/yyyy");
                }
                else 
                {
                    return "";
                }
            }
        }
    }
}