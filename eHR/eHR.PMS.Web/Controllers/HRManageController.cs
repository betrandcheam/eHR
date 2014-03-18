using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;

namespace eHR.PMS.Web.Controllers
{
    public class HRManageController : BaseController
    {
        private int int_page_size = 10;

        #region New Cycle

        public ActionResult NewCycle(string cycleDateRangeStart, string cycleDateRangeEnd, int? cycleId)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            Models.DTO.NewCycleManagementPage obj_cycle_management_page = new Models.DTO.NewCycleManagementPage();

            if (string.IsNullOrEmpty(cycleDateRangeStart) || string.IsNullOrEmpty(cycleDateRangeEnd))
            {
                obj_cycle_management_page.CurrentCycle = new PMS.Model.DTO.Cycle.Cycle();
                dict.Add("Stage1EndDate", "");
                dict.Add("Stage3EndDate", "");
                dict.Add("strStage1EndDate", "");
                dict.Add("strStage3EndDate", "");

                dict.Add("Stage1SubmissionDeadline", "");
                dict.Add("Stage1Level1ApprovalDeadline", "");
                dict.Add("Stage1Level2ApprovalDeadline", "");
                dict.Add("Stage2SubmissionDeadline", "");
                dict.Add("Stage2Level1ApprovalDeadline", "");
                dict.Add("Stage2Level2ApprovalDeadline", "");
                TempData["QueryData"] = dict;
            }
            else
            {
                cycleDateRangeStart = Lib.Utility.Common.ChangeDateFormat(cycleDateRangeStart);
                cycleDateRangeEnd = Lib.Utility.Common.ChangeDateFormat(cycleDateRangeEnd);
                string key = cycleDateRangeStart + " " + cycleDateRangeEnd;

                obj_cycle_management_page.Participants = (List<Model.DTO.Core.Employee>)System.Web.HttpContext.Current.Session["CycleParticipantsList"];

                dict.Add("cyclename", (string)System.Web.HttpContext.Current.Session["CycleName"]);
                dict.Add("Stage1StartDate", (string)System.Web.HttpContext.Current.Session["Stage1StartDate"]);
                dict.Add("Stage1EndDate", (string)System.Web.HttpContext.Current.Session["Stage1EndDate"]);
                dict.Add("Stage2StartDate", (string)System.Web.HttpContext.Current.Session["Stage2StartDate"]);
                dict.Add("Stage2EndDate", (string)System.Web.HttpContext.Current.Session["Stage2EndDate"]);
                dict.Add("Stage3StartDate", (string)System.Web.HttpContext.Current.Session["Stage3StartDate"]);
                dict.Add("Stage3EndDate", (string)System.Web.HttpContext.Current.Session["Stage3EndDate"]);
                dict.Add("strStage1EndDate", Lib.Utility.Common.ChangeDateFormatVS(dict["Stage1EndDate"]));
                dict.Add("strStage3EndDate", Lib.Utility.Common.ChangeDateFormatVS(dict["Stage3EndDate"]));

                dict.Add("Stage1SubmissionDeadline", (string)System.Web.HttpContext.Current.Session["Stage1SubmissionDeadline"]);
                dict.Add("Stage1Level1ApprovalDeadline", (string)System.Web.HttpContext.Current.Session["Stage1Level1ApprovalDeadline"]);
                dict.Add("Stage1Level2ApprovalDeadline", (string)System.Web.HttpContext.Current.Session["Stage1Level2ApprovalDeadline"]);

                dict.Add("Stage2SubmissionDeadline", (string)System.Web.HttpContext.Current.Session["Stage2SubmissionDeadline"]);
                dict.Add("Stage2Level1ApprovalDeadline", (string)System.Web.HttpContext.Current.Session["Stage2Level1ApprovalDeadline"]);
                dict.Add("Stage2Level2ApprovalDeadline", (string)System.Web.HttpContext.Current.Session["Stage2Level2ApprovalDeadline"]);

                TempData["QueryData"] = dict;
            }

            //ModelState.Clear();
            ViewData.Model = obj_cycle_management_page;
            return View();
        }

