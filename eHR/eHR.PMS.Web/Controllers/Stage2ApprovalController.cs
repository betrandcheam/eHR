﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eHR.PMS.Web.Controllers
{
    public class Stage2ApprovalController : BaseController
    {
        #region KPI

        public ActionResult KeyPerformanceIndicators(int taskid, int? id)
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
            }
            ViewData["appraisalid"] = id;
            ViewData.Model = obj_appraisal_page;
            return View();
        }

        [HttpPost]
        public ActionResult KeyPerformanceIndicators(int taskid, int id, FormCollection form)
        {
            List<Model.DTO.Appraisal.KPIComment> lst_comments = new List<Model.DTO.Appraisal.KPIComment>();
            Dictionary<string, string> dict_comments = new Dictionary<string, string>();
            string message = string.Empty;
            dict_comments = FormCollectionToDictionary(form);

            foreach (KeyValuePair<string, string> kv in dict_comments)
            {
                if (!string.IsNullOrEmpty(kv.Value.Trim()))
                {
                    Model.DTO.Appraisal.KPIComment obj_comment = new Model.DTO.Appraisal.KPIComment()
                    {
                        AppraisalKPI = new Model.DTO.Appraisal.KPI() { Id = Convert.ToInt32(kv.Key) },
                        Comments = kv.Value.Trim(),
                        Commentor = new Model.DTO.Core.Employee() { Id = CurrentUser.Id },
                        CommentedTimestamp = DateTime.Now,
                        FormSaveOnly = true
                    };
                    lst_comments.Add(obj_comment);
                }
            }

            if (!Lib.Utility.Common.IsNullOrEmptyList(lst_comments))
            {
                if (Model.PMSModel.UpdateAppraisalKPIComment(lst_comments, out message))
                {
                    //TempData["AlertMessage"] = "Appraisal information is saved.";
                    return Redirect(Url.Content("~/Stage2Approval/CoreValues/" + taskid + "/" + id));
                }
                else
                {
                    ViewData["appraisalid"] = id;
                    TempData["AlertMessage"] = "Unable to save Appraisal information. Please try again or contact IT Department.";
                    return View();
                }
            }
            else
            {
                return Redirect(Url.Content("~/Stage2Approval/CoreValues/" + taskid + "/" + id));
            }
        }

        [HttpPost]
        public JsonResult KPISave(string[] KPIForDatabase)
        {
            string message = string.Empty;
            string temp = KPIForDatabase[0];
            string[] splitString = { "},{" };
            string[] result = temp.Substring(2, temp.Length - 2).Split(splitString, StringSplitOptions.None);
            Model.PMSModel.UpdateAppraisalKPIComment(Business.AppraisalManager.GetKPICommentItemsToSave(result, CurrentUser.Id, DateTime.Now), out message);
            return Json(message);
        }

        #endregion KPI

        #region Core Value

        public ActionResult CoreValues(int taskid, int? id)
        {
            Models.DTO.AppraisalPage obj_appraisal_page = new Models.DTO.AppraisalPage();

            if (id.HasValue)
            {
                obj_appraisal_page.Appraisal = PMS.Model.PMSModel.GetAppraisalById(Convert.ToInt32(id));
                obj_appraisal_page.User = CurrentUser;
                obj_appraisal_page.Sections = PMS.Model.PMSModel.GetMasterSectionList(true);
                obj_appraisal_page.CurrentTaskId = taskid;
                obj_appraisal_page.CurrentSection = obj_appraisal_page.Sections.Where(a => a.Id == PMS.Model.PMSConstants.SECTION_ID_CORE_VALUES).First();
                ViewData["Stage"] = obj_appraisal_page.Appraisal.Stage.Name;
                ViewData["Status"] = obj_appraisal_page.Appraisal.Status.Name;
            }
            ViewData["appraisalid"] = id;
            ViewData.Model = obj_appraisal_page;
            return View();
        }

        [HttpPost]
        public ActionResult CoreValues(int taskid, int id, FormCollection form)
        {
            List<Model.DTO.Appraisal.CoreValueComment> lst_comments = new List<Model.DTO.Appraisal.CoreValueComment>();
            Dictionary<string, string> dict_comments = new Dictionary<string, string>();
            dict_comments = FormCollectionToDictionary(form);
            string message = string.Empty;

            foreach (KeyValuePair<string, string> kv in dict_comments)
            {
                if (!string.IsNullOrEmpty(kv.Value.Trim()))
                {
                    Model.DTO.Appraisal.CoreValueComment obj_comment = new Model.DTO.Appraisal.CoreValueComment()
                    {
                        AppraisalCoreValue = new Model.DTO.Appraisal.CoreValue() { Id = Convert.ToInt32(kv.Key) },
                        Comments = kv.Value.Trim(),
                        Commentor = new Model.DTO.Core.Employee() { Id = CurrentUser.Id },
                        CommentedTimestamp = DateTime.Now,
                        FormSaveOnly = true
                    };
                    lst_comments.Add(obj_comment);
                }
            }

            if (!Lib.Utility.Common.IsNullOrEmptyList(lst_comments))
            {
                if (Model.PMSModel.UpdateAppraisalCoreValueComment(lst_comments, out message))
                {
                    //TempData["AlertMessage"] = "Appraisal information is saved.";
                    return Redirect(Url.Content("~/Stage2Approval/PerformanceCoachingandReview/" + taskid + "/" + id));
                }
                else
                {
                    ViewData["appraisalid"] = id;
                    TempData["AlertMessage"] = "Unable to save Appraisal information. Please try again or contact IT Department.";
                    return View();
                }
            }
            else
            {
                //TempData["AlertMessage"] = "Appraisal information is saved.";
                return Redirect(Url.Content("~/Stage2Approval/PerformanceCoachingandReview/" + taskid + "/" + id));
            }
        }

        [HttpPost]
        public JsonResult CoreValuesSave(string[] KPIForDatabase)
        {
            string message = string.Empty;
            string temp = KPIForDatabase[0];
            string[] splitString = { "},{" };
            string[] result = temp.Substring(2, temp.Length - 2).Split(splitString, StringSplitOptions.None);
            Model.PMSModel.UpdateAppraisalCoreValueComment(Business.AppraisalManager.GetCoreValueCommentItemsToSave(result, CurrentUser.Id, DateTime.Now), out message);
            return Json(message);
        }

        #endregion Core Value

        #region Performance Coaching

        public ActionResult PerformanceCoachingandReview(int taskid, int? id)
        {
            Models.DTO.AppraisalPage obj_appraisal_page = new Models.DTO.AppraisalPage();

            if (id.HasValue)
            {
                obj_appraisal_page.Appraisal = PMS.Model.PMSModel.GetAppraisalById(Convert.ToInt32(id));
                obj_appraisal_page.User = CurrentUser;
                obj_appraisal_page.Sections = PMS.Model.PMSModel.GetMasterSectionList(true);
                obj_appraisal_page.CurrentTaskId = taskid;
                obj_appraisal_page.CurrentSection = obj_appraisal_page.Sections.Where(a => a.Id == PMS.Model.PMSConstants.SECTION_ID_PERFORMANCE_COACHING).First();
                ViewData["Stage"] = obj_appraisal_page.Appraisal.Stage.Name;
                ViewData["Status"] = obj_appraisal_page.Appraisal.Status.Name;
            }
            ViewData["appraisalid"] = id;
            ViewData.Model = obj_appraisal_page;
            return View();
        }

        [HttpPost]
        public ActionResult PerformanceCoachingandReview(int taskid, int id, FormCollection form)
        {
            string message = string.Empty;
            Dictionary<string, string> dict_comments = new Dictionary<string, string>();
            foreach (string key in form.AllKeys)
            {
                dict_comments.Add(key, form[key]);
            }

            List<Model.DTO.Appraisal.PerformanceCoachingComment> lst_comments = new List<Model.DTO.Appraisal.PerformanceCoachingComment>();

            Model.DTO.Appraisal.PerformanceCoachingComment obj_comment = new Model.DTO.Appraisal.PerformanceCoachingComment()
            {
                AppraisalPerformanceCoaching = new Model.DTO.Appraisal.PerformanceCoaching() { Id = Convert.ToInt32(dict_comments["KPIID"]) },
                CommentedTimestamp = DateTime.Now,
                Commentor = new Model.DTO.Core.Employee() { Id = CurrentUser.Id },
                Comments = dict_comments["Comments"].Trim(),
                FormSaveOnly = true,
            };
            lst_comments.Add(obj_comment);

            if (Model.PMSModel.UpdateAppraisalPerformanceCoachingComment(lst_comments, out message))
            {
                //TempData["AlertMessage"] = "Appraisal information is saved.";
                return Redirect(Url.Content("~/Stage2Approval/CareerDevelopment/" + taskid + "/" + id));
            }
            else
            {
                ViewData["appraisalid"] = id;
                TempData["AlertMessage"] = "Unable to save Appraisal information. Please try again or contact IT Department.";
                return View();
            }
        }

        [HttpPost]
        public JsonResult PerformanceCoachingandReviewSave(string KPIID, string Comments)
        {
            string message = string.Empty;
            List<Model.DTO.Appraisal.PerformanceCoachingComment> lst_comments = new List<Model.DTO.Appraisal.PerformanceCoachingComment>();

            Model.DTO.Appraisal.PerformanceCoachingComment obj_comment = new Model.DTO.Appraisal.PerformanceCoachingComment()
            {
                AppraisalPerformanceCoaching = new Model.DTO.Appraisal.PerformanceCoaching() { Id = Convert.ToInt32(KPIID) },
                CommentedTimestamp = DateTime.Now,
                Commentor = new Model.DTO.Core.Employee() { Id = CurrentUser.Id },
                Comments = Comments.Trim(),
                FormSaveOnly = true,
            };
            lst_comments.Add(obj_comment);

            Model.PMSModel.UpdateAppraisalPerformanceCoachingComment(lst_comments, out message);

            return Json(message);
        }

        #endregion Performance Coaching

        #region Career Development

        public ActionResult CareerDevelopment(int taskid, int? id)
        {
            Models.DTO.AppraisalPage obj_appraisal_page = new Models.DTO.AppraisalPage();

            if (id.HasValue)
            {
                obj_appraisal_page.Appraisal = PMS.Model.PMSModel.GetAppraisalById(Convert.ToInt32(id));
                obj_appraisal_page.User = CurrentUser;
                obj_appraisal_page.Sections = PMS.Model.PMSModel.GetMasterSectionList(true);
                obj_appraisal_page.CurrentTaskId = taskid;
                obj_appraisal_page.CurrentSection = obj_appraisal_page.Sections.Where(a => a.Id == PMS.Model.PMSConstants.SECTION_ID_CAREER_DEVELOPMENT).First();
                ViewData["Stage"] = obj_appraisal_page.Appraisal.Stage.Name;
                ViewData["Status"] = obj_appraisal_page.Appraisal.Status.Name;
            }
            ViewData["appraisalid"] = id;
            ViewData.Model = obj_appraisal_page;
            return View();
        }

        [HttpPost]
        public ActionResult CareerDevelopment(int taskid, int id, FormCollection form)
        {
            string message = string.Empty;
            Dictionary<string, string> dict_comments = new Dictionary<string, string>();
            foreach (string key in form.AllKeys)
            {
                dict_comments.Add(key, form[key]);
            }

            List<Model.DTO.Appraisal.CareerDevelopmentComment> lst_comments = new List<Model.DTO.Appraisal.CareerDevelopmentComment>();

            Model.DTO.Appraisal.CareerDevelopmentComment obj_comment = new Model.DTO.Appraisal.CareerDevelopmentComment()
            {
                AppraisalCareerDevelopment = new Model.DTO.Appraisal.CareerDevelopment() { Id = Convert.ToInt32(dict_comments["KPIID"]) },
                CommentedTimestamp = DateTime.Now,
                Commentor = new Model.DTO.Core.Employee() { Id = CurrentUser.Id },
                Comments = dict_comments["Comments"].Trim(),
                FormSaveOnly = false,
            };
            lst_comments.Add(obj_comment);

            bool boo_is_approved = Convert.ToInt32(dict_comments["ApORRe"]) == 1 ? true : false;

            if (Model.PMSModel.UpdateAppraisalCareerDevelopmentComment(lst_comments, out message))
            {
                if (Business.AppraisalManager.ProcessAppraisalApproval(id, taskid, boo_is_approved, CurrentUser, out message))
                {
                    TempData["AlertMessage"] = boo_is_approved == true ? "The appraisal is approved." : "The appraisal is rejected. It will be routed to the employee for re-submission.";
                    return Redirect(Url.Content("~/"));
                }
                else
                {
                    ViewData["appraisalid"] = id;
                    TempData["AlertMessage"] = "Unable to process approval for application. Please try again or contact IT Department.";
                    return View();
                }
            }
            else
            {
                ViewData["appraisalid"] = id;
                TempData["AlertMessage"] = "Unable to save Appraisal information. Please try again or contact IT Department.";
                return View();
            }
        }

        [HttpPost]
        public JsonResult CareerDevelopmentSave(string KPIID, string Comments)
        {
            string message = string.Empty;
            List<Model.DTO.Appraisal.CareerDevelopmentComment> lst_comments = new List<Model.DTO.Appraisal.CareerDevelopmentComment>();

            Model.DTO.Appraisal.CareerDevelopmentComment obj_comment = new Model.DTO.Appraisal.CareerDevelopmentComment()
            {
                AppraisalCareerDevelopment = new Model.DTO.Appraisal.CareerDevelopment() { Id = Convert.ToInt32(KPIID) },
                CommentedTimestamp = DateTime.Now,
                Commentor = new Model.DTO.Core.Employee() { Id = CurrentUser.Id },
                Comments = Comments.Trim(),
                FormSaveOnly = true,
            };
            lst_comments.Add(obj_comment);

            Model.PMSModel.UpdateAppraisalCareerDevelopmentComment(lst_comments, out message);
            return Json(message);
        }

        #endregion Career Development

        public Dictionary<string, string> FormCollectionToDictionary(FormCollection form)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            string temp = string.Empty;
            foreach (string key in form.AllKeys.Where(sec => sec.Contains("KPIID")))
            {
                temp = key.Replace("KPIID", "");
                dict.Add(temp, form["CommentContent" + temp]);
            }
            return dict;
        }
    }
}