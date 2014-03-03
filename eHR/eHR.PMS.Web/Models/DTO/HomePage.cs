using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eHR.PMS.Web.Models.DTO
{
    public class HomePage
    {
        private PMS.Model.DTO.Core.Security.User obj_user;
        private List<PMS.Model.DTO.Core.Task.Task> lst_my_tasks;
        private List<PMS.Model.DTO.Appraisal.Appraisal> lst_my_appraisals;
        private List<PMS.Model.DTO.Appraisal.Appraisal> lst_my_appraisals_to_approve;
        private List<PMS.Model.DTO.Appraisal.Appraisal> lst_my_appraisals_to_review;

        public PMS.Model.DTO.Core.Security.User User
        {
            get { return obj_user; }
            set { obj_user = value; }
        }

        public List<PMS.Model.DTO.Core.Task.Task> MyTasks
        {
            get { return lst_my_tasks; }
            set { lst_my_tasks = value; }
        }

        public List<PMS.Model.DTO.Appraisal.Appraisal> MyAppraisals
        {
            get { return lst_my_appraisals; }
            set { lst_my_appraisals = value; }
        }

        public List<PMS.Model.DTO.Appraisal.Appraisal> MyAppraisalsToApprove
        {
            get { return lst_my_appraisals_to_approve; }
            set { lst_my_appraisals_to_approve = value; }
        }

        public List<PMS.Model.DTO.Appraisal.Appraisal> MyAppraisalsToReview
        {
            get { return lst_my_appraisals_to_review; }
            set { lst_my_appraisals_to_review = value; }
        }

    }
}