        [HttpPost]
        public ActionResult NewCycle(FormCollection form)
        {
            string message = string.Empty;
            Dictionary<string, string> dict = FormCollectionToDict(form);
            bool boo_start_cycle = Convert.ToInt32(dict["startcycle"]) == 1 ? true : false;
            Models.DTO.NewCycleManagementPage obj_cycle_management_page = new Models.DTO.NewCycleManagementPage();

            if (!boo_start_cycle)
            {
                if (form["Stage1EndDate"] != null && form["Stage3EndDate"] != null)
                {
                    if (string.IsNullOrEmpty(dict["Stage1EndDate"]) || string.IsNullOrEmpty(dict["Stage1EndDate"]))
                    {
                        ViewData.Model = obj_cycle_management_page;
                        TempData["ErrorMessage"] = "Please enter all the dates for the appraisal cycle before retrieving the participants.";
                        return View();
                    }

                    DateTime eligibilityRangeStart = DateTime.ParseExact(form["Stage1EndDate"].ToString(), "d/M/yyyy", null);
                    DateTime eligibilityRangeEnd = DateTime.ParseExact(form["Stage3EndDate"].ToString(), "d/M/yyyy", null);

                    List<Model.DTO.Core.Employee> lst_selected_employees = Business.AppraisalManager.GetEligibleEmployeesForCycle(eligibilityRangeStart, eligibilityRangeEnd);

                    if (!Lib.Utility.Common.IsNullOrEmptyList(lst_selected_employees))
                    {
                        System.Web.HttpContext.Current.Session.Add("CycleParticipantsList", lst_selected_employees);
                        obj_cycle_management_page.Participants = lst_selected_employees.OrderBy(rec => rec.Department.Name).OrderBy(rec => rec.PreferredName).ToList();
                    }
                    else
                    {
                        obj_cycle_management_page.Participants = new List<Model.DTO.Core.Employee>();
                    }

                    System.Web.HttpContext.Current.Session.Add("CycleName", dict["cyclename"].ToString());
                    System.Web.HttpContext.Current.Session.Add("Stage1StartDate", dict["Stage1StartDate"].ToString());
                    System.Web.HttpContext.Current.Session.Add("Stage1EndDate", dict["Stage1EndDate"].ToString());
                    System.Web.HttpContext.Current.Session.Add("Stage2StartDate", dict["Stage2StartDate"].ToString());
                    System.Web.HttpContext.Current.Session.Add("Stage2EndDate", dict["Stage2EndDate"].ToString());
                    System.Web.HttpContext.Current.Session.Add("Stage3StartDate", dict["Stage3StartDate"].ToString());
                    System.Web.HttpContext.Current.Session.Add("Stage3EndDate", dict["Stage3EndDate"].ToString());

                    System.Web.HttpContext.Current.Session.Add("Stage1SubmissionDeadline", dict["Stage1SubmissionDeadline"].ToString());
                    System.Web.HttpContext.Current.Session.Add("Stage1Level1ApprovalDeadline", dict["Stage1Level1ApprovalDeadline"].ToString());
                    System.Web.HttpContext.Current.Session.Add("Stage1Level2ApprovalDeadline", dict["Stage1Level2ApprovalDeadline"].ToString());

                    System.Web.HttpContext.Current.Session.Add("Stage2SubmissionDeadline", dict["Stage1SubmissionDeadline"].ToString());
                    System.Web.HttpContext.Current.Session.Add("Stage2Level1ApprovalDeadline", dict["Stage1Level1ApprovalDeadline"].ToString());
                    System.Web.HttpContext.Current.Session.Add("Stage2Level2ApprovalDeadline", dict["Stage1Level2ApprovalDeadline"].ToString());                
                }

                ViewData.Model = obj_cycle_management_page;

                dict.Add("strStage1EndDate", Lib.Utility.Common.ChangeDateFormatVS(dict["Stage1EndDate"].ToString()));
                dict.Add("strStage3EndDate", Lib.Utility.Common.ChangeDateFormatVS(dict["Stage3EndDate"].ToString()));
                TempData["QueryData"] = dict;
                return View();
            }
            else
            {
                if (System.Web.HttpContext.Current.Session["CycleParticipantsList"] != null)
                {
                    PMS.Model.DTO.Cycle.Cycle obj_cycle = new PMS.Model.DTO.Cycle.Cycle()
                    {
                        Name = form["cyclename"].Trim(),
                        Stage = new Model.DTO.Master.Stage() { Id = Model.PMSConstants.STAGE_ID_PRE_CYCLE },
                        Status = new Model.DTO.Master.Status() { Id = Model.PMSConstants.STATUS_ID_OPEN },
                        Starter = new Model.DTO.Core.Employee() { Id = CurrentUser.Id },
                        StartedTimestamp = DateTime.Now
                    };

                    Dictionary<string, DateTime> dict_cycle_stage_dates = new Dictionary<string, DateTime>();
                    dict_cycle_stage_dates.Add("PreCStart", DateTime.Now.Date);
                    dict_cycle_stage_dates.Add("PreCEnd", DateTime.ParseExact(dict["Stage1StartDate"], "dd/MM/yyyy", null).AddDays(-1));
                    dict_cycle_stage_dates.Add("Stage1StartDate", DateTime.ParseExact(dict["Stage1StartDate"], "dd/MM/yyyy", null));
                    dict_cycle_stage_dates.Add("Stage1EndDate", DateTime.ParseExact(dict["Stage1EndDate"], "dd/MM/yyyy", null));
                    dict_cycle_stage_dates.Add("Stage2StartDate", DateTime.ParseExact(dict["Stage2StartDate"], "dd/MM/yyyy", null));
                    dict_cycle_stage_dates.Add("Stage2EndDate", DateTime.ParseExact(dict["Stage2EndDate"], "dd/MM/yyyy", null));
                    dict_cycle_stage_dates.Add("Stage3StartDate", DateTime.ParseExact(dict["Stage3StartDate"], "dd/MM/yyyy", null));
                    dict_cycle_stage_dates.Add("Stage3EndDate", DateTime.ParseExact(dict["Stage3EndDate"], "dd/MM/yyyy", null));

                    dict_cycle_stage_dates.Add("Stage1SubmissionDeadline", DateTime.ParseExact(dict["Stage1SubmissionDeadline"], "dd/MM/yyyy", null));
                    dict_cycle_stage_dates.Add("Stage1Level1ApprovalDeadline", DateTime.ParseExact(dict["Stage1Level1ApprovalDeadline"], "dd/MM/yyyy", null));
                    dict_cycle_stage_dates.Add("Stage1Level2ApprovalDeadline", DateTime.ParseExact(dict["Stage1Level2ApprovalDeadline"], "dd/MM/yyyy", null));
                    dict_cycle_stage_dates.Add("Stage2SubmissionDeadline", DateTime.ParseExact(dict["Stage2SubmissionDeadline"], "dd/MM/yyyy", null));
                    dict_cycle_stage_dates.Add("Stage2Level1ApprovalDeadline", DateTime.ParseExact(dict["Stage2Level1ApprovalDeadline"], "dd/MM/yyyy", null));
                    dict_cycle_stage_dates.Add("Stage2Level2ApprovalDeadline", DateTime.ParseExact(dict["Stage2Level2ApprovalDeadline"], "dd/MM/yyyy", null));

                    obj_cycle.CycleStages = CreateDefaultStagesForNewCycle(dict_cycle_stage_dates);

                    obj_cycle.Appriasals = Business.AppraisalManager.CreateAppraisalsForNewCycle((List<Model.DTO.Core.Employee>)System.Web.HttpContext.Current.Session["CycleParticipantsList"], obj_cycle.CycleStages, CurrentUser);

                    if (Business.AppraisalManager.CreateNewCycle(obj_cycle, Model.Mappers.CoreMapper.MapUserDTOToEmployeeDTO(CurrentUser), out message))
                    {
                        ClearAllCreatedSessionObjects();
                        return Redirect(Url.Content("~/"));
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Unable to save cycle information. Please try again or contact IT Department.";
                        return View();
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Cycle information is not found. Please try again or contact IT Department.";
                    return View();
                }
            }
        }

        #endregion New Cycle

        #region Manage Cycle

        public ActionResult ManageCycle(string cycleDateRangeStart, string cycleDateRangeEnd,int? cycleId)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            Models.DTO.CycleManagementPage obj_cycle_management_page = new Models.DTO.CycleManagementPage();
            obj_cycle_management_page.Cycles = eHR.PMS.Model.PMSModel.GetCycleByStatus(null);
            if (string.IsNullOrEmpty(cycleDateRangeStart) || string.IsNullOrEmpty(cycleDateRangeEnd))
            {
                //obj_cycle_management_page.Cycle = new PMS.Model.DTO.Cycle.Cycle();
                ;
            }
            else
            {
                cycleDateRangeStart = Lib.Utility.Common.ChangeDateFormat(cycleDateRangeStart);
                cycleDateRangeEnd = Lib.Utility.Common.ChangeDateFormat(cycleDateRangeEnd);
                //string key = cycleDateRangeStart + " " + cycleDateRangeEnd;
                if (cycleId.HasValue)
                {
                    obj_cycle_management_page.CurrentCycle = eHR.PMS.Model.PMSModel.GetCycleById(cycleId.Value);
                    obj_cycle_management_page.CurrentCycle.CycleStages = eHR.PMS.Model.PMSModel.GetStagesByCycleId(cycleId.Value);
                    List<Model.DTO.Appraisal.Appraisal> templist = eHR.PMS.Model.PMSModel.GetEmployeesInAppraisalsByCycleId(cycleId.Value);
                    if (System.Web.HttpContext.Current.Session["TempRemoveApprIds"] != null)
                    {
                        List<int> tempRemoveApprIds = (List<int>)System.Web.HttpContext.Current.Session["TempRemoveApprIds"];
                        templist = eHR.PMS.Model.PMSModel.GetEmployeesInAppraisalsByCycleId(cycleId.Value).Where(sec => !tempRemoveApprIds.Contains(sec.Id)).ToList();
                    }
                    if (System.Web.HttpContext.Current.Session["CycleExistParticipantsList"] != null)
                        templist.AddRange((List<Model.DTO.Appraisal.Appraisal>)System.Web.HttpContext.Current.Session["CycleExistParticipantsList"]);
                    obj_cycle_management_page.Participants = templist;
                }
                else
                    obj_cycle_management_page.Participants = (List<Model.DTO.Appraisal.Appraisal>)System.Web.HttpContext.Current.Session["CycleExistParticipantsList"];

                /*dict.Add("cyclename", (string)System.Web.HttpContext.Current.Session["CycleName"]);
                dict.Add("Stage1StartDate", (string)System.Web.HttpContext.Current.Session["Stage1StartDate"]);
                dict.Add("Stage1EndDate", (string)System.Web.HttpContext.Current.Session["Stage1EndDate"]);
                dict.Add("Stage2StartDate", (string)System.Web.HttpContext.Current.Session["Stage2StartDate"]);
                dict.Add("Stage2EndDate", (string)System.Web.HttpContext.Current.Session["Stage2EndDate"]);
                dict.Add("Stage3StartDate", (string)System.Web.HttpContext.Current.Session["Stage3StartDate"]);
                dict.Add("Stage3EndDate", (string)System.Web.HttpContext.Current.Session["Stage3EndDate"]);
                dict.Add("strStage1EndDate", Lib.Utility.Common.ChangeDateFormatVS(dict["Stage1EndDate"]));
                dict.Add("strStage3EndDate", Lib.Utility.Common.ChangeDateFormatVS(dict["Stage3EndDate"]));
                TempData["QueryData"] = dict;*/
            }

            ModelState.Clear();
            ViewData.Model = obj_cycle_management_page;
            ViewData["NowDate"] = DateTime.Now.ToString("yyyy-MM-dd");
            return View();
        }

