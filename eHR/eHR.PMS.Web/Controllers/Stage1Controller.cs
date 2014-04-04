using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eHR.PMS.Web.Controllers
{
    public class Stage1Controller : BaseController
    {
        private bool boo_view_only = true;
        private bool boo_has_access = false;

        #region KPI

        public ActionResult KeyPerformanceIndicators(int taskId, int? id)
        {
            Models.DTO.AppraisalPage obj_appraisal_page = new Models.DTO.AppraisalPage();
            
            if (id.HasValue)
            {
                obj_appraisal_page.User = CurrentUser;
                obj_appraisal_page.Priorities = PMS.Model.PMSModel.GetMasterPriorityList(true);
                obj_appraisal_page.Sections = PMS.Model.PMSModel.GetMasterSectionList(true);
                obj_appraisal_page.Appraisal = PMS.Model.PMSModel.GetAppraisalById(Convert.ToInt32(id));
                obj_appraisal_page.CurrentTaskId = taskId;
                //SetPageViewOnly(obj_appraisal_page.Appraisal.Status.Id, taskId);
                //SetPageViewOnly(obj_appraisal_page.Appraisal, taskId);
                
            }

            if (obj_appraisal_page.Appraisal != null)
            {
                obj_appraisal_page.CurrentSection = obj_appraisal_page.Sections.Where(a => a.Id == PMS.Model.PMSConstants.SECTION_ID_KPI).First();
                
                ViewData["Stage"] = obj_appraisal_page.Appraisal.Stage.Name;
                ViewData["Status"] = obj_appraisal_page.Appraisal.Status.Name;
                ViewData["appraisalid"] = id;

                if (!CheckAccessAndSetViewMode(obj_appraisal_page.Appraisal, taskId))
                {
                    TempData["AlertMessage"] = Resources.Resource.MSG_APPRAISAL_NO_ACCESS;
                    return Redirect(Url.Content("~/Home/Index"));
                }
                else
                {
                    ViewData.Model = obj_appraisal_page;
                    obj_appraisal_page.ViewOnly = boo_view_only;
                    return View();
                }
            }
            else 
            {
                TempData["AlertMessage"] = Resources.Resource.MSG_NO_APPRAISAL_FOUND;
                return Redirect(Url.Content("~/Home/Index"));
            }
        }

        [HttpPost]
        public ActionResult KeyPerformanceIndicators(int taskid, int id, FormCollection form)
        {
            int index = 0;
            string[] result = new string[form.Count - 1];
            string message = string.Empty;
            foreach (string key in form.AllKeys.Where(s => s.Contains("KPIforDatabase")))
            {
                result[index] = form[key];
                index++;
            }

            string[] deleteKpiId = null;

            if (form["deleteKPIid"].ToString().Length > 1)
            {
                deleteKpiId = form["deleteKPIid"].ToString().Substring(1).Split('-');
            }


            if (PMS.Model.PMSModel.UpdateAppraisalKPIs(
                    Business.AppraisalManager.GetKPIItemsToInsert(result),
                    Business.AppraisalManager.GetKPIItemsToUpdate(result),
                    Business.AppraisalManager.GetKPIItemsToDelete(deleteKpiId), out message))
            {
                //TempData["AlertMessage"] = Resources.Resource.MSG_NEXT_SUCCESS;
                return Redirect(Url.Content("~/Stage1/CoreValues/" + taskid + "/" + id));
            }
            else
            {
                TempData["AlertMessage"] = Resources.Resource.MSG_NEXT_FAIL;
                return View();
            }
        }

        [HttpPost]
        public JsonResult KPISave(string[] KPIForDatabase, string DeleteKPI)
        {
            string message = string.Empty;
            //string temp = KPIForDatabase[0];
            string temp = Uri.UnescapeDataString(KPIForDatabase[0]);
            string[] deleteKpiId = DeleteKPI.Split('-');
            string newkpiidarray=string.Empty;
            if (string.IsNullOrEmpty(temp))
            {
                return Json(Resources.Resource.MSG_SAVE_FAIL);
            }         

            string[] splitString = { "ONERECORDENDED\",\"" };

            if (Lib.Utility.Common.IsNullOrEmptyList(splitString))
            {
                return Json(Resources.Resource.MSG_SAVE_FAIL);
            }   
       
            string[] result = temp.Split(splitString, StringSplitOptions.None);

            if (Lib.Utility.Common.IsNullOrEmptyList(result))
            {
                return Json(Resources.Resource.MSG_SAVE_FAIL);
            }

            if (result[0].Length > 0)
            {
                result[0] = result[0].Substring(2);

                if (result[result.Length - 1].IndexOf("^&*ONERECORDENDED") > 0)
                {
                    result[result.Length - 1] = result[result.Length - 1].Substring(0, result[result.Length - 1].IndexOf("^&*ONERECORDENDED"));
                   
                    PMS.Model.PMSModel.UpdateAppraisalKPIsForAjax(
                                        Business.AppraisalManager.GetKPIItemsToInsert(result),
                                        Business.AppraisalManager.GetKPIItemsToUpdate(result),
                                        Business.AppraisalManager.GetKPIItemsToDelete(deleteKpiId), out message,out newkpiidarray);
                }
                else
                {
                    PMS.Model.PMSModel.UpdateAppraisalKPIsForAjax(
                                        Business.AppraisalManager.GetKPIItemsToInsert(null),
                                        Business.AppraisalManager.GetKPIItemsToUpdate(null),
                                        Business.AppraisalManager.GetKPIItemsToDelete(deleteKpiId), out message,out newkpiidarray);
                }
            }
            return Json(new { message = message, kpiid = newkpiidarray });
        }

        #endregion KPI

        #region Core Values

        public ActionResult CoreValues(int taskid, int? id)
        {
            Models.DTO.AppraisalPage obj_appraisal_page = new Models.DTO.AppraisalPage();

            if (id.HasValue)
            {
                obj_appraisal_page.User = CurrentUser;
                obj_appraisal_page.CoreValueCompetencies = PMS.Model.PMSModel.GetCoreValueCompetencyByGrade(CurrentUser.ACRGrade.Id);
                obj_appraisal_page.Sections = PMS.Model.PMSModel.GetMasterSectionList(true);
                obj_appraisal_page.Appraisal = PMS.Model.PMSModel.GetAppraisalById(Convert.ToInt32(id));
                obj_appraisal_page.CurrentTaskId = taskid;
            }


            if (obj_appraisal_page.Appraisal != null)
            {
                obj_appraisal_page.CurrentSection = obj_appraisal_page.Sections.Where(a => a.Id == PMS.Model.PMSConstants.SECTION_ID_CORE_VALUES).First();
                ViewData["Stage"] = obj_appraisal_page.Appraisal.Stage.Name;
                ViewData["Status"] = obj_appraisal_page.Appraisal.Status.Name;
                ViewData["appraisalid"] = id;

                if (!CheckAccessAndSetViewMode(obj_appraisal_page.Appraisal, taskid))
                {
                    TempData["AlertMessage"] = Resources.Resource.MSG_APPRAISAL_NO_ACCESS;
                    return Redirect(Url.Content("~/Home/Index"));
                }
                else
                {
                    ViewData.Model = obj_appraisal_page;
                    obj_appraisal_page.ViewOnly = boo_view_only;
                    return View();
                }
            }
            else
            {
                TempData["AlertMessage"] = Resources.Resource.MSG_NO_APPRAISAL_FOUND;
                return Redirect(Url.Content("~/Home/Index"));
            }
        }

        [HttpPost]
        public ActionResult CoreValues(int taskid, int id, FormCollection form)
        {
            int index = 0;
            string[] splitString2 = { "^%*" };
            string[] result = new string[form.Count];
            string message = string.Empty;
            foreach (string key in form.AllKeys.Where(s => s.Contains("KPIforDatabase")))
            {
                result[index] = form[key];
                index++;
            }
            string[] deleteKpiId = null;
            if (form["deleteKPIid"].ToString().Length > 1)
            {
                deleteKpiId = form["deleteKPIid"].ToString().Substring(1).Split('-');
            }

            if (PMS.Model.PMSModel.UpdateAppraisalCoreValues(
                Business.AppraisalManager.GetCoreValueItemsToInsert(result),
                Business.AppraisalManager.GetCoreValueItemsToUpdate(result),
                Business.AppraisalManager.GetCoreValueItemsToDelete(deleteKpiId), out message))
            {
                //TempData["AlertMessage"] = Resources.Resource.MSG_NEXT_SUCCESS;
                return Redirect(Url.Content("~/Stage1/PerformanceCoachingandReview/" + taskid + "/" + id));
            }
            else
            {
                ViewData["appraisalid"] = id;
                TempData["AlertMessage"] = Resources.Resource.MSG_NEXT_FAIL;
                return View();
            }
        }

        [HttpPost]
        public JsonResult CoreValuesSave(string[] KPIForDatabase, string DeleteKPI)
        {
            string message = string.Empty;
            string temp = Uri.UnescapeDataString(KPIForDatabase[0]);
            string[] deleteKpiId = DeleteKPI.Split('-');
            string newkpiidarray = string.Empty;
            if (string.IsNullOrEmpty(temp))
            {
                return Json(Resources.Resource.MSG_SAVE_FAIL);
            }

            string[] splitString = { "ONERECORDENDED\",\"" };

            if (Lib.Utility.Common.IsNullOrEmptyList(splitString))
            {
                return Json(Resources.Resource.MSG_SAVE_FAIL);
            }

            string[] result = temp.Split(splitString, StringSplitOptions.None);

            if (Lib.Utility.Common.IsNullOrEmptyList(result))
            {
                return Json(Resources.Resource.MSG_SAVE_FAIL);
            }

            if (result[0].Length > 0)
            {
                result[0] = result[0].Substring(2);

                if (result[result.Length - 1].IndexOf("^&*ONERECORDENDED") > 0)
                {
                    result[result.Length - 1] = result[result.Length - 1].Substring(0, result[result.Length - 1].IndexOf("^&*ONERECORDENDED"));
                    PMS.Model.PMSModel.UpdateAppraisalCoreValuesForAjax(
                                        Business.AppraisalManager.GetCoreValueItemsToInsert(result),
                                        Business.AppraisalManager.GetCoreValueItemsToUpdate(result),
                                        Business.AppraisalManager.GetCoreValueItemsToDelete(deleteKpiId), out message,out newkpiidarray);
                }
                else 
                {
                    PMS.Model.PMSModel.UpdateAppraisalCoreValuesForAjax(
                                        Business.AppraisalManager.GetCoreValueItemsToInsert(null),
                                        Business.AppraisalManager.GetCoreValueItemsToUpdate(null),
                                        Business.AppraisalManager.GetCoreValueItemsToDelete(deleteKpiId), out message,out newkpiidarray);
                }
            }
            return Json(new { message = message, kpiid = newkpiidarray });
        }

        #endregion Core Values

        #region Performance Coaching

        public ActionResult PerformanceCoachingandReview(int taskid, int? id)
        {
            Models.DTO.AppraisalPage obj_appraisal_page = new Models.DTO.AppraisalPage();

            if (id.HasValue)
            {
                obj_appraisal_page.User = CurrentUser;
                obj_appraisal_page.Sections = PMS.Model.PMSModel.GetMasterSectionList(true);
                obj_appraisal_page.Appraisal = PMS.Model.PMSModel.GetAppraisalById(Convert.ToInt32(id));
                obj_appraisal_page.CurrentTaskId = taskid;
                //obj_appraisal_page.CurrentSection = obj_appraisal_page.Sections.Where(a=> a.Id == PMS.Model.PMSConstants.SECTION_ID_KPI).First();
                //ViewData["Stage"] = obj_appraisal_page.Appraisal.Stage.Name;
                //ViewData["Status"] = obj_appraisal_page.Appraisal.Status.Name;

                //SetPageViewOnly(obj_appraisal_page.Appraisal.Status.Id, taskid);
                //SetPageViewOnly(obj_appraisal_page.Appraisal, taskid);
                //obj_appraisal_page.ViewOnly = boo_view_only;
            }

            //ViewData["appraisalid"] = id;
            //ViewData.Model = obj_appraisal_page;

            if (obj_appraisal_page.Appraisal != null)
            {
                obj_appraisal_page.CurrentSection = obj_appraisal_page.Sections.Where(a => a.Id == PMS.Model.PMSConstants.SECTION_ID_PERFORMANCE_COACHING).First();
                ViewData["Stage"] = obj_appraisal_page.Appraisal.Stage.Name;
                ViewData["Status"] = obj_appraisal_page.Appraisal.Status.Name;
                ViewData["appraisalid"] = id;

                if (!CheckAccessAndSetViewMode(obj_appraisal_page.Appraisal, taskid))
                {
                    TempData["AlertMessage"] = Resources.Resource.MSG_APPRAISAL_NO_ACCESS;
                    return Redirect(Url.Content("~/Home/Index"));
                }
                else
                {
                    ViewData.Model = obj_appraisal_page;
                    obj_appraisal_page.ViewOnly = boo_view_only;
                    return View();
                }
            }
            else
            {
                TempData["AlertMessage"] = Resources.Resource.MSG_NO_APPRAISAL_FOUND;
                return Redirect(Url.Content("~/Home/Index"));
            }
        }

        [HttpPost]
        public ActionResult PerformanceCoachingandReview(int taskid, int id, FormCollection form)
        {
            Dictionary<string, string> dictform = FormCollectionToDict(form);
            dictform.Add("AppraisalID", id.ToString());
            string message = string.Empty;

            if (PMS.Model.PMSModel.UpdateAppraisalPerformanceCoaching(Business.AppraisalManager.GetPerformanceCoachingItemFromFormInput(dictform), out message))
            {
               // TempData["AlertMessage"] = "Appraisal information is saved.";
                return Redirect(Url.Content("~/Stage1/CareerDevelopment/" + taskid + "/" + id));
            }
            else
            {
                ViewData["appraisalid"] = id;
                TempData["AlertMessage"] = Resources.Resource.MSG_NEXT_FAIL;
                return View();
            }
        }

        [HttpPost]
        public JsonResult PerformanceCoachingandReviewSave(string ApprID, string SectionID, string StrengthsArea, string ImprovementsArea)
        {
            string message = string.Empty;
            Dictionary<string, string> dictform = new Dictionary<string, string>();
            dictform.Add("AppraisalID", ApprID);
            dictform.Add("SectionID", SectionID);
            dictform.Add("StrengthsArea",StrengthsArea);
            dictform.Add("ImprovementsArea",ImprovementsArea);

            PMS.Model.PMSModel.UpdateAppraisalPerformanceCoaching(Business.AppraisalManager.GetPerformanceCoachingItemFromFormInput(dictform), out message);
            return Json(message);
        }

        #endregion Performance Coaching

        #region Career Development

        public ActionResult CareerDevelopment(int taskid, int? id)
        {
            Models.DTO.AppraisalPage obj_appraisal_page = new Models.DTO.AppraisalPage();

            if (id.HasValue)
            {
                obj_appraisal_page.User = CurrentUser;
                obj_appraisal_page.Sections = PMS.Model.PMSModel.GetMasterSectionList(true);
                obj_appraisal_page.Appraisal = PMS.Model.PMSModel.GetAppraisalById(Convert.ToInt32(id));
                obj_appraisal_page.CurrentTaskId = taskid;
                //obj_appraisal_page.CurrentSection = obj_appraisal_page.Sections.Where(a => a.Id == PMS.Model.PMSConstants.SECTION_ID_KPI).First();
                //ViewData["Stage"] = obj_appraisal_page.Appraisal.Stage.Name;
                //ViewData["Status"] = obj_appraisal_page.Appraisal.Status.Name;

                //SetPageViewOnly(obj_appraisal_page.Appraisal.Status.Id, taskid);
                //SetPageViewOnly(obj_appraisal_page.Appraisal, taskid);
                //obj_appraisal_page.ViewOnly = boo_view_only;
            }
            //ViewData["appraisalid"] = id;
            //ViewData.Model = obj_appraisal_page;

            if (obj_appraisal_page.Appraisal != null)
            {
                obj_appraisal_page.CurrentSection = obj_appraisal_page.Sections.Where(a => a.Id == PMS.Model.PMSConstants.SECTION_ID_CAREER_DEVELOPMENT).First();
                ViewData["Stage"] = obj_appraisal_page.Appraisal.Stage.Name;
                ViewData["Status"] = obj_appraisal_page.Appraisal.Status.Name;
                ViewData["appraisalid"] = id;

                if (!CheckAccessAndSetViewMode(obj_appraisal_page.Appraisal, taskid))
                {
                    TempData["AlertMessage"] = Resources.Resource.MSG_APPRAISAL_NO_ACCESS;
                    return Redirect(Url.Content("~/Home/Index"));
                }
                else
                {
                    ViewData.Model = obj_appraisal_page;
                    obj_appraisal_page.ViewOnly = boo_view_only;
                    return View();
                }
            }
            else
            {
                TempData["AlertMessage"] = Resources.Resource.MSG_NO_APPRAISAL_FOUND;
                return Redirect(Url.Content("~/Home/Index"));
            }
        }

        [HttpPost]
        public ActionResult CareerDevelopment(int taskid, int id, FormCollection form)
        {
            Dictionary<string, string> dictform = new Dictionary<string, string>();
            dictform = FormCollectionToDict(form);
            dictform.Add("AppraisalID", Convert.ToString(id));
            dictform.Add("TaskID", Convert.ToString(taskid));
            string message = string.Empty;

            if (Model.PMSModel.UpdateAppraisalCareerDevelopment(Business.AppraisalManager.GetCareerDevelopmentItemFromFormInput(dictform), out message))
            {
                if (Business.AppraisalManager.ProcessAppraisalSubmission(id, taskid, CurrentUser, out message))
                {
                    TempData["AlertMessage"] = Resources.Resource.MSG_APPRAISAL_SUBMITTED;
                    return Redirect(Url.Content("~/"));
                }
                else
                {
                    ViewData["appraisalid"] = id;
                    TempData["AlertMessage"] = Resources.Resource.MSG_SAVE_FAIL;
                    return View();
                }
            }
            else
            {
                ViewData["appraisalid"] = id;
                TempData["AlertMessage"] = Resources.Resource.MSG_SAVE_FAIL;
                return View();
            }
        }

        public JsonResult CareerDevelopmentSave(string ApprID, string SectionID, string ShorttermCareerGoal, string DevelopmentPlan, string Learninganddevelopment)
        {
            Dictionary<string, string> form = new Dictionary<string, string>();
            form.Add("AppraisalID", ApprID);
            form.Add("SectionID", SectionID);
            form.Add("ShorttermCareerGoal",ShorttermCareerGoal);
            form.Add("DevelopmentPlan", DevelopmentPlan);
            form.Add("Learninganddevelopment", Learninganddevelopment);
            string message = string.Empty;
            Model.PMSModel.UpdateAppraisalCareerDevelopment(Business.AppraisalManager.GetCareerDevelopmentItemFromFormInput(form), out message);
            return Json(message);
        }

        #endregion Career Development

        private bool CheckAccessRights(Model.DTO.Appraisal.Appraisal appraisal)
        {
            bool boo_has_access = false;

            if (Business.SecurityManager.HasHRRole(CurrentUser)) 
            {
                boo_has_access = true;
            }
            else if (CurrentUser.Id == appraisal.Employee.Id)
            {
                boo_has_access = true;
            }
            else if (IsCurrentUserApprover(appraisal.Approvers))
            {
                boo_has_access = true;
            }

            else if (IsCurrentUserReviewer(appraisal.Reviewers))
            {
                boo_has_access = true;
            }

            return boo_has_access;
        }

        private bool IsCurrentUserApprover(List<Model.DTO.Appraisal.Approver> approvers)
        {
            bool boo_is_approver = false;

            if (!Lib.Utility.Common.IsNullOrEmptyList(approvers))
            {
                var var_approvers = approvers.Where(rec => rec.EmployeeId == CurrentUser.Id);

                if (!Lib.Utility.Common.IsNullOrEmptyList(var_approvers))
                {
                    boo_is_approver = true;
                }
            }
            return boo_is_approver;
        }

        private bool IsCurrentUserReviewer(List<Model.DTO.Appraisal.Reviewer> reviewers)
        {
            bool boo_is_reviewer = false;

            if (!Lib.Utility.Common.IsNullOrEmptyList(reviewers))
            {
                var var_approvers = reviewers.Where(rec => rec.EmployeeId == CurrentUser.Id);

                if (!Lib.Utility.Common.IsNullOrEmptyList(var_approvers))
                {
                    boo_is_reviewer = true;
                }
            }
            return boo_is_reviewer;
        }

        private void SetPageViewOnly(Model.DTO.Appraisal.Appraisal appraisal, int taskId)
        {
            if (taskId > 0)
            {
                if (appraisal.Status.Id == PMS.Model.PMSConstants.STATUS_ID_NEW || appraisal.Status.Id == PMS.Model.PMSConstants.STATUS_ID_DRAFT)
                {
                    if (CurrentUser.Id == appraisal.Employee.Id)
                    {
                        boo_view_only = false;
                    }
                    else 
                    {
                        boo_view_only = true;
                    }                 
                }
                else
                {
                    boo_view_only = true;
                }
            }
            else
            {
                boo_view_only = true;
            }
        }
        
        private void SetPageViewOnly(int appraisalStatusId, int taskId)
        {
            if (taskId > 0)
            {
                if (appraisalStatusId == PMS.Model.PMSConstants.STATUS_ID_NEW || appraisalStatusId == PMS.Model.PMSConstants.STATUS_ID_DRAFT)
                {
                    boo_view_only = false;
                }
                else
                {
                    boo_view_only = true;
                }
            }
            else 
            {
                boo_view_only = true;
            }

        }

        public Dictionary<string, string> FormCollectionToDict(FormCollection form)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (string key in form.AllKeys)
            {
                dict.Add(key, form[key]);
            }
            return dict;
        }

        public bool CheckAccessAndSetViewMode(Model.DTO.Appraisal.Appraisal appraisal, int taskId)
        {
            bool boo_has_access = false;

            if (taskId > 0)
            {
                // if current user is owner of task, input mode
                // if current user is not user of task, redirect to stage1/keyperformanceindicator/0/appraisalId

                Model.DTO.Core.Task.Task obj_task = Model.PMSModel.GetTaskById(taskId);

                if (obj_task != null)
                {
                    // check if task id is for appraisal

                    if (obj_task.RecordId != appraisal.Id)
                    {
                        boo_has_access = false;
                    }
                    else if (!Lib.Utility.Common.IsNullOrEmptyList(obj_task.Owners))
                    {
                        var var_owners = obj_task.Owners.Where(rec => rec.EmployeeId == CurrentUser.Id);

                        if (!Lib.Utility.Common.IsNullOrEmptyList(var_owners))
                        {
                            boo_has_access = true;
                            if (obj_task.Status.Id == Model.PMSConstants.STATUS_CORE_ID_OPEN)
                            {
                                boo_view_only = false;
                            }
                            else
                            {
                                boo_view_only = true;
                            }
                        }
                        else
                        {
                            boo_view_only = true;
                            boo_has_access = CheckAccessRights(appraisal);
                        }
                    }
                    else 
                    {
                        boo_view_only = true;
                        boo_has_access = CheckAccessRights(appraisal);
                    }

                    // if current user is owner of task, and task is incomplete, do nothing
                    // if current user is owner of task and task is completed, got to view mode
                    // if current user is not owner of task, go to view mode
                }
                else
                {
                    boo_view_only = true;
                    boo_has_access = CheckAccessRights(appraisal);
                }
            }
            else
            { 
                boo_view_only = true;
                boo_has_access = CheckAccessRights(appraisal);
            }

            return boo_has_access;
        }
    }
}
