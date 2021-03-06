﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Text.RegularExpressions;
//using System.Text.RegularExpressions;

namespace eHR.PMS.Business
{
    public class AppraisalManager
    {
        #region Cycle

        public static bool CreateNewCycle(PMS.Model.DTO.Cycle.Cycle cycle,  Model.DTO.Core.Employee user, out string message) 
        {
            bool boo_success = false;
            message = string.Empty;

            if (!Lib.Utility.Common.IsNullOrEmptyList(cycle.Appriasals))
            {
                boo_success = Model.PMSModel.InsertNewCycleAndCreateAppraisalTasks(cycle, out message);

                if (boo_success)
                {
                    cycle = Model.PMSModel.GetCycleById(Convert.ToInt32(message));
                    Model.DTO.Cycle.Stage obj_stage = cycle.CycleStages.Where(rec => rec.StageId == Model.PMSConstants.STAGE_ID_GOAL_SETTING).Single();

                    if (obj_stage.StartDate == DateTime.Now.Date)
                    {
                        PreCycleStageManagement(cycle, DateTime.Now.Date, user);
                    }
                }
            }
            else 
            {
                boo_success = true;
            }
            return boo_success;
        }


        public static bool UpdateCycleStage(List<PMS.Model.DTO.Cycle.Stage> cycleStages, List<PMS.Model.DTO.Appraisal.Appraisal> appraisals, out string message)
        {
            message = string.Empty;
            bool boo_success = false;

            if (!Lib.Utility.Common.IsNullOrEmptyList(cycleStages))
            {
                boo_success = Model.PMSModel.UpdateCycleStages(cycleStages, appraisals, out message);
            }
            else
            {
                boo_success = true;
            }

            return boo_success;
        }
 
        public static bool UpdateCycle(PMS.Model.DTO.Cycle.Cycle cycle,int cycleId, Model.DTO.Core.Employee user, out string message)
        {
            bool boo_success = false;
            message = string.Empty;
            List<Model.DTO.Appraisal.Appraisal> NewAddAppraisals = new List<Model.DTO.Appraisal.Appraisal>();
            if (!Lib.Utility.Common.IsNullOrEmptyList(cycle.Appriasals))
            {
                boo_success = Model.PMSModel.UpdateCycleAndCreateAppraisalTasks(cycle, cycleId,out message,out NewAddAppraisals);
                
                if (boo_success)
                {
                    cycle = Model.PMSModel.GetCycleById(Convert.ToInt32(message));
                    cycle.Appriasals = NewAddAppraisals;
                    Model.DTO.Cycle.Stage obj_gs_stage = cycle.CycleStages.Where(rec => rec.StageId == Model.PMSConstants.STAGE_ID_GOAL_SETTING).SingleOrDefault();
                    Model.DTO.Cycle.Stage obj_pr_stage = cycle.CycleStages.Where(rec => rec.StageId == Model.PMSConstants.STAGE_ID_PROGRESS_REVIEW).SingleOrDefault();
                    Model.DTO.Cycle.Stage obj_fr_stage = cycle.CycleStages.Where(rec => rec.StageId == Model.PMSConstants.STAGE_ID_FINAL_YEAR).SingleOrDefault();

                    int flag = CheckStageDate(obj_gs_stage, obj_pr_stage, obj_fr_stage);
                    UpdateCycleStageManagement(cycle, DateTime.Now.Date, user,flag);
                }
            }
            else
            {
                boo_success = true;
            }
            return boo_success;
        }
       
        public static int CheckStageDate(Model.DTO.Cycle.Stage obj_gs_stage, Model.DTO.Cycle.Stage obj_pr_stage, Model.DTO.Cycle.Stage obj_fr_stage)
        {
            if (DateTime.Now < obj_gs_stage.StartDate)
                return 1;
            if (DateTime.Now.Date == obj_gs_stage.StartDate)
                return 2;
            if (DateTime.Now.Date > obj_gs_stage.StartDate && DateTime.Now.Date < obj_gs_stage.EndDate)
                return 3;
            if (DateTime.Now.Date >= obj_gs_stage.EndDate && DateTime.Now.Date < obj_pr_stage.StartDate)
                return 4;
            if (DateTime.Now.Date == obj_pr_stage.StartDate)
                return 5;
            if (DateTime.Now.Date > obj_pr_stage.StartDate && DateTime.Now.Date < obj_pr_stage.EndDate)
                return 6;
            if (DateTime.Now.Date >= obj_pr_stage.EndDate && DateTime.Now.Date < obj_fr_stage.StartDate)
                return 7;
            return 0;
        }
       
        public static Model.DTO.Master.Stage GetCycleNextStage(Model.DTO.Cycle.Cycle cycle, DateTime stageStartDate)
        {
            Model.DTO.Master.Stage obj_next_stage = null;

            if (cycle.Stage.Id == Model.PMSConstants.STAGE_ID_PRE_CYCLE)
            {
                Model.DTO.Cycle.Stage obj_stage = cycle.CycleStages.Where(rec => rec.StageId == Model.PMSConstants.STAGE_ID_GOAL_SETTING).SingleOrDefault();
                if (obj_stage != null && Convert.ToDateTime(obj_stage.StartDate).Date == stageStartDate.Date)
                {
                    obj_next_stage = new Model.DTO.Master.Stage() { Id = Model.PMSConstants.STAGE_ID_GOAL_SETTING };
                }
            }
            else if (cycle.Stage.Id == Model.PMSConstants.STAGE_ID_GOAL_SETTING)
            {
                Model.DTO.Cycle.Stage obj_stage = cycle.CycleStages.Where(rec => rec.StageId == Model.PMSConstants.STAGE_ID_PROGRESS_REVIEW).SingleOrDefault();
                if (obj_stage != null && Convert.ToDateTime(obj_stage.StartDate).Date == stageStartDate.Date)
                {
                    obj_next_stage = new Model.DTO.Master.Stage() { Id = Model.PMSConstants.STAGE_ID_PROGRESS_REVIEW };
                }
            }
            else if (cycle.Stage.Id == Model.PMSConstants.STAGE_ID_PROGRESS_REVIEW)
            {
                Model.DTO.Cycle.Stage obj_stage = cycle.CycleStages.Where(rec => rec.StageId == Model.PMSConstants.STAGE_ID_FINAL_YEAR).SingleOrDefault();
                if (obj_stage != null && Convert.ToDateTime(obj_stage.StartDate).Date == stageStartDate.Date)
                {
                    obj_next_stage = new Model.DTO.Master.Stage() { Id = Model.PMSConstants.STAGE_ID_FINAL_YEAR };
                }
            }
            return obj_next_stage;
        }

        public static bool PreCycleStageManagement(Model.DTO.Cycle.Cycle cycle, DateTime stageStartDate, Model.DTO.Core.Employee user)
        {
            bool boo_success = false;
            List<Model.DTO.Cycle.Cycle> lst_cycles = new List<Model.DTO.Cycle.Cycle>();
            List<Model.DTO.Core.Task.Task> lst_all_tasks = new List<Model.DTO.Core.Task.Task>();
            List<Model.DTO.Appraisal.Appraisal> lst_appraisals_to_update = new List<Model.DTO.Appraisal.Appraisal>();
            List<System.Net.Mail.MailMessage> lst_email_messages = new List<System.Net.Mail.MailMessage>();


            cycle.Stage = GetCycleNextStage(cycle, stageStartDate.Date);
            if (!Lib.Utility.Common.IsNullOrEmptyList(cycle.Appriasals))
            {
                foreach (Model.DTO.Appraisal.Appraisal obj_appraisal in cycle.Appriasals)
                {
                    // attaching cycle to appraisal
                    obj_appraisal.Cycle = cycle;


                    //if (obj_appraisal.Stage.Id == Model.PMSConstants.STAGE_ID_PRE_CYCLE)
                    //{
                        obj_appraisal.Locked = false;

                        if (user.Id != null && user.Id > 0)
                        {
                            obj_appraisal.AddTrail(CreateAppraisalTrail(obj_appraisal, user, new Model.DTO.Master.Action() { Id = Model.PMSConstants.ACTION_ID_APPRAISAL_OPENED }));
                        }                       
                        obj_appraisal.Stage = Model.Mappers.PMSMapper.MapAppraisalStageDTOToStageDTO(obj_appraisal.AppraisalStages.Where(rec => rec.StageId == Model.PMSConstants.STAGE_ID_GOAL_SETTING).SingleOrDefault());
                        obj_appraisal.Status = new Model.DTO.Master.Status() { Id = Model.PMSConstants.STATUS_ID_NEW };
                        lst_all_tasks.Add(CreateTasksForCycleStageChange(obj_appraisal));
                        lst_appraisals_to_update.Add(obj_appraisal);
                        if (!string.IsNullOrEmpty(obj_appraisal.Employee.OfficeEmailAddress) && Lib.Utility.Common.IsValidEmail(obj_appraisal.Employee.OfficeEmailAddress))
                        {
                            lst_email_messages.Add(GenerateEmailMessageForCycleStageStart(obj_appraisal));
                        }
                    //}
                }
            }
            lst_cycles.Add(cycle);

            boo_success = Model.PMSModel.AppraisalStageManager(lst_cycles, lst_appraisals_to_update, lst_all_tasks, null);

            if (boo_success)
            {
                SendEmailNotification(lst_email_messages);
            }
            return boo_success;
        }

        public static bool UpdateCycleStageManagement(Model.DTO.Cycle.Cycle cycle, DateTime stageStartDate, Model.DTO.Core.Employee user,int flag)
        {
            bool boo_success = false;
            //List<Model.DTO.Cycle.Cycle> lst_cycles = new List<Model.DTO.Cycle.Cycle>();
            List<Model.DTO.Core.Task.Task> lst_all_tasks = new List<Model.DTO.Core.Task.Task>();
            List<Model.DTO.Appraisal.Appraisal> lst_appraisals_to_update = new List<Model.DTO.Appraisal.Appraisal>();
            List<System.Net.Mail.MailMessage> lst_email_messages = new List<System.Net.Mail.MailMessage>();

            switch (flag)
            {
                case 2:
                case 5:
                    cycle.Stage = GetCycleNextStage(cycle, stageStartDate.Date);
                    break;
                default:
                    break;
            }
            
            if (!Lib.Utility.Common.IsNullOrEmptyList(cycle.Appriasals))
            {
                switch (flag)
                {
                    case 2:
                    case 3:
                    case 5:
                    case 6:
                        foreach (Model.DTO.Appraisal.Appraisal obj_appraisal in cycle.Appriasals)
                        {
                            //if (obj_appraisal.Stage.Id == Model.PMSConstants.STAGE_ID_PRE_CYCLE)
                            //{
                            obj_appraisal.Locked = false;
                            obj_appraisal.AddTrail(CreateAppraisalTrail(obj_appraisal, user, new Model.DTO.Master.Action() { Id = Model.PMSConstants.ACTION_ID_APPRAISAL_OPENED }));
                            obj_appraisal.Stage = Model.Mappers.PMSMapper.MapAppraisalStageDTOToStageDTO(obj_appraisal.AppraisalStages.Where(rec => rec.StageId == Model.PMSConstants.STAGE_ID_GOAL_SETTING).SingleOrDefault());
                            obj_appraisal.Status = new Model.DTO.Master.Status() { Id = Model.PMSConstants.STATUS_ID_NEW };
                            lst_all_tasks.Add(CreateTasksForCycleStageChange(obj_appraisal));
                            lst_appraisals_to_update.Add(obj_appraisal);
                            if (!string.IsNullOrEmpty(obj_appraisal.Employee.OfficeEmailAddress) && Lib.Utility.Common.IsValidEmail(obj_appraisal.Employee.OfficeEmailAddress))
                            {
                                lst_email_messages.Add(GenerateEmailMessageForCycleStageStart(obj_appraisal));
                            }
                            //}
                        }
                        break;
                    case 4:
                    case 7:
                        foreach (Model.DTO.Appraisal.Appraisal obj_appraisal in cycle.Appriasals)
                        {
                            //if (obj_appraisal.Stage.Id == Model.PMSConstants.STAGE_ID_PRE_CYCLE)
                            //{
                            obj_appraisal.Locked = true;
                            obj_appraisal.AddTrail(CreateAppraisalTrail(obj_appraisal, user, new Model.DTO.Master.Action() { Id = Model.PMSConstants.ACTION_ID_APPRAISAL_OPENED }));
                            obj_appraisal.Stage = Model.Mappers.PMSMapper.MapAppraisalStageDTOToStageDTO(obj_appraisal.AppraisalStages.Where(rec => rec.StageId == Model.PMSConstants.STAGE_ID_GOAL_SETTING).SingleOrDefault());
                            obj_appraisal.Status = new Model.DTO.Master.Status() { Id = Model.PMSConstants.STATUS_ID_NEW };
                            lst_appraisals_to_update.Add(obj_appraisal);
                            //}
                        }
                        break;
                }
                /*foreach (Model.DTO.Appraisal.Appraisal obj_appraisal in cycle.Appriasals)
                {
                    //if (obj_appraisal.Stage.Id == Model.PMSConstants.STAGE_ID_PRE_CYCLE)
                    //{
                    obj_appraisal.Locked = false;
                    obj_appraisal.AddTrail(CreateAppraisalTrail(obj_appraisal, user, new Model.DTO.Master.Action() { Id = Model.PMSConstants.ACTION_ID_APPRAISAL_OPENED }));
                    obj_appraisal.Stage = Model.Mappers.PMSMapper.MapAppraisalStageDTOToStageDTO(obj_appraisal.AppraisalStages.Where(rec => rec.StageId == Model.PMSConstants.STAGE_ID_GOAL_SETTING).SingleOrDefault());
                    obj_appraisal.Status = new Model.DTO.Master.Status() { Id = Model.PMSConstants.STATUS_ID_NEW };
                    lst_all_tasks.Add(CreateTasksForCycleStageChange(obj_appraisal));
                    lst_appraisals_to_update.Add(obj_appraisal);
                    if (!string.IsNullOrEmpty(obj_appraisal.Employee.OfficeEmailAddress))
                    {
                        lst_email_messages.Add(GenerateEmailMessageForCycleStageStart(obj_appraisal));
                    }
                    //}
                }*/
            }
            //lst_cycles.Add(cycle);

            boo_success = Model.PMSModel.AppraisalStageManager(null, lst_appraisals_to_update, lst_all_tasks, null);

            /*if (boo_success)
            {
                SendEmailNotification(lst_email_messages);
            }*/
            return boo_success;
        }
        #endregion Cycle

