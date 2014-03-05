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
            Web.Models.DTO.HomePage homePageDTO = new Models.DTO.HomePage()
            {
                User = CurrentUser,
                MyTasks = PMS.Business.AppraisalManager.GetTasksByOwner(CurrentUser.Id, Model.PMSConstants.STATUS_CORE_ID_OPEN),
                MyAppraisals = PMS.Business.AppraisalManager.GetAppraisalsByEmployeeForDisplay(CurrentUser.Id, null),
                MyAppraisalsToApprove = PMS.Business.AppraisalManager.GetAppraisalsToApproveByApproverId(CurrentUser.Id,LatestCycleId),
                MyAppraisalsToReview = PMS.Model.PMSModel.GetAppraisalsInCycleToReview(CurrentUser.Id, LatestCycleId),
            };
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
        public JsonResult SaveApproversForProfile(string approver1, string approver2, int appraisalId)
        {
            return Json("");
        }

        #endregion Appraisal Profile
    }
}
