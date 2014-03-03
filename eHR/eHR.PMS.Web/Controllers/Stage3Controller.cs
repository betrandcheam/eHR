using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ACR_PMSSystem.Models;
using ACR_PMSSystem.Common;
using eHR.PMS.Web.Controllers;

namespace ACR_PMSSystem.Controllers
{
    public class Stage3Controller : BaseController
    {
        //
        // GET: /Stage3/

        public ActionResult KeyPerformanceIndicators(int taskid,int? id)
        {
            PMSPageInfo PI = new PMSPageInfo();
            if (id.HasValue)
            {
                var temp = PMSModel.GetApprisalByID(id.Value);
                ViewData["Stage"] = temp.Stage;
                ViewData["Status"] = temp.Status;
                PMSHelper.GetSectionInfo(PI, id.Value);
                PI.ApprisalId = id.Value;
                PI.TaskId = taskid;
                int sectionId = PMSHelper.GetCurrentSectionID(PI.SCList, RouteData.Values["Action"].ToString());
                PI.CurSectionId = sectionId;
                PMSHelper.GetBlockInfo(PI, sectionId);
                List<PMSCommentsInfo> comments = new List<PMSCommentsInfo>();
                PMSHelper.GetKPIList(PI, id.Value);
                comments = PMSHelper.GetAllKPIComments(PI.KPIList);
                if (comments.Count > 0)
                {
                    foreach (PMSKPIInfo kpiinfo in PI.KPIList)
                    {
                        PMSHelper.GetEachKPIComments(kpiinfo, comments);
                    }
                }
            }
            ViewData.Model = PI;
            return View();
        }

        [HttpPost]
        public ActionResult KeyPerformanceIndicators(int taskid,int id,FormCollection form)
        {
            string message = string.Empty;
            Dictionary<string,string> dict=new Dictionary<string,string>();
            dict = FormCollectionToDict(form);

            if (PMSModel.UpdateKPIScore(dict, 0, out message))
            {
                TempData["AlertMessage"] = "KPI data saved successfully";
                return Redirect("/Stage3/CoreValues/" + taskid + "/" + id);
            }
            else
            {
                TempData["AlertMessage"] = "Ops! KPI data saving failed, please try again or contact IT member";
                return View();
            }

        }
        [HttpPost]
        public JsonResult KPISave(string[] KPIForDatabase)
        {
            string temp = KPIForDatabase[0];
            string[] splitString = { "},{" };
            string[] result = temp.Substring(2,temp.Length-2).Split(splitString, StringSplitOptions.None);
            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (string s in result)
            {
                string[] kparray = s.Replace("\"", "").Split(',');
                dict.Add((kparray[0].Split(':'))[1], (kparray[1].Split(':'))[1]);
            }
            string message = string.Empty;
            PMSModel.UpdateKPIScore(dict, 0, out message);
            return Json(message);
        }

        public ActionResult CoreValues(int? id)
        {
            PMSPageInfo PI = new PMSPageInfo();
            if (id.HasValue)
            {
                var temp = PMSModel.GetApprisalByID(id.Value);
                ViewData["Stage"] = temp.Stage;
                ViewData["Status"] = temp.Status;
                PMSHelper.GetSectionInfo(PI, id.Value);
                PI.ApprisalId = id.Value;
                int sectionId = PMSHelper.GetCurrentSectionID(PI.SCList, RouteData.Values["Action"].ToString());
                PMSHelper.GetBlockInfo(PI, sectionId);
                List<PMSCommentsInfo> comments = new List<PMSCommentsInfo>();
                PMSHelper.GetCoreValuesList(PI, id.Value);
                comments = PMSHelper.GetAllCoreValuesComments(PI.CoreValuesList);
                if (comments.Count > 0)
                {
                    foreach (PMSCoreValuesInfo kpiinfo in PI.CoreValuesList)
                    {
                        PMSHelper.GetEachCoreValuesComments(kpiinfo, comments);
                    }
                }
            }
            ViewData.Model = PI;
            return View();
        }

        [HttpPost]
        public ActionResult CoreValues(int taskid,int id,FormCollection form)
        {
            string message = string.Empty;
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict = FormCollectionToDict(form);

            if (PMSModel.UpdateCoreValueScore(dict, 0, out message))
            {
                TempData["AlertMessage"] = "CoreValue data saved successfully";
                return Redirect("/Stage3/PerformanceCoachingandReview/" + taskid + "/" + id);
            }
            else
            {
                TempData["AlertMessage"] = "Ops! CoreValue data saving failed, please try again or contact IT member";
                return View();
            }
        }