        #region Appraisal

        public static List<Model.DTO.Appraisal.Appraisal> CreateAppraisalsForNewCycle(List<Model.DTO.Core.Employee> participants, List<Model.DTO.Cycle.Stage> stages, Model.DTO.Core.Security.User user)
        {
            List<Model.DTO.Appraisal.Appraisal> lst_appraisals = null;
            if (!Lib.Utility.Common.IsNullOrEmptyList(participants))
            {
                lst_appraisals = new List<Model.DTO.Appraisal.Appraisal>();
                foreach (Model.DTO.Core.Employee obj_participant in participants)
                {
                    Model.DTO.Appraisal.Appraisal obj_appraisal = new Model.DTO.Appraisal.Appraisal()
                    {
                        Stage = new Model.DTO.Master.Stage() { Id = Model.PMSConstants.STAGE_ID_PRE_CYCLE },
                        Status = new Model.DTO.Master.Status() { Id = Model.PMSConstants.STATUS_ID_NEW },
                        Employee = obj_participant,
                        Department = Model.Mappers.CoreMapper.MapDepartmentDTOToMasterDepartmentDTO(obj_participant.Department),
                        Locked = true
                    };

                    obj_appraisal.AddAppraisalSection(new Model.DTO.Appraisal.Section() { SectionId = Model.PMSConstants.SECTION_ID_KPI });
                    obj_appraisal.AddAppraisalSection(new Model.DTO.Appraisal.Section() { SectionId = Model.PMSConstants.SECTION_ID_CORE_VALUES });

                    foreach (Model.DTO.Cycle.Stage obj_cycle_stage in stages)
                    {
                        Model.DTO.Appraisal.Stage obj_appraisal_stage = new Model.DTO.Appraisal.Stage()
                        {
                            StageId = obj_cycle_stage.StageId,
                            StartDate = obj_cycle_stage.StartDate,
                            EndDate = obj_cycle_stage.EndDate,
                            SubmissionDeadline = obj_cycle_stage.SubmissionDeadline,
                            Level1ApprovalDeadline = obj_cycle_stage.Level1ApprovalDeadline,
                            Level2ApprovalDeadline = obj_cycle_stage.Level2ApprovalDeadline
                        };
                        obj_appraisal.AddAppraisalStage(obj_appraisal_stage);
                    }

                    if (obj_participant.Level1Approver != null) { obj_appraisal.AddApprover(Model.Mappers.PMSMapper.MapEmployeeDTOToApproverDTO(obj_participant.Level1Approver, 1)); }
                    if (obj_participant.Level2Approver != null) { obj_appraisal.AddApprover(Model.Mappers.PMSMapper.MapEmployeeDTOToApproverDTO(obj_participant.Level2Approver, 2)); }

                    Model.DTO.Appraisal.Trail obj_trail = new Model.DTO.Appraisal.Trail()
                    {
                        Appraisal = obj_appraisal,
                        Stage = new Model.DTO.Master.Stage() { Id = PMS.Model.PMSConstants.STAGE_ID_PRE_CYCLE },
                        Action = new Model.DTO.Master.Action() { Id = PMS.Model.PMSConstants.ACTION_ID_APPRAISAL_CREATED },
                        ActionTimestamp = DateTime.Now,
                        Actioner = Model.Mappers.CoreMapper.MapUserDTOToEmployeeDTO(user)
                    };
                    obj_appraisal.AddTrail(obj_trail);

                    if (obj_participant.SMPEmploeeId != null && obj_participant.SMPEmploeeId > 0)
                    {
                        Model.DTO.Appraisal.Reviewer obj_reviewer = new Model.DTO.Appraisal.Reviewer() 
                        { 
                            EmployeeId = obj_participant.SMPEmploeeId,
                            Appraisal = obj_appraisal,
                            SMT = true
                        };
                        obj_appraisal.AddReviewer(obj_reviewer);
                    }

                    lst_appraisals.Add(obj_appraisal);
                }
            }
            return lst_appraisals;
        }

        public static List<Model.DTO.Appraisal.Appraisal> CreateAppraisalsForUpdateCycle(List<Model.DTO.Core.Employee> participants, List<Model.DTO.Cycle.Stage> stages, Model.DTO.Core.Security.User user)
        {
            List<Model.DTO.Appraisal.Appraisal> lst_appraisals = null;
            if (!Lib.Utility.Common.IsNullOrEmptyList(participants))
            {
                lst_appraisals = new List<Model.DTO.Appraisal.Appraisal>();
                foreach (Model.DTO.Core.Employee obj_participant in participants)
                {
                    Model.DTO.Appraisal.Appraisal obj_appraisal = new Model.DTO.Appraisal.Appraisal()
                    {
                        Stage = new Model.DTO.Master.Stage() { Id = Model.PMSConstants.STAGE_ID_PRE_CYCLE },
                        Status = new Model.DTO.Master.Status() { Id = Model.PMSConstants.STATUS_ID_NEW },
                        Employee = obj_participant,
                        Department = Model.Mappers.CoreMapper.MapDepartmentDTOToMasterDepartmentDTO(obj_participant.Department),
                        Locked = true
                    };

                    obj_appraisal.AddAppraisalSection(new Model.DTO.Appraisal.Section() { SectionId = Model.PMSConstants.SECTION_ID_KPI });
                    obj_appraisal.AddAppraisalSection(new Model.DTO.Appraisal.Section() { SectionId = Model.PMSConstants.SECTION_ID_CORE_VALUES });

                    int flag = CheckStageDate(stages[1], stages[2], stages[3]);
                    switch (flag)
                    {
                        case 1:
                            foreach (Model.DTO.Cycle.Stage obj_cycle_stage in stages)
                            {

                                Model.DTO.Appraisal.Stage obj_appraisal_stage = new Model.DTO.Appraisal.Stage()
                                {
                                    StageId = obj_cycle_stage.StageId,
                                    StartDate = obj_cycle_stage.StartDate,
                                    EndDate = obj_cycle_stage.EndDate,
                                    SubmissionDeadline = obj_cycle_stage.SubmissionDeadline,
                                    Level1ApprovalDeadline = obj_cycle_stage.Level1ApprovalDeadline,
                                    Level2ApprovalDeadline = obj_cycle_stage.Level2ApprovalDeadline
                                };
                                obj_appraisal.AddAppraisalStage(obj_appraisal_stage);
                            }
                            break;
                        case 2:
                        case 3:
                            foreach (Model.DTO.Cycle.Stage obj_cycle_stage in stages.Where(sec=>sec.StageId>eHR.PMS.Model.PMSConstants.STAGE_ID_PRE_CYCLE))
                            {

                                Model.DTO.Appraisal.Stage obj_appraisal_stage = new Model.DTO.Appraisal.Stage()
                                {
                                    StageId = obj_cycle_stage.StageId,
                                    StartDate = obj_cycle_stage.StartDate,
                                    EndDate = obj_cycle_stage.EndDate,
                                    SubmissionDeadline = obj_cycle_stage.SubmissionDeadline,
                                    Level1ApprovalDeadline = obj_cycle_stage.Level1ApprovalDeadline,
                                    Level2ApprovalDeadline = obj_cycle_stage.Level2ApprovalDeadline
                                };
                                obj_appraisal.AddAppraisalStage(obj_appraisal_stage);
                            }
                            break;
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                            foreach (Model.DTO.Cycle.Stage obj_cycle_stage in stages.Where(sec => (sec.StageId == eHR.PMS.Model.PMSConstants.STAGE_ID_GOAL_SETTING || sec.Id == eHR.PMS.Model.PMSConstants.STAGE_ID_FINAL_YEAR)))
                            {

                                Model.DTO.Appraisal.Stage obj_appraisal_stage = new Model.DTO.Appraisal.Stage()
                                {
                                    StageId = obj_cycle_stage.StageId,
                                    StartDate = obj_cycle_stage.StartDate,
                                    EndDate = obj_cycle_stage.EndDate,
                                    SubmissionDeadline = obj_cycle_stage.SubmissionDeadline,
                                    Level1ApprovalDeadline = obj_cycle_stage.Level1ApprovalDeadline,
                                    Level2ApprovalDeadline = obj_cycle_stage.Level2ApprovalDeadline
                                };
                                obj_appraisal.AddAppraisalStage(obj_appraisal_stage);
                            }
                            break;
                        default:
                            break;
                    }
                    

                    if (obj_participant.Level1Approver != null) { obj_appraisal.AddApprover(Model.Mappers.PMSMapper.MapEmployeeDTOToApproverDTO(obj_participant.Level1Approver, 1)); }
                    if (obj_participant.Level2Approver != null) { obj_appraisal.AddApprover(Model.Mappers.PMSMapper.MapEmployeeDTOToApproverDTO(obj_participant.Level2Approver, 2)); }

                    if (obj_participant.SMPEmploeeId != null && obj_participant.SMPEmploeeId > 0)
                    {
                        Model.DTO.Appraisal.Reviewer obj_reviewer = new Model.DTO.Appraisal.Reviewer()
                        {
                            EmployeeId = obj_participant.SMPEmploeeId,
                            Appraisal = obj_appraisal,
                            SMT = true
                        };
                        obj_appraisal.AddReviewer(obj_reviewer);
                    }

                    Model.DTO.Appraisal.Trail obj_trail = new Model.DTO.Appraisal.Trail()
                    {
                        Appraisal = obj_appraisal,
                        Stage = new Model.DTO.Master.Stage() { Id = PMS.Model.PMSConstants.STAGE_ID_PRE_CYCLE },
                        Action = new Model.DTO.Master.Action() { Id = PMS.Model.PMSConstants.ACTION_ID_APPRAISAL_CREATED },
                        ActionTimestamp = DateTime.Now,
                        Actioner = Model.Mappers.CoreMapper.MapUserDTOToEmployeeDTO(user)
                    };
                    obj_appraisal.AddTrail(obj_trail);

                    lst_appraisals.Add(obj_appraisal);
                }
            }
            return lst_appraisals;
        }

        public static List<PMS.Model.DTO.Appraisal.Appraisal> GetAppraisalsToApproveByApproverId(int approverId, int? cycleId)
        {
            List<PMS.Model.DTO.Appraisal.Appraisal> lst_appraisals = null;
            List<PMS.Model.DTO.Appraisal.Appraisal> lst_all_appraisals = Model.PMSModel.GetAppraisalsInCycleToApprove(approverId, cycleId);
            if (!Lib.Utility.Common.IsNullOrEmptyList(lst_all_appraisals))
            {
                lst_appraisals = new List<Model.DTO.Appraisal.Appraisal>();
                foreach (PMS.Model.DTO.Appraisal.Appraisal obj_appraisal in lst_all_appraisals)
                {
                    if (Lib.Utility.Common.IsNullOrEmptyList(lst_appraisals.Where(rec => rec.Id == obj_appraisal.Id)))
                    {
                        lst_appraisals.Add(obj_appraisal);
                    }
                }
            }
            return lst_appraisals;
        }

        public static List<PMS.Model.DTO.Appraisal.Appraisal> GetAppraisalsByEmployeeForDisplay(int employeeId, int? statusId)
        {
            List<PMS.Model.DTO.Appraisal.Appraisal> lst_appraisals = PMS.Model.PMSModel.GetAppraisalsByEmployee(employeeId, statusId);
            if (!Lib.Utility.Common.IsNullOrEmptyList(lst_appraisals))
            {
                PMS.Model.DTO.Appraisal.Appraisal obj_to_remove = lst_appraisals.Where(rec => rec.Stage.Id == PMS.Model.PMSConstants.STAGE_ID_PRE_CYCLE).SingleOrDefault();
                if (obj_to_remove != null)
                {
                    lst_appraisals.Remove(obj_to_remove);
                }
            }
            return lst_appraisals;
        }

        public static Model.DTO.Master.Stage GetAppraisalNextStage(int currentStageId)
        {
            Model.DTO.Master.Stage obj_stage = null;

            if (currentStageId == Model.PMSConstants.STAGE_ID_GOAL_SETTING)
            {
                obj_stage = new Model.DTO.Master.Stage()
                {
                    Id = Model.PMSConstants.STAGE_ID_PROGRESS_REVIEW
                };
            }

            if (currentStageId == Model.PMSConstants.STAGE_ID_PROGRESS_REVIEW)
            {
                obj_stage = new Model.DTO.Master.Stage()
                {
                    Id = Model.PMSConstants.STAGE_ID_FINAL_YEAR
                };

            }

            return obj_stage;
        }

