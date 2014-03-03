using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace eHR.PMS.Job
{
    class AppraisalStageManager
    {
        static void Main(string[] args)
        {
            try
            {
                ManageAppraisalStage(DateTime.Now.Date);
            }
            catch (Exception exc)
            {
                Lib.Utility.Common.Log(ConfigurationManager.AppSettings["logfilepath"], "[ERROR][AppraisalStageManager]");
                Lib.Utility.Common.Log(ConfigurationManager.AppSettings["logfilepath"], exc.Message);
                Lib.Utility.Common.Log(ConfigurationManager.AppSettings["logfilepath"], exc.StackTrace);

                if (exc.InnerException != null)
                {
                    Lib.Utility.Common.Log(ConfigurationManager.AppSettings["logfilepath"], exc.InnerException.Message);
                    Lib.Utility.Common.Log(ConfigurationManager.AppSettings["logfilepath"], exc.InnerException.StackTrace);
                }
            }
        }


        /* 
         * On cycle creation
         *  create cycle :
         *      current stage = pre-cycle, status = open
         *      cycle stages = pre-c, gs, pr, fr, post-c
         *  create appraisals
         *      current stage = pre-cycle, status = open
         *      appraisal stage = pre-c, gs, pr, fr
         *      appraisal stage date = same as cycle
         *      appraisal is locked
         *      trails = action appraisal created
         * */

        /*
         * Manual additions of appraisals to existing cycle
         *  cycle stage is in pre-c
         *      appraisal current stage = pre-c, status = open
         *      appraisal stage = pre-c, gs, pr, fr
         *      appraisal is locked
         *      trails = action appraisal created
         *  cycle stage is in gs and 1 week before gs end date
         *      appraisal stage current stage = gs, status = open
         *      appraisal stage = gs, pr, fr
         *      appraisal is unlocked
         *      appraisal stage date = same as cycle
         *      trails = action appraisal created
         *      create task
         *  cycle stage is gs and less than 1 week before gs end date and before pr start date
         *      appraisal stage current stage = gs, status = open
         *      appraisal stage = gs, fr
         *      appraisal stage date = same as cycle
         *      appraisal is locked
         *      trails = action appraisal created 
         *  cycle stage is in pr and 1 week before pr end date
         *      appraisal stage current stage = gs, status = open
         *      appraisal stage = gs, fr
         *      appraisal is unlocked
         *      appraisal stage date = same as cycle
         *      trails = action appraisal created 
         *      create task
         *  cycle stage is pr and less than 1 week before gs end date and before fr start date
         *      appraisal stage current stage = gs, status = open
         *      appraisal stage = gs, fr
         *      appraisal stage date = same as cycle
         *      appraisal is locked
         *      trails = action appraisal created        
         * */

        /*
         * Daily checks:
         *  if cycle is in pre-c and to start gs today
         *      update cycle stage to gs
         *      update all appraisals to stage next stage
         *      unlock all appraisals
         *      trails = action appraisal open
         *      send email to all appraisal employee : mass email for gs stage opening
         *  if cycle is in gs and to start pr today
         *      update cycle stage to pr
         *      for unlocked appraisals
         *          approved appraisals
         *              update stage next stage
         *              trails = action appraisal open
         *              send email to appraisal employee : mass email for stage opening
         *      for locked appraisals
         *          unclock appraisal
         *          update appraisal to stage to gs
         *          trails = action appraisal open
         *  if cycle is in pr and to start fr today
         *      
         * */


        /*
         * On cycle creation
         *  create cycle : stage = pre-cycle, status = open
         *  create cylce stages : 4 stages, less post-cycle (pre-cycle start = day of creation, pre-cycle end = one day before goal setting start)
         *  create appraisals : stage = pre-cycle, status = new
         *  no need to create trails
         * 
         * */

        /*
         * Get all open cycles and their stages
         *  if cycle is in pre-cycle: 
         *      check if today needs to start goal setting stage
         *          if needs to start:  update cycle stage to goal setting stage
         *      check if today needs to start progress review stage
         *          if needs to start:  update cycle stage to progress review stage
         *      check if today needs to start the final year review date
         *          if needs to start:  update cycle stage to final year review date
         *  
         *  for each appraisal in cycle
         *      check appraisal stage and status
         *          if stage = pre-cycle
         *              if today needs to start start goal setting stage
         *                  update appraisal stage to goal setting stage
         *                  create tasks for each appraisal
         *                  create trials for each appraisal
         *                  send email notification (reciepients = all appraisal owner)
         *          if stage = goal setting and status = approved
         *              if today needs to start progress review stage
         *                  update appraisal stage to progress review stage
         *                  update appraisal status to new
         *                  create tasks for each appraisal
         *                  create trials for each appraisal
         *                  send email notification (reciepients = all appraisal owner)
         *          if stage = progress review and status = approved
         *              if today needs to start progress review stage
         *                  update appraisal stage to final review stage
         *                  update appraisal status to new                 
         *                  create tasks for each appraisal
         *                  create trials for each appraisal
         *                  send email notification (reciepients = all appraisal owner)             *                  
         * */

        private static void ManageAppraisalStage(DateTime runDate)
        {
            List<Model.DTO.Cycle.Cycle> lst_cycles = Model.PMSModel.GetCycleByStatus(Model.PMSConstants.STATUS_ID_OPEN);

            if (!Lib.Utility.Common.IsNullOrEmptyList(lst_cycles))
            {
                foreach (Model.DTO.Cycle.Cycle obj_cycle in lst_cycles)
                {
                    if (obj_cycle.Stage.Id == Model.PMSConstants.STAGE_ID_PRE_CYCLE)
                    {
                        ManageCycleInPreCycleStage(obj_cycle, runDate);
                    }

                    if (obj_cycle.Stage.Id == Model.PMSConstants.STAGE_ID_GOAL_SETTING)
                    {
                        ManageCycleInGoalSettingStage(obj_cycle, runDate);
                    }
                }
            }
        }

        private static void ManageCycleInPreCycleStage(Model.DTO.Cycle.Cycle cycle, DateTime runDate)
        {
            Model.DTO.Cycle.Stage obj_cycle_stage = cycle.CycleStages.Where(rec => rec.StageId == Model.PMSConstants.STAGE_ID_GOAL_SETTING).SingleOrDefault();

            if (obj_cycle_stage != null && Convert.ToDateTime(obj_cycle_stage.StartDate).Date == runDate)
            {
                Business.AppraisalManager.PreCycleStageManagement(cycle, runDate.Date, new Model.DTO.Core.Employee());
            }
        }

        private static void ManageCycleInGoalSettingStage(Model.DTO.Cycle.Cycle cycle, DateTime runDate) 
        {
            List<System.Net.Mail.MailMessage> lst_email_messages = new List<System.Net.Mail.MailMessage>();
            List<Model.DTO.Core.Task.Task> lst_all_tasks = new List<Model.DTO.Core.Task.Task>();
            List<Model.DTO.Core.Task.Task> lst_tasks_to_delete = new List<Model.DTO.Core.Task.Task>();
            List<Model.DTO.Appraisal.Appraisal> lst_appraisals_to_update = new List<Model.DTO.Appraisal.Appraisal>();
            List<Model.DTO.Cycle.Cycle> lst_cycles = new List<Model.DTO.Cycle.Cycle>();

            Model.DTO.Cycle.Stage obj_current_cycle_stage = cycle.CycleStages.Where(rec => rec.StageId == Model.PMSConstants.STAGE_ID_GOAL_SETTING).SingleOrDefault();
            Model.DTO.Cycle.Stage obj_next_cycle_stage = cycle.CycleStages.Where(rec => rec.StageId == Model.PMSConstants.STAGE_ID_PROGRESS_REVIEW).SingleOrDefault();

            // stage closing
            if (obj_current_cycle_stage != null && Convert.ToDateTime(obj_current_cycle_stage.EndDate).Date == runDate)
            {
                if (!Lib.Utility.Common.IsNullOrEmptyList(cycle.Appriasals))
                {
                    IEnumerable<Model.DTO.Appraisal.Appraisal> lst_appraisals = cycle.Appriasals.Where(rec => rec.Stage.Id == obj_current_cycle_stage.StageId && rec.Status.Id != Model.PMSConstants.STATUS_ID_APPROVED);

                    if (!Lib.Utility.Common.IsNullOrEmptyList(lst_appraisals)) 
                    {
                        foreach (Model.DTO.Appraisal.Appraisal obj_appraisal in lst_appraisals)
                        {
                            // force close for those that are not approved.
                            obj_appraisal.Locked = false;
                            obj_appraisal.AddTrail(Business.AppraisalManager.CreateAppraisalTrail(obj_appraisal, new Model.DTO.Core.Employee(), new Model.DTO.Master.Action() { Id = Model.PMSConstants.ACTION_ID_APPRAISAL_HR_ADMINISTERED }));
                            obj_appraisal.Status = new Model.DTO.Master.Status() { Id = Model.PMSConstants.STATUS_ID_HR_ADMINISTERED };
                            obj_appraisal.Stage = cycle.Stage;

                            // remove open task belong to appraisal
                            lst_tasks_to_delete = Business.AppraisalManager.GetTasksByAppraisal(obj_appraisal.Id, Model.PMSConstants.STATUS_CORE_ID_OPEN);
                            lst_appraisals_to_update.Add(obj_appraisal);
                        }
                    }
                }
            }

            // stage opening
            if (obj_next_cycle_stage != null && Convert.ToDateTime(obj_next_cycle_stage.StartDate).Date == runDate)
            {
                cycle.Stage = Business.AppraisalManager.GetCycleNextStage(cycle, runDate);
                lst_cycles.Add(cycle);
                if (!Lib.Utility.Common.IsNullOrEmptyList(cycle.Appriasals))
                {
                    IEnumerable<Model.DTO.Appraisal.Appraisal> lst_appraisals_to_convert_stage = cycle.Appriasals.Where(rec => rec.Stage.Id == Model.PMSConstants.STAGE_ID_GOAL_SETTING && rec.Status.Id != Model.PMSConstants.STATUS_ID_APPROVED);
                    IEnumerable<Model.DTO.Appraisal.Appraisal> lst_new_appraisals = cycle.Appriasals.Where(rec => rec.Stage.Id == Model.PMSConstants.STAGE_ID_PRE_CYCLE);

                    if (!Lib.Utility.Common.IsNullOrEmptyList(lst_appraisals_to_convert_stage))
                    {
                        foreach (Model.DTO.Appraisal.Appraisal obj_appraisal in lst_appraisals_to_convert_stage)
                        {
                            obj_appraisal.Locked = false;
                            obj_appraisal.AddTrail(Business.AppraisalManager.CreateAppraisalTrail(obj_appraisal, new Model.DTO.Core.Employee(), new Model.DTO.Master.Action() { Id = Model.PMSConstants.ACTION_ID_APPRAISAL_OPENED }));
                            obj_appraisal.Status = new Model.DTO.Master.Status() { Id = Model.PMSConstants.STATUS_ID_NEW };
                            obj_appraisal.Stage = Model.Mappers.PMSMapper.MapAppraisalStageDTOToStageDTO(obj_appraisal.AppraisalStages.Where(rec => rec.StageId == Model.PMSConstants.STAGE_ID_PROGRESS_REVIEW).SingleOrDefault());

                            if (!string.IsNullOrEmpty(obj_appraisal.Employee.OfficeEmailAddress))
                            {
                                lst_email_messages.Add(Business.AppraisalManager.GenerateEmailMessageForCycleStageStart(obj_appraisal));
                            }
                            lst_appraisals_to_update.Add(obj_appraisal);
                            lst_all_tasks.Add(Business.AppraisalManager.CreateTasksForCycleStageChange(obj_appraisal));
                        }
                    }

                    if (!Lib.Utility.Common.IsNullOrEmptyList(lst_new_appraisals))
                    {
                        foreach (Model.DTO.Appraisal.Appraisal obj_appraisal in lst_new_appraisals)
                        {
                            // these are appraisals that are created after the goal setting stage of cycle
                            obj_appraisal.Locked = false;
                            obj_appraisal.AddTrail(Business.AppraisalManager.CreateAppraisalTrail(obj_appraisal, new Model.DTO.Core.Employee(), new Model.DTO.Master.Action() { Id = Model.PMSConstants.ACTION_ID_APPRAISAL_OPENED }));
                            obj_appraisal.Status = new Model.DTO.Master.Status() { Id = Model.PMSConstants.STATUS_ID_NEW };
                            obj_appraisal.Stage = new Model.DTO.Master.Stage() { Id = Model.PMSConstants.STAGE_ID_GOAL_SETTING };

                            if (!string.IsNullOrEmpty(obj_appraisal.Employee.OfficeEmailAddress))
                            {
                                lst_email_messages.Add(Business.AppraisalManager.GenerateEmailMessageForCycleStageStart(obj_appraisal));
                            }
                            lst_appraisals_to_update.Add(obj_appraisal);
                            lst_all_tasks.Add(Business.AppraisalManager.CreateTasksForCycleStageChange(obj_appraisal));
                        }
                    }
                }
            }

            if (Model.PMSModel.AppraisalStageManager(lst_cycles, lst_appraisals_to_update, lst_all_tasks, lst_tasks_to_delete))
            {
                Business.AppraisalManager.SendEmailNotification(lst_email_messages);
            }
        }
    }
}
