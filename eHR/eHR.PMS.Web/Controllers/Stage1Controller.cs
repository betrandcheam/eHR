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
                obj_appraisal_page.CurrentSection = obj_appraisal_page.Sections.Where(a=> a.Id == PMS.Model.PMSConstants.SECTION_ID_KPI).First();
                ViewData["Stage"] = obj_appraisal_page.Appraisal.Stage.Name;
                ViewData["Status"] = obj_appraisal_page.Appraisal.Status.Name;

                SetPageViewOnly(obj_appraisal_page.Appraisal.Status.Id, taskId);
                obj_appraisal_page.ViewOnly = boo_view_only;
            }

            ViewData["appraisalid"] = id;
            ViewData.Model = obj_appraisal_page;
            return View();
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
            string temp = KPIForDatabase[0];
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
                obj_appraisal_page.CurrentSection = obj_appraisal_page.Sections.Where(a => a.Id == PMS.Model.PMSConstants.SECTION_ID_CORE_VALUES).First();
                ViewData["Stage"] = obj_appraisal_page.Appraisal.Stage.Name;
                ViewData["Status"] = obj_appraisal_page.Appraisal.Status.Name;

                SetPageViewOnly(obj_appraisal_page.Appraisal.Status.Id, taskid);
                obj_appraisal_page.ViewOnly = boo_view_only;
            }

            ViewData["appraisalid"] = id;
            ViewData.Model = obj_appraisal_page;
            return View();
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
            string temp = KPIForDatabase[0];
            string[] deleteKpiId = DeleteKPI.Split('-');

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
                    PMS.Model.PMSModel.UpdateAppraisalCoreValues(
                                        Business.AppraisalManager.GetCoreValueItemsToInsert(result),
                                        Business.AppraisalManager.GetCoreValueItemsToUpdate(result),
                                        Business.AppraisalManager.GetCoreValueItemsToDelete(deleteKpiId), out message);
                }
                else 
                {
                    PMS.Model.PMSModel.UpdateAppraisalCoreValues(
                                        Business.AppraisalManager.GetCoreValueItemsToInsert(null),
                                        Business.AppraisalManager.GetCoreValueItemsToUpdate(null),
                                        Business.AppraisalManager.GetCoreValueItemsToDelete(deleteKpiId), out message);
                }
            }
            return Json(message);
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
                obj_appraisal_page.CurrentSection = obj_appraisal_page.Sections.Where(a=> a.Id == PMS.Model.PMSConstants.SECTION_ID_KPI).First();
                ViewData["Stage"] = obj_appraisal_page.Appraisal.Stage.Name;
                ViewData["Status"] = obj_appraisal_page.Appraisal.Status.Name;

                SetPageViewOnly(obj_appraisal_page.Appraisal.Status.Id, taskid);
                obj_appraisal_page.ViewOnly = boo_view_only;
            }

            ViewData["appraisalid"] = id;
            ViewData.Model = obj_appraisal_page;
            return View();
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
            dictform.Add("StrengthsArea", StrengthsArea);
            dictform.Add("ImprovementsArea", ImprovementsArea);

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
                obj_appraisal_page.CurrentSection = obj_appraisal_page.Sections.Where(a => a.Id == PMS.Model.PMSConstants.SECTION_ID_KPI).First();
                ViewData["Stage"] = obj_appraisal_page.Appraisal.Stage.Name;
                ViewData["Status"] = obj_appraisal_page.Appraisal.Status.Name;

                SetPageViewOnly(obj_appraisal_page.Appraisal.Status.Id, taskid);
                obj_appraisal_page.ViewOnly = boo_view_only;
            }
            ViewData["appraisalid"] = id;
            ViewData.Model = obj_appraisal_page;
            return View();
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
            form.Add("ShorttermCareerGoal", ShorttermCareerGoal);
            form.Add("DevelopmentPlan", DevelopmentPlan);
            form.Add("Learninganddevelopment", Learninganddevelopment);
            string message = string.Empty;
            Model.PMSModel.UpdateAppraisalCareerDevelopment(Business.AppraisalManager.GetCareerDevelopmentItemFromFormInput(form), out message);
            return Json(message);
        }

        #endregion Career Development

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
    }
}