        public static bool ProcessAppraisalSubmission(int appraisalId, int taskId, Model.DTO.Core.Security.User user, out string message)
        {
            bool boo_success = false;
            message = string.Empty;
            Model.DTO.Appraisal.Appraisal obj_appraisal = Model.PMSModel.GetAppraisalById(appraisalId);
            List<Model.DTO.Core.Task.Task> lst_completed_tasks = new List<Model.DTO.Core.Task.Task>();
            List<Model.DTO.Core.Task.Task> lst_new_tasks = new List<Model.DTO.Core.Task.Task>();

            if (obj_appraisal.Status.Id == Model.PMSConstants.STATUS_ID_NEW || obj_appraisal.Status.Id == Model.PMSConstants.STATUS_ID_DRAFT)
            {
                obj_appraisal.AddTrail(CreateAppraisalTrail(obj_appraisal, Model.Mappers.CoreMapper.MapUserDTOToEmployeeDTO(user), new Model.DTO.Master.Action() { Id = PMS.Model.PMSConstants.ACTION_ID_APPRAISAL_SUBMITTED }));
                obj_appraisal.Status = new Model.DTO.Master.Status() { Id = Model.PMSConstants.STATUS_ID_PENDING_LEVEL_1_APPROVAL };
                lst_completed_tasks.Add(new Model.DTO.Core.Task.Task() { Id = taskId, Status = new Model.DTO.Core.Master.Status() { Id = Model.PMSConstants.STATUS_CORE_ID_COMPLETED } });

                StringBuilder sb_task_name = new StringBuilder("Pending Level 1 Manager Approval For ");
                sb_task_name.Append(obj_appraisal.Employee.PreferredName);
                sb_task_name.Append("'s Appraisal.");

                string str_task_address = null;
                if (obj_appraisal.Stage.Id == Model.PMSConstants.STAGE_ID_GOAL_SETTING) { str_task_address = "/Stage1Approval/KeyPerformanceIndicators"; }
                if (obj_appraisal.Stage.Id == Model.PMSConstants.STAGE_ID_PROGRESS_REVIEW) { str_task_address = "/Stage2Approval/KeyPerformanceIndicators"; }

                lst_new_tasks.Add(CreateNewTaskForAppraisal(appraisalId,
                                            Model.PMSConstants.STATUS_CORE_ID_OPEN,
                                            sb_task_name.ToString(),
                                            str_task_address,
                                            Model.Mappers.CoreMapper.MapApproverDTOToOwnerDTO(obj_appraisal.GetApproverByLevel(1))));

                if (Model.PMSModel.UpdateAppraisalAndTask(obj_appraisal, lst_completed_tasks, lst_new_tasks, out message))
                {
                    List<System.Net.Mail.MailMessage> lst_email_messages = new List<System.Net.Mail.MailMessage>();
                    if (!string.IsNullOrEmpty(obj_appraisal.Employee.OfficeEmailAddress))
                    {
                        lst_email_messages.Add(GenerateEmailMessageForAppraisalSubmission(obj_appraisal));
                        SendEmailNotification(lst_email_messages);
                    }
                    boo_success = true;
                }
                else
                {
                    boo_success = false;
                }
            }

            return boo_success;
        }

        public static bool ProcessAppraisalApproval(int appraisalId, int taskId, bool approved, Model.DTO.Core.Security.User user, out string message)
        {
            bool boo_success = false;
            message = string.Empty;
            Model.DTO.Appraisal.Appraisal obj_appraisal = Model.PMSModel.GetAppraisalById(appraisalId);

            if (approved)
            {
                boo_success = ManageApprovedAppraisal(obj_appraisal, taskId, Model.Mappers.CoreMapper.MapUserDTOToEmployeeDTO(user), out message);
            }
            else
            {
                boo_success = ManageRejectedAppraisal(obj_appraisal, taskId, Model.Mappers.CoreMapper.MapUserDTOToEmployeeDTO(user), out message);
            }
            return boo_success;
        }

        private static bool ManageApprovedAppraisal(Model.DTO.Appraisal.Appraisal appraisal, int taskId, Model.DTO.Core.Employee manager, out string message)
        {
            bool boo_success = false;
            message = string.Empty;
            List<Model.DTO.Core.Task.Task> lst_completed_tasks = new List<Model.DTO.Core.Task.Task>();
            List<Model.DTO.Core.Task.Task> lst_new_tasks = new List<Model.DTO.Core.Task.Task>();
            List<System.Net.Mail.MailMessage> lst_email_messages;

            appraisal.AddTrail(CreateAppraisalTrail(appraisal, manager, 
                                    appraisal.Status.Id == Model.PMSConstants.STATUS_ID_PENDING_LEVEL_1_APPROVAL ? new Model.DTO.Master.Action() { Id = PMS.Model.PMSConstants.ACTION_ID_APPRAISAL_APPROVED_LEVEL_1 } : new Model.DTO.Master.Action() { Id = PMS.Model.PMSConstants.ACTION_ID_APPRAISAL_APPROVED_LEVEL_2 }));

            if (appraisal.Status.Id == Model.PMSConstants.STATUS_ID_PENDING_LEVEL_1_APPROVAL)
            {
                lst_email_messages = GenerateEmailMessageForApprovedAppraisal(appraisal, 1);

                appraisal.Status = new Model.DTO.Master.Status() { Id = Model.PMSConstants.STATUS_ID_PENDING_LEVEL_2_APPROVAL };
                StringBuilder sb_task_name = new StringBuilder("Pending Level 2 Manager Approval For ");
                sb_task_name.Append(appraisal.Employee.PreferredName);
                sb_task_name.Append("'s Appraisal.");

                string str_task_address = null;
                if (appraisal.Stage.Id == Model.PMSConstants.STAGE_ID_GOAL_SETTING) { str_task_address = "/Stage1Approval/KeyPerformanceIndicators"; }
                if (appraisal.Stage.Id == Model.PMSConstants.STAGE_ID_PROGRESS_REVIEW) { str_task_address = "/Stage2Approval/KeyPerformanceIndicators"; }


                lst_new_tasks.Add(CreateNewTaskForAppraisal(appraisal.Id,
                        Model.PMSConstants.STATUS_CORE_ID_OPEN,
                        sb_task_name.ToString(),
                        str_task_address,
                        Model.Mappers.CoreMapper.MapApproverDTOToOwnerDTO(appraisal.GetApproverByLevel(2))));
            }
            else
            {
                lst_email_messages = GenerateEmailMessageForApprovedAppraisal(appraisal, 2);
                appraisal.Status = new Model.DTO.Master.Status() { Id = Model.PMSConstants.STATUS_ID_APPROVED };
            }

            lst_completed_tasks.Add(new Model.DTO.Core.Task.Task() { Id = taskId, Status = new Model.DTO.Core.Master.Status() { Id = Model.PMSConstants.STATUS_CORE_ID_COMPLETED } });

            if (Model.PMSModel.UpdateAppraisalAndTask(appraisal, lst_completed_tasks, lst_new_tasks, out message))
            {
                boo_success = true;
                SendEmailNotification(lst_email_messages);
            }
            else
            {
                boo_success = false;
            }
            return boo_success;
        }

        private static bool ManageRejectedAppraisal(Model.DTO.Appraisal.Appraisal appraisal, int taskId, Model.DTO.Core.Employee manager, out string message)
        {
            bool boo_success = false;
            message = string.Empty;
            List<Model.DTO.Core.Task.Task> lst_completed_tasks = new List<Model.DTO.Core.Task.Task>();
            List<Model.DTO.Core.Task.Task> lst_new_tasks = new List<Model.DTO.Core.Task.Task>();
            List<System.Net.Mail.MailMessage> lst_email_messages = new List<System.Net.Mail.MailMessage>();

            lst_email_messages.Add(GenerateEmailMessageForRejectedAppraisal(appraisal, appraisal.Status.Id == Model.PMSConstants.STATUS_ID_PENDING_LEVEL_1_APPROVAL ? 1 : 2));

            Model.DTO.Appraisal.Trail obj_trail = new Model.DTO.Appraisal.Trail()
            {
                Appraisal = appraisal,
                Stage = appraisal.Stage,
                ActionTimestamp = DateTime.Now,
                Actioner = manager,
                Action = appraisal.Status.Id == Model.PMSConstants.STATUS_ID_PENDING_LEVEL_1_APPROVAL ? new Model.DTO.Master.Action() { Id = PMS.Model.PMSConstants.ACTION_ID_APPRAISAL_REJECTED_LEVEL_1 } : new Model.DTO.Master.Action() { Id = PMS.Model.PMSConstants.ACTION_ID_APPRAISAL_REJECTED_LEVEL_2 }
            };
            appraisal.AddTrail(obj_trail);
            appraisal.Status = new Model.DTO.Master.Status() { Id = Model.PMSConstants.STATUS_ID_NEW };

            string str_task_address = null;
            if (appraisal.Stage.Id == Model.PMSConstants.STAGE_ID_GOAL_SETTING) { str_task_address = "/Stage1/KeyPerformanceIndicators"; }
            if (appraisal.Stage.Id == Model.PMSConstants.STAGE_ID_PROGRESS_REVIEW) { str_task_address = "/Stage2/KeyPerformanceIndicators"; }


            lst_new_tasks.Add(CreateNewTaskForAppraisal(appraisal.Id,
                            Model.PMSConstants.STATUS_CORE_ID_OPEN,
                            "Re-submission of performance appraisal.",
                            str_task_address,
                            Model.Mappers.CoreMapper.MapEmployeeDTOToOwnerDTO(appraisal.Employee)));

            lst_completed_tasks.Add(new Model.DTO.Core.Task.Task() { Id = taskId, Status = new Model.DTO.Core.Master.Status() { Id = Model.PMSConstants.STATUS_CORE_ID_COMPLETED } });

            if (Model.PMSModel.UpdateAppraisalAndTask(appraisal, lst_completed_tasks, lst_new_tasks, out message))
            {
                boo_success = true;
                SendEmailNotification(lst_email_messages);
            }
            else
            {
                boo_success = false;
            }
            return boo_success;
        }
        
        public static bool ManageChangeReviewerMember(Model.DTO.Appraisal.Appraisal appraisal, List<Model.DTO.Appraisal.Reviewer> lst_reviewers, out string message)
        {

            message = string.Empty;
            bool result = false;
            List<Model.DTO.Appraisal.Reviewer> lst_oldReviewers=new List<Model.DTO.Appraisal.Reviewer>();
            result=Model.PMSModel.UpdateReviewersForAppraisal(appraisal.Id, lst_reviewers, out message,out lst_oldReviewers);
            if (result && lst_reviewers!=null)
            {
                List<System.Net.Mail.MailMessage> lst_email_messages=new List<System.Net.Mail.MailMessage>();

                lst_email_messages=GenerateEmailMessagesForReviewers(appraisal,lst_reviewers,lst_oldReviewers);
                SendEmailNotification(lst_email_messages);
            }

            return result;
        }
        
        public static bool ManageChangeApprover(Model.DTO.Appraisal.Appraisal appraisal, List<Model.DTO.Appraisal.Approver> newApprovers, out string message)
        {
            message = string.Empty;
            List<Model.DTO.Core.Task.Owner> lst_owners_to_update = null;
            bool result = false;
            List<Model.DTO.Appraisal.Approver> lst_oldApprovers = new List<Model.DTO.Appraisal.Approver>();
            if (!Lib.Utility.Common.IsNullOrEmptyList(newApprovers))
            {
                List<Model.DTO.Core.Task.Task> lst_tasks = Model.PMSModel.GetTasksByAppraisal(appraisal.Id, Model.PMSConstants.STATUS_CORE_ID_OPEN);
                lst_owners_to_update = new List<Model.DTO.Core.Task.Owner>();

                Model.DTO.Appraisal.Approver obj_new_level_1_approver = newApprovers.Where(rec => rec.ApprovalLevel == 1).SingleOrDefault();
                Model.DTO.Appraisal.Approver obj_new_level_2_approver = newApprovers.Where(rec => rec.ApprovalLevel == 2).SingleOrDefault();

                if (appraisal.Status.Id == Model.PMSConstants.STATUS_ID_PENDING_LEVEL_1_APPROVAL)
                {
                    if (obj_new_level_1_approver != null)
                    {
                        if (!Lib.Utility.Common.IsNullOrEmptyList(lst_tasks))
                        {
                            foreach (Model.DTO.Core.Task.Task obj_task in lst_tasks)
                            {
                                foreach (Model.DTO.Core.Task.Owner obj_owner in obj_task.Owners)
                                {
                                    obj_owner.EmployeeId = obj_new_level_1_approver.EmployeeId;
                                    lst_owners_to_update.Add(obj_owner);
                                }
                            }
                        }
                    }
                }
                else if (appraisal.Status.Id == Model.PMSConstants.STATUS_ID_PENDING_LEVEL_2_APPROVAL)
                {
                    if (obj_new_level_2_approver != null)
                    {
                        if (!Lib.Utility.Common.IsNullOrEmptyList(lst_tasks))
                        {
                            foreach (Model.DTO.Core.Task.Task obj_task in lst_tasks)
                            {
                                foreach (Model.DTO.Core.Task.Owner obj_owner in obj_task.Owners)
                                {
                                    obj_owner.EmployeeId = obj_new_level_2_approver.EmployeeId;
                                    lst_owners_to_update.Add(obj_owner);
                                }
                            }
                        }
                    }
                }
            }

            result = Model.PMSModel.UpdateApproversAndTasks(appraisal, newApprovers, lst_owners_to_update, out message, out lst_oldApprovers);
            if (result)
            {
                List<System.Net.Mail.MailMessage> lst_email_messages = new List<System.Net.Mail.MailMessage>();

                lst_email_messages = GenerateEmailMessagesForApprovers(appraisal, newApprovers, lst_oldApprovers);
                SendEmailNotification(lst_email_messages);
            }
            return result;
        }