        [HttpPost]
        public JsonResult CoreValueSave(string[] KPIForDatabase)
        {
            string temp = KPIForDatabase[0];
            string[] splitString = { "},{" };
            string[] result = temp.Substring(2, temp.Length - 2).Split(splitString, StringSplitOptions.None);
            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (string s in result)
            {
                string[] kparray = s.Replace("\"", "").Split(',');
                dict.Add((kparray[0].Split(':'))[1], (kparray[1].Split(':'))[1]);
            }
            string message = string.Empty;
            PMSModel.UpdateCoreValueScore(dict, 0, out message);
            return Json(message);
        }
        public ActionResult PerformanceCoachingandReview(int taskid, int? id)
        {
            PMSPageInfo PI = new PMSPageInfo();

            if (id.HasValue)
            {
                var temp = PMSModel.GetApprisalByID(id.Value);
                ViewData["Stage"] = temp.Stage;
                ViewData["Status"] = temp.Status;
                PI.ApprisalId = id.Value;
                PI.TaskId = taskid;
                PMSHelper.GetSectionInfo(PI, id.Value);
            }

            int sectionId = PMSHelper.GetCurrentSectionID(PI.SCList, RouteData.Values["Action"].ToString());
            PI.CurSectionId = sectionId;
            PMSHelper.GetBlockInfo(PI, sectionId);

            PMSHelper.GetPerformanceCoaching(PI, id.Value);
            List<PMSCommentsInfo> comments = new List<PMSCommentsInfo>();
            PMSCommentsInfo curcomments = new PMSCommentsInfo();
            PI.pcInfo.Comments = PMSModel.GetCommentsInfoByPerformanceID(PI.pcInfo.ID, out curcomments);
            PI.pcInfo.CurComment = curcomments;
            //foreach()
            ViewData.Model = PI;
            return View();
        }
        

        public ActionResult CareerDevelopment(int taskid, int? id)
        {
            PMSPageInfo PI = new PMSPageInfo();

            if (id.HasValue)
            {
                var temp = PMSModel.GetApprisalByID(id.Value);
                ViewData["Stage"] = temp.Stage;
                ViewData["Status"] = temp.Status;
                PI.ApprisalId = id.Value;
                PI.TaskId = taskid;
                PMSHelper.GetSectionInfo(PI, id.Value);
            }

            int sectionId = PMSHelper.GetCurrentSectionID(PI.SCList, RouteData.Values["Action"].ToString());
            PI.CurSectionId = sectionId;
            PMSHelper.GetBlockInfo(PI, sectionId);

            PMSHelper.GetCareerDevelopment(PI, id.Value);
            List<PMSCommentsInfo> comments = new List<PMSCommentsInfo>();
            PMSCommentsInfo curcomments = new PMSCommentsInfo();
            PI.cdInfo.Comments = PMSModel.GetCommentsInfoByCareerID(PI.cdInfo.ID, out curcomments);
            PI.cdInfo.CurComment = curcomments;
            //foreach()
            ViewData.Model = PI;
            return View();
        }

        [HttpPost]
        public ActionResult CareerDevelopment(int taskid, int id, FormCollection form)
        {
            string message = string.Empty;
            Dictionary<string, string> dictform = new Dictionary<string, string>();

            dictform.Add("ModuleID", ModuleID.ToString());
            dictform.Add("TaskID", taskid.ToString());
            dictform.Add("UserID", UserInfo.userinfo.ID.ToString());
            dictform.Add("ApprID", id.ToString());
            int ApprovalLevel = PMSHelper.GetApprovalLevel(id, UserInfo.userinfo.ID);
            if (PMSModel.FinishTaskAndUpdateApprStatus(dictform,(int)ApprStatus.New,ApprovalLevel,"", out message))
            {
                TempData["AlertMessage"] = "CarrerDevelopment data saved successfully";
                return Redirect("/");
            }
            else
            {
                TempData["AlertMessage"] = "Ops! CarrerDevelopment data saving failed, please try again or contact IT member";
                return View();
            }
        }

        public Dictionary<string, string> FormCollectionToDict(FormCollection form)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            string temp = string.Empty;
            foreach (string key in form.AllKeys.Where(sec => sec.Contains("KPIID")))
            {
                temp = key.Replace("KPIID", "");
                dict.Add(temp, form["score" + temp]);
            }
            return dict;
        }


    }
}