        [HttpPost]
        public ActionResult ManageCycle(FormCollection form)
        {
            string message = string.Empty;
            Dictionary<string, string> dict = FormCollectionToDict(form);
            bool boo_update_cycle = Convert.ToInt32(dict["updatecycle"]) == 1 ? true : false;
            Models.DTO.CycleManagementPage obj_cycle_management_page = new Models.DTO.CycleManagementPage();
            obj_cycle_management_page.Cycles = eHR.PMS.Model.PMSModel.GetCycleByStatus(null);
            int cycleid = Int32.Parse(form["cycleid"]);
            if (!boo_update_cycle)
            {
                if (cycleid > 0)
                {
                    obj_cycle_management_page.CurrentCycle = eHR.PMS.Model.PMSModel.GetCycleById(cycleid);
                    obj_cycle_management_page.CurrentCycle.CycleStages = eHR.PMS.Model.PMSModel.GetStagesByCycleId(cycleid);
                    obj_cycle_management_page.Participants = eHR.PMS.Model.PMSModel.GetEmployeesInAppraisalsByCycleId(cycleid);
                }
            }
            else
            {
                PMS.Model.DTO.Cycle.Cycle obj_cycle = eHR.PMS.Model.PMSModel.GetCycleById(cycleid);

                Dictionary<string, DateTime> dict_cycle_stage_dates = new Dictionary<string, DateTime>();
                dict_cycle_stage_dates.Add("PreCStart", DateTime.Now.Date);
                dict_cycle_stage_dates.Add("PreCEnd", DateTime.ParseExact(dict["Stage1StartDate"], "dd/MM/yyyy", null).AddDays(-1));
                dict_cycle_stage_dates.Add("Stage1StartDate", DateTime.ParseExact(dict["Stage1StartDate"], "dd/MM/yyyy", null));
                dict_cycle_stage_dates.Add("Stage1EndDate", DateTime.ParseExact(dict["Stage1EndDate"], "dd/MM/yyyy", null));
                dict_cycle_stage_dates.Add("Stage2StartDate", DateTime.ParseExact(dict["Stage2StartDate"], "dd/MM/yyyy", null));
                dict_cycle_stage_dates.Add("Stage2EndDate", DateTime.ParseExact(dict["Stage2EndDate"], "dd/MM/yyyy", null));
                dict_cycle_stage_dates.Add("Stage3StartDate", DateTime.ParseExact(dict["Stage3StartDate"], "dd/MM/yyyy", null));
                dict_cycle_stage_dates.Add("Stage3EndDate", DateTime.ParseExact(dict["Stage3EndDate"], "dd/MM/yyyy", null));

                List<int> deleteApprIdList = new List<int>();
                if (System.Web.HttpContext.Current.Session["TempRemoveApprIds"] != null)
                {
                    deleteApprIdList = (List<int>)System.Web.HttpContext.Current.Session["TempRemoveApprIds"];
                    if (eHR.PMS.Model.PMSModel.DeleteApprisalInCyle(deleteApprIdList, out message))
                    {
                        ClearAllCreatedSessionObjects();
                        return Redirect(Url.Content("~/"));
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Unable to save cycle information. Please try again or contact IT Department.";
                        return View();
                    }
                }

                List<PMS.Model.DTO.Appraisal.Appraisal> ParticipantsListSessionPart = (List<PMS.Model.DTO.Appraisal.Appraisal>)System.Web.HttpContext.Current.Session["CycleExistParticipantsList"];
                if (ParticipantsListSessionPart != null)
                {

                    obj_cycle.CycleStages = CreateDefaultStagesForNewCycle(dict_cycle_stage_dates);
                    List<Model.DTO.Core.Employee> emplist = ApprListToEmplList(ParticipantsListSessionPart);
                    obj_cycle.Appriasals = Business.AppraisalManager.CreateAppraisalsForUpdateCycle(emplist, obj_cycle.CycleStages, CurrentUser);
                    
                    if (Business.AppraisalManager.UpdateCycle(obj_cycle, cycleid, Model.Mappers.CoreMapper.MapUserDTOToEmployeeDTO(CurrentUser), out message))
                    {
                        ClearAllCreatedSessionObjects();
                        return Redirect(Url.Content("~/"));
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Unable to save cycle information. Please try again or contact IT Department.";
                        return View();
                    }
                }
                /*else
                {
                    TempData["ErrorMessage"] = "Unable to save cycle information. Please try again or contact IT Department.";
                    return View();
                }*/
            }
            TempData["QueryData"] = dict;
            ViewData["NowDate"] = DateTime.Now.ToString("yyyy-MM-dd");
            ViewData.Model = obj_cycle_management_page;
            return View();
        }


        #endregion Manage Cycle

        #region Add Participants

        public ActionResult AddNewParticipants(string cycleDateRangeStart, string cycleDateRangeEnd)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            Models.DTO.NewCycleManagementPage obj_cycle_management_page = new Models.DTO.NewCycleManagementPage();