        public static bool ManageChangeSMTMember(Model.DTO.Appraisal.Appraisal appraisal, Model.DTO.Appraisal.Reviewer smtMember, out string message)
        {
            message = string.Empty;
            bool result = false;
            Model.DTO.Appraisal.Reviewer oldMember=new Model.DTO.Appraisal.Reviewer();
            result=Model.PMSModel.UpdateSMTMember(appraisal,smtMember,out message,out oldMember);
            // no sending of email notification if it is SMT change
            //if (result)
            //{
            //    List<System.Net.Mail.MailMessage> lst_email_messages=new List<System.Net.Mail.MailMessage>();
            //    if(smtMember.EmployeeId!=oldMember.EmployeeId && smtMember.OfficeEmailAddress!=null && IsValidEmail(smtMember.OfficeEmailAddress))
            //        lst_email_messages.Add(GenerateEmailMessageForChangeSMTPerson(appraisal, smtMember, oldMember));
            //    SendEmailNotification(lst_email_messages);
            //}

            return result;
        }

        public static List<PMS.Model.DTO.Appraisal.Appraisal> GetAppraisalsForView(int cycleId, string employeeName, string employeeDomainId, string departmentName)
        {
            List<PMS.Model.DTO.Appraisal.Appraisal> lst_appraisals = new List<Model.DTO.Appraisal.Appraisal>();

            if (cycleId > 0)
            {
                lst_appraisals = Model.PMSModel.GetEmployeesInAppraisalsByCycleId(cycleId);
            }

            if (!Lib.Utility.Common.IsNullOrEmptyList(lst_appraisals))
            {
                if (!string.IsNullOrEmpty(employeeName))
                {
                    if (!string.IsNullOrEmpty(employeeDomainId))
                    {
                        if (!string.IsNullOrEmpty(departmentName))
                        {
                            lst_appraisals = lst_appraisals.Where(rec => rec.Employee.PreferredName.ToUpper().Contains(employeeName.ToUpper()) &&
                                                                                   rec.Employee.DomainId.ToUpper().Contains(employeeDomainId.ToUpper()) &&
                                                                                   rec.Department.Name.ToUpper().Contains(departmentName.ToUpper())).ToList();
                        }
                        else 
                        {
                            lst_appraisals = lst_appraisals.Where(rec => rec.Employee.PreferredName.ToUpper().Contains(employeeName.ToUpper()) &&
                                                                rec.Employee.DomainId.ToUpper().Contains(employeeDomainId.ToUpper())).ToList();
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(departmentName))
                        {
                            lst_appraisals = lst_appraisals.Where(rec => rec.Employee.PreferredName.ToUpper().Contains(employeeName.ToUpper())).ToList();
                        }
                        else
                        {
                            lst_appraisals = lst_appraisals.Where(rec => rec.Employee.PreferredName.ToUpper().Contains(employeeName.ToUpper()) &&
                                                                    rec.Department.Name.ToUpper().Contains(departmentName.ToUpper())).ToList();
                        }
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(employeeDomainId))
                    {
                        if (!string.IsNullOrEmpty(departmentName))
                        {
                            lst_appraisals = lst_appraisals.Where(rec => rec.Department.Name.ToUpper().Contains(departmentName.ToUpper())).ToList();
                        }
                    }
                    else 
                    {
                        if (string.IsNullOrEmpty(departmentName))
                        {
                            lst_appraisals = lst_appraisals.Where(rec => rec.Employee.DomainId.ToUpper().Contains(employeeDomainId.ToUpper())).ToList();
                        }
                        else
                        {
                            lst_appraisals = lst_appraisals.Where(rec => rec.Employee.DomainId.ToUpper().Contains(employeeDomainId.ToUpper()) &&
                                                                    rec.Department.Name.ToUpper().Contains(departmentName.ToUpper())).ToList();
                        }
                    }
                }
            }
            return lst_appraisals;
        }

        #endregion Appraisal

        #region Email

        public static System.Net.Mail.MailMessage GenerateEmailMessageForCycleStageStart(Model.DTO.Appraisal.Appraisal appraisal)
        {
            System.Net.Mail.MailMessage obj_email_message = null;
            if (!string.IsNullOrEmpty(appraisal.Employee.OfficeEmailAddress))
            {
                StringBuilder sb_subject = new StringBuilder("Performance Management ");
                sb_subject.Append(appraisal.Stage.Name);
                sb_subject.Append(" has started.");

                obj_email_message = new System.Net.Mail.MailMessage()
                {
                    Subject = sb_subject.ToString(),
                    From = new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["emailsenderaddress"], ConfigurationManager.AppSettings["emailsendername"]),
                    IsBodyHtml = true
                };

                Model.DTO.Appraisal.Stage obj_appraisal_start_stage = appraisal.AppraisalStages.OrderBy(rec => rec.StartDate).First();
                Model.DTO.Appraisal.Stage obj_appraisal_end_stage = appraisal.AppraisalStages.OrderByDescending(rec => rec.EndDate).First();
                Model.DTO.Appraisal.Stage obj_appraisal_current_stage = appraisal.AppraisalStages.Where(rec => rec.StageId == appraisal.Stage.Id).Single();

                StringBuilder sb_body = new StringBuilder();
                sb_body.Append("<p>Dear ");
                sb_body.Append(appraisal.Employee.PreferredName);
                sb_body.Append(", </p>");
                sb_body.Append("<p>FY");
                sb_body.Append(Convert.ToDateTime(obj_appraisal_start_stage.StartDate).ToString("yy"));
                sb_body.Append("/");
                sb_body.Append(Convert.ToDateTime(obj_appraisal_end_stage.EndDate).ToString("yy"));
                sb_body.Append(" Performance Management ");
                sb_body.Append(appraisal.Stage.Name);
                sb_body.Append(" Phase starts today. The Online Performance Management System is now open for employee input in the <a style='font-style:italic;' href='");
                sb_body.Append(ConfigurationManager.AppSettings["pmsweburl"]);
                sb_body.Append("'>eHR Portal</a>.</p>");
                sb_body.Append("<p>Please note that the submission date for Employees' KPIs for Managers' review is <u>");
                sb_body.Append(Convert.ToDateTime(obj_appraisal_current_stage.SubmissionDeadline).ToString("dd/MM/yyyy"));
                sb_body.Append("</u>.</p><br />");
                sb_body.Append("<p><span style='font-style:italic; font-size:small;'>This is a computer generated email. Please do not reply.</span></p>");

                obj_email_message.Body = sb_body.ToString();
                if (!string.IsNullOrEmpty(appraisal.Employee.OfficeEmailAddress) && Lib.Utility.Common.IsValidEmail(appraisal.Employee.OfficeEmailAddress))
                {
                    obj_email_message.To.Add(appraisal.Employee.OfficeEmailAddress);
                }
            }
            return obj_email_message;
        }


        private static System.Net.Mail.MailMessage GenerateEmailMessageForAppraisalSubmission(Model.DTO.Appraisal.Appraisal appraisal)
        {
            Model.DTO.Appraisal.Stage obj_appraisal_current_stage = appraisal.AppraisalStages.Where(rec => rec.StageId == appraisal.Stage.Id).Single();
            StringBuilder sb_subject = new StringBuilder("Approval required for ");
            sb_subject.Append(appraisal.Employee.PreferredName);
            sb_subject.Append("'s performance appraisal.");

            System.Net.Mail.MailMessage obj_email_message = new System.Net.Mail.MailMessage()
            {
                Subject = sb_subject.ToString(),
                From = new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["emailsenderaddress"], ConfigurationManager.AppSettings["emailsendername"]),
                IsBodyHtml = true  
            };

            StringBuilder sb_body = new StringBuilder();
            sb_body.Append("<p>Dear ");
            sb_body.Append(appraisal.GetApproverByLevel(1).PreferredName);
            sb_body.Append(", </p>");
            sb_body.Append("<p>The ");
            sb_body.Append(appraisal.Stage.Name.ToLower());
            sb_body.Append(" document of ");
            sb_body.Append(appraisal.Employee.PreferredName);
            sb_body.Append(" is now ready for your review and approval via the <a style='font-style:italic;' href='");
            sb_body.Append(ConfigurationManager.AppSettings["pmsweburl"]);
            sb_body.Append("'>eHR Portal</a>.</p>");
            sb_body.Append("<p>Please note that the submission date for Level 1 Managers' approval is <u>");
            sb_body.Append(Convert.ToDateTime(obj_appraisal_current_stage.Level1ApprovalDeadline).ToString("dd/MM/yyyy"));
            sb_body.Append("</u>.</p><br />");
            sb_body.Append("<p><span style='font-style:italic; font-size:small;'>This is a computer generated email. Please do not reply.</span></p>");

