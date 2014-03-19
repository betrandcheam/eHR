using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eHR.PMS.Web.Controllers
{
    public class HomeController : BaseController
    {
        [HandleError]
        public ActionResult Index()
        {
            List<PMS.Model.DTO.Appraisal.Appraisal> tempapprlist = new List<Model.DTO.Appraisal.Appraisal>();
            List<PMS.Model.DTO.Appraisal.Appraisal> tempapprlistsmt = new List<Model.DTO.Appraisal.Appraisal>();
            Web.Models.DTO.HomePage homePageDTO = new Models.DTO.HomePage()
            {
                User = CurrentUser,
                MyTasks = PMS.Business.AppraisalManager.GetTasksByOwner(CurrentUser.Id, Model.PMSConstants.STATUS_CORE_ID_OPEN),
                MyAppraisals = PMS.Business.AppraisalManager.GetAppraisalsByEmployeeForDisplay(CurrentUser.Id, null),
                MyAppraisalsToApprove = PMS.Business.AppraisalManager.GetAppraisalsToApproveByApproverId(CurrentUser.Id,LatestCycleId)
            };
            PMS.Model.PMSModel.GetAppraisalsInCycleToReview(CurrentUser.Id, LatestCycleId, out tempapprlist, out tempapprlistsmt);
            homePageDTO.MyAppraisalsToReview = tempapprlist;
            homePageDTO.MyAppraisalsToReviewSMT = tempapprlistsmt;
            ViewData.Model = homePageDTO;
            return View();
        }

        #region Appraisal Profile

        public ActionResult AppraisalProfile(int? id)
        { 
            // retrieve profile by appraisal
            Web.Models.DTO.AppraisalProfilePage appraisalProfilePageDTO = new Models.DTO.AppraisalProfilePage() 
            {
                User = CurrentUser
            };

            if (id.HasValue)
            {
                appraisalProfilePageDTO.Appraisal = PMS.Model.PMSModel.GetAppraisalById(Convert.ToInt32(id));
            }
            else 
            {
                TempData["AlertMessage"] = "Unable to retrieve appraisal profile.";
                return Redirect(Url.Content("~/"));
            }
            ViewData.Model = appraisalProfilePageDTO;
            return View();
        }

        [HttpPost]
        public JsonResult SearchEmployeeByName(string name)
        {
            List<Model.DTO.Core.Employee> lst_employees = null;
            if (!string.IsNullOrEmpty(name) && name.Length > 4)
            {
                lst_employees = Model.PMSModel.GetEmployeesByName(true, name);                    
            }
            return Json(lst_employees);
        }

        [HttpPost]
        public JsonResult SaveProfile(string reviewers, int apprid)
        {
            string message = string.Empty;
            List<Model.DTO.Appraisal.Reviewer> lst_reviewers = null;
            if (!string.IsNullOrEmpty(reviewers))
            {
                string[] reviewarray = reviewers.Split('|');

                if (!Lib.Utility.Common.IsNullOrEmptyList(reviewarray))
                {
                    List<string> lst_temp = new List<string>();
                    lst_reviewers = new List<Model.DTO.Appraisal.Reviewer>();
                    foreach(string str_reviewer in reviewarray)
                    {
                        if (Lib.Utility.Common.IsNullOrEmptyList(lst_temp.Where(rec=>rec.Equals(str_reviewer))))
                        {
                            lst_temp.Add(str_reviewer);
                        }

                        foreach (string str_reviewer_id in lst_temp)
                        {
                            Model.DTO.Appraisal.Reviewer obj_reviewer = new Model.DTO.Appraisal.Reviewer()
                            {
                                EmployeeId = Convert.ToInt32(str_reviewer_id),
                                Appraisal = new Model.DTO.Appraisal.Appraisal() { Id = apprid }
                            };
                            lst_reviewers.Add(obj_reviewer);
                        }
                    }

                    if (!Model.PMSModel.UpdateReviewersForAppraisal(apprid, lst_reviewers, out message))
                    {
                        return Json(message);
                    }
                    
                }
            }
            return Json(message);
        }

        [HttpPost]
        public JsonResult SaveProfileApprovers(string approver1, string approver2, int appraisalId)
        {
            string message = string.Empty;
            List<Model.DTO.Appraisal.Approver> lst_new_approvers = null;

            if (!string.IsNullOrEmpty(approver1) && !string.IsNullOrEmpty(approver2)) {
                Model.DTO.Appraisal.Appraisal obj_appraisal = Model.PMSModel.GetAppraisalById(appraisalId);
                lst_new_approvers = new List<Model.DTO.Appraisal.Approver>();

                if (obj_appraisal != null)
                {
                    Model.DTO.Appraisal.Approver obj_level_1_approver = obj_appraisal.GetApproverByLevel(1);
                    Model.DTO.Appraisal.Approver obj_level_2_approver = obj_appraisal.GetApproverByLevel(2);

                    if (obj_level_1_approver != null && obj_level_1_approver.EmployeeId != Convert.ToInt32(approver1))
                    {
                        Model.DTO.Appraisal.Approver obj_approver = new Model.DTO.Appraisal.Approver() 
                        { 
                            Id = obj_level_1_approver.Id,
                            Appraisal= obj_appraisal,
                            EmployeeId = Convert.ToInt32(approver1),
                            ApprovalLevel = 1
                        };

                        lst_new_approvers.Add(obj_approver);
                    }

                    if (obj_level_2_approver != null && obj_level_2_approver.EmployeeId != Convert.ToInt32(approver2))
                    {
                        Model.DTO.Appraisal.Approver obj_approver = new Model.DTO.Appraisal.Approver()
                        {
                            Id = obj_level_2_approver.Id,
                            Appraisal = obj_appraisal,
                            EmployeeId = Convert.ToInt32(approver2),
                            ApprovalLevel = 2
                        };
                        lst_new_approvers.Add(obj_approver);
                    }

                    if (!Business.AppraisalManager.ManageChangeApprover(obj_appraisal, lst_new_approvers, out message)) 
                    {
                        return Json(message);
                    }
                }
            }
            return Json(message);
        }

        #endregion Appraisal Profile
    }
}
