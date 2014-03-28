using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Text;

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
                                Appraisal = new Model.DTO.Appraisal.Appraisal() { Id = apprid },
                                SMT = false
                            };
                            lst_reviewers.Add(obj_reviewer);
                        }
                    } 
                }
            }

            if (!Model.PMSModel.UpdateReviewersForAppraisal(apprid, lst_reviewers, out message))
            {
                return Json(message);
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

        [HttpPost]
        public JsonResult SaveProfileSMT(string smt, int appraisalId)
        {
            string message = string.Empty;
            Model.DTO.Appraisal.Reviewer obj_reviewer = null;

            if (!string.IsNullOrEmpty(smt))
            {
                Model.DTO.Appraisal.Appraisal obj_appraisal = Model.PMSModel.GetAppraisalById(appraisalId);
                obj_reviewer = new Model.DTO.Appraisal.Reviewer() 
                { 
                    EmployeeId = Convert.ToInt32(smt),
                    Appraisal = obj_appraisal,
                    SMT = true
                };

                if (!Business.AppraisalManager.ManageChangeSMTMember(obj_appraisal, obj_reviewer, out message))
                {
                    return Json(message);
                }
            }
            return Json(message);
        }

        #endregion Appraisal Profile

        #region pdf export
        public JsonResult pdfExport(int id)
        {
            #region GetFilePath
            string fileName = CurrentUser.PreferredName + "'s Apprisal on "+DateTime.Now.ToString()+".pdf";
           fileName = fileName.Replace("/", "-").Replace(":","-");
            
           string filePath = Path.Combine(Server.MapPath("~/PDFFiles"), fileName);
            #endregion

 
           string logoImgPath = Server.MapPath("~/Content/img/logo.gif");
           string CorevalueImg = Server.MapPath("~/Content/img/pms_corevalue_rating_description.jpg");
           Document doc = new Document(PageSize.A4, 2, 2, 10, 2);

           try
           {
               PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));
               #region set headerfooter
               HeaderFooter header = new HeaderFooter(new Phrase("Aisa Capital Reinsurance", FontFactory.GetFont("Arial", 6, Font.ITALIC)), false);
               header.Border = Rectangle.BOTTOM_BORDER;
               header.BorderColor = Color.GRAY;
               header.BorderWidth = 0.5f;
               header.Alignment = Rectangle.ALIGN_CENTER;
               doc.Header = header;

               HeaderFooter footer = new HeaderFooter(new Phrase("Page: ", FontFactory.GetFont("Arial", 6, Font.ITALIC)), true);
               footer.Border = Rectangle.TOP_BORDER;
               footer.BorderColor = Color.GRAY;
               footer.BorderWidth = 0.5f;
               footer.Alignment = Rectangle.ALIGN_CENTER;
               doc.Footer = footer;
               #endregion
               doc.Open();

               #region define helpercell
               Paragraph empty = new Paragraph("");
               PdfPCell emptycell = new PdfPCell(empty);
               #endregion

               #region writing logo and title
               Paragraph p1 = new Paragraph();
               Image jpg1 = Image.GetInstance(logoImgPath);
               jpg1.Alignment = Element.ALIGN_LEFT;
               p1.Add(jpg1);
               
               Paragraph p2 = new Paragraph(new Phrase(CurrentUser.PreferredName + "'s Apprisal", FontFactory.GetFont("Arial", 20)));
               p2.SetAlignment("center");
               
               doc.Add(p1);
               doc.Add(p2);
               #endregion

               #region define table
               PdfPTable kpiTab = new PdfPTable(4);
               kpiTab.HorizontalAlignment = 1;
               kpiTab.SpacingBefore = 20f;
               kpiTab.SpacingAfter = 20f;


               PdfPTable corevaluesTab = new PdfPTable(2);
               corevaluesTab.HorizontalAlignment = 1;
               corevaluesTab.SpacingBefore = 20f;
               corevaluesTab.SpacingAfter = 20f;

               PdfPTable performanceCoachingTab = new PdfPTable(1);
               performanceCoachingTab.HorizontalAlignment = 1;
               performanceCoachingTab.SpacingBefore = 20f;
               performanceCoachingTab.SpacingAfter = 20f;

               PdfPTable careerDevelopmentTab = new PdfPTable(1);
               careerDevelopmentTab.HorizontalAlignment = 1;
               careerDevelopmentTab.SpacingBefore = 20f;
               careerDevelopmentTab.SpacingAfter = 20f;
               #endregion 

               #region writing kpitable

               List<Model.DTO.Master.Section> sections = PMS.Model.PMSModel.GetMasterSectionList(true);
               Model.DTO.Appraisal.Appraisal appr=PMS.Model.PMSModel.GetAppraisalById(id);
               List<Model.DTO.GradeCompetency> gcList = PMS.Model.PMSModel.GetCoreValueCompetencyByGrade(CurrentUser.ACRGrade.Id);
               Paragraph sectionName = new Paragraph(sections[0].Name+"\n"+"\n");

               PdfPTable descriptionTable=new PdfPTable(1);
               descriptionTable.WidthPercentage=90f;
               PdfPCell pCell = new PdfPCell(new Paragraph(sectionName));
               pCell.Border = Rectangle.NO_BORDER;
               descriptionTable.AddCell(pCell);
               string kpiDescription = "\n Key Performance Indicators Descriptions :\n"
                                        +"\n"
                                        +"\"Financials\", \"Build\", \"Governance/Risk\" and \"People\" are four main themes derived from the Corporate and Underwriting Principles that capture the corporate focus and priorities and serve to align individual's Key Performance Indicators (KPIs) to achieving the Corporate KPIs and Corporate Vision. KPIs are manually set goals and metrics that measure individual's achievements and progress."
                                       + "You will need to set at least 1 or more KPI within each of the four main themes."
                                       + "You are to complete the form and initiate discussion with your Manager."
                                       + "Once you have submitted the form your Manager, your Manager will complete the relevant sections. All submissions and comments will be tracked.\n"
                                        +"\n"
                                        +"Performance Rating Descriptions :\n"
                                        +"\n"
                                        + "5 - Far Exceeds Expections (Exceptional/ Outstanding Performer)\n"
                                        + "4 - Exceeds Expectation (Strong Performer)\n"
                                        + "3 - Meets Expectation (Solid Performer)\n"
                                        + "2 - Improvements Needed (Under Performer)\n"
                                        + "1 - Poor/Does not meet expectation\n\n";
               pCell = new PdfPCell(new Paragraph(kpiDescription));
               pCell.BackgroundColor = new Color(70,136,71);
               descriptionTable.AddCell(pCell);
               
               doc.Add(descriptionTable);
               foreach (Model.DTO.Master.Block b in sections[0].Blocks)
               {
                   Paragraph blockName = new Paragraph("\n\n"+b.Name+"\n\n");
                   PdfPCell cell = new PdfPCell(blockName);
                   cell.BorderWidth=0;
                   cell.Colspan = 4;
                   kpiTab.AddCell(cell);
                   cell = new PdfPCell(new Paragraph("Key Performance Indicator"));
                   cell.BackgroundColor = new Color(91, 192, 222);
                   kpiTab.AddCell(cell);
                   cell = new PdfPCell(new Paragraph("Priority"));
                   cell.BackgroundColor = new Color(91, 192, 222);                   
                   kpiTab.AddCell(cell);
                   cell = new PdfPCell(new Paragraph("Performance Target"));
                   cell.BackgroundColor = new Color(91, 192, 222);
                   kpiTab.AddCell(cell);
                   cell = new PdfPCell(new Paragraph("Comments"));
                   cell.BackgroundColor = new Color(91, 192, 222);
                   kpiTab.AddCell(cell);

                   if (!Lib.Utility.Common.IsNullOrEmptyList(appr.KPIs.Where(s => s.Block.Id == b.Id)))
                   {
                       foreach (Model.DTO.Appraisal.KPI k in appr.KPIs.Where(s => s.Block.Id == b.Id))
                       {
                           kpiTab.AddCell(k.Description);
                           kpiTab.AddCell(k.Priority.Name);
                           kpiTab.AddCell(k.Target);
                           kpiTab.AddCell("");
                           //pdfTab.AddCell(k.c);
                       }
                   }
                   else
                   {
                       emptycell.FixedHeight = 80;
                       kpiTab.AddCell(emptycell);
                       kpiTab.AddCell(emptycell);
                       kpiTab.AddCell(emptycell);
                       kpiTab.AddCell(emptycell);
                   }
               }
               doc.Add(kpiTab);
               #endregion

               #region writing corevaluesTable
               sectionName = new Paragraph(sections[1].Name+"\n\n");
               descriptionTable = new PdfPTable(1);
               descriptionTable.WidthPercentage = 90f;
               pCell = new PdfPCell(new Paragraph(sectionName));
               pCell.Border = Rectangle.NO_BORDER;
               descriptionTable.AddCell(pCell);
               kpiDescription = "\n  ACR Core Values :" +
                                "\n" +
                                "  The ACR Core Values guides our behaviours, the way we conduct business and how we treat our clients and colleagues. Living the values is essential to creating and reinforcing our corporate values.\n" +
                                "\n" +
                                "  All Core Values (RIPPLES) must be evaluated for the year.\n" +
                                "\n" +
                                "  Core Value Ratings :\n" +
                                "\n\n";
               Paragraph corevaluesP = new Paragraph(kpiDescription);
               //Image corevaluejpg = Image.GetInstance(CorevalueImg);
               
               Image corevaluejpg = Image.GetInstance(CorevalueImg);
               corevaluejpg.ScaleAbsolute(230, 158);
               pCell = new PdfPCell(corevaluesP);
               pCell.BackgroundColor = new Color(70, 136, 71);
               descriptionTable.AddCell(pCell);
               PdfPCell pCell1 = new PdfPCell(corevaluejpg,false);
               pCell1.Border = Rectangle.NO_BORDER;
               //pCell1.BackgroundColor = new Color(70, 136, 71);
               descriptionTable.AddCell(pCell1);
               doc.Add(descriptionTable);
               foreach (Model.DTO.Master.Block b in sections[1].Blocks)
               {

                   Paragraph blockName = new Paragraph("\n\n"+b.Name+"\n\n");
                   PdfPCell cell = new PdfPCell(blockName);
                   cell.BorderWidth = 0;
                   cell.Colspan = 2;
                   corevaluesTab.AddCell(cell);
                   cell = new PdfPCell(new Paragraph("Core Competency:"));
                   cell.Colspan = 2;
                   corevaluesTab.AddCell(cell);
                   StringBuilder competency=new StringBuilder();
                   int index = 1;
                   foreach(Model.DTO.GradeCompetency gc in gcList.Where(rec => rec.Block.Id == b.Id))
                   {
                       competency.Append((index++).ToString()+". "+gc.Name+"\n\n");
                                       
                   }
                   cell = new PdfPCell(new Paragraph(competency.ToString()));
                   cell.Colspan = 2;   
                   corevaluesTab.AddCell(cell);

                   cell = new PdfPCell(new Paragraph("Performance Target"));
                   cell.BackgroundColor = new Color(91, 192, 222);
                   corevaluesTab.AddCell(cell);

                   cell = new PdfPCell(new Paragraph("Comments"));
                   cell.BackgroundColor = new Color(91, 192, 222);
                   corevaluesTab.AddCell(cell);
                   if (!Lib.Utility.Common.IsNullOrEmptyList(appr.CoreValues))
                   {
                       foreach (Model.DTO.Appraisal.CoreValue k in appr.CoreValues.Where(s => s.Block.Id == b.Id))
                       {
                           corevaluesTab.AddCell(k.Target);
                           corevaluesTab.AddCell("");
                           //pdfTab.AddCell(k.c);
                       }
                   }
                   else
                   {                    
                       emptycell.FixedHeight = 80;
                       corevaluesTab.AddCell(emptycell);
                       corevaluesTab.AddCell(emptycell);
                   }
               }
               doc.Add(corevaluesTab);
               #endregion

               #region writing performancecoaching
               if (sections.Count > 2)
               {
                   sectionName = new Paragraph(sections[2].Name+"\n\n");
                   pCell = new PdfPCell(new Paragraph(sectionName));
                   pCell.Border = Rectangle.NO_BORDER;
                   performanceCoachingTab.AddCell(pCell);
                   Paragraph content = new Paragraph("Employee's areas of strengths:");
                   PdfPCell cell = new PdfPCell(content);
                   performanceCoachingTab.AddCell(cell);
                   if (!eHR.PMS.Lib.Utility.Common.IsNullOrEmptyList(appr.PerformanceCoachings))
                   {
                       cell = new PdfPCell(new Paragraph(appr.PerformanceCoachings.First().AreasOfStrength));
                       performanceCoachingTab.AddCell(cell);
                   }
                   else
                   {
                       emptycell.FixedHeight = 80;
                       performanceCoachingTab.AddCell(emptycell);
                   }
                   content = new Paragraph("Employee's areas for improvements and developmental needs");
                   cell = new PdfPCell(content);
                   performanceCoachingTab.AddCell(cell);
                   if (!eHR.PMS.Lib.Utility.Common.IsNullOrEmptyList(appr.PerformanceCoachings))
                   {
                       cell = new PdfPCell(new Paragraph(appr.PerformanceCoachings.First().AreasOfImprovement));
                       performanceCoachingTab.AddCell(cell);
                   }
                   else
                   {
                       emptycell.FixedHeight = 80;
                       performanceCoachingTab.AddCell(emptycell);
                   }
                   doc.Add(performanceCoachingTab);
               }
               #endregion

               #region writing careerdevelopment
               if (sections.Count > 3)
               {
                   sectionName = new Paragraph(sections[3].Name+"\n\n");
                   pCell = new PdfPCell(new Paragraph(sectionName));
                   pCell.Border = Rectangle.NO_BORDER;
                   careerDevelopmentTab.AddCell(pCell);
                   Paragraph content = new Paragraph("Short-term Career Goals:");
                   PdfPCell cell = new PdfPCell(content);
                   careerDevelopmentTab.AddCell(cell);
                   if (!eHR.PMS.Lib.Utility.Common.IsNullOrEmptyList(appr.CareerDevelopments))
                   {
                       cell = new PdfPCell(new Paragraph(appr.CareerDevelopments.First().ShortTermGoals));
                       careerDevelopmentTab.AddCell(cell);
                   }
                   else
                   {
                       emptycell.FixedHeight = 80;
                       careerDevelopmentTab.AddCell(emptycell);
                   }
                   content = new Paragraph("Career Development Plan:");
                   cell = new PdfPCell(content);
                   careerDevelopmentTab.AddCell(cell);
                   if (!eHR.PMS.Lib.Utility.Common.IsNullOrEmptyList(appr.CareerDevelopments))
                   {
                       cell = new PdfPCell(new Paragraph(appr.CareerDevelopments.First().CareerPlans));
                       careerDevelopmentTab.AddCell(cell);
                   }
                   else
                   {
                       emptycell.FixedHeight = 80;
                       careerDevelopmentTab.AddCell(emptycell);
                   }
                   content = new Paragraph("Learning and development:");
                   cell = new PdfPCell(content);
                   careerDevelopmentTab.AddCell(cell);
                   if (!eHR.PMS.Lib.Utility.Common.IsNullOrEmptyList(appr.CareerDevelopments))
                   {
                       cell = new PdfPCell(new Paragraph(appr.CareerDevelopments.First().LearningPlans));
                       careerDevelopmentTab.AddCell(cell);
                   }
                   else
                   {
                       emptycell.FixedHeight = 80;
                       careerDevelopmentTab.AddCell(emptycell);
                   }
                   doc.Add(careerDevelopmentTab);
               }
               #endregion

               doc.Close();
               return Json(Url.Content("~/PDFFiles/"+fileName));
               //return File(filePath, "application/pdf", fileName);
           }
           catch (Exception)
           {
               throw;
           }
           finally
           {
               doc.Close();
           }
            
        }
        #endregion pdf export
    }
}