            obj_email_message.Body = sb_body.ToString();
            obj_email_message.To.Add(appraisal.GetApproverByLevel(1).OfficeEmailAddress);
            return obj_email_message;
        }

        private static System.Net.Mail.MailMessage GenerateEmailMessageForRejectedAppraisal(Model.DTO.Appraisal.Appraisal appraisal, int approvalLevel)
        {
            StringBuilder sb_subject = new StringBuilder("Your appraisal is rejected by your level ");
            sb_subject.Append(approvalLevel);
            sb_subject.Append(" manager.");

            System.Net.Mail.MailMessage obj_email_message = new System.Net.Mail.MailMessage()
            {
                Subject = sb_subject.ToString(),
                From = new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["emailsenderaddress"], ConfigurationManager.AppSettings["emailsendername"]),
                IsBodyHtml = true
            };

            StringBuilder sb_body = new StringBuilder();
            sb_body.Append("<p>Dear ");
            sb_body.Append(appraisal.Employee.PreferredName);
            sb_body.Append(", </p>");
            sb_body.Append("<p>Your Level ");
            sb_body.Append(approvalLevel);
            sb_body.Append(" Manager has reviewed your ");
            sb_body.Append(appraisal.Stage.Name.ToLower());
            sb_body.Append(" document and has returned to you for your further inputs via the <a style='font-style:italic;' href='");
            sb_body.Append(ConfigurationManager.AppSettings["pmsweburl"]);
            sb_body.Append("'>eHR Portal</a>.</p>");

            if (approvalLevel == 1)
            {
                Model.DTO.Appraisal.Stage obj_appraisal_current_stage = appraisal.AppraisalStages.Where(rec => rec.StageId == appraisal.Stage.Id).Single();
                sb_body.Append("<p>Please note that the submission deadline for Employees' KPIs for Managers' review is <u>");
                sb_body.Append(Convert.ToDateTime(obj_appraisal_current_stage.SubmissionDeadline).ToString("dd/MM/yyyy"));
                sb_body.Append("</u>.</p>");
            }

            if (approvalLevel == 2)
            {
                sb_body.Append("<p>Please submit your revised ");
                sb_body.Append(appraisal.Stage.Name.ToLower());
                sb_body.Append(" document for your Level 1 Manager's review urgently.</p>");
            }

            sb_body.Append("<br />");
            sb_body.Append("<p><span style='font-style:italic; font-size:small;'>This is a computer generated email. Please do not reply.</span></p>");

            obj_email_message.Body = sb_body.ToString();
            obj_email_message.To.Add(appraisal.Employee.OfficeEmailAddress);

            if (approvalLevel == 2)
            { 
                obj_email_message.CC.Add(appraisal.GetApproverByLevel(approvalLevel).OfficeEmailAddress);
            }
            return obj_email_message;
        }

        private static List<System.Net.Mail.MailMessage> GenerateEmailMessagesForApprovers(Model.DTO.Appraisal.Appraisal appr, List<Model.DTO.Appraisal.Approver> newMembers, List<Model.DTO.Appraisal.Approver> oldMembers)
        {
            List<System.Net.Mail.MailMessage> lst_messages = new List<System.Net.Mail.MailMessage>();
            int oldApproverersIndex = 0;
            foreach (Model.DTO.Appraisal.Approver approver in newMembers)
            {
                if (approver.OfficeEmailAddress != null && Lib.Utility.Common.IsValidEmail(approver.OfficeEmailAddress))
                {
                    if (oldMembers != null && oldMembers.Count >= (oldApproverersIndex + 1) && approver.EmployeeId != oldMembers[oldApproverersIndex].EmployeeId)
                        lst_messages.Add(GenerateEmailMessageForChangeApproverPerson(appr, approver, oldMembers[oldApproverersIndex++]));
                    else
                        lst_messages.Add(GenerateEmailMessageForChangeApproverPerson(appr, approver, null));
                }
            }
            return lst_messages;
        }
        private static List<System.Net.Mail.MailMessage> GenerateEmailMessagesForReviewers(Model.DTO.Appraisal.Appraisal appr, List<Model.DTO.Appraisal.Reviewer> newMembers,List<Model.DTO.Appraisal.Reviewer> oldMembers)
        {
            List<System.Net.Mail.MailMessage> lst_messages = new List<System.Net.Mail.MailMessage>();
            int oldReviewersIndex = 0;
            foreach (Model.DTO.Appraisal.Reviewer reviewer in newMembers)
            {
                if (reviewer.OfficeEmailAddress != null && Lib.Utility.Common.IsValidEmail(reviewer.OfficeEmailAddress))
                {
                    if (oldMembers!=null && oldMembers.Count >= (oldReviewersIndex + 1) && reviewer.EmployeeId != oldMembers[oldReviewersIndex].EmployeeId)
                        lst_messages.Add(GenerateEmailMessageForChangeReviewerPerson(appr, reviewer, oldMembers[oldReviewersIndex++]));
                    else
                        lst_messages.Add(GenerateEmailMessageForChangeReviewerPerson(appr, reviewer, null));
                }
            }
            return lst_messages;
        }

        private static System.Net.Mail.MailMessage GenerateEmailMessageForChangeApproverPerson(Model.DTO.Appraisal.Appraisal appr, Model.DTO.Appraisal.Approver newMember, Model.DTO.Appraisal.Approver oldmember)
        {

            StringBuilder sb_subject = new StringBuilder();
            sb_subject.Append("Change in Approver for ");
            sb_subject.Append(appr.Employee.PreferredName);
            sb_subject.Append("’s Performance Appraisal");


            System.Net.Mail.MailMessage obj_email_message = new System.Net.Mail.MailMessage()
            {
                Subject = sb_subject.ToString(),
                From = new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["emailsenderaddress"], ConfigurationManager.AppSettings["emailsendername"]),
                IsBodyHtml = true
            };
            StringBuilder sb_body = new StringBuilder();
             sb_body.Append("<p>Hi ");
             sb_body.Append(newMember.PreferredName);
             sb_body.Append(",</p><p>   ");
             //sb_body.Append(appr.Employee.PreferredName);
             sb_body.Append(" You are now the Level ");
             sb_body.Append(newMember.ApprovalLevel.HasValue ? newMember.ApprovalLevel.Value.ToString() : "");
             sb_body.Append(" Manager for ");
             sb_body.Append(appr.Employee.PreferredName);
             sb_body.Append("’s appraisal. You can now process his/her appraisal form via the <a style='font-style:italic;' href='"+ConfigurationManager.AppSettings["pmsweburl"]+"'> eHR Portal </a> </p>");
             sb_body.Append("<p><span style='font-style:italic; font-size:small;'>This is a computer generated email. Please do not reply.</span></p>");            
            obj_email_message.Body = sb_body.ToString();
            //if (newMember.OfficeEmailAddress != null && IsValidEmail(newMember.OfficeEmailAddress))
            obj_email_message.To.Add(newMember.OfficeEmailAddress);
            if (oldmember != null && oldmember.OfficeEmailAddress != null && Lib.Utility.Common.IsValidEmail(oldmember.OfficeEmailAddress))
                obj_email_message.CC.Add(oldmember.OfficeEmailAddress);
            return obj_email_message;
        }
        private static System.Net.Mail.MailMessage GenerateEmailMessageForChangeReviewerPerson(Model.DTO.Appraisal.Appraisal appr, Model.DTO.Appraisal.Reviewer newMember, Model.DTO.Appraisal.Reviewer oldmember)
        {

            StringBuilder sb_subject = new StringBuilder();
            sb_subject.Append("Change in Reviewer for ");
            sb_subject.Append(appr.Employee.PreferredName);
            sb_subject.Append("’s Performance Appraisal");


            System.Net.Mail.MailMessage obj_email_message = new System.Net.Mail.MailMessage()
            {
                Subject = sb_subject.ToString(),
                From = new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["emailsenderaddress"], ConfigurationManager.AppSettings["emailsendername"]),
                IsBodyHtml = true
            };
            StringBuilder sb_body = new StringBuilder();
             sb_body.Append("<p>Hi ");
             sb_body.Append(newMember.PreferredName);
             sb_body.Append(",</p><p>   ");
             sb_body.Append(appr.Employee.PreferredName);
             sb_body.Append(" has selected you as the Reviewer for his/her appraisal. You can now view his/her appraisal form via the<a style='font-style:italic;' href='"+ConfigurationManager.AppSettings["pmsweburl"]+"'> eHR Portal </a> </p>");
             sb_body.Append("<p><span style='font-style:italic; font-size:small;'>This is a computer generated email. Please do not reply.</span></p>");            
            obj_email_message.Body = sb_body.ToString();
            //if (newMember.OfficeEmailAddress != null && IsValidEmail(newMember.OfficeEmailAddress))
            obj_email_message.To.Add(newMember.OfficeEmailAddress);
            if (oldmember != null && oldmember.OfficeEmailAddress != null && Lib.Utility.Common.IsValidEmail(oldmember.OfficeEmailAddress))
                obj_email_message.CC.Add(oldmember.OfficeEmailAddress);
            return obj_email_message;
        }

        private static System.Net.Mail.MailMessage GenerateEmailMessageForChangeSMTPerson(Model.DTO.Appraisal.Appraisal appr, Model.DTO.Appraisal.Reviewer newMember, Model.DTO.Appraisal.Reviewer oldmember)
        {
            
            StringBuilder sb_subject = new StringBuilder();
            sb_subject.Append("Change in Approver for ");
            sb_subject.Append(appr.Employee.PreferredName);
            sb_subject.Append("’s Performance Appraisal");
                   
            
            System.Net.Mail.MailMessage obj_email_message = new System.Net.Mail.MailMessage()
            {
                Subject = sb_subject.ToString(),
                From = new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["emailsenderaddress"], ConfigurationManager.AppSettings["emailsendername"]),
                IsBodyHtml = true
            };
            StringBuilder sb_body = new StringBuilder();
                    sb_body.Append("<p>Hi ");
                    sb_body.Append(newMember.PreferredName);
                    sb_body.Append(",</p><p>   You are now the Senior Management Team member for ");
                    sb_body.Append(appr.Employee.PreferredName);
                    sb_body.Append(". You can now process his/her appraisal form via the<a style='font-style:italic;' href='"+ConfigurationManager.AppSettings["pmsweburl"]+"'> eHR Portal </a> </p>");
                    sb_body.Append("<p><span style='font-style:italic; font-size:small;'>This is a computer generated email. Please do not reply.</span></p>");            
            obj_email_message.Body = sb_body.ToString();
            //if (newMember.OfficeEmailAddress != null && IsValidEmail(newMember.OfficeEmailAddress))
            obj_email_message.To.Add(newMember.OfficeEmailAddress);
            if (oldmember != null && oldmember.OfficeEmailAddress != null && Lib.Utility.Common.IsValidEmail(oldmember.OfficeEmailAddress))
                obj_email_message.CC.Add(oldmember.OfficeEmailAddress);
            return obj_email_message;
        }
        private static System.Net.Mail.MailMessage GenerateEmailMessageToEmployeeForApprovedAppraisal(Model.DTO.Appraisal.Appraisal appraisal, int approvalLevel)
        {
            StringBuilder sb_subject = new StringBuilder("Your appraisal is approved by your level ");
            sb_subject.Append(approvalLevel);
            sb_subject.Append(" manager.");

            System.Net.Mail.MailMessage obj_email_message = new System.Net.Mail.MailMessage()
            {
                Subject = sb_subject.ToString(),
                From = new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["emailsenderaddress"], ConfigurationManager.AppSettings["emailsendername"]),
                IsBodyHtml = true
            };

            StringBuilder sb_body = new StringBuilder();
            sb_body.Append("<p>Dear ");
            sb_body.Append(appraisal.Employee.PreferredName);
            sb_body.Append(", </p>");
            if (approvalLevel == 1)
            {
                sb_body.Append("<p>Your Level 1 Manager has concurred with your goals.</p>");
            }
            else
            {
                sb_body.Append("<p>Your Level 2 Manager has approved your ");
                sb_body.Append(appraisal.Stage.Name.ToLower());
                sb_body.Append(" document and you may view them at <a style='font-style:italic;' href='");
                sb_body.Append(ConfigurationManager.AppSettings["pmsweburl"]);
                sb_body.Append("'>eHR Portal</a>. Your ");
                sb_body.Append(appraisal.Stage.Name);
                sb_body.Append(" Phase is now completed.</p>");
            }
            
            sb_body.Append("<br />");
            sb_body.Append("<p><span style='font-style:italic; font-size:small;'>This is a computer generated email. Please do not reply.</span></p>");

            obj_email_message.Body = sb_body.ToString();
            if (appraisal.Employee.OfficeEmailAddress != null && Lib.Utility.Common.IsValidEmail(appraisal.Employee.OfficeEmailAddress))
                obj_email_message.To.Add(appraisal.Employee.OfficeEmailAddress);

            if (approvalLevel == 2 && appraisal.GetApproverByLevel(1).OfficeEmailAddress!=null)
            {
                obj_email_message.CC.Add(appraisal.GetApproverByLevel(1).OfficeEmailAddress);
            }

            return obj_email_message;
        }

        private static List<System.Net.Mail.MailMessage> GenerateEmailMessageForApprovedAppraisal(Model.DTO.Appraisal.Appraisal appraisal, int approvalLevel)
        {
            Model.DTO.Appraisal.Stage obj_appraisal_current_stage = appraisal.AppraisalStages.Where(rec => rec.StageId == appraisal.Stage.Id).Single();
            List<System.Net.Mail.MailMessage> lst_messages = new List<System.Net.Mail.MailMessage>();

            System.Net.Mail.MailMessage temp = new System.Net.Mail.MailMessage();
            if (approvalLevel == 1)
            {
                temp = GenerateEmailMessageToEmployeeForApprovedAppraisal(appraisal, approvalLevel);
                if(temp!=null)
                    lst_messages.Add(temp);
                StringBuilder sb_subject = new StringBuilder("Approval required for ");
                sb_subject.Append(appraisal.Employee.PreferredName);
                sb_subject.Append("'s performance appraisal.");

                System.Net.Mail.MailMessage obj_email_message = new System.Net.Mail.MailMessage()
                {
                    Subject = sb_subject.ToString(),
                    From = new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["emailsenderaddress"], ConfigurationManager.AppSettings["emailsendername"]),
                    IsBodyHtml = true
                };

                StringBuilder sb_body = new StringBuilder();
                sb_body.Append("<p>Dear ");
                sb_body.Append(appraisal.GetApproverByLevel(2).PreferredName);
                sb_body.Append(", </p>");
                sb_body.Append("<p>");
                sb_body.Append(appraisal.GetApproverByLevel(1).PreferredName);
                sb_body.Append(", has reviewed and approved the ");
                sb_body.Append(appraisal.Stage.Name.ToLower());
                sb_body.Append(" document for ");
                sb_body.Append(appraisal.Employee.PreferredName);
                sb_body.Append(" and it is now ready for your approval via <a style='font-style:italic;' href='");
                sb_body.Append(ConfigurationManager.AppSettings["pmsweburl"]);
                sb_body.Append("'>eHR Portal</a>.</p>");
                sb_body.Append("<p>Please note that the submission deadline for Level 2 Manager's approval is <u>");
                sb_body.Append(Convert.ToDateTime(obj_appraisal_current_stage.Level2ApprovalDeadline).ToString("dd/MM/yyyy"));
                sb_body.Append("</u>. Upon approval, the ");
                sb_body.Append(appraisal.Stage.Name);
                sb_body.Append(" Phase will be completed and will be submitted to HR.</p>");
                sb_body.Append("<br />");
                sb_body.Append("<p><span style='font-style:italic; font-size:small;'>This is a computer generated email. Please do not reply.</span></p>");

                obj_email_message.Body = sb_body.ToString();
                if (appraisal.GetApproverByLevel(2).OfficeEmailAddress != null && Lib.Utility.Common.IsValidEmail(appraisal.GetApproverByLevel(2).OfficeEmailAddress))
                    obj_email_message.To.Add(appraisal.GetApproverByLevel(2).OfficeEmailAddress);
                lst_messages.Add(obj_email_message);
            }

            if (approvalLevel == 2)
            {
                temp = GenerateEmailMessageToEmployeeForApprovedAppraisal(appraisal, approvalLevel);
                if (temp != null)
                    lst_messages.Add(temp);
            }

            return lst_messages;
        }

        public static void SendEmailNotification(List<System.Net.Mail.MailMessage> emails)
        {
            if (!Lib.Utility.Common.IsNullOrEmptyList(emails))
            {
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(ConfigurationManager.AppSettings["smtphost"]);
                try
                {
                    foreach (System.Net.Mail.MailMessage obj_message in emails)
                    {
                        smtp.Send(obj_message);
                    }
                    smtp.Dispose();
                }
                catch (Exception exc)
                {
                    throw exc;
                }
                finally
                {
                    smtp.Dispose();
                }
            }
        }

        #endregion

        #region Employee Retrieval

        private static List<Model.DTO.Core.Employee> FilterEmployees(string employeeName, string employeeDomainId, string employeeDepartmentName, IEnumerable<PMS.Model.DTO.Core.Employee> lst_filtered_employees)
        {
            if (!string.IsNullOrEmpty(employeeName))
            {
                if (!string.IsNullOrEmpty(employeeDomainId))
                {
                    if (!string.IsNullOrEmpty(employeeDepartmentName))
                    {
                        // all 3 search criteria provided
                        lst_filtered_employees = lst_filtered_employees.Where(rec => rec.PreferredName.ToUpper().Contains(employeeName.ToUpper()) &&
                                                                                        rec.DomainId.ToUpper().Contains(employeeDomainId.ToUpper()) &&
                                                                                        rec.Department.Name.ToUpper().Contains(employeeDepartmentName.ToUpper()));
                    }
                    else
                    {
                        // employ name and domain id provided
                        lst_filtered_employees = lst_filtered_employees.Where(rec => rec.PreferredName.ToUpper().Contains(employeeName.ToUpper()) &&
                                                                                         rec.DomainId.ToUpper().Contains(employeeDomainId.ToUpper()));
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(employeeDepartmentName))
                    {
                        //employee name and department name provided
                        lst_filtered_employees = lst_filtered_employees.Where(rec => rec.PreferredName.ToUpper().Contains(employeeName.ToUpper()) &&
                                                                                        rec.Department.Name.ToUpper().Contains(employeeDepartmentName.ToUpper()));
                    }
                    else
                    {
                        //employee name provided
                        lst_filtered_employees = lst_filtered_employees.Where(rec => rec.PreferredName.ToUpper().Contains(employeeName.ToUpper()));
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(employeeDomainId))
                {
                    if (!string.IsNullOrEmpty(employeeDepartmentName))
                    {
                        // domain id and department name provided
                        lst_filtered_employees = lst_filtered_employees.Where(rec => rec.DomainId.ToUpper().Contains(employeeDomainId.ToUpper()) &&
                                                                                        rec.Department.Name.ToUpper().Contains(employeeDepartmentName.ToUpper()));

                    }
                    else
                    {
                        //only domain id provided
                        lst_filtered_employees = lst_filtered_employees.Where(rec => rec.DomainId.ToUpper().Contains(employeeDomainId.ToUpper()));

                    }
                }
                else
                {
                    // only department name is provided
                    lst_filtered_employees = lst_filtered_employees.Where(rec => rec.Department.Name.ToUpper().Contains(employeeDepartmentName.ToUpper()));
                }
            }

            if (!Lib.Utility.Common.IsNullOrEmptyList(lst_filtered_employees))
            {
                return lst_filtered_employees.ToList();
            }
            else
                return null;
        }
        public static List<Model.DTO.Core.Employee> GetEmployeesToAddToCycle(string employeeName, string employeeDomainId, string employeeDepartmentName, List<Model.DTO.Core.Employee> currentParticipants)
        {
            List<PMS.Model.DTO.Core.Employee> lst_active_employees = null; 
            List<PMS.Model.DTO.Core.Employee> lst_eligible_employees = null;
            List<Int32> lst_current_participant_ids = null;

            if (!string.IsNullOrEmpty(employeeName) || !string.IsNullOrEmpty(employeeDomainId) || !string.IsNullOrEmpty(employeeDepartmentName))
            {
                lst_active_employees = Model.PMSModel.GetEmployees(true);
                lst_current_participant_ids = new List<int>();
                if (!Lib.Utility.Common.IsNullOrEmptyList(currentParticipants))
                {
                    
                    foreach (Model.DTO.Core.Employee obj_employee in currentParticipants)
                    {
                        lst_current_participant_ids.Add(obj_employee.Id);
                    }
                }

                if (!Lib.Utility.Common.IsNullOrEmptyList(lst_active_employees))
                {
                    IEnumerable<PMS.Model.DTO.Core.Employee> lst_filtered_employees = lst_active_employees.Where(rec => rec.Department != null &&
                                                                                                                       rec.ACRGrade != null &&
                                                                                                                       rec.EmploymentType != null &&
                                                                                                                       rec.GetNumberOfApprovers() == 2 &&
                                                                                                                       !lst_current_participant_ids.Contains(rec.Id));
                    lst_eligible_employees = FilterEmployees(employeeName, employeeDomainId, employeeDepartmentName, lst_filtered_employees);
                }
            }
            return lst_eligible_employees;
        }

        public static List<Model.DTO.Core.Employee> GetEmployeesToRemoveFromCycle(string employeeName, string employeeDomainId, string employeeDepartmentName, List<Model.DTO.Core.Employee> currentParticipants)
        {
            List<PMS.Model.DTO.Core.Employee> lst_remove_employees = null;
            IEnumerable<PMS.Model.DTO.Core.Employee> lst_to_remove_employees = null;

            if (!string.IsNullOrEmpty(employeeName) || !string.IsNullOrEmpty(employeeDomainId) || !string.IsNullOrEmpty(employeeDepartmentName))
            {
                //lst_active_employees = Model.PMSModel.GetEmployees(true);

                if (!Lib.Utility.Common.IsNullOrEmptyList(currentParticipants))
                {
                    if (!string.IsNullOrEmpty(employeeName))
                    {
                        if (!string.IsNullOrEmpty(employeeDomainId))
                        {
                            if (!string.IsNullOrEmpty(employeeDepartmentName))
                            {
                                // all 3 search criteria provided
                                lst_to_remove_employees = currentParticipants.Where(rec => rec.PreferredName.ToUpper().Contains(employeeName.ToUpper()) &&
                                                                                                rec.DomainId.ToUpper().Contains(employeeDomainId.ToUpper()) &&
                                                                                                rec.Department.Name.ToUpper().Contains(employeeDepartmentName.ToUpper()));
                            }
                            else
                            {
                                // employ name and domain id provided
                                lst_to_remove_employees = currentParticipants.Where(rec => rec.PreferredName.ToUpper().Contains(employeeName.ToUpper()) &&
                                                                                                 rec.DomainId.ToUpper().Contains(employeeDomainId.ToUpper()));
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(employeeDepartmentName))
                            {
                                //employee name and department name provided
                                lst_to_remove_employees = currentParticipants.Where(rec => rec.PreferredName.ToUpper().Contains(employeeName.ToUpper()) &&
                                                                                                rec.Department.Name.ToUpper().Contains(employeeDepartmentName.ToUpper()));
                            }
                            else
                            {
                                //employee name provided
                                lst_to_remove_employees = currentParticipants.Where(rec => rec.PreferredName.ToUpper().Contains(employeeName.ToUpper()));
                            }
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(employeeDomainId))
                        {
                            if (!string.IsNullOrEmpty(employeeDepartmentName))
                            {
                                // domain id and department name provided
                                lst_to_remove_employees = currentParticipants.Where(rec => rec.DomainId.ToUpper().Contains(employeeDomainId.ToUpper()) &&
                                                                                                rec.Department.Name.ToUpper().Contains(employeeDepartmentName.ToUpper()));

                            }
                            else
                            {
                                //only domain id provided
                                lst_to_remove_employees = currentParticipants.Where(rec => rec.DomainId.ToUpper().Contains(employeeDomainId.ToUpper()));

                            }
                        }
                        else
                        {
                            // only department name is provided
                            lst_to_remove_employees = currentParticipants.Where(rec => rec.Department.Name.ToUpper().Contains(employeeDepartmentName.ToUpper()));
                        }
                    }

                    if (!Lib.Utility.Common.IsNullOrEmptyList(lst_to_remove_employees))
                    {
                        lst_remove_employees = lst_to_remove_employees.ToList();
                    }
                }
            }
            return lst_remove_employees;
        }

        public static List<Model.DTO.Appraisal.Appraisal> GetApprisalsToRemoveFromCycle(string employeeName, string employeeDomainId, string employeeDepartmentName, List<Model.DTO.Appraisal.Appraisal> currentParticipants)
        {
            List<PMS.Model.DTO.Appraisal.Appraisal> lst_remove_employees = null;
            IEnumerable<PMS.Model.DTO.Appraisal.Appraisal> lst_to_remove_employees = null;

            if (!string.IsNullOrEmpty(employeeName) || !string.IsNullOrEmpty(employeeDomainId) || !string.IsNullOrEmpty(employeeDepartmentName))
            {
                //lst_active_employees = Model.PMSModel.GetEmployees(true);

                if (!Lib.Utility.Common.IsNullOrEmptyList(currentParticipants))
                {
                    if (!string.IsNullOrEmpty(employeeName))
                    {
                        if (!string.IsNullOrEmpty(employeeDomainId))
                        {
                            if (!string.IsNullOrEmpty(employeeDepartmentName))
                            {
                                // all 3 search criteria provided
                                lst_to_remove_employees = currentParticipants.Where(rec => rec.Employee.PreferredName.ToUpper().Contains(employeeName.ToUpper()) &&
                                                                                                rec.Employee.DomainId.ToUpper().Contains(employeeDomainId.ToUpper()) &&
                                                                                                rec.Department.Name.ToUpper().Contains(employeeDepartmentName.ToUpper()));
                            }
                            else
                            {
                                // employ name and domain id provided
                                lst_to_remove_employees = currentParticipants.Where(rec => rec.Employee.PreferredName.ToUpper().Contains(employeeName.ToUpper()) &&
                                                                                                 rec.Employee.DomainId.ToUpper().Contains(employeeDomainId.ToUpper()));
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(employeeDepartmentName))
                            {
                                //employee name and department name provided
                                lst_to_remove_employees = currentParticipants.Where(rec => rec.Employee.PreferredName.ToUpper().Contains(employeeName.ToUpper()) &&
                                                                                                rec.Department.Name.ToUpper().Contains(employeeDepartmentName.ToUpper()));
                            }
                            else
                            {
                                //employee name provided
                                lst_to_remove_employees = currentParticipants.Where(rec => rec.Employee.PreferredName.ToUpper().Contains(employeeName.ToUpper()));
                            }
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(employeeDomainId))
                        {
                            if (!string.IsNullOrEmpty(employeeDepartmentName))
                            {
                                // domain id and department name provided
                                lst_to_remove_employees = currentParticipants.Where(rec => rec.Employee.DomainId.ToUpper().Contains(employeeDomainId.ToUpper()) &&
                                                                                                rec.Department.Name.ToUpper().Contains(employeeDepartmentName.ToUpper()));

                            }
                            else
                            {
                                //only domain id provided
                                lst_to_remove_employees = currentParticipants.Where(rec => rec.Employee.DomainId.ToUpper().Contains(employeeDomainId.ToUpper()));

                            }
                        }
                        else
                        {
                            // only department name is provided
                            lst_to_remove_employees = currentParticipants.Where(rec => rec.Department.Name.ToUpper().Contains(employeeDepartmentName.ToUpper()));
                        }
                    }

                    if (!Lib.Utility.Common.IsNullOrEmptyList(lst_to_remove_employees))
                    {
                        lst_remove_employees = lst_to_remove_employees.ToList();
                    }
                }
            }
            return lst_remove_employees;
        }
        public static List<Model.DTO.Core.Employee> GetEligibleEmployeesForCycle(DateTime eligibleDateStart, DateTime eligibleDateEnd)
        {
            List<PMS.Model.DTO.Core.Employee> lst_active_employees = Model.PMSModel.GetEmployees(true);
            List<PMS.Model.DTO.Core.Employee> lst_eligible_employees = null;

            if (!Lib.Utility.Common.IsNullOrEmptyList(lst_active_employees))
            {
                IEnumerable<PMS.Model.DTO.Core.Employee> lst_filtered_employees = lst_active_employees.Where(rec => rec.Department != null &&
                                                                                                                    rec.ACRGrade != null &&
                                                                                                                    (rec.EmploymentType !=null && rec.EmploymentType.Id == PMS.Model.PMSConstants.EMPLOYMENT_TYPE_ID_PERMANENT) &&
                                                                                                                    rec.GetNumberOfApprovers() == 2 &&
                                                                                                                    (rec.DateOfHire != null && rec.DateOfHire < eligibleDateStart) &&
                                                                                                                    (rec.DateOfDeparture == null || rec.DateOfDeparture > eligibleDateEnd) &&
                                                                                                                    (rec.SMPEmploeeId != null && rec.SMPEmploeeId > 0)
                                                                                                                   );
                if (!Lib.Utility.Common.IsNullOrEmptyList(lst_filtered_employees))
                {
                    lst_eligible_employees = lst_filtered_employees.ToList();
                }
            }
            return lst_eligible_employees;
        }

        #endregion Employee Retrieval

        #region KPI Items

        public static List<PMS.Model.DTO.Appraisal.KPI> GetKPIItemsToUpdate(string[] result) 
        {
            List<PMS.Model.DTO.Appraisal.KPI> lst_kpis = null;

            if (!Lib.Utility.Common.IsNullOrEmptyList(result))
            {
                lst_kpis = new List<Model.DTO.Appraisal.KPI>();
                string[] arr_values = new string[7];
                string[] arr_seperator = { "^&*" };

                foreach (string str_result in result.Where(sec => !sec.Contains("NewKPI"))) 
                {
                    arr_values = Uri.UnescapeDataString(str_result).Split(arr_seperator, StringSplitOptions.None);
                    PMS.Model.DTO.Appraisal.KPI obj_kpi = new Model.DTO.Appraisal.KPI()
                    {
                        Id = Convert.ToInt32(arr_values[0]),
                        Appraisal = new Model.DTO.Appraisal.Appraisal() { Id = Convert.ToInt32(arr_values[1]) },
                        Section = new Model.DTO.Master.Section() { Id = Convert.ToInt32(arr_values[2]) },
                        Block = new Model.DTO.Master.Block() { Id = Convert.ToInt32(arr_values[3]) },
                        //Description = arr_values[4].Trim().TrimEnd('\r', '\n').Replace("\\n", Environment.NewLine),
                        //Target = arr_values[5].Trim().TrimEnd('\r', '\n').Replace("\\n", Environment.NewLine),
                        Description = Lib.Utility.Common.ReplaceLineBreaksForDatabase(arr_values[4].Trim()),
                        Target = Lib.Utility.Common.ReplaceLineBreaksForDatabase(arr_values[5].Trim()),
                        Priority = new Model.DTO.Master.Priority() { Id = Convert.ToInt32(arr_values[6]) }
                    };
                    lst_kpis.Add(obj_kpi);
                }
            }
            return lst_kpis;
        }

        public static List<PMS.Model.DTO.Appraisal.KPI> GetKPIItemsToInsert(string[] result) 
        {
            List<PMS.Model.DTO.Appraisal.KPI> lst_kpis = null;

            if (!Lib.Utility.Common.IsNullOrEmptyList(result))
            {
                lst_kpis = new List<Model.DTO.Appraisal.KPI>();
                string[] arr_values = new string[7];
                string[] arr_seperator = { "^&*" };

                foreach (string str_result in result.Where(sec => sec.Contains("NewKPI")))
                {
                    arr_values = Uri.UnescapeDataString(str_result).Split(arr_seperator, StringSplitOptions.None);

                    if (arr_values[4].Trim().Length != 0 && arr_values[5].Trim().Length != 0)
                    {
                        PMS.Model.DTO.Appraisal.KPI obj_kpi = new Model.DTO.Appraisal.KPI()
                        {
                            Appraisal = new Model.DTO.Appraisal.Appraisal() { Id = Convert.ToInt32(arr_values[1]) },
                            Section = new Model.DTO.Master.Section() { Id = Convert.ToInt32(arr_values[2]) },
                            Block = new Model.DTO.Master.Block() { Id = Convert.ToInt32(arr_values[3]) },
                            //Description = arr_values[4].Trim().TrimEnd('\r', '\n').Replace("\\n", Environment.NewLine),
                            //Target = arr_values[5].Trim().TrimEnd('\r', '\n').Replace("\\n", Environment.NewLine),
                            Description = Lib.Utility.Common.ReplaceLineBreaksForDatabase(arr_values[4].Trim()),
                            Target = Lib.Utility.Common.ReplaceLineBreaksForDatabase(arr_values[5].Trim()),
                            Priority = new Model.DTO.Master.Priority() { Id = Convert.ToInt32(arr_values[6]) }
                        };
                        lst_kpis.Add(obj_kpi);
                    }
                }
            }
            return lst_kpis;
        }

        public static List<PMS.Model.DTO.Appraisal.KPI> GetKPIItemsToDelete(string[] result)
        {
            List<PMS.Model.DTO.Appraisal.KPI> lst_kpis = null;

            if (!Lib.Utility.Common.IsNullOrEmptyList(result))
            {
                lst_kpis = new List<Model.DTO.Appraisal.KPI>();

                foreach (string str_result in result)
                {
                    if (!string.IsNullOrEmpty(str_result)) 
                    {
                        PMS.Model.DTO.Appraisal.KPI obj_kpi = new Model.DTO.Appraisal.KPI() { Id = Int32.Parse(str_result) };
                        lst_kpis.Add(obj_kpi);
                    }
                }   
            }
            return lst_kpis;
        }

        public static List<PMS.Model.DTO.Appraisal.KPI> GetKPIProgressToUpdate(string[] result)
        {
            List<PMS.Model.DTO.Appraisal.KPI> lst_kpis = null;

            if (!Lib.Utility.Common.IsNullOrEmptyList(result))
            {
                lst_kpis = new List<Model.DTO.Appraisal.KPI>();
                string[] arr_values = new string[2];
                string[] arr_seperator = { "^&*" };

                foreach (string str_result in result)
                {
                    arr_values = Uri.UnescapeDataString(str_result).Split(arr_seperator, StringSplitOptions.None);
                    PMS.Model.DTO.Appraisal.KPI obj_kpi = new Model.DTO.Appraisal.KPI()
                    {
                        Id = Convert.ToInt32(arr_values[1].Substring(8)),
                        Progress = Lib.Utility.Common.ReplaceLineBreaksForDatabase(arr_values[0])
                    };
                    lst_kpis.Add(obj_kpi);
                } 
            }
            return lst_kpis;
        }
        public static List<PMS.Model.DTO.Appraisal.KPI> GetKPIProgressToUpdateForAjax(string[] result)
        {
            List<PMS.Model.DTO.Appraisal.KPI> lst_kpis = null;

            if (!Lib.Utility.Common.IsNullOrEmptyList(result))
            {
                lst_kpis = new List<Model.DTO.Appraisal.KPI>();
                string[] arr_values = new string[2];

                foreach (string str_result in result)
                {
                    string[] kparray = str_result.Replace("\"", "").Split(',');
                    //string tmp_comment = kparray[1].Split(':')[1].Replace("}]", "").Trim().TrimEnd('\r', '\n').Replace("\\n", Environment.NewLine).Trim();
                    string tmp_progress = Lib.Utility.Common.ReplaceLineBreaksForDatabase(kparray[1].Split(':')[1].Replace("}]", "").Trim());
                    //string tmp_comment_id = Lib.Utility.Common.ReplaceLineBreaksForDatabase(kparray[2].Split(':')[1].Replace("}]", "").Trim());
                    string tmp_comment_id = kparray[0].Split(':')[1].Trim().Substring(8);
                    PMS.Model.DTO.Appraisal.KPI obj_kpi = new Model.DTO.Appraisal.KPI()
                    {
                        Id = Convert.ToInt32(tmp_comment_id),
                        Progress = Lib.Utility.Common.ReplaceLineBreaksForDatabase(tmp_progress)
                    };
                    lst_kpis.Add(obj_kpi);
                }
            }
            return lst_kpis;
        }
        public static List<PMS.Model.DTO.Appraisal.KPIComment> GetKPICommentItemsToSave(string[] result, int commentorId, DateTime commentTimestamp, out List<PMS.Model.DTO.Appraisal.KPIComment> lst_comments, out List<PMS.Model.DTO.Appraisal.KPIComment> lst_delete_comments)
        {
            lst_comments = null;
            lst_delete_comments = null;
            if (!Lib.Utility.Common.IsNullOrEmptyList(result))
            {
                lst_comments = new List<Model.DTO.Appraisal.KPIComment>();
                lst_delete_comments = new List<Model.DTO.Appraisal.KPIComment>();
                foreach (string str_string in result)
                {
                    string[] kparray = str_string.Replace("\"", "").Split(',');
                    //string tmp_comment = kparray[1].Split(':')[1].Replace("}]", "").Trim().TrimEnd('\r', '\n').Replace("\\n", Environment.NewLine).Trim();
                    string tmp_comment = Lib.Utility.Common.ReplaceLineBreaksForDatabase(kparray[1].Split(':')[1].Replace("}]", "").Trim());
                    //string tmp_comment_id = Lib.Utility.Common.ReplaceLineBreaksForDatabase(kparray[2].Split(':')[1].Replace("}]", "").Trim());
                    string tmp_comment_id = kparray[2].Split(':')[1].Trim();
                    if (!string.IsNullOrEmpty(tmp_comment)) 
                    {
                        PMS.Model.DTO.Appraisal.KPIComment obj_comment = new Model.DTO.Appraisal.KPIComment()
                        {
                            AppraisalKPI = new Model.DTO.Appraisal.KPI() { Id = Convert.ToInt32(kparray[0].Split(':')[1]) },
                            Comments = tmp_comment,
                            FormSaveOnly = true,
                            Commentor = new Model.DTO.Core.Employee() { Id = commentorId },
                            CommentedTimestamp = commentTimestamp
                        };
                        lst_comments.Add(obj_comment);
                    }
                    else
                    {
                        if (!tmp_comment_id.Contains("NewComment"))
                        {
                            PMS.Model.DTO.Appraisal.KPIComment obj_comment = new Model.DTO.Appraisal.KPIComment()
                            {
                                Id = Int32.Parse(tmp_comment_id)
                            };
                            lst_delete_comments.Add(obj_comment);
                        }
                    }
                }
            }

            return lst_comments;
        }

        public static void GetCoreValueCommentItemsToSave(string[] result, int commentorId, DateTime commentTimestamp,out List<PMS.Model.DTO.Appraisal.CoreValueComment> lst_comments,out List<PMS.Model.DTO.Appraisal.CoreValueComment> lst_delete_comments)
        {
            lst_comments = null;
            lst_delete_comments = null;
            if (!Lib.Utility.Common.IsNullOrEmptyList(result))
            {
                lst_comments = new List<Model.DTO.Appraisal.CoreValueComment>();
                lst_delete_comments = new List<Model.DTO.Appraisal.CoreValueComment>();
                foreach (string str_string in result)
                {
                    if (!string.IsNullOrEmpty(str_string))
                    {
                        string[] kparray = str_string.Replace("\"", "").Split(',');

                        string tmp_comment = Lib.Utility.Common.ReplaceLineBreaksForDatabase(kparray[1].Split(':')[1].Replace("}]", "").Trim());
                        string tmp_comment_id = kparray[0].Split(':')[1].Trim();
                        
                        
                        if (!string.IsNullOrEmpty(tmp_comment))
                        {
                            PMS.Model.DTO.Appraisal.CoreValueComment obj_comment = new Model.DTO.Appraisal.CoreValueComment()
                            {
                                AppraisalCoreValue = new Model.DTO.Appraisal.CoreValue() { Id = Convert.ToInt32(kparray[0].Split(':')[1]) },
                                Comments = tmp_comment,
                                FormSaveOnly = true,
                                Commentor = new Model.DTO.Core.Employee() { Id = commentorId },
                                CommentedTimestamp = commentTimestamp
                            };
                            lst_comments.Add(obj_comment);
                        }
                        else
                        {
                            if (!tmp_comment_id.Contains("NewComment"))
                            {
                                PMS.Model.DTO.Appraisal.CoreValueComment obj_comment = new Model.DTO.Appraisal.CoreValueComment()
                                {
                                    AppraisalCoreValue = new Model.DTO.Appraisal.CoreValue() { Id = Convert.ToInt32(kparray[0].Split(':')[1]) },
                                    FormSaveOnly = true,
                                    Commentor = new Model.DTO.Core.Employee() { Id = commentorId },
                                    Id= Int32.Parse(tmp_comment_id)
                                };
                                lst_delete_comments.Add(obj_comment);
                            }
                        }
                    }
                }
            }
            return;
        }

        #endregion KPI Items

        #region Core Value Items

        public static List<PMS.Model.DTO.Appraisal.CoreValue> GetCoreValueItemsToUpdate(string[] result)
        {
            List<PMS.Model.DTO.Appraisal.CoreValue> lst_core_values = null;

            if (!Lib.Utility.Common.IsNullOrEmptyList(result))
            {
                lst_core_values = new List<Model.DTO.Appraisal.CoreValue>();
                string[] arr_values = new string[5];
                string[] arr_seperator = { "^&*" };

                foreach (string str_result in result)
                {
                    if (!string.IsNullOrEmpty(str_result))
                    {
                        //foreach (string str_result in result.Where(sec => sec.Contains("NewKPI")))
                        //{
                        if (!str_result.Contains("NewKPI"))
                        {
                            arr_values = Uri.UnescapeDataString(str_result).Split(arr_seperator, StringSplitOptions.None);
                            PMS.Model.DTO.Appraisal.CoreValue obj_kpi = new Model.DTO.Appraisal.CoreValue()
                            {
                                Id = Convert.ToInt32(arr_values[0]),
                                Appraisal = new Model.DTO.Appraisal.Appraisal() { Id = Convert.ToInt32(arr_values[1]) },
                                Section = new Model.DTO.Master.Section() { Id = Convert.ToInt32(arr_values[2]) },
                                Block = new Model.DTO.Master.Block() { Id = Convert.ToInt32(arr_values[3]) },
                                //CoreValueCompetency = new Model.DTO.Master.CoreValueCompetency() { Id = Convert.ToInt32(arr_values[4]) },
                                //Target = arr_values[4].Trim().TrimEnd('\r', '\n').Replace("\\n", Environment.NewLine)
                                Target = Lib.Utility.Common.ReplaceLineBreaksForDatabase(arr_values[4].Trim())
                            };
                            lst_core_values.Add(obj_kpi);
                        }
                    }
                }
            }
            return lst_core_values;
        }

        public static List<PMS.Model.DTO.Appraisal.CoreValue> GetCoreValueItemsToInsert(string[] result)
        {
            List<PMS.Model.DTO.Appraisal.CoreValue> lst_core_values = null;

            if (!Lib.Utility.Common.IsNullOrEmptyList(result))
            {
                lst_core_values = new List<Model.DTO.Appraisal.CoreValue>();
                string[] arr_values = new string[5];
                string[] arr_seperator = { "^&*" };

                foreach (string str_result in result) 
                {
                    if (!string.IsNullOrEmpty(str_result)) 
                    {
                        if (str_result.Contains("NewKPI")) 
                        {
                            arr_values = Uri.UnescapeDataString(str_result).Split(arr_seperator, StringSplitOptions.None);
                            PMS.Model.DTO.Appraisal.CoreValue obj_kpi = new Model.DTO.Appraisal.CoreValue()
                            {
                                Appraisal = new Model.DTO.Appraisal.Appraisal() { Id = Convert.ToInt32(arr_values[1]) },
                                Section = new Model.DTO.Master.Section() { Id = Convert.ToInt32(arr_values[2]) },
                                Block = new Model.DTO.Master.Block() { Id = Convert.ToInt32(arr_values[3]) },
                                //CoreValueCompetency = new Model.DTO.Master.CoreValueCompetency() { Id = Convert.ToInt32(arr_values[4]) },
                                //Target = arr_values[4].Trim().TrimEnd('\r', '\n').Replace("\\n", Environment.NewLine)
                                Target = Lib.Utility.Common.ReplaceLineBreaksForDatabase(arr_values[4].Trim())
                            };
                            lst_core_values.Add(obj_kpi);
                        }
                    }
                }
            }
            return lst_core_values;
        }

        public static List<PMS.Model.DTO.Appraisal.CoreValue> GetCoreValueItemsToDelete(string[] result)
        {
            List<PMS.Model.DTO.Appraisal.CoreValue> lst_core_values = null;

            if (!Lib.Utility.Common.IsNullOrEmptyList(result))
            {
                lst_core_values = new List<Model.DTO.Appraisal.CoreValue>();

                foreach (string str_result in result)
                {
                    if (!string.IsNullOrEmpty(str_result))
                    {
                        PMS.Model.DTO.Appraisal.CoreValue obj_kpi = new Model.DTO.Appraisal.CoreValue() { Id = Int32.Parse(str_result) };
                        lst_core_values.Add(obj_kpi);
                    }
                }
            }
            return lst_core_values;
        }

        public static List<PMS.Model.DTO.Appraisal.CoreValue> GetCoreValuesProgressToUpdate(string[] result)
        {
            List<PMS.Model.DTO.Appraisal.CoreValue> lst_kpis = null;

            if (!Lib.Utility.Common.IsNullOrEmptyList(result))
            {
                lst_kpis = new List<Model.DTO.Appraisal.CoreValue>();
                string[] arr_values = new string[2];
                string[] arr_seperator = { "^&*" };

                foreach (string str_result in result)
                {
                    arr_values = Uri.UnescapeDataString(str_result).Split(arr_seperator, StringSplitOptions.None);
                    PMS.Model.DTO.Appraisal.CoreValue obj_kpi = new Model.DTO.Appraisal.CoreValue()
                    {
                        Id = Convert.ToInt32(arr_values[1].Substring(8)),
                        Progress = Lib.Utility.Common.ReplaceLineBreaksForDatabase(arr_values[0])
                    };
                    lst_kpis.Add(obj_kpi);
                }
            }
            return lst_kpis;
        }

        public static List<PMS.Model.DTO.Appraisal.CoreValue> GetCoreValuesProgressToUpdateForAjax(string[] result)
        {
            List<PMS.Model.DTO.Appraisal.CoreValue> lst_kpis = null;

            if (!Lib.Utility.Common.IsNullOrEmptyList(result))
            {
                lst_kpis = new List<Model.DTO.Appraisal.CoreValue>();
                string[] arr_values = new string[2];

                foreach (string str_result in result)
                {
                    string[] kparray = str_result.Replace("\"", "").Split(',');
                    //string tmp_comment = kparray[1].Split(':')[1].Replace("}]", "").Trim().TrimEnd('\r', '\n').Replace("\\n", Environment.NewLine).Trim();
                    string tmp_progress = Lib.Utility.Common.ReplaceLineBreaksForDatabase(kparray[1].Split(':')[1].Replace("}]", "").Trim());
                    //string tmp_comment_id = Lib.Utility.Common.ReplaceLineBreaksForDatabase(kparray[2].Split(':')[1].Replace("}]", "").Trim());
                    string tmp_comment_id = kparray[0].Split(':')[1].Trim().Substring(8);
                    PMS.Model.DTO.Appraisal.CoreValue obj_kpi = new Model.DTO.Appraisal.CoreValue()
                    {
                        Id = Convert.ToInt32(tmp_comment_id),
                        Progress = Lib.Utility.Common.ReplaceLineBreaksForDatabase(tmp_progress)
                    };
                    lst_kpis.Add(obj_kpi);
                }
            }
            return lst_kpis;
        }
        #endregion Core Value Items

        #region Performance Coaching Items

        public static PMS.Model.DTO.Appraisal.PerformanceCoaching GetPerformanceCoachingItemFromFormInput(Dictionary<string, string> form)
        {
            PMS.Model.DTO.Appraisal.PerformanceCoaching obj_performance_coaching = null;

            if (!Lib.Utility.Common.IsNullOrEmptyList(form))
            {
                obj_performance_coaching = new Model.DTO.Appraisal.PerformanceCoaching()
                {
                    Appraisal = new Model.DTO.Appraisal.Appraisal() { Id = Convert.ToInt32(form["AppraisalID"]) },
                    //Section = new Model.DTO.Master.Section() { Id = Convert.ToInt32(form["SectionID"]) },
                    //AreasOfImprovement = form["ImprovementsArea"].Trim().TrimEnd('\r', '\n').Replace("\\n", Environment.NewLine),
                    //AreasOfStrength = form["StrengthsArea"].Trim().TrimEnd('\r', '\n').Replace("\\n", Environment.NewLine)
                    AreasOfImprovement = Lib.Utility.Common.ReplaceLineBreaksForDatabase(Uri.UnescapeDataString(form["ImprovementsArea"].Trim())),
                    AreasOfStrength = Lib.Utility.Common.ReplaceLineBreaksForDatabase(Uri.UnescapeDataString(form["StrengthsArea"].Trim()))
                };
            }
            return obj_performance_coaching;
        }

        public static PMS.Model.DTO.Appraisal.PerformanceCoaching GetPerformanceCoachingItemProgressFromFormInput(Dictionary<string, string> form)
        {
            PMS.Model.DTO.Appraisal.PerformanceCoaching obj_performance_coaching = null;

            if (!Lib.Utility.Common.IsNullOrEmptyList(form))
            {
                obj_performance_coaching = new Model.DTO.Appraisal.PerformanceCoaching()
                {
                    Appraisal = new Model.DTO.Appraisal.Appraisal() { Id = Convert.ToInt32(form["AppraisalID"]) },

                    Progress = Lib.Utility.Common.ReplaceLineBreaksForDatabase(Uri.UnescapeDataString(form["Progress"].Trim()))
                };
            }
            return obj_performance_coaching;
        }

        #endregion Performance Coaching Items

        #region Career Development Items

        public static PMS.Model.DTO.Appraisal.CareerDevelopment GetCareerDevelopmentItemFromFormInput(Dictionary<string, string> form)
        {
            PMS.Model.DTO.Appraisal.CareerDevelopment obj_career_development = null;

            if (!Lib.Utility.Common.IsNullOrEmptyList(form))
            {
                obj_career_development = new Model.DTO.Appraisal.CareerDevelopment()
                {
                    Appraisal = new Model.DTO.Appraisal.Appraisal() { Id = Convert.ToInt32(form["AppraisalID"]) },
                    Section = new Model.DTO.Master.Section() { Id = Convert.ToInt32(form["SectionID"]) },
                    //ShortTermGoals = form["ShorttermCareerGoal"].Trim().TrimEnd('\r', '\n').Replace("\\n", Environment.NewLine),
                    //CareerPlans = form["DevelopmentPlan"].Trim().TrimEnd('\r', '\n').Replace("\\n", Environment.NewLine),
                    //LearningPlans = form["Learninganddevelopment"].Trim().TrimEnd('\r', '\n').Replace("\\n", Environment.NewLine)
                    ShortTermGoals = Lib.Utility.Common.ReplaceLineBreaksForDatabase(Uri.UnescapeDataString(form["ShorttermCareerGoal"].Trim())),
                    CareerPlans = Lib.Utility.Common.ReplaceLineBreaksForDatabase(Uri.UnescapeDataString(form["DevelopmentPlan"].Trim())),
                    LearningPlans = Lib.Utility.Common.ReplaceLineBreaksForDatabase(Uri.UnescapeDataString(form["Learninganddevelopment"].Trim()))
                };
            }
            return obj_career_development;
        }

        public static PMS.Model.DTO.Appraisal.CareerDevelopment GetCareerDevelopmentProgressFromFormInput(Dictionary<string, string> form)
        {
            PMS.Model.DTO.Appraisal.CareerDevelopment obj_career_development = null;

            if (!Lib.Utility.Common.IsNullOrEmptyList(form))
            {
                obj_career_development = new Model.DTO.Appraisal.CareerDevelopment()
                {
                    Appraisal = new Model.DTO.Appraisal.Appraisal() { Id = Convert.ToInt32(form["AppraisalID"]) },                   
                    Progress = Lib.Utility.Common.ReplaceLineBreaksForDatabase(Uri.UnescapeDataString(form["Progress"].Trim())),
                };
            }
            return obj_career_development;
        }

        #endregion Career Development Items

        #region Tasks

        public static List<PMS.Model.DTO.Core.Task.Task> GetTasksByOwner(int ownerEmployeeId, int? statusId)
        {
            List<PMS.Model.DTO.Core.Task.Task> lst_task = Model.PMSModel.GetTasksByOwner(ownerEmployeeId, statusId);
            StringBuilder sb_address = new StringBuilder();
            if (!Lib.Utility.Common.IsNullOrEmptyList(lst_task))
            {
                foreach (PMS.Model.DTO.Core.Task.Task obj_task in lst_task)
                {
                    sb_address.Append(obj_task.Address);
                    sb_address.Append("/");
                    sb_address.Append(obj_task.Id);
                    sb_address.Append("/");
                    sb_address.Append(obj_task.RecordId);
                    obj_task.Address = sb_address.ToString();
                    sb_address.Clear();
                }
            }
            return lst_task;
        }

        public static List<PMS.Model.DTO.Core.Task.Task> GetTasksByAppraisal(int appraisalId, int? statusId)
        {
            List<PMS.Model.DTO.Core.Task.Task> lst_task = Model.PMSModel.GetTasksByAppraisal(appraisalId, statusId);
            StringBuilder sb_address = new StringBuilder();
            if (!Lib.Utility.Common.IsNullOrEmptyList(lst_task))
            {
                foreach (PMS.Model.DTO.Core.Task.Task obj_task in lst_task)
                {
                    sb_address.Append(obj_task.Address);
                    sb_address.Append("/");
                    sb_address.Append(obj_task.Id);
                    sb_address.Append("/");
                    sb_address.Append(obj_task.RecordId);
                    obj_task.Address = sb_address.ToString();
                    sb_address.Clear();
                }
            }
            return lst_task;
        }

        public static Model.DTO.Core.Task.Task CreateNewTaskForAppraisal(int appraisalId, int statusId, string name, string address, Model.DTO.Core.Task.Owner owner)
        {
            Model.DTO.Core.Task.Task obj_task = new Model.DTO.Core.Task.Task()
            {
                Module = new Model.DTO.Core.Master.Module() { Id = Model.PMSConstants.MODULE_ID_PMS },
                RecordId = appraisalId,
                Status = new Model.DTO.Core.Master.Status() { Id = statusId },
                Name = name,
                Address = address
            };

            if (owner != null)
            {
                obj_task.AddOwner(owner);
            }
            return obj_task;
        }

        public static Model.DTO.Core.Task.Task CreateTasksForCycleStageChange(Model.DTO.Appraisal.Appraisal appraisal)
        {
            Model.DTO.Core.Task.Task obj_task = new Model.DTO.Core.Task.Task()
            {
                Module = new Model.DTO.Core.Master.Module() { Id = Model.PMSConstants.MODULE_ID_PMS },
                Status = new Model.DTO.Core.Master.Status() { Id = Model.PMSConstants.STATUS_CORE_ID_OPEN },
                RecordId = appraisal.Id
            };

            if (appraisal.Stage.Id == Model.PMSConstants.STAGE_ID_GOAL_SETTING)
            {
                obj_task.Name = "Submission of performance appraisal.";
                obj_task.Address = "/Stage1/KeyPerformanceIndicators";
            }
            else if (appraisal.Stage.Id == Model.PMSConstants.STAGE_ID_PROGRESS_REVIEW)
            {
                obj_task.Name = "Submission of performance appraisal for progress review.";
                obj_task.Address = "/Stage2/KeyPerformanceIndicators";
            }
            else if (appraisal.Stage.Id == Model.PMSConstants.STAGE_ID_FINAL_YEAR)
            {
                obj_task.Name = "Submission of performance appraisal for final year review.";
                obj_task.Address = "/Stage3/KeyPerformanceIndicators";
            }

            obj_task.AddOwner(Model.Mappers.CoreMapper.MapEmployeeDTOToOwnerDTO(appraisal.Employee));
            return obj_task;
        }

        #endregion Tasks

        #region Trails

        public static Model.DTO.Appraisal.Trail CreateAppraisalTrail(Model.DTO.Appraisal.Appraisal appraisal, Model.DTO.Core.Employee actioner,  Model.DTO.Master.Action action)
        {
            Model.DTO.Appraisal.Trail obj_trail = new Model.DTO.Appraisal.Trail()
            {
                Appraisal = appraisal,
                Action = action,
                Actioner = actioner,
                ActionTimestamp = DateTime.Now,
                Stage = appraisal.Stage
            };
            return obj_trail;
        }

        #endregion

    }
}
