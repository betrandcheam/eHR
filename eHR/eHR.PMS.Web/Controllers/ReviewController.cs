using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ACR_PMSSystem.Models;
using ACR_PMSSystem.Common;

namespace ACR_PMSSystem.Controllers
{
    public class ReviewController : Controller
    {
        //
        // GET: /Review/

        public ActionResult KeyPerformanceIndicators(int? id)
        {
            PMSPageInfo PI = new PMSPageInfo();
            if (id.HasValue)
            {
                var temp = PMSModel.GetApprisalByID(id.Value);
                ViewData["Stage"] = temp.Stage;
                ViewData["Status"] = temp.Status;
                PI.StageID=PMSModel.GetCurrentStageID(id.Value);
                PMSHelper.GetSectionInfo(PI, id.Value);
                PI.ApprisalId = id.Value;
                int sectionId = PMSHelper.GetCurrentSectionID(PI.SCList, RouteData.Values["Action"].ToString());
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

        public ActionResult CoreValues(int? id)
        {
            PMSPageInfo PI = new PMSPageInfo();
            if (id.HasValue)
            {
                var temp = PMSModel.GetApprisalByID(id.Value);
                ViewData["Stage"] = temp.Stage;
                ViewData["Status"] = temp.Status;
                PI.StageID = PMSModel.GetCurrentStageID(id.Value);
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
    }
}
