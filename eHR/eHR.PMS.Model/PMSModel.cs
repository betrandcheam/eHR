using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eHR.PMS.Model
{
    public class PMSModel
    {
        #region Employee

        public static List<PMS.Model.DTO.Core.Task.Task> GetTasksByOwner(int ownerEmployeeId, int? statusId)
        {
            PMS.Model.Context.PMSEntities dc_pms = new PMS.Model.Context.PMSEntities();
            List<PMS.Model.DTO.Core.Task.Task> lst_tasks = null;
            System.Data.Objects.ObjectQuery<PMS.Model.Context.TASK> entities;

            dc_pms.ContextOptions.LazyLoadingEnabled = false;

            if (statusId != null)
            {
                entities = ((from ent_tasks in dc_pms.TASKs
                             join ent_owners in dc_pms.TASK_OWNER on ent_tasks.ID equals ent_owners.TASK_ID
                             where
                                 ent_tasks.MODULE_ID == PMSConstants.MODULE_ID_PMS &&
                                 ent_tasks.STATUS_ID == statusId &&
                                 ent_owners.EMPLOYEE_ID == ownerEmployeeId
                             select ent_tasks) as System.Data.Objects.ObjectQuery<PMS.Model.Context.TASK>)
                                .Include("MST_MODULE")
                                .Include("MST_STATUS")
                                .Include("TASK_OWNER")
                                .Include("TASK_OWNER.EMPLOYEE");
            }
            else
            {
                entities = ((from ent_tasks in dc_pms.TASKs
                             join ent_owners in dc_pms.TASK_OWNER on ent_tasks.ID equals ent_owners.TASK_ID
                             where
                                 ent_tasks.MODULE_ID == PMSConstants.MODULE_ID_PMS &&
                                 ent_owners.EMPLOYEE_ID == ownerEmployeeId
                             select ent_tasks) as System.Data.Objects.ObjectQuery<PMS.Model.Context.TASK>)
                                .Include("MST_MODULE")
                                .Include("MST_STATUS")
                                .Include("TASK_OWNER")
                                .Include("TASK_OWNER.EMPLOYEE");
            }

            if (!Lib.Utility.Common.IsNullOrEmptyList(entities))
            {
                lst_tasks = PMS.Model.Mappers.CoreMapper.MapTaskEntitiesToDTOs(entities.ToList(), true);
            }

            dc_pms.Dispose();
            return lst_tasks;
        }

        public static List<PMS.Model.DTO.Core.Task.Task> GetTasksByAppraisal(int appraisalId, int? statusId)
        {
            PMS.Model.Context.PMSEntities dc_pms = new PMS.Model.Context.PMSEntities();
            List<PMS.Model.DTO.Core.Task.Task> lst_tasks = null;
            System.Data.Objects.ObjectQuery<PMS.Model.Context.TASK> entities;

            dc_pms.ContextOptions.LazyLoadingEnabled = false;

            if (statusId != null)
            {
                entities = ((from ent_tasks in dc_pms.TASKs
                             where
                                 ent_tasks.MODULE_ID == PMSConstants.MODULE_ID_PMS &&
                                 ent_tasks.STATUS_ID == statusId &&
                                 ent_tasks.RECORD_ID == appraisalId
                             select ent_tasks) as System.Data.Objects.ObjectQuery<PMS.Model.Context.TASK>)
                                .Include("MST_MODULE")
                                .Include("MST_STATUS")
                                .Include("TASK_OWNER")
                                .Include("TASK_OWNER.EMPLOYEE");
            }
            else
            {
                entities = ((from ent_tasks in dc_pms.TASKs
                             where
                                 ent_tasks.MODULE_ID == PMSConstants.MODULE_ID_PMS &&
                                 ent_tasks.RECORD_ID == appraisalId
                             select ent_tasks) as System.Data.Objects.ObjectQuery<PMS.Model.Context.TASK>)
                                .Include("MST_MODULE")
                                .Include("MST_STATUS")
                                .Include("TASK_OWNER")
                                .Include("TASK_OWNER.EMPLOYEE");
            }

            if (!Lib.Utility.Common.IsNullOrEmptyList(entities))
            {
                lst_tasks = PMS.Model.Mappers.CoreMapper.MapTaskEntitiesToDTOs(entities.ToList(), true);
            }

            dc_pms.Dispose();
            return lst_tasks;
        }

        public static List<PMS.Model.DTO.GradeCompetency> GetCoreValueCompetencyByGrade(int? gradeId)
        {
            PMS.Model.Context.PMSEntities dc_pms = new PMS.Model.Context.PMSEntities();
            List<PMS.Model.DTO.GradeCompetency> lst_competencies = null;
            System.Data.Objects.ObjectQuery<PMS.Model.Context.PMS_GRADE_CORE_VALUE_COMPETENCY> entities;

            dc_pms.ContextOptions.LazyLoadingEnabled = false;

            if (gradeId == null)
            {
                entities = ((from ent_grade_competency in dc_pms.PMS_GRADE_CORE_VALUE_COMPETENCY
                             select ent_grade_competency) as System.Data.Objects.ObjectQuery<PMS.Model.Context.PMS_GRADE_CORE_VALUE_COMPETENCY>)
                                .Include("MST_ACR_GRADE")
                                .Include("PMS_MST_BLOCK")
                                .Include("PMS_MST_CORE_VALUE_COMPETENCY");
            }
            else
            {
                entities = ((from ent_grade_competency in dc_pms.PMS_GRADE_CORE_VALUE_COMPETENCY
                             where ent_grade_competency.GRADE_MASTER_ID == gradeId
                             select ent_grade_competency) as System.Data.Objects.ObjectQuery<PMS.Model.Context.PMS_GRADE_CORE_VALUE_COMPETENCY>)
                                .Include("MST_ACR_GRADE")
                                .Include("PMS_MST_BLOCK")
                                .Include("PMS_MST_CORE_VALUE_COMPETENCY");
            }

            if (!Lib.Utility.Common.IsNullOrEmptyList(entities))
            {
                lst_competencies = Mappers.PMSMapper.MapGradeCompetencyEntitiesToDTOs(entities.ToList());
            }

            dc_pms.Dispose();
            return lst_competencies;
        }

        public static List<PMS.Model.DTO.Core.Employee> GetEmployeesForCycle(bool? active)
        {

            PMS.Model.Context.PMSEntities dc_pms = new PMS.Model.Context.PMSEntities();
            List<PMS.Model.DTO.Core.Employee> lst_employees = null;
            System.Data.Objects.ObjectQuery<PMS.Model.Context.EMPLOYEE> entities;

            if (active == null)
            {
                entities = ((from ent_employees in dc_pms.EMPLOYEEs
                             select ent_employees) as System.Data.Objects.ObjectQuery<PMS.Model.Context.EMPLOYEE>)
                                .Include("PMS_LEVEL_1_APPROVER")
                                .Include("PMS_LEVEL_2_APPROVER")
                                .Include("DEPARMENT")
                                .Include("DEPARTMENT.MST_DEPARTMENT");
            }
            else
            {
                entities = ((from ent_employees in dc_pms.EMPLOYEEs
                             where
                                ent_employees.ACTIVE == Convert.ToBoolean(active)
                             select ent_employees) as System.Data.Objects.ObjectQuery<PMS.Model.Context.EMPLOYEE>)
                                .Include("PMS_LEVEL_1_APPROVER")
                                .Include("PMS_LEVEL_2_APPROVER")
                                .Include("DEPARTMENT")
                                .Include("DEPARTMENT.MST_DEPARTMENT");
            }

            if (!Lib.Utility.Common.IsNullOrEmptyList(entities))
            {
                lst_employees = Mappers.CoreMapper.MapEmployeeEntitiesToDTOs(entities.ToList(), true);
            }

            dc_pms.Dispose();
            return lst_employees;
        }

        public static List<PMS.Model.DTO.Core.Employee> GetEligibleCycleParticipants(DateTime dateRangeStart, DateTime dateRangeEnd)
        {
            PMS.Model.Context.PMSEntities dc_pms = new PMS.Model.Context.PMSEntities();
            List<PMS.Model.DTO.Core.Employee> lst_participants = null;
            System.Data.Objects.ObjectQuery<PMS.Model.Context.EMPLOYEE> entities;

            dc_pms.ContextOptions.LazyLoadingEnabled = false;

            entities = ((from ent_employees in dc_pms.EMPLOYEEs
                         where
                           ent_employees.ACTIVE == true &&
                           ent_employees.EMPLOYMENT_TYPE_ID == PMSConstants.EMPLOYMENT_TYPE_ID_PERMANENT &&
                           ent_employees.DATE_OF_HIRE < dateRangeEnd &&
                           (ent_employees.DATE_OF_DEPARTURE > dateRangeStart || !ent_employees.DATE_OF_DEPARTURE.HasValue) &&
                           ent_employees.DEPARTMENT_ID != null &&
                           ent_employees.PERFORMANCE_APPRAISAL_LEVEL1_APPROVER_ID != null &&
                           ent_employees.PERFORMANCE_APPRAISAL_LEVEL2_APPROVER_ID != null
                         select ent_employees) as System.Data.Objects.ObjectQuery<PMS.Model.Context.EMPLOYEE>)
                                .Include("PMS_LEVEL_1_APPROVER")
                                .Include("PMS_LEVEL_2_APPROVER")
                                .Include("MST_ACR_GRADE")
                                .Include("DEPARTMENT")
                                .Include("DEPARTMENT.MST_DEPARTMENT");

            if (!Lib.Utility.Common.IsNullOrEmptyList(entities))
            {
                lst_participants = Mappers.CoreMapper.MapEmployeeEntitiesToDTOs(entities.ToList(), true);
            }

            dc_pms.Dispose();
            return lst_participants;
        }

        public static List<PMS.Model.DTO.Core.Employee> GetIneligibleCycleParticipants(DateTime dateRangeStart, DateTime dateRangeEnd)
        {
            PMS.Model.Context.PMSEntities dc_pms = new PMS.Model.Context.PMSEntities();
            List<PMS.Model.DTO.Core.Employee> lst_participants = null;
            System.Data.Objects.ObjectQuery<PMS.Model.Context.EMPLOYEE> entities;

            dc_pms.ContextOptions.LazyLoadingEnabled = false;

            entities = ((from ent_employees in dc_pms.EMPLOYEEs
                         where
                           ent_employees.ACTIVE == false &&
                           ent_employees.EMPLOYMENT_TYPE_ID != PMSConstants.EMPLOYMENT_TYPE_ID_PERMANENT &&
                           ent_employees.DATE_OF_HIRE >= dateRangeEnd &&
                           (ent_employees.DATE_OF_DEPARTURE <= dateRangeStart)
                         select ent_employees) as System.Data.Objects.ObjectQuery<PMS.Model.Context.EMPLOYEE>)
                                .Include("PMS_LEVEL_1_APPROVER")
                                .Include("PMS_LEVEL_2_APPROVER");

            if (!Lib.Utility.Common.IsNullOrEmptyList(entities))
            {
                lst_participants = Mappers.CoreMapper.MapEmployeeEntitiesToDTOs(entities.ToList(), true);
            }

            dc_pms.Dispose();
            return lst_participants;
        }

        public static List<PMS.Model.DTO.Core.Employee> GetEmployees(bool? active)
        {
            PMS.Model.Context.PMSEntities dc_pms = new PMS.Model.Context.PMSEntities();
            List<PMS.Model.DTO.Core.Employee> lst_employees = null;
            System.Data.Objects.ObjectQuery<PMS.Model.Context.EMPLOYEE> entities;

            dc_pms.ContextOptions.LazyLoadingEnabled = false;

            if (active == null)
            {
                entities = ((from ent_employees in dc_pms.EMPLOYEEs
                             select ent_employees) as System.Data.Objects.ObjectQuery<PMS.Model.Context.EMPLOYEE>)
                                .Include("PMS_LEVEL_1_APPROVER")
                                .Include("PMS_LEVEL_2_APPROVER")
                                .Include("MST_EMPLOYMENT_TYPE")
                                .Include("DEPARTMENT")
                                .Include("DEPARTMENT.MST_DEPARTMENT")
                                .Include("MST_ACR_GRADE");
            }
            else
            {
                entities = ((from ent_employees in dc_pms.EMPLOYEEs
                             where ent_employees.ACTIVE == active
                             select ent_employees) as System.Data.Objects.ObjectQuery<PMS.Model.Context.EMPLOYEE>)
                                .Include("PMS_LEVEL_1_APPROVER")
                                .Include("PMS_LEVEL_2_APPROVER")
                                .Include("MST_EMPLOYMENT_TYPE")
                                .Include("DEPARTMENT")
                                .Include("DEPARTMENT.MST_DEPARTMENT")
                                .Include("MST_ACR_GRADE");
            }

            if (!Lib.Utility.Common.IsNullOrEmptyList(entities))
            {
                lst_employees = Mappers.CoreMapper.MapEmployeeEntitiesToDTOs(entities.ToList(), true);
            }

            dc_pms.Dispose();
            return lst_employees;
        }

        public static List<PMS.Model.DTO.Core.Employee> GetEmployeesByName(bool? active, string name)
        {
            PMS.Model.Context.PMSEntities dc_pms = new PMS.Model.Context.PMSEntities();
            List<PMS.Model.DTO.Core.Employee> lst_employees = null;
            System.Data.Objects.ObjectQuery<PMS.Model.Context.EMPLOYEE> entities;

            dc_pms.ContextOptions.LazyLoadingEnabled = false;

            if (active == null)
            {
                entities = ((from ent_employees in dc_pms.EMPLOYEEs
                             where
                              ((ent_employees.FIRST_NAME + " " + ent_employees.LAST_NAME).Contains(name)) ||
                              ent_employees.DOMAIN_ID.Contains(name)
                             select ent_employees) as System.Data.Objects.ObjectQuery<PMS.Model.Context.EMPLOYEE>);
            }
            else
            {
                entities = ((from ent_employees in dc_pms.EMPLOYEEs
                             where
                              ent_employees.ACTIVE == active &&
                              ((ent_employees.FIRST_NAME + " " + ent_employees.LAST_NAME).Contains(name)) ||
                              ent_employees.DOMAIN_ID.Contains(name)
                             select ent_employees) as System.Data.Objects.ObjectQuery<PMS.Model.Context.EMPLOYEE>);
            }

            if (!Lib.Utility.Common.IsNullOrEmptyList(entities))
            {
                lst_employees = Mappers.CoreMapper.MapEmployeeEntitiesToDTOs(entities.Take(5).ToList(), true);
            }

            dc_pms.Dispose();
            return lst_employees;
        }

        #endregion Employee

        #region Cycle

        public static int GetLatestActiveCycleId()
        {
            PMS.Model.Context.PMSEntities dc_pms = new PMS.Model.Context.PMSEntities();
            int int_id = 0;

            try
            {
                dc_pms.ContextOptions.LazyLoadingEnabled = false;

                var cycle = (from ent_cycle in dc_pms.PMS_CYCLE.OrderByDescending(a => a.STARTED_TIMESTAMP)
                             where ent_cycle.STATUS_ID == PMSConstants.STATUS_ID_OPEN
                             select ent_cycle).FirstOrDefault();

                if (cycle != null)
                {
                    int_id = cycle.ID;
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
            finally
            {
                dc_pms.Dispose();
            }
            return int_id;
        }

        public static bool InsertNewCycleAndCreateAppraisalTasks(PMS.Model.DTO.Cycle.Cycle cycle, out string message)
        {
            bool boo_success = false;
            message = string.Empty;
            PMS.Model.Context.PMSEntities dc_pms = new PMS.Model.Context.PMSEntities();

            try
            {
                PMS.Model.Context.PMS_CYCLE entity_cycle = Mappers.PMSMapper.MapCycleDTOToEntity(cycle, false);
                dc_pms.PMS_CYCLE.AddObject(entity_cycle);

                foreach (Model.DTO.Cycle.Stage dto_cycle_stage in cycle.CycleStages)
                {
                    Model.Context.PMS_CYCLE_STAGE entity_cycle_stage = Mappers.PMSMapper.MapCycleStageDTOToEntity(dto_cycle_stage);
                    entity_cycle_stage.CYCLE_ID = entity_cycle.ID;
                    dc_pms.PMS_CYCLE_STAGE.AddObject(entity_cycle_stage);
                }
                dc_pms.SaveChanges();
                message = Convert.ToString(entity_cycle.ID);

                foreach (Model.DTO.Appraisal.Appraisal dto_appraisal in cycle.Appriasals)
                {
                    Model.Context.PMS_APPRAISAL entity_appraisal = Mappers.PMSMapper.MapAppraisalDTOToEntity(dto_appraisal);
                    entity_appraisal.CYCLE_ID = entity_cycle.ID;
                    dc_pms.PMS_APPRAISAL.AddObject(entity_appraisal);
                    dc_pms.SaveChanges();

                    foreach (Model.DTO.Appraisal.Section dto_appraisal_section in dto_appraisal.AppraisalSections)
                    {
                        Model.Context.PMS_APPRAISAL_SECTION entity_appraisal_section = Model.Mappers.PMSMapper.MapAppraisalSectionDTOToEntity(dto_appraisal_section);
                        entity_appraisal_section.APPRAISAL_ID = entity_appraisal.ID;
                        dc_pms.PMS_APPRAISAL_SECTION.AddObject(entity_appraisal_section);
                    }

                    foreach (Model.DTO.Appraisal.Stage dto_appraisal_stage in dto_appraisal.AppraisalStages)
                    {
                        Model.Context.PMS_APPRAISAL_STAGE entity_appraisal_stage = Model.Mappers.PMSMapper.MapAppraisalStageDTOToEntity(dto_appraisal_stage);
                        entity_appraisal_stage.APPRAISAL_ID = entity_appraisal.ID;
                        dc_pms.PMS_APPRAISAL_STAGE.AddObject(entity_appraisal_stage);
                    }

                    foreach (Model.DTO.Appraisal.Trail dto_appraisal_trail in dto_appraisal.Trails)
                    {
                        Model.Context.PMS_APPRAISAL_TRAIL entity_appraisal_trail = Model.Mappers.PMSMapper.MapAppraisalTrailDTOToEntity(dto_appraisal_trail);
                        entity_appraisal_trail.APPRAISAL_ID = entity_appraisal.ID;
                        dc_pms.PMS_APPRAISAL_TRAIL.AddObject(entity_appraisal_trail);
                    }

                    foreach (Model.DTO.Appraisal.Approver dto_appraisal_approver in dto_appraisal.Approvers)
                    {
                        Model.Context.PMS_APPRAISAL_APPROVER entity_appraisal_approver = Model.Mappers.PMSMapper.MapApproverDTOToEntity(dto_appraisal_approver);
                        entity_appraisal_approver.APPRAISAL_ID = entity_appraisal.ID;
                        dc_pms.PMS_APPRAISAL_APPROVER.AddObject(entity_appraisal_approver);
                    }

                    /*
                    if (dto_appraisal.Task != null)
                    {
                        Model.Context.TASK entity_task = Mappers.CoreMapper.MapTaskDTOToEntity(dto_appraisal.Task);
                        entity_task.RECORD_ID = entity_appraisal.ID;
                        dc_pms.TASKs.AddObject(entity_task);

                        foreach (Model.DTO.Core.Task.Owner obj_owner in dto_appraisal.Task.Owners)
                        {
                            Model.Context.TASK_OWNER entity_owner = Mappers.CoreMapper.MapTaskOwnerDTOToEntity(obj_owner);
                            entity_owner.TASK_ID = entity_task.ID;
                            dc_pms.TASK_OWNER.AddObject(entity_owner);
                        }
                    }
                     */
                }
                dc_pms.SaveChanges();
                boo_success = true;
            }
            catch (Exception exc)
            {
                message = exc.Message;
            }
            finally
            {
                dc_pms.Dispose();
            }
            return boo_success;
        }

        public static bool UpdateCycleAndCreateAppraisalTasks(PMS.Model.DTO.Cycle.Cycle cycle, int cycleId, out string message,out List<DTO.Appraisal.Appraisal> NewAddAppraisals)
        {
            bool boo_success = false;
            message = string.Empty;
            PMS.Model.Context.PMSEntities dc_pms = new PMS.Model.Context.PMSEntities();
            NewAddAppraisals = new List<DTO.Appraisal.Appraisal>();
            try
            {
                List<PMS.Model.Context.PMS_CYCLE_STAGE> stages = dc_pms.PMS_CYCLE_STAGE.Where(sec => sec.CYCLE_ID == cycleId).OrderBy(sec=>sec.STAGE_ID).ToList();
                int index = 0;
                foreach (PMS.Model.Context.PMS_CYCLE_STAGE stage in stages)
                {
                    stage.START_DATE = cycle.CycleStages[index].StartDate;
                    stage.END_DATE = cycle.CycleStages[index].EndDate;
                    index++;
                }
                dc_pms.SaveChanges();

                message = Convert.ToString(cycleId);
                
                foreach (Model.DTO.Appraisal.Appraisal dto_appraisal in cycle.Appriasals)
                {
                    Model.Context.PMS_APPRAISAL entity_appraisal = Mappers.PMSMapper.MapAppraisalDTOToEntity(dto_appraisal);
                    entity_appraisal.CYCLE_ID = cycleId;
                    dc_pms.PMS_APPRAISAL.AddObject(entity_appraisal);
                    dc_pms.SaveChanges();
                    
                    foreach (Model.DTO.Appraisal.Section dto_appraisal_section in dto_appraisal.AppraisalSections)
                    {
                        Model.Context.PMS_APPRAISAL_SECTION entity_appraisal_section = Model.Mappers.PMSMapper.MapAppraisalSectionDTOToEntity(dto_appraisal_section);
                        entity_appraisal_section.APPRAISAL_ID = entity_appraisal.ID;
                        dc_pms.PMS_APPRAISAL_SECTION.AddObject(entity_appraisal_section);
                    }

                    foreach (Model.DTO.Appraisal.Stage dto_appraisal_stage in dto_appraisal.AppraisalStages)
                    {
                        Model.Context.PMS_APPRAISAL_STAGE entity_appraisal_stage = Model.Mappers.PMSMapper.MapAppraisalStageDTOToEntity(dto_appraisal_stage);
                        entity_appraisal_stage.APPRAISAL_ID = entity_appraisal.ID;
                        dc_pms.PMS_APPRAISAL_STAGE.AddObject(entity_appraisal_stage);
                    }

                    foreach (Model.DTO.Appraisal.Trail dto_appraisal_trail in dto_appraisal.Trails)
                    {
                        Model.Context.PMS_APPRAISAL_TRAIL entity_appraisal_trail = Model.Mappers.PMSMapper.MapAppraisalTrailDTOToEntity(dto_appraisal_trail);
                        entity_appraisal_trail.APPRAISAL_ID = entity_appraisal.ID;
                        dc_pms.PMS_APPRAISAL_TRAIL.AddObject(entity_appraisal_trail);
                    }

                    foreach (Model.DTO.Appraisal.Approver dto_appraisal_approver in dto_appraisal.Approvers)
                    {
                        Model.Context.PMS_APPRAISAL_APPROVER entity_appraisal_approver = Model.Mappers.PMSMapper.MapApproverDTOToEntity(dto_appraisal_approver);
                        entity_appraisal_approver.APPRAISAL_ID = entity_appraisal.ID;
                        dc_pms.PMS_APPRAISAL_APPROVER.AddObject(entity_appraisal_approver);
                    }
                    NewAddAppraisals.Add(Mappers.PMSMapper.MapAppraisalEntityToDTO(entity_appraisal, true));
                    /*
                    if (dto_appraisal.Task != null)
                    {
                        Model.Context.TASK entity_task = Mappers.CoreMapper.MapTaskDTOToEntity(dto_appraisal.Task);
                        entity_task.RECORD_ID = entity_appraisal.ID;
                        dc_pms.TASKs.AddObject(entity_task);

                        foreach (Model.DTO.Core.Task.Owner obj_owner in dto_appraisal.Task.Owners)
                        {
                            Model.Context.TASK_OWNER entity_owner = Mappers.CoreMapper.MapTaskOwnerDTOToEntity(obj_owner);
                            entity_owner.TASK_ID = entity_task.ID;
                            dc_pms.TASK_OWNER.AddObject(entity_owner);
                        }
                    }
                     */
                }
                dc_pms.SaveChanges();
                boo_success = true;
            }
            catch (Exception exc)
            {
                message = exc.Message;
            }
            finally
            {
                dc_pms.Dispose();
            }
            return boo_success;
        }

        public static bool DeleteApprisalInCyle(List<int> cids, out string message)
        {
            bool boo_success = false;
            message = string.Empty;
            PMS.Model.Context.PMSEntities dc_pms = new PMS.Model.Context.PMSEntities();
            try
            {
                foreach (int cid in cids)
                {
                    foreach (var item in dc_pms.PMS_APPRAISAL_TRAIL.Where(sec => sec.APPRAISAL_ID == cid))
                    {
                        dc_pms.PMS_APPRAISAL_TRAIL.DeleteObject(item);
                    }
                    foreach (var item in dc_pms.PMS_APPRAISAL_SECTION.Where(sec => sec.APPRAISAL_ID == cid))
                    {
                        foreach (var childitem in dc_pms.PMS_APPRAISAL_SECTION_COMMENT.Where(sec => sec.SECTION_ID == item.ID))
                        {
                            dc_pms.PMS_APPRAISAL_SECTION_COMMENT.DeleteObject(childitem);
                        }
                        dc_pms.PMS_APPRAISAL_SECTION.DeleteObject(item);
                    }
                    foreach (var item in dc_pms.PMS_APPRAISAL_APPROVER.Where(sec => sec.APPRAISAL_ID == cid))
                    {
                        dc_pms.PMS_APPRAISAL_APPROVER.DeleteObject(item);
                    }
                    foreach (var item in dc_pms.PMS_APPRAISAL_STAGE.Where(sec => sec.APPRAISAL_ID == cid))
                    {
                        dc_pms.PMS_APPRAISAL_STAGE.DeleteObject(item);
                    }
                    foreach (var item in dc_pms.PMS_APPRAISAL_REVIEWER.Where(sec => sec.APPRAISAL_ID == cid))
                    {
                        dc_pms.PMS_APPRAISAL_REVIEWER.DeleteObject(item);
                    }
                    foreach (var item in dc_pms.PMS_APPRAISAL_PERFORMANCE_COACHING.Where(sec => sec.APPRAISAL_ID == cid))
                    {
                        foreach (var childitem in dc_pms.PMS_APPRAISAL_PERFORMANCE_COACHING_COMMENT.Where(sec => sec.ITEM_ID == item.ID))
                        {
                            dc_pms.PMS_APPRAISAL_PERFORMANCE_COACHING_COMMENT.DeleteObject(childitem);
                        }
                        dc_pms.PMS_APPRAISAL_PERFORMANCE_COACHING.DeleteObject(item);
                    }
                    foreach (var item in dc_pms.PMS_APPRAISAL_KPI.Where(sec => sec.APPRAISAL_ID == cid))
                    {
                        foreach (var childitem in dc_pms.PMS_APPRAISAL_KPI_COMMENT.Where(sec => sec.ITEM_ID == item.ID))
                        {
                            dc_pms.PMS_APPRAISAL_KPI_COMMENT.DeleteObject(childitem);
                        }
                        dc_pms.PMS_APPRAISAL_KPI.DeleteObject(item);
                    }
                    foreach (var item in dc_pms.PMS_APPRAISAL_CORE_VALUE.Where(sec => sec.APPRAISAL_ID == cid))
                    {
                        foreach (var childitem in dc_pms.PMS_APPRAISAL_CORE_VALUE_COMMENT.Where(sec => sec.ITEM_ID == item.ID))
                        {
                            dc_pms.PMS_APPRAISAL_CORE_VALUE_COMMENT.DeleteObject(childitem);
                        }
                        dc_pms.PMS_APPRAISAL_CORE_VALUE.DeleteObject(item);
                    }
                    foreach (var item in dc_pms.PMS_APPRAISAL_CAREER_DEVELOPMENT.Where(sec => sec.APPRAISAL_ID == cid))
                    {
                        foreach (var childitem in dc_pms.PMS_APPRAISAL_CAREER_DEVELOPMENT_COMMENT.Where(sec => sec.ITEM_ID == item.ID))
                        {
                            dc_pms.PMS_APPRAISAL_CAREER_DEVELOPMENT_COMMENT.DeleteObject(childitem);
                        }
                        dc_pms.PMS_APPRAISAL_CAREER_DEVELOPMENT.DeleteObject(item);
                    }
                    foreach (var item in dc_pms.TASKs.Where(sec => sec.RECORD_ID == cid && sec.MODULE_ID == eHR.PMS.Model.PMSConstants.MODULE_ID_PMS))
                    {
                        foreach (var childitem in dc_pms.TASK_OWNER.Where(sec => sec.TASK_ID == item.ID))
                        {
                            dc_pms.TASK_OWNER.DeleteObject(childitem);
                        }
                        dc_pms.TASKs.DeleteObject(item);
                    }
                    var appr = dc_pms.PMS_APPRAISAL.Where(sec => sec.ID == cid).FirstOrDefault();
                    dc_pms.PMS_APPRAISAL.DeleteObject(appr);


                }
                dc_pms.SaveChanges();
                boo_success = true;
            }
            catch (Exception exc)
            {
                message = exc.Message;
            }
            finally
            {
                dc_pms.Dispose();
            }
            return boo_success;
        }
        public static List<DTO.Cycle.Cycle> GetCycleByStatus(int? statusId)
        {
            PMS.Model.Context.PMSEntities dc_pms = new PMS.Model.Context.PMSEntities();
            List<DTO.Cycle.Cycle> lst_cycles = null;
            IEnumerable<PMS.Model.Context.PMS_CYCLE> entities;

            if (statusId == null)
            {
                entities = from ent_cycles in dc_pms.PMS_CYCLE
                           select ent_cycles;
            }
            else
            {
                entities = from ent_cycles in dc_pms.PMS_CYCLE
                           where ent_cycles.STATUS_ID == statusId
                           select ent_cycles;
            }

            if (!Lib.Utility.Common.IsNullOrEmptyList(entities))
            {
                lst_cycles = Mappers.PMSMapper.MapCycleEntitiesToDTOs(entities.ToList(), true);
            }

            return lst_cycles;
        }

        public static DTO.Cycle.Cycle GetCycleById(int Id)
        {
            PMS.Model.Context.PMSEntities dc_pms = new PMS.Model.Context.PMSEntities();
            DTO.Cycle.Cycle obj_cycle = null;
            PMS.Model.Context.PMS_CYCLE entity;

            entity = (from ent_cycles in dc_pms.PMS_CYCLE
                      where ent_cycles.ID == Id
                      select ent_cycles).SingleOrDefault();

            if (entity != null)
            {
                obj_cycle = Mappers.PMSMapper.MapCycleEntityToDTO(entity, true);

            }
            return obj_cycle;
        }

        public static bool UpdateCycle(PMS.Model.DTO.Cycle.Cycle cycle, out string message)
        {
            bool boo_success = false;
            message = string.Empty;
            PMS.Model.Context.PMSEntities dc_pms = new PMS.Model.Context.PMSEntities();

            try
            {
                foreach (PMS.Model.DTO.Cycle.Stage stage in cycle.CycleStages)
                {
                    PMS.Model.Context.PMS_CYCLE_STAGE entity_cycle_stage = dc_pms.PMS_CYCLE_STAGE.FirstOrDefault(sec => sec.ID == stage.Id);
                    entity_cycle_stage.START_DATE = stage.StartDate;
                    entity_cycle_stage.END_DATE = stage.EndDate;
                }
                dc_pms.SaveChanges();
                boo_success = true;
            }
            catch (Exception exc)
            {
                message = exc.Message;
            }
            finally
            {
                dc_pms.Dispose();
            }
            return boo_success;
        }

        public static List<DTO.Cycle.Stage> GetStagesByCycleId(int cycleId)
        {
            PMS.Model.Context.PMSEntities dc_pms = new PMS.Model.Context.PMSEntities();
            List<DTO.Cycle.Stage> lst_stages = null;
            IEnumerable<PMS.Model.Context.PMS_CYCLE_STAGE> entities;

            entities = from ent_cycle_stage in dc_pms.PMS_CYCLE_STAGE
                       where ent_cycle_stage.CYCLE_ID == cycleId
                       orderby ent_cycle_stage.STAGE_ID
                       select ent_cycle_stage;

            if (!Lib.Utility.Common.IsNullOrEmptyList(entities))
            {
                lst_stages = Mappers.PMSMapper.MapCycleStageEntitiesToDTOs(entities.ToList());
            }

            return lst_stages;
        }
        #endregion

        #region Security

        public static PMS.Model.DTO.Core.Security.User GetUserByDomainId(string domainId)
        {
            PMS.Model.Context.PMSEntities dc_pms = new PMS.Model.Context.PMSEntities();
            PMS.Model.DTO.Core.Security.User obj_user = null;
            System.Data.Objects.ObjectQuery<PMS.Model.Context.EMPLOYEE> entities;

            dc_pms.ContextOptions.LazyLoadingEnabled = false;

            entities = ((from ent_employees in dc_pms.EMPLOYEEs.OrderByDescending(a => a.ID)
                         join ent_user_roles in dc_pms.USER_ROLE on ent_employees.ID equals ent_user_roles.USER_ID into j_emp_role
                         where ent_employees.DOMAIN_ID.Equals(domainId)
                         from ent_emp_role in j_emp_role.DefaultIfEmpty()
                         select ent_employees) as System.Data.Objects.ObjectQuery<PMS.Model.Context.EMPLOYEE>)
                           .Include("USER_ROLE")
                           .Include("USER_ROLE.MST_ROLE")
                           .Include("MST_ACR_GRADE");

            if (!Lib.Utility.Common.IsNullOrEmptyList(entities))
            {
                if (entities.Count() > 1) { }
                obj_user = PMS.Model.Mappers.CoreMapper.MapEmployeeEntityToUserDTO(entities.First(), true);
            }

            dc_pms.Dispose();
            return obj_user;
        }

        #endregion

        #region Appraisal

        public static List<PMS.Model.DTO.Appraisal.Appraisal> GetAppraisalsByEmployee(int employeeId, int? statusId)
        {
            PMS.Model.Context.PMSEntities dc_pms = new PMS.Model.Context.PMSEntities();
            List<PMS.Model.DTO.Appraisal.Appraisal> lst_appraisals = null;
            System.Data.Objects.ObjectQuery<PMS.Model.Context.PMS_APPRAISAL> entities;

            dc_pms.ContextOptions.LazyLoadingEnabled = false;

            if (statusId == null)
            {
                entities = ((from ent_appraisal in dc_pms.PMS_APPRAISAL
                             join ent_cycle in dc_pms.PMS_CYCLE on ent_appraisal.CYCLE_ID equals ent_cycle.ID
                             where ent_appraisal.EMPLOYEE_ID == employeeId
                             select ent_appraisal) as System.Data.Objects.ObjectQuery<PMS.Model.Context.PMS_APPRAISAL>)
                                .Include("PMS_CYCLE")
                                .Include("PMS_APPRAISAL_STAGE")
                                .Include("PMS_APPRAISAL_STAGE.PMS_MST_STAGE")
                                .Include("PMS_APPRAISAL_REVIEWER")
                                .Include("PMS_APPRAISAL_REVIEWER.EMPLOYEE")
                                .Include("PMS_MST_STATUS")
                                .Include("MST_DEPARTMENT")
                                .Include("EMPLOYEE")
                                .Include("PMS_APPRAISAL_TRAIL")
                                .Include("PMS_APPRAISAL_APPROVER")
                                .Include("PMS_APPRAISAL_APPROVER.EMPLOYEE");
            }
            else
            {
                entities = ((from ent_appraisal in dc_pms.PMS_APPRAISAL
                             join ent_cycle in dc_pms.PMS_CYCLE on ent_appraisal.CYCLE_ID equals ent_cycle.ID
                             where
                              ent_appraisal.EMPLOYEE_ID == employeeId &&
                              ent_appraisal.STAGE_ID == statusId
                             select ent_appraisal) as System.Data.Objects.ObjectQuery<PMS.Model.Context.PMS_APPRAISAL>)
                                .Include("PMS_CYCLE")
                                .Include("PMS_APPRAISAL_STAGE")
                                .Include("PMS_APPRAISAL_STAGE.PMS_MST_STAGE")
                                .Include("PMS_APPRAISAL_REVIEWER")
                                .Include("PMS_APPRAISAL_REVIEWER.EMPLOYEE")
                                .Include("PMS_MST_STATUS")
                                .Include("MST_DEPARTMENT")
                                .Include("EMPLOYEE")
                                .Include("PMS_APPRAISAL_TRAIL")
                                .Include("PMS_APPRAISAL_APPROVER")
                                .Include("PMS_APPRAISAL_APPROVER.EMPLOYEE");
            }

            if (!Lib.Utility.Common.IsNullOrEmptyList(entities))
            {
                lst_appraisals = PMS.Model.Mappers.PMSMapper.MapAppraisalEntitiesToDTOs(entities.ToList(), true);
            }

            dc_pms.Dispose();
            return lst_appraisals;
        }

        public static List<PMS.Model.DTO.Appraisal.Appraisal> GetAppraisalsInCycleToApprove(int approverEmployeeId, int? cycleId)
        {
            PMS.Model.Context.PMSEntities dc_pms = new PMS.Model.Context.PMSEntities();
            List<PMS.Model.DTO.Appraisal.Appraisal> lst_appraisals = null;
            System.Data.Objects.ObjectQuery<PMS.Model.Context.PMS_APPRAISAL> entities;

            dc_pms.ContextOptions.LazyLoadingEnabled = false;

            if (cycleId == null)
            {
                entities = ((from ent_appraisal in dc_pms.PMS_APPRAISAL
                             join ent_approvers in dc_pms.PMS_APPRAISAL_APPROVER on ent_appraisal.ID equals ent_approvers.APPRAISAL_ID
                             where
                              ent_approvers.APPROVER_ID == approverEmployeeId
                             select ent_appraisal) as System.Data.Objects.ObjectQuery<PMS.Model.Context.PMS_APPRAISAL>)
                                .Include("PMS_CYCLE")
                                .Include("MST_DEPARTMENT")
                                .Include("EMPLOYEE")
                                .Include("PMS_APPRAISAL_STAGE")
                                .Include("PMS_APPRAISAL_STAGE.PMS_MST_STAGE")
                                .Include("PMS_MST_STATUS")
                                .Include("PMS_APPRAISAL_APPROVER")
                                .Include("PMS_APPRAISAL_APPROVER.EMPLOYEE");
            }
            else
            {
                entities = ((from ent_appraisal in dc_pms.PMS_APPRAISAL
                             join ent_cycles in dc_pms.PMS_CYCLE on ent_appraisal.CYCLE_ID equals ent_cycles.ID
                             join ent_approvers in dc_pms.PMS_APPRAISAL_APPROVER on ent_appraisal.ID equals ent_approvers.APPRAISAL_ID
                             where
                              ent_cycles.ID == cycleId &&
                              ent_approvers.APPROVER_ID == approverEmployeeId
                             select ent_appraisal) as System.Data.Objects.ObjectQuery<PMS.Model.Context.PMS_APPRAISAL>)
                                .Include("PMS_CYCLE")
                                .Include("MST_DEPARTMENT")
                                .Include("EMPLOYEE")
                                .Include("PMS_APPRAISAL_STAGE")
                                .Include("PMS_APPRAISAL_STAGE.PMS_MST_STAGE")
                                .Include("PMS_MST_STATUS")
                                .Include("PMS_APPRAISAL_APPROVER")
                                .Include("PMS_APPRAISAL_APPROVER.EMPLOYEE");
            }

            if (!Lib.Utility.Common.IsNullOrEmptyList(entities))
            {
                lst_appraisals = PMS.Model.Mappers.PMSMapper.MapAppraisalEntitiesToDTOs(entities.ToList(), true);
            }

            dc_pms.Dispose();
            return lst_appraisals;
        }

        public static void GetAppraisalsInCycleToReview(int reviewerEmployeeId, int? cycleId, out List<PMS.Model.DTO.Appraisal.Appraisal> MyAppraisalsToReview, out List<PMS.Model.DTO.Appraisal.Appraisal>  MyAppraisalsToReviewSMT)
        {
            PMS.Model.Context.PMSEntities dc_pms = new PMS.Model.Context.PMSEntities();
            //List<PMS.Model.DTO.Appraisal.Appraisal> lst_appraisals = null;
            MyAppraisalsToReview = new List<DTO.Appraisal.Appraisal>();
            MyAppraisalsToReviewSMT = new List<DTO.Appraisal.Appraisal>();
            System.Data.Objects.ObjectQuery<PMS.Model.Context.PMS_APPRAISAL> entities;
            System.Data.Objects.ObjectQuery<PMS.Model.Context.PMS_APPRAISAL> entitiessmt;
            dc_pms.ContextOptions.LazyLoadingEnabled = false;

            if (cycleId == null)
            {
                entities = ((from ent_appraisal in dc_pms.PMS_APPRAISAL
                             join ent_cycle in dc_pms.PMS_CYCLE on ent_appraisal.CYCLE_ID equals ent_cycle.ID
                             join ent_reviewer in dc_pms.PMS_APPRAISAL_REVIEWER on ent_appraisal.ID equals ent_reviewer.APPRAISAL_ID
                             where
                              ent_reviewer.REVIEWER_ID == reviewerEmployeeId && (ent_reviewer.IS_SMT==null || ent_reviewer.IS_SMT==false)
                             select ent_appraisal) as System.Data.Objects.ObjectQuery<PMS.Model.Context.PMS_APPRAISAL>)
                                .Include("EMPLOYEE");
                entitiessmt = ((from ent_appraisal in dc_pms.PMS_APPRAISAL
                             join ent_cycle in dc_pms.PMS_CYCLE on ent_appraisal.CYCLE_ID equals ent_cycle.ID
                             join ent_reviewer in dc_pms.PMS_APPRAISAL_REVIEWER on ent_appraisal.ID equals ent_reviewer.APPRAISAL_ID
                             where
                              ent_reviewer.REVIEWER_ID == reviewerEmployeeId && ent_reviewer.IS_SMT == true
                             select ent_appraisal) as System.Data.Objects.ObjectQuery<PMS.Model.Context.PMS_APPRAISAL>)
                                .Include("EMPLOYEE");
            }
            else
            {
                entities = ((from ent_appraisal in dc_pms.PMS_APPRAISAL
                             join ent_cycle in dc_pms.PMS_CYCLE on ent_appraisal.CYCLE_ID equals ent_cycle.ID
                             join ent_reviewer in dc_pms.PMS_APPRAISAL_REVIEWER on ent_appraisal.ID equals ent_reviewer.APPRAISAL_ID
                             where
                              ent_reviewer.REVIEWER_ID == reviewerEmployeeId &&
                              ent_cycle.ID == cycleId && (ent_reviewer.IS_SMT==null || ent_reviewer.IS_SMT==false)
                             select ent_appraisal) as System.Data.Objects.ObjectQuery<PMS.Model.Context.PMS_APPRAISAL>)
                                .Include("EMPLOYEE");
                entitiessmt = ((from ent_appraisal in dc_pms.PMS_APPRAISAL
                             join ent_cycle in dc_pms.PMS_CYCLE on ent_appraisal.CYCLE_ID equals ent_cycle.ID
                             join ent_reviewer in dc_pms.PMS_APPRAISAL_REVIEWER on ent_appraisal.ID equals ent_reviewer.APPRAISAL_ID
                             where
                              ent_reviewer.REVIEWER_ID == reviewerEmployeeId &&
                              ent_cycle.ID == cycleId && ent_reviewer.IS_SMT == true
                             select ent_appraisal) as System.Data.Objects.ObjectQuery<PMS.Model.Context.PMS_APPRAISAL>)
                                .Include("EMPLOYEE");
            }

            if (!Lib.Utility.Common.IsNullOrEmptyList(entities))
            {
                MyAppraisalsToReview = PMS.Model.Mappers.PMSMapper.MapAppraisalEntitiesToDTOs(entities.ToList(), true);
            }
            if (!Lib.Utility.Common.IsNullOrEmptyList(entitiessmt))
            {
                MyAppraisalsToReviewSMT = PMS.Model.Mappers.PMSMapper.MapAppraisalEntitiesToDTOs(entitiessmt.ToList(), true);
            }
            dc_pms.Dispose();
            return;
            //return lst_appraisals;
        }

        public static List<PMS.Model.DTO.Appraisal.Appraisal> GetEmployeesInAppraisalsByCycleId(int cycleId)
        {
            PMS.Model.Context.PMSEntities dc_pms = new PMS.Model.Context.PMSEntities();
            List<PMS.Model.DTO.Appraisal.Appraisal> lst_employees = null;
            System.Data.Objects.ObjectQuery<PMS.Model.Context.PMS_APPRAISAL> entities;

            dc_pms.ContextOptions.LazyLoadingEnabled = false;

            entities = ((from ent_appraisal in dc_pms.PMS_APPRAISAL
                         join ent_employee in dc_pms.EMPLOYEEs on ent_appraisal.EMPLOYEE_ID equals ent_employee.ID
                         where ent_appraisal.CYCLE_ID == cycleId
                         select ent_appraisal) as System.Data.Objects.ObjectQuery<PMS.Model.Context.PMS_APPRAISAL>)
                            .Include("EMPLOYEE")
                            .Include("MST_DEPARTMENT")
                            .Include("PMS_MST_STAGE")
                            .Include("PMS_MST_STATUS")
                            .Include("PMS_APPRAISAL_APPROVER")
                            .Include("PMS_APPRAISAL_APPROVER.EMPLOYEE");


            if (!Lib.Utility.Common.IsNullOrEmptyList(entities))
            {
                lst_employees = Mappers.PMSMapper.MapAppraisalEntitiesToDTOs(entities.ToList(), true);
            }

            dc_pms.Dispose();
            return lst_employees;
        }
        public static PMS.Model.DTO.Appraisal.Appraisal GetAppraisalById(int appraisalId)
        {
            PMS.Model.Context.PMSEntities dc_pms = new PMS.Model.Context.PMSEntities();
            PMS.Model.DTO.Appraisal.Appraisal obj_appraisal = null;
            PMS.Model.Context.PMS_APPRAISAL entity;

            entity = (from ent_appraisals in dc_pms.PMS_APPRAISAL
                      where ent_appraisals.ID == appraisalId
                      select ent_appraisals).Single();

            if (entity != null)
            {
                obj_appraisal = Mappers.PMSMapper.MapAppraisalEntityToDTO(entity, true);
            }
            dc_pms.Dispose();
            return obj_appraisal;
        }

        public static bool UpdateAppraisalAndTask(PMS.Model.DTO.Appraisal.Appraisal appraisal, List<DTO.Core.Task.Task> completedTasks, List<DTO.Core.Task.Task> newTasks, out string message)
        {
            bool boo_success = false;
            message = string.Empty;
            PMS.Model.Context.PMSEntities dc_pms = new PMS.Model.Context.PMSEntities();

            try
            {
                Model.Context.PMS_APPRAISAL entity_appraisal = (from ent_appraisal in dc_pms.PMS_APPRAISAL
                                                                where ent_appraisal.ID == appraisal.Id
                                                                select ent_appraisal).SingleOrDefault();

                entity_appraisal.STAGE_ID = appraisal.Stage.Id;
                entity_appraisal.STATUS_ID = appraisal.Status.Id;

                foreach (DTO.Core.Task.Task obj_task in completedTasks)
                {
                    Model.Context.TASK entity_task = (from ent_task in dc_pms.TASKs
                                                      where ent_task.ID == obj_task.Id
                                                      select ent_task).SingleOrDefault();

                    entity_task.STATUS_ID = obj_task.Status.Id;
                }

                foreach (DTO.Core.Task.Task obj_task in newTasks)
                {
                    Model.Context.TASK entity_task = Mappers.CoreMapper.MapTaskDTOToEntity(obj_task);
                    dc_pms.TASKs.AddObject(entity_task);

                    foreach (DTO.Core.Task.Owner obj_owner in obj_task.Owners)
                    {
                        Model.Context.TASK_OWNER entity_owner = Model.Mappers.CoreMapper.MapTaskOwnerDTOToEntity(obj_owner);
                        entity_owner.TASK_ID = entity_task.ID;
                        dc_pms.TASK_OWNER.AddObject(entity_owner);
                    }
                }

                foreach (Model.DTO.Appraisal.Trail dto_appraisal_trail in appraisal.Trails)
                {
                    if (dto_appraisal_trail.Id < 1)
                    {
                        Model.Context.PMS_APPRAISAL_TRAIL entity_appraisal_trail = Model.Mappers.PMSMapper.MapAppraisalTrailDTOToEntity(dto_appraisal_trail);
                        entity_appraisal_trail.APPRAISAL_ID = entity_appraisal.ID;
                        dc_pms.PMS_APPRAISAL_TRAIL.AddObject(entity_appraisal_trail);
                    }
                }

                var lst_kpi_comments = dc_pms.PMS_APPRAISAL_KPI_COMMENT.Where(rec => rec.FORM_SAVE_ONLY == true);
                if (!Lib.Utility.Common.IsNullOrEmptyList(lst_kpi_comments))
                {
                    foreach (Context.PMS_APPRAISAL_KPI_COMMENT ent_comment in lst_kpi_comments)
                    {
                        if (string.IsNullOrEmpty(ent_comment.COMMENT))
                        {
                            dc_pms.DeleteObject(ent_comment);
                        }
                        else
                        {
                            ent_comment.FORM_SAVE_ONLY = false;
                        }
                    }
                }

                var lst_core_value_comments = dc_pms.PMS_APPRAISAL_CORE_VALUE_COMMENT.Where(rec => rec.FORM_SAVE_ONLY == true);
                if (!Lib.Utility.Common.IsNullOrEmptyList(lst_core_value_comments))
                {
                    foreach (Context.PMS_APPRAISAL_CORE_VALUE_COMMENT ent_comment in lst_core_value_comments)
                    {
                        if (string.IsNullOrEmpty(ent_comment.COMMENT))
                        {
                            dc_pms.DeleteObject(ent_comment);
                        }
                        else
                        {
                            ent_comment.FORM_SAVE_ONLY = false;
                        }
                    }
                }

                var lst_performance_coaching_comments = dc_pms.PMS_APPRAISAL_PERFORMANCE_COACHING_COMMENT.Where(rec => rec.FORM_SAVE_ONLY == true);
                if (!Lib.Utility.Common.IsNullOrEmptyList(lst_performance_coaching_comments))
                {
                    foreach (Context.PMS_APPRAISAL_PERFORMANCE_COACHING_COMMENT ent_comment in lst_performance_coaching_comments)
                    {
                        if (string.IsNullOrEmpty(ent_comment.COMMENT))
                        {
                            dc_pms.DeleteObject(ent_comment);
                        }
                        else
                        {
                            ent_comment.FORM_SAVE_ONLY = false;
                        }
                    }
                }

                var lst_career_development_comments = dc_pms.PMS_APPRAISAL_CAREER_DEVELOPMENT_COMMENT.Where(rec => rec.FORM_SAVE_ONLY == true);
                if (!Lib.Utility.Common.IsNullOrEmptyList(lst_career_development_comments))
                {
                    foreach (Context.PMS_APPRAISAL_CAREER_DEVELOPMENT_COMMENT ent_comment in lst_career_development_comments)
                    {
                        if (string.IsNullOrEmpty(ent_comment.COMMENT))
                        {
                            dc_pms.DeleteObject(ent_comment);
                        }
                        else
                        {
                            ent_comment.FORM_SAVE_ONLY = false;
                        }
                    }
                }
                dc_pms.SaveChanges();
                boo_success = true;
            }
            catch (Exception exc)
            {
                message = exc.Message;
            }
            finally
            {
                dc_pms.Dispose();
            }
            return boo_success;
        }

        public static bool UpdateReviewersForAppraisal(int appraisalId, List<DTO.Appraisal.Reviewer> reviewers, out string message)
        {
            bool boo_success = false;
            message = string.Empty;
            PMS.Model.Context.PMSEntities dc_pms = new PMS.Model.Context.PMSEntities();

            try
            {
                // remove all reviewers for appraisal
                IEnumerable<Model.Context.PMS_APPRAISAL_REVIEWER> lst_ent_reviewers = from ent_reviewers in dc_pms.PMS_APPRAISAL_REVIEWER
                                                                                      where ent_reviewers.APPRAISAL_ID == appraisalId
                                                                                      select ent_reviewers;

                if (!Lib.Utility.Common.IsNullOrEmptyList(lst_ent_reviewers))
                {
                    foreach (Model.Context.PMS_APPRAISAL_REVIEWER ent_reviewer in lst_ent_reviewers)
                    {
                        dc_pms.PMS_APPRAISAL_REVIEWER.DeleteObject(ent_reviewer);
                    }
                }

                // add new reviewers
                if (!Lib.Utility.Common.IsNullOrEmptyList(reviewers))
                {
                    foreach (DTO.Appraisal.Reviewer obj_reviewer in reviewers)
                    {
                        dc_pms.PMS_APPRAISAL_REVIEWER.AddObject(Mappers.PMSMapper.MapReviewerDTOTOEntity(obj_reviewer));
                    }
                }
                dc_pms.SaveChanges();
                boo_success = true;
            }
            catch (Exception exc)
            {
                message = exc.Message;
            }
            finally
            {
                dc_pms.Dispose();
            }

            return boo_success;
        }

        #region KPI

        public static bool UpdateAppraisalKPIs(List<PMS.Model.DTO.Appraisal.KPI> insertList, List<PMS.Model.DTO.Appraisal.KPI> updateList, List<PMS.Model.DTO.Appraisal.KPI> deleteList, out string message)
        {
            bool boo_success = false;
            message = string.Empty;
            PMS.Model.Context.PMSEntities dc_pms = new PMS.Model.Context.PMSEntities();

            try
            {
                if (!Lib.Utility.Common.IsNullOrEmptyList(insertList))
                {
                    foreach (PMS.Model.DTO.Appraisal.KPI obj_kpi in insertList)
                    {
                        dc_pms.PMS_APPRAISAL_KPI.AddObject(Mappers.PMSMapper.MapAppraisalKPIDTOToEntity(obj_kpi));
                    }
                }

                if (!Lib.Utility.Common.IsNullOrEmptyList(deleteList))
                {
                    foreach (PMS.Model.DTO.Appraisal.KPI obj_kpi in deleteList)
                    {
                        foreach (PMS.Model.Context.PMS_APPRAISAL_KPI_COMMENT ent_comment in dc_pms.PMS_APPRAISAL_KPI_COMMENT.Where(rec => rec.ITEM_ID == obj_kpi.Id))
                        {
                            dc_pms.PMS_APPRAISAL_KPI_COMMENT.DeleteObject(ent_comment);
                        }

                        Model.Context.PMS_APPRAISAL_KPI entity = dc_pms.PMS_APPRAISAL_KPI.Where(rec => rec.ID == obj_kpi.Id).Single();
                        dc_pms.PMS_APPRAISAL_KPI.DeleteObject(entity);
                    }
                }

                if (!Lib.Utility.Common.IsNullOrEmptyList(updateList))
                {
                    foreach (PMS.Model.DTO.Appraisal.KPI obj_kpi in updateList)
                    {
                        Model.Context.PMS_APPRAISAL_KPI entity = dc_pms.PMS_APPRAISAL_KPI.Where(rec => rec.ID == obj_kpi.Id).Single();
                        entity.DESCRIPTION = obj_kpi.Description;
                        entity.TARGET = obj_kpi.Target;
                        entity.LEVEL_1_SCORE = obj_kpi.Level1Score;
                        entity.LEVEL_2_SCORE = obj_kpi.Level2Score;
                        entity.SELF_SCORE = obj_kpi.SelfScore;
                        entity.PRIORITY_MASTER_ID = obj_kpi.Priority.Id;
                    }
                }

                dc_pms.SaveChanges();
                boo_success = true;
            }
            catch (Exception exc)
            {
                message = exc.Message;
            }
            finally
            {
                dc_pms.Dispose();
            }
            return boo_success;
        }

        public static bool UpdateAppraisalKPIComment(List<PMS.Model.DTO.Appraisal.KPIComment> updateList, out string message)
        {
            bool boo_success = false;
            message = string.Empty;
            PMS.Model.Context.PMSEntities dc_pms = new PMS.Model.Context.PMSEntities();

            try
            {
                if (!Lib.Utility.Common.IsNullOrEmptyList(updateList))
                {
                    foreach (PMS.Model.DTO.Appraisal.KPIComment obj_comment in updateList)
                    {
                        // there should only be 1 new comment for each appraisal kpi item
                        PMS.Model.Context.PMS_APPRAISAL_KPI_COMMENT ent_comment = dc_pms.PMS_APPRAISAL_KPI_COMMENT.Where(rec => rec.ITEM_ID == obj_comment.AppraisalKPI.Id &&
                                                                                                                                rec.COMMENTED_BY_ID == obj_comment.Commentor.Id &&
                                                                                                                                rec.FORM_SAVE_ONLY == true).SingleOrDefault();

                        if (ent_comment != null)
                        {
                            ent_comment.COMMENT = obj_comment.Comments;
                            ent_comment.COMMENTED_TIMESTAMP = obj_comment.CommentedTimestamp;
                        }
                        else
                        {
                            ent_comment = Mappers.PMSMapper.MapAppraisalKPICommentDTOToEntity(obj_comment);
                            dc_pms.PMS_APPRAISAL_KPI_COMMENT.AddObject(ent_comment);
                        }
                    }
                    dc_pms.SaveChanges();
                    boo_success = true;
                }
            }
            catch (Exception exc)
            {
                message = exc.Message;
            }
            finally
            {
                dc_pms.Dispose();
            }
            return boo_success;
        }

        #endregion KPI

        #region Core Values

        public static bool UpdateAppraisalCoreValues(List<PMS.Model.DTO.Appraisal.CoreValue> insertList, List<PMS.Model.DTO.Appraisal.CoreValue> updateList, List<PMS.Model.DTO.Appraisal.CoreValue> deleteList, out string message)
        {
            bool boo_success = false;
            message = string.Empty;
            PMS.Model.Context.PMSEntities dc_pms = new PMS.Model.Context.PMSEntities();

            try
            {
                if (!Lib.Utility.Common.IsNullOrEmptyList(insertList))
                {
                    foreach (PMS.Model.DTO.Appraisal.CoreValue obj_core_value in insertList)
                    {
                        dc_pms.PMS_APPRAISAL_CORE_VALUE.AddObject(Mappers.PMSMapper.MapAppraisalCoreValueDTOToEntity(obj_core_value));
                    }
                }

                if (!Lib.Utility.Common.IsNullOrEmptyList(deleteList))
                {
                    foreach (PMS.Model.DTO.Appraisal.CoreValue obj_core_value in deleteList)
                    {
                        foreach (PMS.Model.Context.PMS_APPRAISAL_CORE_VALUE_COMMENT ent_comment in dc_pms.PMS_APPRAISAL_CORE_VALUE_COMMENT.Where(rec => rec.ITEM_ID == obj_core_value.Id))
                        {
                            dc_pms.PMS_APPRAISAL_CORE_VALUE_COMMENT.DeleteObject(ent_comment);
                        }

                        Model.Context.PMS_APPRAISAL_CORE_VALUE entity = dc_pms.PMS_APPRAISAL_CORE_VALUE.Where(rec => rec.ID == obj_core_value.Id).Single();
                        dc_pms.PMS_APPRAISAL_CORE_VALUE.DeleteObject(entity);
                    }
                }

                if (!Lib.Utility.Common.IsNullOrEmptyList(updateList))
                {
                    foreach (PMS.Model.DTO.Appraisal.CoreValue obj_core_value in updateList)
                    {
                        Model.Context.PMS_APPRAISAL_CORE_VALUE entity = dc_pms.PMS_APPRAISAL_CORE_VALUE.Where(rec => rec.ID == obj_core_value.Id).Single();
                        //entity.COMPETENCY_MASTER_ID = obj_core_value.CoreValueCompetency.Id;
                        entity.TARGET = obj_core_value.Target;
                        entity.LEVEL_1_SCORE = obj_core_value.Level1Score;
                        entity.LEVEL_2_SCORE = obj_core_value.Level2Score;
                        entity.SELF_SCORE = obj_core_value.SelfScore;
                    }
                }

                dc_pms.SaveChanges();
                boo_success = true;
            }
            catch (Exception exc)
            {
                message = exc.Message;
            }
            finally
            {
                dc_pms.Dispose();
            }
            return boo_success;
        }

        public static bool UpdateAppraisalCoreValueComment(List<PMS.Model.DTO.Appraisal.CoreValueComment> updateList, out string message)
        {
            bool boo_success = false;
            message = string.Empty;
            PMS.Model.Context.PMSEntities dc_pms = new PMS.Model.Context.PMSEntities();

            try
            {
                if (!Lib.Utility.Common.IsNullOrEmptyList(updateList))
                {
                    foreach (PMS.Model.DTO.Appraisal.CoreValueComment obj_comment in updateList)
                    {
                        // there should only be 1 new comment for each appraisal kpi item
                        PMS.Model.Context.PMS_APPRAISAL_CORE_VALUE_COMMENT ent_comment = dc_pms.PMS_APPRAISAL_CORE_VALUE_COMMENT.Where(rec => rec.ITEM_ID == obj_comment.AppraisalCoreValue.Id &&
                                                                                                                                                rec.COMMENTED_BY_ID == obj_comment.Commentor.Id &&
                                                                                                                                                rec.FORM_SAVE_ONLY == true).SingleOrDefault();

                        if (ent_comment != null)
                        {
                            ent_comment.COMMENT = obj_comment.Comments;
                            ent_comment.COMMENTED_TIMESTAMP = obj_comment.CommentedTimestamp;
                        }
                        else
                        {
                            ent_comment = Mappers.PMSMapper.MapAppraisalCoreValueCommentDTOToEntity(obj_comment);
                            dc_pms.PMS_APPRAISAL_CORE_VALUE_COMMENT.AddObject(ent_comment);
                        }
                    }
                    dc_pms.SaveChanges();
                    boo_success = true;
                }
            }
            catch (Exception exc)
            {
                message = exc.Message;
            }
            finally
            {
                dc_pms.Dispose();
            }
            return boo_success;
        }

        #endregion Core Values

        #region Performance Coaching

        public static bool UpdateAppraisalPerformanceCoaching(PMS.Model.DTO.Appraisal.PerformanceCoaching performanceCoaching, out string message)
        {
            bool boo_success = false;
            message = string.Empty;
            Model.Context.PMS_APPRAISAL_PERFORMANCE_COACHING entity;
            PMS.Model.Context.PMSEntities dc_pms = new PMS.Model.Context.PMSEntities();

            try
            {
                if (performanceCoaching != null)
                {
                    entity = dc_pms.PMS_APPRAISAL_PERFORMANCE_COACHING.Where(rec => rec.APPRAISAL_ID == performanceCoaching.Appraisal.Id).SingleOrDefault();
                    if (entity == null)
                    {
                        entity = Mappers.PMSMapper.MapAppraisalPerformanceCoachingDTOToEntity(performanceCoaching);
                        dc_pms.PMS_APPRAISAL_PERFORMANCE_COACHING.AddObject(entity);
                    }
                    else
                    {
                        entity.STRENGTH_AREA = performanceCoaching.AreasOfStrength;
                        entity.IMPROVEMENT_AREA = performanceCoaching.AreasOfImprovement;
                    }

                    dc_pms.SaveChanges();
                    boo_success = true;
                }
            }
            catch (Exception exc)
            {
                message = exc.Message;
            }
            finally
            {
                dc_pms.Dispose();
            }

            return boo_success;
        }

        public static bool UpdateAppraisalPerformanceCoachingComment(List<PMS.Model.DTO.Appraisal.PerformanceCoachingComment> updateList, out string message)
        {
            bool boo_success = false;
            message = string.Empty;
            PMS.Model.Context.PMSEntities dc_pms = new PMS.Model.Context.PMSEntities();

            try
            {
                if (!Lib.Utility.Common.IsNullOrEmptyList(updateList))
                {
                    foreach (PMS.Model.DTO.Appraisal.PerformanceCoachingComment obj_comment in updateList)
                    {
                        PMS.Model.Context.PMS_APPRAISAL_PERFORMANCE_COACHING_COMMENT ent_comment = dc_pms.PMS_APPRAISAL_PERFORMANCE_COACHING_COMMENT.Where(rec => rec.ITEM_ID == obj_comment.AppraisalPerformanceCoaching.Id &&
                                                                                                                                rec.COMMENTED_BY_ID == obj_comment.Commentor.Id &&
                                                                                                                                rec.FORM_SAVE_ONLY == true).SingleOrDefault();



                        if (ent_comment != null)
                        {
                            ent_comment.COMMENT = obj_comment.Comments;
                            ent_comment.COMMENTED_TIMESTAMP = obj_comment.CommentedTimestamp;
                        }
                        else
                        {
                            ent_comment = Mappers.PMSMapper.MapAppraisalPerformanceCoachingCommentDTOToEntity(obj_comment);
                            dc_pms.PMS_APPRAISAL_PERFORMANCE_COACHING_COMMENT.AddObject(ent_comment);
                        }


                    }
                    dc_pms.SaveChanges();
                    boo_success = true;
                }
            }
            catch (Exception exc)
            {
                message = exc.Message;
            }
            finally
            {
                dc_pms.Dispose();
            }

            return boo_success;
        }

        #endregion Performance Coaching

        #region Career Development

        public static bool UpdateAppraisalCareerDevelopmentComment(List<PMS.Model.DTO.Appraisal.CareerDevelopmentComment> updateList, out string message)
        {
            bool boo_success = false;
            message = string.Empty;
            PMS.Model.Context.PMSEntities dc_pms = new PMS.Model.Context.PMSEntities();

            try
            {
                if (!Lib.Utility.Common.IsNullOrEmptyList(updateList))
                {
                    foreach (PMS.Model.DTO.Appraisal.CareerDevelopmentComment obj_comment in updateList)
                    {
                        PMS.Model.Context.PMS_APPRAISAL_CAREER_DEVELOPMENT_COMMENT ent_comment = dc_pms.PMS_APPRAISAL_CAREER_DEVELOPMENT_COMMENT.Where(rec => rec.ITEM_ID == obj_comment.AppraisalCareerDevelopment.Id &&
                                                                                                                                rec.COMMENTED_BY_ID == obj_comment.Commentor.Id &&
                                                                                                                                rec.FORM_SAVE_ONLY == true).SingleOrDefault();



                        if (ent_comment != null)
                        {
                            ent_comment.COMMENT = obj_comment.Comments;
                            ent_comment.COMMENTED_TIMESTAMP = obj_comment.CommentedTimestamp;
                        }
                        else
                        {
                            ent_comment = Mappers.PMSMapper.MapAppraisalCareerDevelopmentCommentDTOToEntity(obj_comment);
                            dc_pms.PMS_APPRAISAL_CAREER_DEVELOPMENT_COMMENT.AddObject(ent_comment);
                        }
                    }
                    dc_pms.SaveChanges();
                    boo_success = true;
                }
            }
            catch (Exception exc)
            {
                message = exc.Message;
            }
            finally
            {
                dc_pms.Dispose();
            }

            return boo_success;
        }


        #endregion Career Development

        public static bool UpdateAppraisalCareerDevelopment(PMS.Model.DTO.Appraisal.CareerDevelopment careerDevelopment, out string message)
        {
            bool boo_success = false;
            message = string.Empty;
            Model.Context.PMS_APPRAISAL_CAREER_DEVELOPMENT entity;
            PMS.Model.Context.PMSEntities dc_pms = new PMS.Model.Context.PMSEntities();

            try
            {
                if (careerDevelopment != null)
                {
                    entity = dc_pms.PMS_APPRAISAL_CAREER_DEVELOPMENT.Where(rec => rec.APPRAISAL_ID == careerDevelopment.Appraisal.Id).SingleOrDefault();
                    if (entity == null)
                    {
                        entity = Mappers.PMSMapper.MapAppraisalCareerDevelopmentDTOToEntity(careerDevelopment);
                        dc_pms.PMS_APPRAISAL_CAREER_DEVELOPMENT.AddObject(entity);
                    }
                    else
                    {
                        entity.SHORT_TERM_GOALS = careerDevelopment.ShortTermGoals;
                        entity.LEARNING_PLAN = careerDevelopment.LearningPlans;
                        entity.CAREER_PLAN = careerDevelopment.CareerPlans;
                    }

                    dc_pms.SaveChanges();
                    boo_success = true;
                }
            }
            catch (Exception exc)
            {
                message = exc.Message;
            }
            finally
            {
                dc_pms.Dispose();
            }

            return boo_success;
        }

        #endregion

        #region Approver

        public static List<PMS.Model.DTO.Appraisal.Approver> GetApproversByAppraisalId(int appraisalId)
        {
            PMS.Model.Context.PMSEntities dc_pms = new PMS.Model.Context.PMSEntities();
            List<PMS.Model.DTO.Appraisal.Approver> lst_approvers = null;
            System.Data.Objects.ObjectQuery<PMS.Model.Context.PMS_APPRAISAL_APPROVER> entities;

            dc_pms.ContextOptions.LazyLoadingEnabled = false;

            entities = ((from ent_approvers in dc_pms.PMS_APPRAISAL_APPROVER
                         where ent_approvers.APPRAISAL_ID == appraisalId
                         select ent_approvers) as System.Data.Objects.ObjectQuery<PMS.Model.Context.PMS_APPRAISAL_APPROVER>)
                            .Include("EMPLOYEE");

            if (!Lib.Utility.Common.IsNullOrEmptyList(entities))
            {
                lst_approvers = PMS.Model.Mappers.PMSMapper.MapApproverEntitiesToDTOs(entities.ToList());
            }
            return lst_approvers;
        }

        public static bool UpdateApproversAndTasks(PMS.Model.DTO.Appraisal.Appraisal appraisal, List<PMS.Model.DTO.Appraisal.Approver> approvers, List<PMS.Model.DTO.Core.Task.Owner> taskOwners, out string message)
        {
            bool boo_success = false;
            message = string.Empty;
            PMS.Model.Context.PMSEntities dc_pms = new PMS.Model.Context.PMSEntities();

            try
            {
                if (!Lib.Utility.Common.IsNullOrEmptyList(approvers))
                {

                    IEnumerable<Model.Context.PMS_APPRAISAL_APPROVER> lst_approver_entities = from ent_approvers in dc_pms.PMS_APPRAISAL_APPROVER
                                                                                              where ent_approvers.APPRAISAL_ID == appraisal.Id
                                                                                              select ent_approvers;

                    foreach (Model.Context.PMS_APPRAISAL_APPROVER ent_approver in lst_approver_entities)
                    {
                        foreach (PMS.Model.DTO.Appraisal.Approver obj_approver in approvers)
                        {
                            if (ent_approver.ID == obj_approver.Id)
                            {
                                ent_approver.APPROVER_ID = obj_approver.EmployeeId;
                                ent_approver.APPROVAL_LEVEL = obj_approver.ApprovalLevel;
                            }
                        }
                    }


                    if (!Lib.Utility.Common.IsNullOrEmptyList(taskOwners))
                    {

                        List<Int32> lst_task_owner_ids = new List<Int32>();
                        foreach (PMS.Model.DTO.Core.Task.Owner obj_owners in taskOwners)
                        {
                            lst_task_owner_ids.Add(obj_owners.Id);
                        }

                        IEnumerable<Model.Context.TASK_OWNER> lst_owner_entities = dc_pms.TASK_OWNER.Where(rec => lst_task_owner_ids.Contains(rec.ID));

                        if (!Lib.Utility.Common.IsNullOrEmptyList(lst_owner_entities))
                        {
                            foreach (Model.Context.TASK_OWNER ent_owner in lst_owner_entities)
                            {
                                foreach (PMS.Model.DTO.Core.Task.Owner obj_owners in taskOwners)
                                {
                                    if (ent_owner.ID == obj_owners.Id)
                                    {
                                        ent_owner.EMPLOYEE_ID = obj_owners.EmployeeId;
                                    }
                                }
                            }
                        }
                    }
                    dc_pms.SaveChanges();
                }
                boo_success = true;
            }
            catch (Exception exc)
            {
                message = exc.Message;
            }
            finally
            {
                dc_pms.Dispose();
            }

            return boo_success;
        }

        #endregion

        #region Master

        public static List<PMS.Model.DTO.Master.Priority> GetMasterPriorityList(bool? active)
        {
            PMS.Model.Context.PMSEntities dc_pms = new PMS.Model.Context.PMSEntities();
            List<PMS.Model.DTO.Master.Priority> lst_priorities = null;
            System.Data.Objects.ObjectQuery<PMS.Model.Context.PMS_MST_PRIORITY> entities;

            dc_pms.ContextOptions.LazyLoadingEnabled = false;

            if (active == null)
            {
                entities = (from ent_priorities in dc_pms.PMS_MST_PRIORITY
                            select ent_priorities).OrderBy(so => so.SORT_ORDER) as System.Data.Objects.ObjectQuery<PMS.Model.Context.PMS_MST_PRIORITY>;
            }
            else
            {
                entities = (from ent_priorities in dc_pms.PMS_MST_PRIORITY
                            where ent_priorities.ACTIVE == active
                            select ent_priorities).OrderBy(so => so.SORT_ORDER) as System.Data.Objects.ObjectQuery<PMS.Model.Context.PMS_MST_PRIORITY>;
            }

            if (!Lib.Utility.Common.IsNullOrEmptyList(entities))
            {
                lst_priorities = Mappers.PMSMapper.MapPriorityEntitiesToDTOs(entities.ToList());
            }

            dc_pms.Dispose();
            return lst_priorities;
        }

        public static List<PMS.Model.DTO.Master.Section> GetMasterSectionList(bool? active)
        {
            PMS.Model.Context.PMSEntities dc_pms = new PMS.Model.Context.PMSEntities();
            List<PMS.Model.DTO.Master.Section> lst_sections = null;
            System.Data.Objects.ObjectQuery<PMS.Model.Context.PMS_MST_SECTION> entities;

            dc_pms.ContextOptions.LazyLoadingEnabled = false;

            if (active == null)
            {
                entities = ((from ent_sections in dc_pms.PMS_MST_SECTION
                             select ent_sections).OrderBy(so => so.SORT_ORDER) as System.Data.Objects.ObjectQuery<PMS.Model.Context.PMS_MST_SECTION>)
                                .Include("PMS_MST_BLOCK");
            }
            else
            {
                entities = ((from ent_sections in dc_pms.PMS_MST_SECTION
                             where ent_sections.ACTIVE == active
                             select ent_sections).OrderBy(so => so.SORT_ORDER) as System.Data.Objects.ObjectQuery<PMS.Model.Context.PMS_MST_SECTION>)
                                .Include("PMS_MST_BLOCK");
            }

            if (!Lib.Utility.Common.IsNullOrEmptyList(entities))
            {
                lst_sections = Mappers.PMSMapper.MapSectionEntitiesToDTOs(entities.ToList(), true);
            }

            dc_pms.Dispose();
            return lst_sections;
        }

        public static List<PMS.Model.DTO.Master.Stage> GetMasterStageList(bool? active)
        {
            PMS.Model.Context.PMSEntities dc_pms = new PMS.Model.Context.PMSEntities();
            List<PMS.Model.DTO.Master.Stage> lst_stages = null;
            System.Data.Objects.ObjectQuery<PMS.Model.Context.PMS_MST_STAGE> entities;

            dc_pms.ContextOptions.LazyLoadingEnabled = false;

            if (active == null)
            {
                entities = ((from ent_stages in dc_pms.PMS_MST_STAGE
                             select ent_stages).OrderBy(so => so.SORT_ORDER) as System.Data.Objects.ObjectQuery<PMS.Model.Context.PMS_MST_STAGE>);
            }
            else
            {
                entities = ((from ent_stages in dc_pms.PMS_MST_STAGE
                             where ent_stages.ACTIVE == active
                             select ent_stages).OrderBy(so => so.SORT_ORDER) as System.Data.Objects.ObjectQuery<PMS.Model.Context.PMS_MST_STAGE>);
            }

            if (!Lib.Utility.Common.IsNullOrEmptyList(entities))
            {
                lst_stages = Mappers.PMSMapper.MapStageEntitiesToDTOs(entities.ToList());
            }

            dc_pms.Dispose();
            return lst_stages;
        }

        public static PMS.Model.DTO.Master.Section GetSectionById(int sectionId)
        {
            PMS.Model.Context.PMSEntities dc_pms = new PMS.Model.Context.PMSEntities();
            PMS.Model.DTO.Master.Section obj_section = null;
            PMS.Model.Context.PMS_MST_SECTION entity;

            dc_pms.ContextOptions.LazyLoadingEnabled = false;

            entity = (from ent_sections in dc_pms.PMS_MST_SECTION
                        .Include("PMS_MST_BLOCK")
                      where ent_sections.ID == sectionId
                      select ent_sections).Single();

            if (entity != null)
            {
                obj_section = Mappers.PMSMapper.MapSectionEntityToDTO(entity, true);
            }
            dc_pms.Dispose();
            return obj_section;
        }

        #endregion

        #region For Batch/Scheduled Jobs

        public static bool AppraisalStageManager(List<DTO.Cycle.Cycle> cycles, List<DTO.Appraisal.Appraisal> appraisals, List<DTO.Core.Task.Task> newTasks, List<DTO.Core.Task.Task> deleteTasks)
        {
            bool boo_success = false;
            PMS.Model.Context.PMSEntities dc_pms = new PMS.Model.Context.PMSEntities();
            Model.Context.PMS_CYCLE ent_cycle;
            Model.Context.PMS_APPRAISAL ent_appraisal;

            if (!Lib.Utility.Common.IsNullOrEmptyList(cycles))
            {
                foreach (Model.DTO.Cycle.Cycle obj_cycle in cycles)
                {
                    ent_cycle = (from cyc in dc_pms.PMS_CYCLE
                                 where cyc.ID == obj_cycle.Id
                                 select cyc).Single();
                    ent_cycle.STAGE_ID = obj_cycle.Stage.Id;
                }
            }

            if (!Lib.Utility.Common.IsNullOrEmptyList(appraisals))
            {
                foreach (DTO.Appraisal.Appraisal obj_appraisal in appraisals)
                {
                    ent_appraisal = (from appr in dc_pms.PMS_APPRAISAL
                                     where appr.ID == obj_appraisal.Id
                                     select appr).Single();

                    ent_appraisal.STAGE_ID = obj_appraisal.Stage.Id;
                    ent_appraisal.STATUS_ID = obj_appraisal.Status.Id;
                    ent_appraisal.LOCKED = obj_appraisal.Locked;

                    if (!Lib.Utility.Common.IsNullOrEmptyList(obj_appraisal.Trails))
                    {
                        foreach (Model.DTO.Appraisal.Trail obj_trail in obj_appraisal.Trails)
                        {
                            if (obj_trail.Id == 0)
                            {
                                dc_pms.PMS_APPRAISAL_TRAIL.AddObject(Mappers.PMSMapper.MapAppraisalTrailDTOToEntity(obj_trail));
                            }
                        }
                    }
                }
            }
            dc_pms.SaveChanges();

            if (!Lib.Utility.Common.IsNullOrEmptyList(newTasks))
            {
                foreach (DTO.Core.Task.Task obj_task in newTasks)
                {
                    Model.Context.TASK ent_task = Mappers.CoreMapper.MapTaskDTOToEntity(obj_task);
                    dc_pms.TASKs.AddObject(ent_task);
                    dc_pms.SaveChanges();

                    foreach (DTO.Core.Task.Owner obj_owner in obj_task.Owners)
                    {
                        Model.Context.TASK_OWNER ent_owner = Mappers.CoreMapper.MapTaskOwnerDTOToEntity(obj_owner);
                        ent_owner.TASK_ID = ent_task.ID;
                        dc_pms.TASK_OWNER.AddObject(ent_owner);
                    }
                }
                dc_pms.SaveChanges();
            }

            if (!Lib.Utility.Common.IsNullOrEmptyList(deleteTasks))
            {
                foreach (DTO.Core.Task.Task obj_task in deleteTasks)
                {
                    dc_pms.DeleteObject(Mappers.CoreMapper.MapTaskDTOToEntity(obj_task));
                }
                dc_pms.SaveChanges();
            }

            boo_success = true;
            return boo_success;
        }

        #endregion
    }
}