            if (string.IsNullOrEmpty(cycleDateRangeStart) || string.IsNullOrEmpty(cycleDateRangeEnd))
            {
                ViewData["Stage1EndDate"] = "";
                ViewData["Stage3EndDate"] = "";

                dict.Add("Stage1EndDate", "");
                dict.Add("Stage3EndDate", "");
            }
            else
            {
                dict.Add("Stage1EndDate", Lib.Utility.Common.ChangeDateFormat(cycleDateRangeStart));
                dict.Add("Stage3EndDate", Lib.Utility.Common.ChangeDateFormat(cycleDateRangeEnd));
                ViewData["Stage1EndDate"] = Lib.Utility.Common.ChangeDateFormat(cycleDateRangeStart);
                ViewData["Stage3EndDate"] = Lib.Utility.Common.ChangeDateFormat(cycleDateRangeEnd);
            }
            ViewData.Model = obj_cycle_management_page;
            return View();
        }

        [HttpPost]
        public ActionResult AddNewParticipants(string cycleDateRangeStart, string cycleDateRangeEnd, FormCollection form)
        {
            Dictionary<string, string> dict = FormCollectionToDict(form);
            Models.DTO.NewCycleManagementPage obj_cycle_management_page = new Models.DTO.NewCycleManagementPage();
            List<Model.DTO.Core.Employee> lst_current_participants = null;

            if (System.Web.HttpContext.Current.Session["CycleParticipantsList"] != null)
            {
                lst_current_participants = (List<Model.DTO.Core.Employee>)System.Web.HttpContext.Current.Session["CycleParticipantsList"];
            }

            obj_cycle_management_page.Participants = Business.AppraisalManager.GetEmployeesToAddToCycle(form.GetValue("EmployeeName").AttemptedValue == null ? null : form.GetValue("EmployeeName").AttemptedValue.Trim(),
                                                                                                        form.GetValue("DomainID").AttemptedValue == null ? null : form.GetValue("DomainID").AttemptedValue.Trim(),
                                                                                                        form.GetValue("DepartmentName").AttemptedValue == null ? null : form.GetValue("DepartmentName").AttemptedValue.Trim(),
                                                                                                        lst_current_participants);

            if (!Lib.Utility.Common.IsNullOrEmptyList(obj_cycle_management_page.Participants))
            {
                obj_cycle_management_page.Participants = obj_cycle_management_page.Participants.OrderBy(rec => rec.Department.Name).OrderBy(rec => rec.PreferredName).ToList();
                System.Web.HttpContext.Current.Session.Add("CycleNewParticipants", obj_cycle_management_page.Participants);
            }

            if (string.IsNullOrEmpty(cycleDateRangeStart) || string.IsNullOrEmpty(cycleDateRangeEnd))
            {
                ViewData["Stage1EndDate"] = "";
                ViewData["Stage3EndDate"] = "";
                dict.Add("Stage1EndDate", "");
                dict.Add("Stage3EndDate", "");
            }
            else
            {
                dict.Add("Stage1EndDate", Lib.Utility.Common.ChangeDateFormat(cycleDateRangeStart));
                dict.Add("Stage3EndDate", Lib.Utility.Common.ChangeDateFormat(cycleDateRangeEnd));
                ViewData["Stage1EndDate"] = Lib.Utility.Common.ChangeDateFormat(cycleDateRangeStart);
                ViewData["Stage3EndDate"] = Lib.Utility.Common.ChangeDateFormat(cycleDateRangeEnd);
            }

            ViewData.Model = obj_cycle_management_page;
            TempData["QueryData"] = dict;
            return View();
        }

        [HttpPost]
        public JsonResult ConfirmAddNew(string cycleDateRangeStart, string cycleDateRangeEnd, string EIForCache)
        {
            List<Model.DTO.Core.Employee> lst_current_participants = null;
            List<Model.DTO.Core.Employee> lst_new_participants = null;
            try
            {
                Dictionary<string, string> dict = new Dictionary<string, string>();
                dict.Add("Stage1EndDate", cycleDateRangeStart);
                dict.Add("Stage3EndDate", cycleDateRangeEnd);

                if (System.Web.HttpContext.Current.Session["CycleNewParticipants"] != null)
                {
                    lst_new_participants = (List<Model.DTO.Core.Employee>)System.Web.HttpContext.Current.Session["CycleNewParticipants"];
                }

                string[] splitString = { "ONERECORDENDED," };
                string[] result = EIForCache.Split(splitString, StringSplitOptions.None);
                string[] obj = new string[6];
                string[] splitString2 = { "^&*" };

                if (!Lib.Utility.Common.IsNullOrEmptyList(result))
                {
                    foreach (string str_result in result)
                    {
                        obj = str_result.Split(splitString2, StringSplitOptions.None);
                        if (!Lib.Utility.Common.IsNullOrEmptyList(obj))
                        {
                            Model.DTO.Core.Employee obj_employee = lst_new_participants.Where(rec => rec.Id == Convert.ToInt32(obj[0])).SingleOrDefault();

                            if (obj_employee == null)
                            {
                                throw new Exception("Unable to added selected employees.");
                            }

                            if (System.Web.HttpContext.Current.Session["CycleParticipantsList"] != null)
                            {
                                lst_current_participants = (List<Model.DTO.Core.Employee>)System.Web.HttpContext.Current.Session["CycleParticipantsList"];
                            }

                            if (Lib.Utility.Common.IsNullOrEmptyList(lst_current_participants))
                            {
                                lst_current_participants = new List<Model.DTO.Core.Employee>();
                            }

                            lst_current_participants.Add(obj_employee);
                            System.Web.HttpContext.Current.Session.Add("CycleParticipantsList", lst_current_participants);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                return Json("Unable to add employees to list.");
            }
            return Json("Employees are added successfully");

        }

        #endregion Add Participants

        #region Remove NewParticipants

        public ActionResult RemoveNewParticipants(string cycleDateRangeStart, string cycleDateRangeEnd)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            Models.DTO.NewCycleManagementPage obj_cycle_management_page = new Models.DTO.NewCycleManagementPage();

            if (System.Web.HttpContext.Current.Session["CycleParticipantsList"] != null)
            {
                obj_cycle_management_page.Participants = (List<Model.DTO.Core.Employee>)System.Web.HttpContext.Current.Session["CycleParticipantsList"];
            }

            if (Lib.Utility.Common.IsNullOrEmptyList(obj_cycle_management_page.Participants))
            {
                ViewData["AlertMessage"] = "There are no employees to remove.";
                return Redirect(Url.Content("~/HRManage/NewCycle/" + cycleDateRangeStart + "/" + cycleDateRangeEnd));
            }

            if (string.IsNullOrEmpty(cycleDateRangeStart) || string.IsNullOrEmpty(cycleDateRangeEnd))
            {
                ViewData["Stage1EndDate"] = "";
                ViewData["Stage3EndDate"] = "";
                dict.Add("Stage1EndDate", "");
                dict.Add("Stage3EndDate", "");
            }
            else
            {
                dict.Add("Stage1EndDate", Lib.Utility.Common.ChangeDateFormat(cycleDateRangeStart));
                dict.Add("Stage3EndDate", Lib.Utility.Common.ChangeDateFormat(cycleDateRangeEnd));
                ViewData["Stage1EndDate"] = Lib.Utility.Common.ChangeDateFormat(cycleDateRangeStart);
                ViewData["Stage3EndDate"] = Lib.Utility.Common.ChangeDateFormat(cycleDateRangeEnd);
            }


            ModelState.Clear();
            ViewData.Model = obj_cycle_management_page;
            return View();
        }

        [HttpPost]
        public ActionResult RemoveNewParticipants(string cycleDateRangeStart, string cycleDateRangeEnd, FormCollection form)
        {
            Dictionary<string, string> dict = FormCollectionToDict(form);
            Models.DTO.NewCycleManagementPage obj_cycle_management_page = new Models.DTO.NewCycleManagementPage();
            List<Model.DTO.Core.Employee> lst_employees = null;

            if (System.Web.HttpContext.Current.Session["CycleParticipantsList"] != null)
            {
                lst_employees = (List<Model.DTO.Core.Employee>)System.Web.HttpContext.Current.Session["CycleParticipantsList"];
            }

            if (string.IsNullOrEmpty(dict["EmployeeName"].Trim()) && string.IsNullOrEmpty(dict["DomainID"].Trim()) && string.IsNullOrEmpty(dict["DepartmentName"].Trim()))
            {
                obj_cycle_management_page.Participants = lst_employees;
            }
            else
            {
                obj_cycle_management_page.Participants = Business.AppraisalManager.GetEmployeesToRemoveFromCycle(dict["EmployeeName"].Trim(), dict["DomainID"].Trim(), dict["DepartmentName"].Trim(), lst_employees);
            }

            if (string.IsNullOrEmpty(cycleDateRangeStart) || string.IsNullOrEmpty(cycleDateRangeEnd))
            {
                ViewData["Stage1EndDate"] = "";
                ViewData["Stage3EndDate"] = "";
                dict.Add("Stage1EndDate", "");
                dict.Add("Stage3EndDate", "");
            }
            else
            {
                dict.Add("Stage1EndDate", Lib.Utility.Common.ChangeDateFormat(cycleDateRangeStart));
                dict.Add("Stage3EndDate", Lib.Utility.Common.ChangeDateFormat(cycleDateRangeEnd));
                ViewData["Stage1EndDate"] = Lib.Utility.Common.ChangeDateFormat(cycleDateRangeStart);
                ViewData["Stage3EndDate"] = Lib.Utility.Common.ChangeDateFormat(cycleDateRangeEnd);
            }

            ViewData.Model = obj_cycle_management_page;
            TempData["QueryData"] = dict;
            return View();
        }

        [HttpPost]
        public JsonResult ConfirmRemoveNew(string cycleDateRangeStart, string cycleDateRangeEnd, string EIForCache)
        {
            List<Model.DTO.Core.Employee> lst_current_participants = null;
            try
            {
                Dictionary<string, string> dict = new Dictionary<string, string>();
                dict.Add("Stage1EndDate", cycleDateRangeStart);
                dict.Add("Stage3EndDate", cycleDateRangeEnd);

                if (System.Web.HttpContext.Current.Session["CycleParticipantsList"] != null)
                {
                    lst_current_participants = (List<Model.DTO.Core.Employee>)System.Web.HttpContext.Current.Session["CycleParticipantsList"];
                }

                string[] splitString = { "ONERECORDENDED," };
                string[] result = EIForCache.Split(splitString, StringSplitOptions.None);
                string[] obj = new string[6];
                string[] splitString2 = { "^&*" };

                if (!Lib.Utility.Common.IsNullOrEmptyList(result))
                {
                    foreach (string str_result in result)
                    {
                        obj = str_result.Split(splitString2, StringSplitOptions.None);
                        if (!Lib.Utility.Common.IsNullOrEmptyList(obj))
                        {
                            lst_current_participants.RemoveAll(rec => rec.Id == Convert.ToInt32(obj[0]));
                            System.Web.HttpContext.Current.Session.Add("CycleParticipantsList", lst_current_participants);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return Json("Unable to remove employee from list.");
            }
            return Json("Employees are removed successfully.");
        }

        #endregion Remove NewParticipants

        #region Add ExsitParticipants

        public ActionResult AddParticipants(string cycleDateRangeStart, string cycleDateRangeEnd,int cycleId)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            Models.DTO.CycleManagementPage obj_cycle_management_page = new Models.DTO.CycleManagementPage();

            if (string.IsNullOrEmpty(cycleDateRangeStart) || string.IsNullOrEmpty(cycleDateRangeEnd))
            {
                ViewData["Stage1EndDate"] = "";
                ViewData["Stage3EndDate"] = "";

                dict.Add("Stage1EndDate", "");
                dict.Add("Stage3EndDate", "");
            }
            else
            {
                dict.Add("Stage1EndDate", Lib.Utility.Common.ChangeDateFormat(cycleDateRangeStart));
                dict.Add("Stage3EndDate", Lib.Utility.Common.ChangeDateFormat(cycleDateRangeEnd));
                ViewData["Stage1EndDate"] = Lib.Utility.Common.ChangeDateFormat(cycleDateRangeStart);
                ViewData["Stage3EndDate"] = Lib.Utility.Common.ChangeDateFormat(cycleDateRangeEnd);
            }
            ViewData.Model = obj_cycle_management_page;
            return View();
        }

        [HttpPost]
        public ActionResult AddParticipants(string cycleDateRangeStart, string cycleDateRangeEnd, int cycleId,FormCollection form)
        {
            Dictionary<string, string> dict = FormCollectionToDict(form);
            Models.DTO.CycleManagementPage obj_cycle_management_page = new Models.DTO.CycleManagementPage();
            obj_cycle_management_page.Participants = new List<Model.DTO.Appraisal.Appraisal>();
            List<Model.DTO.Appraisal.Appraisal> lst_current_participants = new List<Model.DTO.Appraisal.Appraisal>();

            if (System.Web.HttpContext.Current.Session["CycleExistParticipantsList"] != null)
            {
                lst_current_participants=(List<Model.DTO.Appraisal.Appraisal>)System.Web.HttpContext.Current.Session["CycleExistParticipantsList"];
            }
            List<Model.DTO.Core.Employee> tempEmployeeList = ApprListToEmplList(lst_current_participants);
            tempEmployeeList=Business.AppraisalManager.GetEmployeesToAddToCycle(form.GetValue("EmployeeName").AttemptedValue == null ? null : form.GetValue("EmployeeName").AttemptedValue.Trim(),
                                                                                                        form.GetValue("DomainID").AttemptedValue == null ? null : form.GetValue("DomainID").AttemptedValue.Trim(),
                                                                                                        form.GetValue("DepartmentName").AttemptedValue == null ? null : form.GetValue("DepartmentName").AttemptedValue.Trim(),
                                                                                                       tempEmployeeList);
            if (!Lib.Utility.Common.IsNullOrEmptyList(tempEmployeeList))
            {
                foreach (Model.DTO.Core.Employee e in tempEmployeeList)
                {
                    Model.DTO.Appraisal.Appraisal newapp = new Model.DTO.Appraisal.Appraisal();
                    newapp.Id = -1;
                    newapp.Employee = e;
                    newapp.Department = e.Department;
                    obj_cycle_management_page.Participants.Add(newapp);
                }

                if (!Lib.Utility.Common.IsNullOrEmptyList(obj_cycle_management_page.Participants))
                {
                    obj_cycle_management_page.Participants = obj_cycle_management_page.Participants.OrderBy(rec => rec.Department != null ? rec.Department.Name : "").OrderBy(rec => rec.Employee.PreferredName).ToList();
                    System.Web.HttpContext.Current.Session.Add("CycleNewExistParticipants", obj_cycle_management_page.Participants);
                }
            }
            if (string.IsNullOrEmpty(cycleDateRangeStart) || string.IsNullOrEmpty(cycleDateRangeEnd))
            {
                ViewData["Stage1EndDate"] = "";
                ViewData["Stage3EndDate"] = "";
                dict.Add("Stage1EndDate", "");
                dict.Add("Stage3EndDate", "");
            }
            else
            {
                dict.Add("Stage1EndDate", Lib.Utility.Common.ChangeDateFormat(cycleDateRangeStart));
                dict.Add("Stage3EndDate", Lib.Utility.Common.ChangeDateFormat(cycleDateRangeEnd));
                ViewData["Stage1EndDate"] = Lib.Utility.Common.ChangeDateFormat(cycleDateRangeStart);
                ViewData["Stage3EndDate"] = Lib.Utility.Common.ChangeDateFormat(cycleDateRangeEnd);
            }

            ViewData.Model = obj_cycle_management_page;
            TempData["QueryData"] = dict;
            return View();
        }

        [HttpPost]
        public JsonResult ConfirmAdd(string cycleDateRangeStart, string cycleDateRangeEnd, string EIForCache)
        {
            List<Model.DTO.Appraisal.Appraisal> lst_current_participants = null;
            List<Model.DTO.Appraisal.Appraisal> lst_new_participants = null;
            try
            {
                Dictionary<string, string> dict = new Dictionary<string, string>();
                dict.Add("Stage1EndDate", cycleDateRangeStart);
                dict.Add("Stage3EndDate", cycleDateRangeEnd);

                if (System.Web.HttpContext.Current.Session["CycleNewExistParticipants"] != null)
                {
                    lst_new_participants = (List<Model.DTO.Appraisal.Appraisal>)System.Web.HttpContext.Current.Session["CycleNewExistParticipants"];
                }

                string[] splitString = { "ONERECORDENDED," };
                string[] result = EIForCache.Split(splitString, StringSplitOptions.None);
                string[] obj = new string[7];
                string[] splitString2 = { "^&*" };

                if (!Lib.Utility.Common.IsNullOrEmptyList(result))
                {
                    foreach (string str_result in result)
                    {
                        obj = str_result.Split(splitString2, StringSplitOptions.None);
                        if (!Lib.Utility.Common.IsNullOrEmptyList(obj))
                        {
                            Model.DTO.Appraisal.Appraisal obj_employee = lst_new_participants.Where(rec => rec.Employee.Id == Convert.ToInt32(obj[1])).SingleOrDefault();

                            if (obj_employee == null)
                            {
                                throw new Exception("Unable to added selected employees.");
                            }

                            if (System.Web.HttpContext.Current.Session["CycleExistParticipantsList"] != null)
                            {
                                lst_current_participants = (List<Model.DTO.Appraisal.Appraisal>)System.Web.HttpContext.Current.Session["CycleExistParticipantsList"];
                            }

                            if (Lib.Utility.Common.IsNullOrEmptyList(lst_current_participants))
                            {
                                lst_current_participants = new List<Model.DTO.Appraisal.Appraisal>();
                            }

                            lst_current_participants.Add(obj_employee);
                            System.Web.HttpContext.Current.Session.Add("CycleExistParticipantsList", lst_current_participants);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                return Json("Unable to add employees to list.");
            }
            return Json("Employees are added successfully");

        }

        #endregion Add ExistParticipants
        
        #region Remove ExistParticipants

        public ActionResult RemoveParticipants(string cycleDateRangeStart, string cycleDateRangeEnd, int cycleId)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            Models.DTO.CycleManagementPage obj_cycle_management_page = new Models.DTO.CycleManagementPage();
            if (System.Web.HttpContext.Current.Session["TempRemoveApprIds"] != null)
            {
                List<int> tempRemoveApprIds = (List<int>)System.Web.HttpContext.Current.Session["TempRemoveApprIds"];
                obj_cycle_management_page.Participants = eHR.PMS.Model.PMSModel.GetEmployeesInAppraisalsByCycleId(cycleId).Where(sec => !tempRemoveApprIds.Contains(sec.Id)).ToList();
            }
            else
                obj_cycle_management_page.Participants = eHR.PMS.Model.PMSModel.GetEmployeesInAppraisalsByCycleId(cycleId);
            if (System.Web.HttpContext.Current.Session["CycleExistParticipantsList"] != null)
            {
                obj_cycle_management_page.Participants.AddRange((List<Model.DTO.Appraisal.Appraisal>)System.Web.HttpContext.Current.Session["CycleExistParticipantsList"]);
            }

            if (Lib.Utility.Common.IsNullOrEmptyList(obj_cycle_management_page.Participants))
            {
                ViewData["AlertMessage"] = "There are no employees to remove.";
                return Redirect(Url.Content("~/HRManage/ManageCycle/" + cycleDateRangeStart + "/" + cycleDateRangeEnd + "/" + cycleId));
            }

            if (string.IsNullOrEmpty(cycleDateRangeStart) || string.IsNullOrEmpty(cycleDateRangeEnd))
            {
                ViewData["Stage1EndDate"] = "";
                ViewData["Stage3EndDate"] = "";
                dict.Add("Stage1EndDate", "");
                dict.Add("Stage3EndDate", "");
            }
            else
            {
                dict.Add("Stage1EndDate", Lib.Utility.Common.ChangeDateFormat(cycleDateRangeStart));
                dict.Add("Stage3EndDate", Lib.Utility.Common.ChangeDateFormat(cycleDateRangeEnd));
                ViewData["Stage1EndDate"] = Lib.Utility.Common.ChangeDateFormat(cycleDateRangeStart);
                ViewData["Stage3EndDate"] = Lib.Utility.Common.ChangeDateFormat(cycleDateRangeEnd);
            }


            ModelState.Clear();
            ViewData.Model = obj_cycle_management_page;
            return View();
        }

        [HttpPost]
        public ActionResult RemoveParticipants(string cycleDateRangeStart, string cycleDateRangeEnd, int cycleId, FormCollection form)
        {
            Dictionary<string, string> dict = FormCollectionToDict(form);
            Models.DTO.CycleManagementPage obj_cycle_management_page = new Models.DTO.CycleManagementPage();
            List<Model.DTO.Appraisal.Appraisal> lst_employees = null;

            if (System.Web.HttpContext.Current.Session["TempRemoveApprIds"] != null)
            {
                List<int> tempRemoveApprIds = (List<int>)System.Web.HttpContext.Current.Session["TempRemoveApprIds"];
                lst_employees = eHR.PMS.Model.PMSModel.GetEmployeesInAppraisalsByCycleId(cycleId).Where(sec => !tempRemoveApprIds.Contains(sec.Id)).ToList();
            }
            else
                lst_employees = eHR.PMS.Model.PMSModel.GetEmployeesInAppraisalsByCycleId(cycleId);

            if (System.Web.HttpContext.Current.Session["CycleExistParticipantsList"] != null)
            {
                lst_employees.AddRange((List<Model.DTO.Appraisal.Appraisal>)System.Web.HttpContext.Current.Session["CycleExistParticipantsList"]);
            }

            if (string.IsNullOrEmpty(dict["EmployeeName"].Trim()) && string.IsNullOrEmpty(dict["DomainID"].Trim()) && string.IsNullOrEmpty(dict["DepartmentName"].Trim()))
            {
                obj_cycle_management_page.Participants = lst_employees;
            }
            else
            {
                obj_cycle_management_page.Participants = Business.AppraisalManager.GetApprisalsToRemoveFromCycle(dict["EmployeeName"].Trim(), dict["DomainID"].Trim(), dict["DepartmentName"].Trim(), lst_employees);
            }

            if (string.IsNullOrEmpty(cycleDateRangeStart) || string.IsNullOrEmpty(cycleDateRangeEnd))
            {
                ViewData["Stage1EndDate"] = "";
                ViewData["Stage3EndDate"] = "";
                dict.Add("Stage1EndDate", "");
                dict.Add("Stage3EndDate", "");
            }
            else
            {
                dict.Add("Stage1EndDate", Lib.Utility.Common.ChangeDateFormat(cycleDateRangeStart));
                dict.Add("Stage3EndDate", Lib.Utility.Common.ChangeDateFormat(cycleDateRangeEnd));
                ViewData["Stage1EndDate"] = Lib.Utility.Common.ChangeDateFormat(cycleDateRangeStart);
                ViewData["Stage3EndDate"] = Lib.Utility.Common.ChangeDateFormat(cycleDateRangeEnd);
            }

            ViewData.Model = obj_cycle_management_page;
            TempData["QueryData"] = dict;
            return View();
        }

        [HttpPost]
        public JsonResult ConfirmRemove(string cycleDateRangeStart, string cycleDateRangeEnd, int cycleId, string EIForCache)
        {
            List<Model.DTO.Appraisal.Appraisal> lst_current_participantssessionpart = null;
            try
            {
                Dictionary<string, string> dict = new Dictionary<string, string>();
                dict.Add("Stage1EndDate", cycleDateRangeStart);
                dict.Add("Stage3EndDate", cycleDateRangeEnd);
                List<int> tempRemoveApprIds = new List<int>();
                if (System.Web.HttpContext.Current.Session["TempRemoveApprIds"] != null)
                {
                    tempRemoveApprIds = (List<int>)System.Web.HttpContext.Current.Session["TempRemoveApprIds"];
                }

                if (System.Web.HttpContext.Current.Session["CycleExistParticipantsList"] != null)
                {
                    lst_current_participantssessionpart=(List<Model.DTO.Appraisal.Appraisal>)System.Web.HttpContext.Current.Session["CycleExistParticipantsList"];
                }

                string[] splitString = { "ONERECORDENDED," };
                string[] result = EIForCache.Split(splitString, StringSplitOptions.None);
                string[] obj = new string[7];
                string[] splitString2 = { "^&*" };

                if (!Lib.Utility.Common.IsNullOrEmptyList(result))
                {
                    foreach (string str_result in result)
                    {
                        obj = str_result.Split(splitString2, StringSplitOptions.None);
                        if (!Lib.Utility.Common.IsNullOrEmptyList(obj))
                        {
                            int apprid=Convert.ToInt32(obj[0]);
                            int empid = Convert.ToInt32(obj[1]);
                            if (apprid > -1)
                            {
                                tempRemoveApprIds.Add(apprid);
                                System.Web.HttpContext.Current.Session.Add("TempRemoveApprIds", tempRemoveApprIds);
                            }
                            else
                            {
                                lst_current_participantssessionpart.RemoveAll(rec => rec.Id == -1 && rec.Employee.Id == empid);
                                System.Web.HttpContext.Current.Session.Add("CycleExistParticipantsList", lst_current_participantssessionpart);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return Json("Unable to remove employee from list.");
            }
            return Json("Employees are removed successfully.");
        }

        #endregion Remove ExistParticipants

        #region Employee List Pagination

        [HttpPost]
        public JsonResult RetrieveByPageNew(int page, string Stage1EndDate, string Stage3EndDate)
        {
            List<Model.DTO.Core.Employee> lst_eligible_employees = null;
            StringBuilder sb = new StringBuilder();

            try
            {
                Dictionary<string, string> dict = new Dictionary<string, string>();
                dict.Add("Stage1EndDate", Stage1EndDate);
                dict.Add("Stage3EndDate", Stage3EndDate);

                if (System.Web.HttpContext.Current.Session["CycleParticipantsList"] != null)
                {
                    lst_eligible_employees = (List<Model.DTO.Core.Employee>)System.Web.HttpContext.Current.Session["CycleParticipantsList"];
                }
                else
                {
                    return Json("Unable to load retrieved participants list.");
                }

                if (!Lib.Utility.Common.IsNullOrEmptyList(lst_eligible_employees))
                {
                    foreach (Model.DTO.Core.Employee ei in lst_eligible_employees.Skip((page - 1) * int_page_size).Take(int_page_size))
                    {
                        sb.Append("<tr>");
                        sb.Append("<td><input type='hidden' class='eid' value='" + ei.Id + "' /></td>");
                        sb.Append("<td>" + ei.PreferredName + "</td>");
                        sb.Append("<td>" + ei.PreferredName + "</td>");
                        sb.Append("<td>" + ei.GetNumberOfApprovers() + "</td>");
                        sb.Append("</tr>");
                    }
                }
                else
                {
                    return Json("There are no employees.");
                }
            }
            catch (Exception exc)
            {
                return Json("Unable to load next page of participants list. Please try again.");
            }
            return Json(sb.ToString());
        }

        [HttpPost]
        public JsonResult RetrieveByPage(int page, string Stage1EndDate, string Stage3EndDate,string cycleId)
        {
            List<Model.DTO.Appraisal.Appraisal> lst_eligible_employees = null;
            StringBuilder sb = new StringBuilder();
            int iCycleID = Int32.Parse(cycleId);
            try
            {
                List<Model.DTO.Appraisal.Appraisal> templist = eHR.PMS.Model.PMSModel.GetEmployeesInAppraisalsByCycleId(iCycleID);
                if (System.Web.HttpContext.Current.Session["TempRemoveApprIds"] != null)
                {
                    List<int> tempRemoveApprIds = (List<int>)System.Web.HttpContext.Current.Session["TempRemoveApprIds"];
                    templist = templist.Where(sec => !tempRemoveApprIds.Contains(sec.Id)).ToList();
                }
                if (System.Web.HttpContext.Current.Session["CycleExistParticipantsList"] != null)
                    templist.AddRange((List<Model.DTO.Appraisal.Appraisal>)System.Web.HttpContext.Current.Session["CycleExistParticipantsList"]);
                lst_eligible_employees = templist;
                    
                if (!Lib.Utility.Common.IsNullOrEmptyList(lst_eligible_employees))
                {
                    foreach (Model.DTO.Appraisal.Appraisal ei in lst_eligible_employees.Skip((page - 1) * int_page_size).Take(int_page_size))
                    {
                        sb.Append("<tr>");
                        sb.Append("<td><input type='hidden' class='eid' value='" + ei.Id + "' /></td>");
                        sb.Append("<td>" + ei.Employee.PreferredName + "</td>");
                        sb.Append("<td>" + (ei.Department != null ? ei.Department.Name : "") + "</td>");
                        sb.Append("<td>" + (ei.Id > 0 ? ei.Approvers.Count : ei.Employee.GetNumberOfApprovers()) + "</td>");
                        sb.Append("</tr>");
                    }
                }
                else
                {
                    return Json("There are no employees.");
                }
            }
            catch (Exception exc)
            {
                return Json("Unable to load next page of participants list. Please try again.");
            }
            return Json(sb.ToString());
        }
        [HttpPost]
        public JsonResult RetriveAddNewParticipantsByPage(string cycleDateRangeStart, string cycleDateRangeEnd, int page, string EmployeeName, string DomainID, string DepartmentName)
        {
            List<Model.DTO.Core.Employee> lst_eligible_employees = null;
            StringBuilder sb = new StringBuilder();

            try
            {
                Dictionary<string, string> dict = new Dictionary<string, string>();
                dict.Add("Stage1EndDate", cycleDateRangeStart);
                dict.Add("Stage3EndDate", cycleDateRangeEnd);
                dict.Add("EmployeeName", EmployeeName);
                dict.Add("DomainID", DomainID);
                dict.Add("DepartmentName", DepartmentName);

                if (System.Web.HttpContext.Current.Session["CycleNewParticipants"] != null)
                {
                    lst_eligible_employees = (List<Model.DTO.Core.Employee>)System.Web.HttpContext.Current.Session["CycleNewParticipants"];
                }
                else
                {
                    return Json("Unable to load retrieved participants list.");
                }
                if (!string.IsNullOrEmpty(dict["EmployeeName"]) || !string.IsNullOrEmpty(dict["DomainID"]) || !string.IsNullOrEmpty(dict["DepartmentName"]))
                {
                    lst_eligible_employees = Business.AppraisalManager.GetEmployeesToAddToCycle(dict["EmployeeName"].Trim(), dict["DomainID"].Trim(), dict["DepartmentName"].Trim(), lst_eligible_employees);
                }
                if (!Lib.Utility.Common.IsNullOrEmptyList(lst_eligible_employees))
                {
                    foreach (Model.DTO.Core.Employee ei in lst_eligible_employees.Skip((page - 1) * int_page_size).Take(int_page_size))
                    {
                        sb.Append("<tr>");
                        sb.Append("<td style='text-align:center;'><input eid='" + ei.Id + "' objectvalue='" + ei.Id + "^&*" + ei.DomainId + "^&*" + ei.FirstName + "^&*" + ei.LastName + "^&*" + (ei.Department.Name != null ? ei.Department.Name : null) + "^&*" + ei.GetNumberOfApprovers() + "^&*" + (ei.Department != null ? 0 : ei.Department.Id) + "^&*ONERECORDENDED' type='checkbox' class='isadd' /></td>");
                        sb.Append("<td>" + ei.PreferredName + "</td>");
                        sb.Append("<td>" + ei.Department.Name + "</td>");
                        sb.Append("<td>" + ei.GetNumberOfApprovers() + "</td>");
                        sb.Append("</tr>");
                    }
                }
                else
                {
                    return Json("There are no employees.");
                }
            }
            catch (Exception exc)
            {
                return Json("Unable to load next page of participants list. Please try again.");
            }
            return Json(sb.ToString());
        }

        [HttpPost]
        public JsonResult RetriveAddParticipantsByPage(string cycleDateRangeStart, string cycleDateRangeEnd, int page, string EmployeeName, string DomainID, string DepartmentName)
        {
            List<Model.DTO.Appraisal.Appraisal> lst_eligible_employees = null;
            StringBuilder sb = new StringBuilder();

            try
            {
                Dictionary<string, string> dict = new Dictionary<string, string>();
                dict.Add("Stage1EndDate", cycleDateRangeStart);
                dict.Add("Stage3EndDate", cycleDateRangeEnd);
                dict.Add("EmployeeName", EmployeeName);
                dict.Add("DomainID", DomainID);
                dict.Add("DepartmentName", DepartmentName);

                if (System.Web.HttpContext.Current.Session["CycleNewExistParticipants"] != null)
                {
                    lst_eligible_employees = (List<Model.DTO.Appraisal.Appraisal>)System.Web.HttpContext.Current.Session["CycleNewExistParticipants"];
                }
                else
                {
                    return Json("Unable to load retrieved participants list.");
                }
                if (!string.IsNullOrEmpty(dict["EmployeeName"]) || !string.IsNullOrEmpty(dict["DomainID"]) || !string.IsNullOrEmpty(dict["DepartmentName"]))
                {
                    lst_eligible_employees = Business.AppraisalManager.GetApprisalsToRemoveFromCycle(dict["EmployeeName"].Trim(), dict["DomainID"].Trim(), dict["DepartmentName"].Trim(), lst_eligible_employees);
                }
                if (!Lib.Utility.Common.IsNullOrEmptyList(lst_eligible_employees))
                {
                    foreach (Model.DTO.Appraisal.Appraisal ei in lst_eligible_employees.Skip((page - 1) * int_page_size).Take(int_page_size))
                    {
                        sb.Append("<tr>");
                        sb.Append("<td style='text-align:center;'><input eid='" + ei.Id + "' objectvalue='" + ei.Id + "^&*" + ei.Employee.Id + "^&*" + ei.Employee.DomainId+"^&*" + ei.Employee.FirstName + "^&*" + ei.Employee.LastName + "^&*" + (ei.Department.Name != null ? ei.Department.Name : null) + "^&*" + (ei.Id > 0 ? ei.Approvers.Count : ei.Employee.GetNumberOfApprovers()) + "^&*ONERECORDENDED' type='checkbox' class='isadd' /></td>");
                        sb.Append("<td>" + ei.Employee.PreferredName + "</td>");
                        sb.Append("<td>" + (ei.Department != null ? ei.Department.Name : "") + "</td>");
                        sb.Append("<td>" + (ei.Id > 0 ? ei.Approvers.Count : ei.Employee.GetNumberOfApprovers()) + "</td>");
                        sb.Append("</tr>");
                    }
                }
                else
                {
                    return Json("There are no employees.");
                }
            }
            catch (Exception exc)
            {
                return Json("Unable to load next page of participants list. Please try again.");
            }
            return Json(sb.ToString());
        }
        [HttpPost]
        public JsonResult RetriveRemoveNewParticipantsByPage(string cycleDateRangeStart, string cycleDateRangeEnd, int page, string EmployeeName, string DomainID, string DepartmentName)
        {
            List<Model.DTO.Core.Employee> lst_employees = null;
            StringBuilder sb = new StringBuilder();

            try
            {
                Dictionary<string, string> dict = new Dictionary<string, string>();
                dict.Add("Stage1EndDate", cycleDateRangeStart);
                dict.Add("Stage3EndDate", cycleDateRangeEnd);
                dict.Add("EmployeeName", EmployeeName);
                dict.Add("DomainID", DomainID);
                dict.Add("DepartmentName", DepartmentName);

                if (System.Web.HttpContext.Current.Session["CycleParticipantsList"] != null)
                {
                    lst_employees = (List<Model.DTO.Core.Employee>)System.Web.HttpContext.Current.Session["CycleParticipantsList"];
                }
                else
                {
                    return Json("Unable to load retrieved participants list.");
                }
                if (!string.IsNullOrEmpty(dict["EmployeeName"]) || !string.IsNullOrEmpty(dict["DomainID"]) || !string.IsNullOrEmpty(dict["DepartmentName"]))
                {
                    lst_employees = Business.AppraisalManager.GetEmployeesToRemoveFromCycle(dict["EmployeeName"], dict["DomainID"], dict["DepartmentName"], lst_employees);
                }
                if (!Lib.Utility.Common.IsNullOrEmptyList(lst_employees))
                {
                    foreach (Model.DTO.Core.Employee ei in lst_employees.Skip((page - 1) * int_page_size).Take(int_page_size))
                    {
                        sb.Append("<tr>");
                        sb.Append("<td style='text-align:center;'><input eid='" + ei.Id + "' objectvalue='" + ei.Id + "^&*" + ei.DomainId + "^&*" + ei.FirstName + "^&*" + ei.LastName + "^&*" + (ei.Department.Name != null ? ei.Department.Name : null) + "^&*" + ei.GetNumberOfApprovers() + "^&*" + (ei.Department != null ? 0 : ei.Department.Id) + "^&*ONERECORDENDED' type='checkbox' class='isadd' /></td>");
                        sb.Append("<td>" + ei.PreferredName + "</td>");
                        sb.Append("<td>" + ei.Department.Name + "</td>");
                        sb.Append("<td>" + ei.GetNumberOfApprovers() + "</td>");
                        sb.Append("</tr>");
                    }
                }
                else
                {
                    return Json("There are no employees.");
                }
            }
            catch (Exception e)
            {
                return Json("Ops! There is an error. Please check and try again");
            }
            return Json(sb.ToString());

        }

        [HttpPost]
        public JsonResult RetriveRemoveParticipantsByPage(string cycleDateRangeStart, string cycleDateRangeEnd, int page, string EmployeeName, string DomainID, string DepartmentName,string cycleId)
        {
            List<Model.DTO.Appraisal.Appraisal> lst_employees = null;
            StringBuilder sb = new StringBuilder();
            int iCycleID = Int32.Parse(cycleId);
            try
            {
                Dictionary<string, string> dict = new Dictionary<string, string>();
                dict.Add("Stage1EndDate", cycleDateRangeStart);
                dict.Add("Stage3EndDate", cycleDateRangeEnd);
                dict.Add("EmployeeName", EmployeeName);
                dict.Add("DomainID", DomainID);
                dict.Add("DepartmentName", DepartmentName);

                if (System.Web.HttpContext.Current.Session["TempRemoveApprIds"] != null)
                {
                    List<int> tempRemoveApprIds = (List<int>)System.Web.HttpContext.Current.Session["TempRemoveApprIds"];
                    lst_employees = eHR.PMS.Model.PMSModel.GetEmployeesInAppraisalsByCycleId(iCycleID).Where(sec => !tempRemoveApprIds.Contains(sec.Id)).ToList();
                }
                else
                    lst_employees = eHR.PMS.Model.PMSModel.GetEmployeesInAppraisalsByCycleId(iCycleID);

                if (System.Web.HttpContext.Current.Session["CycleExistParticipantsList"] != null)
                {
                    lst_employees.AddRange((List<Model.DTO.Appraisal.Appraisal>)System.Web.HttpContext.Current.Session["CycleExistParticipantsList"]);
                }
                if (!string.IsNullOrEmpty(dict["EmployeeName"]) || !string.IsNullOrEmpty(dict["DomainID"]) || !string.IsNullOrEmpty(dict["DepartmentName"]))
                {
                    lst_employees = Business.AppraisalManager.GetApprisalsToRemoveFromCycle(dict["EmployeeName"], dict["DomainID"], dict["DepartmentName"], lst_employees);
                }
                if (!Lib.Utility.Common.IsNullOrEmptyList(lst_employees))
                {
                    foreach (Model.DTO.Appraisal.Appraisal ei in lst_employees.Skip((page - 1) * int_page_size).Take(int_page_size))
                    {
                        sb.Append("<tr>");
                        sb.Append("<td style='text-align:center;'><input eid='" + ei.Id + "' objectvalue='" + ei.Id + "^&*" + ei.Employee.Id + "^&*" + ei.Employee.DomainId + "^&*" + ei.Employee.FirstName + "^&*" + ei.Employee.LastName + "^&*" + (ei.Department.Name != null ? ei.Department.Name : null) + "^&*" + (ei.Id > 0 ? ei.Approvers.Count : ei.Employee.GetNumberOfApprovers()) + "^&*ONERECORDENDED' type='checkbox' class='isadd' /></td>");
                        sb.Append("<td>" + ei.Employee.PreferredName + "</td>");
                        sb.Append("<td>" + (ei.Department != null ? ei.Department.Name : "") + "</td>");
                        sb.Append("<td>" + (ei.Id > 0 ? ei.Approvers.Count : ei.Employee.GetNumberOfApprovers()) + "</td>");
                        sb.Append("</tr>");
                    }
                }
                else
                {
                    return Json("There are no employees.");
                }
            }
            catch (Exception e)
            {
                return Json("Ops! There is an error. Please check and try again");
            }
            return Json(sb.ToString());

        }

        [HttpPost]
        public JsonResult RetrieveAppraisalsByPage(int page)
        {
            List<Model.DTO.Appraisal.Appraisal> lst_appraisals = null;
            StringBuilder sb = new StringBuilder();

            if (System.Web.HttpContext.Current.Session["AppraisalsList"] != null)
            {
                lst_appraisals = (List<Model.DTO.Appraisal.Appraisal>)System.Web.HttpContext.Current.Session["AppraisalsList"];
            }
            else
            {
                return Json("Unable to load retrieved appraisals list.");
            }

            if (!Lib.Utility.Common.IsNullOrEmptyList(lst_appraisals))
            {
                foreach (Model.DTO.Appraisal.Appraisal obj_appraisal in lst_appraisals.Skip((page - 1) * int_page_size).Take(int_page_size))
                {
                    sb.Append("<tr>");
                    sb.Append("<td align=\"center\">");
                    sb.Append("<a class=\"btn btn-xs btn-danger\" href=\"");
                    sb.Append(Url.Content("~/stage1/KeyPerformanceIndicators/0/"));
                    sb.Append(obj_appraisal.Id);
                    sb.Append("\"><i class=\"glyphicon glyphicon-file\">></i> View</a></td>");
                    sb.Append("<td>" + obj_appraisal.Employee.PreferredName + "</td>");
                    sb.Append("<td>" + obj_appraisal.Department.Name + "</td>");
                    sb.Append("<td>" + obj_appraisal.Stage.Name + "</td>");
                    sb.Append("<td>" + obj_appraisal.Status.Name + "</td>");
                    sb.Append("</tr>");
                }
            }

            return Json(sb.ToString());
        }

        #endregion Employee List Pagination

        public ActionResult CancelAddRemoveNew(string cycleDateRangeStart, string cycleDateRangeEnd)
        {
            System.Web.HttpContext.Current.Session.Remove("CycleNewParticipants");
            return Redirect(Url.Content("~/HRManage/NewCycle/" + cycleDateRangeStart + "/" + cycleDateRangeEnd));
        }
       
        public ActionResult CancelAddRemove(string cycleDateRangeStart, string cycleDateRangeEnd,string cycleId)
        {
            System.Web.HttpContext.Current.Session.Remove("CycleNewExistParticipants");
            return Redirect(Url.Content("~/HRManage/ManageCycle/" + cycleDateRangeStart + "/" + cycleDateRangeEnd+"/"+cycleId));
        }

        public ActionResult CancelViewAppraisal()
        {
            ClearAllCreatedSessionObjects();
            return Redirect(Url.Content("~/"));

        }

        public Dictionary<string, string> FormCollectionToDict(FormCollection form)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (string key in form.AllKeys)
            {
                dict.Add(key, form[key].ToString());
            }
            return dict;
        }
       
        private List<Model.DTO.Core.Employee> ApprListToEmplList(List<Model.DTO.Appraisal.Appraisal> applist)
        {
            List<Model.DTO.Core.Employee> result=new List<Model.DTO.Core.Employee>();
            foreach (Model.DTO.Appraisal.Appraisal a in applist)
            {
                result.Add(a.Employee);
            }
            return result;
        }
        
        private Model.DTO.Cycle.Stage CreateCycleStage(int id, DateTime startDate, DateTime endDate, DateTime? submissionDeadline, DateTime? level1ApprovalDeadline, DateTime? level2ApprovalDeadline)
        {
            Model.DTO.Cycle.Stage obj_cycle_stage = new Model.DTO.Cycle.Stage()
            {
                StageId = id,
                StartDate = startDate,
                EndDate = endDate,
                PreStartEmailSent = false,
                SubmissionDeadline = submissionDeadline,
                Level1ApprovalDeadline = level1ApprovalDeadline,
                Level2ApprovalDeadline = level2ApprovalDeadline
            };


            return obj_cycle_stage;
        }

        private List<Model.DTO.Cycle.Stage> CreateDefaultStagesForNewCycle(Dictionary<string, DateTime> stageDates)
        {
            List<Model.DTO.Cycle.Stage> lst_stages = new List<Model.DTO.Cycle.Stage>();
            lst_stages.Add(CreateCycleStage(Model.PMSConstants.STAGE_ID_PRE_CYCLE, stageDates["PreCStart"], stageDates["PreCEnd"],null,null,null));
            lst_stages.Add(CreateCycleStage(Model.PMSConstants.STAGE_ID_GOAL_SETTING, stageDates["Stage1StartDate"], stageDates["Stage1EndDate"], stageDates["Stage1SubmissionDeadline"], stageDates["Stage1Level1ApprovalDeadline"], stageDates["Stage1Level2ApprovalDeadline"]));
            lst_stages.Add(CreateCycleStage(Model.PMSConstants.STAGE_ID_PROGRESS_REVIEW, stageDates["Stage2StartDate"], stageDates["Stage2EndDate"], stageDates["Stage2SubmissionDeadline"], stageDates["Stage2Level1ApprovalDeadline"], stageDates["Stage2Level2ApprovalDeadline"]));
            lst_stages.Add(CreateCycleStage(Model.PMSConstants.STAGE_ID_FINAL_YEAR, stageDates["Stage3StartDate"], stageDates["Stage3EndDate"], null, null, null));

            return lst_stages;
        }

        private void ClearAllCreatedSessionObjects()
        {
            System.Web.HttpContext.Current.Session.Remove("CycleExistParticipantsList");
            System.Web.HttpContext.Current.Session.Remove("CycleParticipantsList");
            System.Web.HttpContext.Current.Session.Remove("CycleName");
            System.Web.HttpContext.Current.Session.Remove("Stage1StartDate");
            System.Web.HttpContext.Current.Session.Remove("Stage1EndDate");
            System.Web.HttpContext.Current.Session.Remove("Stage2StartDate");
            System.Web.HttpContext.Current.Session.Remove("Stage2EndDate");
            System.Web.HttpContext.Current.Session.Remove("Stage3StartDate");
            System.Web.HttpContext.Current.Session.Remove("Stage3EndDate");

            System.Web.HttpContext.Current.Session.Remove("CycleNewParticipants");
            System.Web.HttpContext.Current.Session.Remove("CycleNewExistParticipants");

            System.Web.HttpContext.Current.Session.Remove("TempRemoveApprIds");

            System.Web.HttpContext.Current.Session.Remove("AppraisalsList");

            System.Web.HttpContext.Current.Session.Remove("Stage1SubmissionDeadline");
            System.Web.HttpContext.Current.Session.Remove("Stage1Level1ApprovalDeadline");
            System.Web.HttpContext.Current.Session.Remove("Stage1Level2ApprovalDeadline");
            System.Web.HttpContext.Current.Session.Remove("Stage2SubmissionDeadline");
            System.Web.HttpContext.Current.Session.Remove("Stage2Level1ApprovalDeadline");
            System.Web.HttpContext.Current.Session.Remove("Stage2Level2ApprovalDeadline");
        }

        #region View Appraisal

        public ActionResult ViewAppraisal()
        {
            Models.DTO.ViewAppraisalPage obj_view_appraisal_page = new Models.DTO.ViewAppraisalPage()
            {
                Cycles = Model.PMSModel.GetCycleByStatus(null),
                Appraisals = new List<Model.DTO.Appraisal.Appraisal>()
            };

            ViewData.Model = obj_view_appraisal_page;
            return View();
        }

        [HttpPost]
        public ActionResult ViewAppraisal(FormCollection form)
        {
            Dictionary<string, string> dict_form = FormCollectionToDict(form);

            Models.DTO.ViewAppraisalPage obj_view_appraisal_page = new Models.DTO.ViewAppraisalPage()
            {
                Cycles = Model.PMSModel.GetCycleByStatus(null),
                Appraisals = new List<Model.DTO.Appraisal.Appraisal>()
            };

            if (dict_form["cycleid"] != null)
            {
                string str_employeeName = dict_form["EmployeeName"] == null ? null : dict_form["EmployeeName"];
                string str_employeeDomainId = dict_form["DomainID"] == null ? null : dict_form["DomainID"];
                string str_departmentName = dict_form["DepartmentName"] == null ? null : dict_form["DepartmentName"];

                obj_view_appraisal_page.Appraisals = Business.AppraisalManager.GetAppraisalsForView(Convert.ToInt32(dict_form["cycleid"]), str_employeeName, str_employeeDomainId, str_departmentName);
                obj_view_appraisal_page.Appraisals = obj_view_appraisal_page.Appraisals.OrderBy(rec => rec.Employee.PreferredName).OrderBy(rec => rec.Department.Name).ToList();

            }

            System.Web.HttpContext.Current.Session.Add("AppraisalsList", obj_view_appraisal_page.Appraisals);

            TempData["QueryData"] = dict_form;
            ViewData.Model = obj_view_appraisal_page;
            return View();
        }

        #endregion
    }
}
