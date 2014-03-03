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

        public ActionResult NewCycle(string cycleDateRangeStart, string cycleDateRangeEnd)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            Models.DTO.CycleManagementPage obj_cycle_management_page = new Models.DTO.CycleManagementPage();

            if (string.IsNullOrEmpty(cycleDateRangeStart) || string.IsNullOrEmpty(cycleDateRangeEnd))
            {
                obj_cycle_management_page.Cycle = new PMS.Model.DTO.Cycle.Cycle();
                dict.Add("Stage1EndDate", "");
                dict.Add("Stage3EndDate", "");
                dict.Add("strStage1EndDate", "");
                dict.Add("strStage3EndDate", "");
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
                TempData["QueryData"] = dict;
            }

            ModelState.Clear();
            ViewData.Model = obj_cycle_management_page;
            return View();
        }

        [HttpPost]
        public ActionResult NewCycle(FormCollection form)
        {
            string message = string.Empty;
            Dictionary<string, string> dict = FormCollectionToDict(form);
            bool boo_start_cycle = Convert.ToInt32(dict["startcycle"]) == 1 ? true : false;
            Models.DTO.CycleManagementPage obj_cycle_management_page = new Models.DTO.CycleManagementPage();

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

        public ActionResult ManageCycle(string cycleDateRangeStart, string cycleDateRangeEnd)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            Models.DTO.CycleManagementPage obj_cycle_management_page = new Models.DTO.CycleManagementPage();

            if (string.IsNullOrEmpty(cycleDateRangeStart) || string.IsNullOrEmpty(cycleDateRangeEnd))
            {
                obj_cycle_management_page.Cycle = new PMS.Model.DTO.Cycle.Cycle();
                dict.Add("Stage1EndDate", "");
                dict.Add("Stage3EndDate", "");
                dict.Add("strStage1EndDate", "");
                dict.Add("strStage3EndDate", "");
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
                TempData["QueryData"] = dict;
            }

            ModelState.Clear();
            ViewData.Model = obj_cycle_management_page;

            return View();
        }
        #endregion Manage Cycle

        #region Add Participants

        public ActionResult AddParticipants(string cycleDateRangeStart, string cycleDateRangeEnd)
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
        public ActionResult AddParticipants(string cycleDateRangeStart, string cycleDateRangeEnd, FormCollection form)
        {
            Dictionary<string, string> dict = FormCollectionToDict(form);
            Models.DTO.CycleManagementPage obj_cycle_management_page = new Models.DTO.CycleManagementPage();
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
        public JsonResult ConfirmAdd(string cycleDateRangeStart, string cycleDateRangeEnd, string EIForCache)
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

        #region Remove Participants

        public ActionResult RemoveParticipants(string cycleDateRangeStart, string cycleDateRangeEnd)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            Models.DTO.CycleManagementPage obj_cycle_management_page = new Models.DTO.CycleManagementPage();

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
        public ActionResult RemoveParticipants(string cycleDateRangeStart, string cycleDateRangeEnd, FormCollection form)
        {
            Dictionary<string, string> dict = FormCollectionToDict(form);
            Models.DTO.CycleManagementPage obj_cycle_management_page = new Models.DTO.CycleManagementPage();
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
                obj_cycle_management_page.Participants = Business.AppraisalManager.GetEmployeesToRemoveFromCycle(dict["EmployeeName"].Trim(),dict["DomainID"].Trim(),dict["DepartmentName"].Trim(),lst_employees);
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
        public JsonResult ConfirmRemove(string cycleDateRangeStart, string cycleDateRangeEnd, string EIForCache)
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

        #endregion Remove Participants

        #region Employee List Pagination

        [HttpPost]
        public JsonResult RetrieveByPage(int page, string Stage1EndDate, string Stage3EndDate)
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
        public JsonResult RetriveAddParticipantsByPage(string cycleDateRangeStart, string cycleDateRangeEnd, int page, string EmployeeName, string DomainID, string DepartmentName)
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
        public JsonResult RetriveRemoveParticipantsByPage(string cycleDateRangeStart, string cycleDateRangeEnd, int page, string EmployeeName, string DomainID, string DepartmentName)
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

        #endregion Employee List Pagination

        public ActionResult CancelAddRemove(string cycleDateRangeStart, string cycleDateRangeEnd)
        {
            System.Web.HttpContext.Current.Session.Remove("CycleNewParticipants");
            return Redirect(Url.Content("~/HRManage/NewCycle/" + cycleDateRangeStart + "/" + cycleDateRangeEnd));
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

        private Model.DTO.Cycle.Stage CreateCycleStage(int id, DateTime startDate, DateTime endDate)
        {
            Model.DTO.Cycle.Stage obj_cycle_stage = new Model.DTO.Cycle.Stage() 
            { 
                StageId = id,
                StartDate = startDate,
                EndDate = endDate,
                PreStartEmailSent = false
            };


            return obj_cycle_stage;
        }

        private List<Model.DTO.Cycle.Stage> CreateDefaultStagesForNewCycle(Dictionary<string, DateTime> stageDates)
        {
            List<Model.DTO.Cycle.Stage> lst_stages = new List<Model.DTO.Cycle.Stage>();

            lst_stages.Add(CreateCycleStage(Model.PMSConstants.STAGE_ID_PRE_CYCLE, stageDates["PreCStart"], stageDates["PreCStart"]));
            lst_stages.Add(CreateCycleStage(Model.PMSConstants.STAGE_ID_GOAL_SETTING, stageDates["Stage1StartDate"], stageDates["Stage1EndDate"]));
            lst_stages.Add(CreateCycleStage(Model.PMSConstants.STAGE_ID_PROGRESS_REVIEW, stageDates["Stage2StartDate"], stageDates["Stage2EndDate"]));
            lst_stages.Add(CreateCycleStage(Model.PMSConstants.STAGE_ID_FINAL_YEAR, stageDates["Stage3StartDate"], stageDates["Stage3EndDate"]));
            
            return lst_stages;
        }

        private void ClearAllCreatedSessionObjects()
        {
            System.Web.HttpContext.Current.Session.Remove("CycleParticipantsList");
            System.Web.HttpContext.Current.Session.Remove("CycleName");
            System.Web.HttpContext.Current.Session.Remove("Stage1StartDate");
            System.Web.HttpContext.Current.Session.Remove("Stage1EndDate");
            System.Web.HttpContext.Current.Session.Remove("Stage2StartDate");
            System.Web.HttpContext.Current.Session.Remove("Stage2EndDate");
            System.Web.HttpContext.Current.Session.Remove("Stage3StartDate");
            System.Web.HttpContext.Current.Session.Remove("Stage3EndDate");

            System.Web.HttpContext.Current.Session.Remove("CycleNewParticipants");
        }
    }
}
