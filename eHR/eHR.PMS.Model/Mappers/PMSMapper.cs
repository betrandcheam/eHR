using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eHR.PMS.Model.Mappers
{
    public class PMSMapper
    {

        #region Cycle

        public static PMS.Model.DTO.Cycle.Cycle MapCycleEntityToDTO(PMS.Model.Context.PMS_CYCLE entity, bool mapChildEntities)
        {
            PMS.Model.DTO.Cycle.Cycle obj_dto = new PMS.Model.DTO.Cycle.Cycle()
            {
                Id = entity.ID,
                Name = entity.NAME,
                Status = entity.PMS_MST_STATUS == null ? null : MapStatusEntityToDTO(entity.PMS_MST_STATUS),
                Stage = entity.PMS_MST_STAGE == null ? null : MapStageEntityToDTO(entity.PMS_MST_STAGE),
                StartedTimestamp = entity.STARTED_TIMESTAMP
            };

            if (mapChildEntities)
            {
                obj_dto.CycleStages = Lib.Utility.Common.IsNullOrEmptyList(entity.PMS_CYCLE_STAGE) == true ? null : MapCycleStageEntitiesToDTOs(entity.PMS_CYCLE_STAGE.ToList());
                obj_dto.Appriasals = Lib.Utility.Common.IsNullOrEmptyList(entity.PMS_APPRAISAL) == true ? null : MapAppraisalEntitiesToDTOs(entity.PMS_APPRAISAL.ToList(), true);
            }

            return obj_dto;
        }

        public static List<PMS.Model.DTO.Cycle.Cycle> MapCycleEntitiesToDTOs(List<PMS.Model.Context.PMS_CYCLE> entities, bool mapChildEntities)
        {
            List<PMS.Model.DTO.Cycle.Cycle> lst_dtos = new List<PMS.Model.DTO.Cycle.Cycle>();
            foreach (PMS.Model.Context.PMS_CYCLE entity in entities)
            {
                lst_dtos.Add(MapCycleEntityToDTO(entity, mapChildEntities));
            }
            return lst_dtos;
        }

        public static List<PMS.Model.DTO.Cycle.Stage> MapCycleStageEntitiesToDTOs(List<PMS.Model.Context.PMS_CYCLE_STAGE> entities) 
        {
            List<PMS.Model.DTO.Cycle.Stage> lst_dtos = new List<PMS.Model.DTO.Cycle.Stage>();
            foreach (PMS.Model.Context.PMS_CYCLE_STAGE entity in entities)
            {
                lst_dtos.Add(MapCycleStageEntityToDTO(entity));
            }
            return lst_dtos;
        }

        public static PMS.Model.Context.PMS_CYCLE MapCycleDTOToEntity(PMS.Model.DTO.Cycle.Cycle dto, bool mapChildren)
        {
            PMS.Model.Context.PMS_CYCLE obj_entity = new PMS.Model.Context.PMS_CYCLE() 
            { 
                ID = dto.Id,
                NAME = dto.Name,
                STATUS_ID = dto.Status.Id,
                STAGE_ID = dto.Stage.Id,
                STARTED_BY_ID = dto.Starter.Id,
                STARTED_TIMESTAMP = dto.StartedTimestamp
            };

            if (mapChildren)
            {
                obj_entity.PMS_CYCLE_STAGE = Lib.Utility.Common.IsNullOrEmptyList(dto.CycleStages) == true ? null : MapCycleStageDTOsToEntities(dto.CycleStages);
            }

            // cycle stage
            // 
            // appraisal

            return obj_entity;
        }

        public static System.Data.Objects.DataClasses.EntityCollection<PMS.Model.Context.PMS_CYCLE_STAGE> MapCycleStageDTOsToEntities(List<PMS.Model.DTO.Cycle.Stage> dto)
        {
            System.Data.Objects.DataClasses.EntityCollection<PMS.Model.Context.PMS_CYCLE_STAGE> lst_entities = new System.Data.Objects.DataClasses.EntityCollection<Context.PMS_CYCLE_STAGE>();
            foreach (PMS.Model.DTO.Cycle.Stage obj_entity in dto)
            {
                lst_entities.Add(MapCycleStageDTOToEntity(obj_entity));
            }
            return lst_entities;
        
        }

        public static PMS.Model.Context.PMS_CYCLE_STAGE MapCycleStageDTOToEntity(PMS.Model.DTO.Cycle.Stage dto)
        {
            PMS.Model.Context.PMS_CYCLE_STAGE obj_entity = new Context.PMS_CYCLE_STAGE()
            {
                ID = dto.Id,
                CYCLE_ID = dto.Cycle == null ? (int?)null : dto.Cycle.Id,
                STAGE_ID = dto.StageId,
                START_DATE = dto.StartDate,
                END_DATE = dto.EndDate,
                PRE_START_EMAIL_SENT = dto.PreStartEmailSent,
                SUBMISSION_DEADLINE = dto.SubmissionDeadline,
                LEVEL_1_APPROVAL_DEADLINE = dto.Level1ApprovalDeadline,
                LEVEL_2_APPROVAL_DEADLINE = dto.Level2ApprovalDeadline

            };
            return obj_entity;
        }

        public static PMS.Model.DTO.Cycle.Stage MapCycleStageEntityToDTO(PMS.Model.Context.PMS_CYCLE_STAGE entity)
        {
            PMS.Model.DTO.Cycle.Stage obj_dto = new PMS.Model.DTO.Cycle.Stage()
            {
                Id = entity.ID,
                StageId = entity.PMS_MST_STAGE.ID,
                Description = entity.PMS_MST_STAGE.DESCRIPTION,
                Active = entity.PMS_MST_STAGE.ACTIVE,
                Name = entity.PMS_MST_STAGE.NAME,
                SortOrder = entity.PMS_MST_STAGE.SORT_ORDER,
                StartDate = entity.START_DATE,
                EndDate = entity.END_DATE,
                PreStartEmailSent = entity.PRE_START_EMAIL_SENT,
                SubmissionDeadline = entity.SUBMISSION_DEADLINE,
                Level1ApprovalDeadline = entity.LEVEL_1_APPROVAL_DEADLINE,
                Level2ApprovalDeadline = entity.LEVEL_2_APPROVAL_DEADLINE
            };

            obj_dto.Cycle = entity.PMS_CYCLE == null ? null : MapCycleEntityToDTO(entity.PMS_CYCLE, false);

            return obj_dto;
        }

        #endregion Cycle

        #region Appraisal

        public static PMS.Model.DTO.Appraisal.Appraisal MapAppraisalEntityToDTO(PMS.Model.Context.PMS_APPRAISAL entity, bool mapChildEntities)
        {
            PMS.Model.DTO.Appraisal.Appraisal obj_dto = new PMS.Model.DTO.Appraisal.Appraisal()
            {
                Id = entity.ID,
                Status = entity.PMS_MST_STATUS == null ? null : MapStatusEntityToDTO(entity.PMS_MST_STATUS),
                Stage = entity.PMS_MST_STAGE == null ? null : MapStageEntityToDTO(entity.PMS_MST_STAGE),
                Department = entity.MST_DEPARTMENT == null ? null : CoreMapper.MapDepartmentEntityToDTO(entity.MST_DEPARTMENT),
                Employee = entity.EMPLOYEE == null ? null : CoreMapper.MapEmployeeEntityToDTO(entity.EMPLOYEE),
                Locked = entity.LOCKED
            };

            obj_dto.Cycle = entity.PMS_CYCLE == null ? null : MapCycleEntityToDTO(entity.PMS_CYCLE, false);

            if (mapChildEntities)
            {
                obj_dto.Approvers = Lib.Utility.Common.IsNullOrEmptyList(entity.PMS_APPRAISAL_APPROVER) == true ? null : MapApproverEntitiesToDTOs(entity.PMS_APPRAISAL_APPROVER.ToList());
                obj_dto.Reviewers = Lib.Utility.Common.IsNullOrEmptyList(entity.PMS_APPRAISAL_REVIEWER) == true ? null : MapReviewerEntitiesToDTOs(entity.PMS_APPRAISAL_REVIEWER.ToList());
                obj_dto.AppraisalStages = Lib.Utility.Common.IsNullOrEmptyList(entity.PMS_APPRAISAL_STAGE) == true ? null : MapAppraisalStageEntitiesToDTOs(entity.PMS_APPRAISAL_STAGE.ToList());
                obj_dto.AppraisalSections = Lib.Utility.Common.IsNullOrEmptyList(entity.PMS_APPRAISAL_SECTION) == true ? null : MapAppraisalSectionEntitiesToDTOs(entity.PMS_APPRAISAL_SECTION.ToList(), true);
                obj_dto.KPIs = Lib.Utility.Common.IsNullOrEmptyList(entity.PMS_APPRAISAL_KPI) == true ? null : MapAppraisalKPIEntitiesToDTOs(entity.PMS_APPRAISAL_KPI.ToList(), true);
                obj_dto.CoreValues = Lib.Utility.Common.IsNullOrEmptyList(entity.PMS_APPRAISAL_CORE_VALUE) == true ? null : MapAppraisalCoreValueEntitiesToDTOs(entity.PMS_APPRAISAL_CORE_VALUE.ToList(), true);
                obj_dto.PerformanceCoachings = Lib.Utility.Common.IsNullOrEmptyList(entity.PMS_APPRAISAL_PERFORMANCE_COACHING) == true ? null : MapAppraisalPerformanceCoachingEntitiesToDTOs(entity.PMS_APPRAISAL_PERFORMANCE_COACHING.ToList(), true);
                obj_dto.CareerDevelopments = Lib.Utility.Common.IsNullOrEmptyList(entity.PMS_APPRAISAL_CAREER_DEVELOPMENT) == true ? null : MapAppraisalCareerDevelopmentEntitiesToDTOs(entity.PMS_APPRAISAL_CAREER_DEVELOPMENT.ToList(), true);
                obj_dto.Trails = Lib.Utility.Common.IsNullOrEmptyList(entity.PMS_APPRAISAL_TRAIL) == true ? null : MapAppraisalTrailEntitiesToDTOs(entity.PMS_APPRAISAL_TRAIL.ToList());
            }
            return obj_dto;
        }

        public static List<PMS.Model.DTO.Appraisal.Appraisal> MapAppraisalEntitiesToDTOs(List<PMS.Model.Context.PMS_APPRAISAL> entities, bool mapChildEntities)
        {
            List<PMS.Model.DTO.Appraisal.Appraisal> lst_dtos = new List<PMS.Model.DTO.Appraisal.Appraisal>();
            foreach (PMS.Model.Context.PMS_APPRAISAL entity in entities)
            {
                lst_dtos.Add(MapAppraisalEntityToDTO(entity, mapChildEntities));
            }
            return lst_dtos;
        }

        public static PMS.Model.Context.PMS_APPRAISAL MapAppraisalDTOToEntity(PMS.Model.DTO.Appraisal.Appraisal dto)
        {
            PMS.Model.Context.PMS_APPRAISAL obj_entity = new Context.PMS_APPRAISAL() 
            { 
                ID = dto.Id,
                CYCLE_ID = dto.Cycle == null ? (int?)null : dto.Cycle.Id,
                STAGE_ID = dto.Stage == null ? (int?)null : dto.Stage.Id,
                STATUS_ID = dto.Status == null ? (int?)null : dto.Status.Id,
                EMPLOYEE_ID = dto.Employee == null ? (int?)null : dto.Employee.Id,
                EMPLOYEE_DEPARTMENT_ID = dto.Department.Id,
                LOCKED = dto.Locked
            };
            return obj_entity;
        }

        #endregion Appraisal

        #region Appraisal Stage

        public static PMS.Model.DTO.Appraisal.Stage MapAppraisalStageEntityToDTO(PMS.Model.Context.PMS_APPRAISAL_STAGE entity)
        {
            PMS.Model.DTO.Appraisal.Stage obj_dto = new PMS.Model.DTO.Appraisal.Stage()
            {
                Id = entity.ID,
                StageId = entity.PMS_MST_STAGE.ID,
                Description = entity.PMS_MST_STAGE.DESCRIPTION,
                Active = entity.PMS_MST_STAGE.ACTIVE,
                Name = entity.PMS_MST_STAGE.NAME,
                SortOrder = entity.PMS_MST_STAGE.SORT_ORDER,
                StartDate = entity.START_DATE,
                EndDate = entity.END_DATE,
                SubmissionDeadline = entity.SUBMISSION_DEADLINE,
                Level1ApprovalDeadline = entity.LEVEL_1_APPROVAL_DEADLINE,
                Level2ApprovalDeadline = entity.LEVEL_2_APPROVAL_DEADLINE
            };

            obj_dto.Appraisal = entity.PMS_APPRAISAL == null ? null : MapAppraisalEntityToDTO(entity.PMS_APPRAISAL, false);

            return obj_dto;
        }

        public static List<PMS.Model.DTO.Appraisal.Stage> MapAppraisalStageEntitiesToDTOs(List<PMS.Model.Context.PMS_APPRAISAL_STAGE> entities)
        {
            List<PMS.Model.DTO.Appraisal.Stage> lst_dtos = new List<PMS.Model.DTO.Appraisal.Stage>();
            foreach (PMS.Model.Context.PMS_APPRAISAL_STAGE entity in entities)
            {
                lst_dtos.Add(MapAppraisalStageEntityToDTO(entity));
            }
            return lst_dtos;
        }

        public static PMS.Model.Context.PMS_APPRAISAL_STAGE MapAppraisalStageDTOToEntity(PMS.Model.DTO.Appraisal.Stage dto)
        {
            PMS.Model.Context.PMS_APPRAISAL_STAGE obj_entity = new Context.PMS_APPRAISAL_STAGE()
            {
                ID = dto.Id,
                APPRAISAL_ID = dto.Appraisal == null ? (int?)null : dto.Appraisal.Id,
                STAGE_ID = dto.StageId,
                START_DATE = dto.StartDate,
                END_DATE = dto.EndDate,
                SUBMISSION_DEADLINE = dto.SubmissionDeadline,
                LEVEL_1_APPROVAL_DEADLINE = dto.Level1ApprovalDeadline,
                LEVEL_2_APPROVAL_DEADLINE = dto.Level2ApprovalDeadline
            };
            return obj_entity;
        }

        #endregion Appraisal Stage

        #region Appraisal Section

        public static PMS.Model.DTO.Appraisal.Section MapAppraisalSectionEntityToDTO(PMS.Model.Context.PMS_APPRAISAL_SECTION entity, bool mapChildEntities)
        {
            PMS.Model.DTO.Appraisal.Section obj_dto = new PMS.Model.DTO.Appraisal.Section()
            {
                Id = entity.ID,
                SectionId = entity.PMS_MST_SECTION.ID,
                SelfScore = entity.SELF_SCORE,
                Level1Score = entity.LEVEL_1_SCORE,
                Level2Score = entity.LEVEL_2_SCORE,
                FinalScore = entity.FINAL_SCORE,
                Description = entity.PMS_MST_SECTION.DESCRIPTION,
                Active = entity.PMS_MST_SECTION.ACTIVE,
                Name = entity.PMS_MST_SECTION.NAME
            };

            obj_dto.Appraisal = entity.PMS_APPRAISAL == null ? null : MapAppraisalEntityToDTO(entity.PMS_APPRAISAL, false);

            if (mapChildEntities)
            {
                obj_dto.Comments = Lib.Utility.Common.IsNullOrEmptyList(entity.PMS_APPRAISAL_SECTION_COMMENT) == true ? null : MapAppraisalSectionCommentEntitiesToDTOs(entity.PMS_APPRAISAL_SECTION_COMMENT.ToList());
            }

            return obj_dto;
        }

        public static List<PMS.Model.DTO.Appraisal.Section> MapAppraisalSectionEntitiesToDTOs(List<PMS.Model.Context.PMS_APPRAISAL_SECTION> entities, bool mapChildEntities)
        {
            List<PMS.Model.DTO.Appraisal.Section> lst_dtos = new List<PMS.Model.DTO.Appraisal.Section>();
            foreach (PMS.Model.Context.PMS_APPRAISAL_SECTION entity in entities)
            {
                lst_dtos.Add(MapAppraisalSectionEntityToDTO(entity, mapChildEntities));
            }
            return lst_dtos;
        }

        public static PMS.Model.Context.PMS_APPRAISAL_SECTION MapAppraisalSectionDTOToEntity(PMS.Model.DTO.Appraisal.Section dto)
        {
            PMS.Model.Context.PMS_APPRAISAL_SECTION obj_entity = new Context.PMS_APPRAISAL_SECTION()
            {
                ID = dto.Id,
                APPRAISAL_ID = dto.Appraisal == null ? (int?)null : dto.Appraisal.Id,
                SECTION_MASTER_ID = dto.SectionId,
                SELF_SCORE = dto.SelfScore,
                LEVEL_1_SCORE = dto.Level1Score,
                LEVEL_2_SCORE = dto.Level2Score,
                FINAL_SCORE = dto.FinalScore
            };
            return obj_entity;
        }

        public static PMS.Model.DTO.Appraisal.SectionComment MapAppraisalSectionCommentEntityToDTO(PMS.Model.Context.PMS_APPRAISAL_SECTION_COMMENT entity)
        {
            PMS.Model.DTO.Appraisal.SectionComment obj_dto = new PMS.Model.DTO.Appraisal.SectionComment()
            {
                Id = entity.ID,
               // Stage = MapStageEntityToDTO(entity.PMS_MST_STAGE),
                Comments = entity.COMMENTS,
                //Commentor = CoreMapper.MapEmployeeEntityToDTO(entity.EMPLOYEE),
                CommentedTimestamp = entity.COMMENTED_TIMESTAMP
            };

            obj_dto.AppraisalSection = entity.PMS_APPRAISAL_SECTION == null ? null : MapAppraisalSectionEntityToDTO(entity.PMS_APPRAISAL_SECTION, false);

            return obj_dto;
        }

        public static List<PMS.Model.DTO.Appraisal.SectionComment> MapAppraisalSectionCommentEntitiesToDTOs(List<PMS.Model.Context.PMS_APPRAISAL_SECTION_COMMENT> entities)
        {
            List<PMS.Model.DTO.Appraisal.SectionComment> lst_dtos = new List<PMS.Model.DTO.Appraisal.SectionComment>();
            foreach (PMS.Model.Context.PMS_APPRAISAL_SECTION_COMMENT entity in entities)
            {
                lst_dtos.Add(MapAppraisalSectionCommentEntityToDTO(entity));
            }
            return lst_dtos;
        }

        #endregion Appraisal Section

        #region Appraisal KPI

        public static PMS.Model.DTO.Appraisal.KPI MapAppraisalKPIEntityToDTO(PMS.Model.Context.PMS_APPRAISAL_KPI entity, bool mapChildEntities)
        {
            PMS.Model.DTO.Appraisal.KPI obj_dto = new PMS.Model.DTO.Appraisal.KPI()
            {
                Id = entity.ID,
                //Section = MapSectionEntityToDTO(entity.PMS_MST_SECTION, true),
                Block = MapBlockEntityToDTO(entity.PMS_MST_BLOCK),
                Description = entity.DESCRIPTION,
                Target = entity.TARGET,
                Priority = MapPriorityEntityToDTO(entity.PMS_MST_PRIORITY),
                Progress = entity.PROGRESS_UPDATE,
                SelfScore = entity.SELF_SCORE,
                Level1Score = entity.LEVEL_1_SCORE,
                Level2Score = entity.LEVEL_2_SCORE,
                FinalScore = entity.FINAL_SCORE
            };

            obj_dto.Appraisal = entity.PMS_APPRAISAL == null ? null : MapAppraisalEntityToDTO(entity.PMS_APPRAISAL, false);

            if (mapChildEntities)
            {
                obj_dto.Comments = Lib.Utility.Common.IsNullOrEmptyList(entity.PMS_APPRAISAL_KPI_COMMENT) == true ? null : MapAppraisalKPICommentEntitiesToDTOs(entity.PMS_APPRAISAL_KPI_COMMENT.ToList());
            }

            return obj_dto;
        }

        public static List<PMS.Model.DTO.Appraisal.KPI> MapAppraisalKPIEntitiesToDTOs(List<PMS.Model.Context.PMS_APPRAISAL_KPI> entities, bool mapChildEntities)
        {
            List<PMS.Model.DTO.Appraisal.KPI> lst_dtos = new List<PMS.Model.DTO.Appraisal.KPI>();
            foreach (PMS.Model.Context.PMS_APPRAISAL_KPI entity in entities)
            {
                lst_dtos.Add(MapAppraisalKPIEntityToDTO(entity, mapChildEntities));
            }
            return lst_dtos;
        }

        public static PMS.Model.DTO.Appraisal.KPIComment MapAppraisalKPICommentEntityToDTO(PMS.Model.Context.PMS_APPRAISAL_KPI_COMMENT entity)
        {
            PMS.Model.DTO.Appraisal.KPIComment obj_dto = new PMS.Model.DTO.Appraisal.KPIComment()
            {
                Id = entity.ID,
                Comments = entity.COMMENT,
                FormSaveOnly = entity.FORM_SAVE_ONLY,
                Commentor = CoreMapper.MapEmployeeEntityToDTO(entity.EMPLOYEE),
                CommentedTimestamp = entity.COMMENTED_TIMESTAMP
            };

            obj_dto.AppraisalKPI = entity.PMS_APPRAISAL_KPI == null ? null : MapAppraisalKPIEntityToDTO(entity.PMS_APPRAISAL_KPI, false);

            return obj_dto;
        }

        public static List<PMS.Model.DTO.Appraisal.KPIComment> MapAppraisalKPICommentEntitiesToDTOs(List<PMS.Model.Context.PMS_APPRAISAL_KPI_COMMENT> entities)
        {
            List<PMS.Model.DTO.Appraisal.KPIComment> lst_dtos = new List<PMS.Model.DTO.Appraisal.KPIComment>();
            foreach (PMS.Model.Context.PMS_APPRAISAL_KPI_COMMENT entity in entities)
            {
                lst_dtos.Add(MapAppraisalKPICommentEntityToDTO(entity));
            }
            return lst_dtos;
        }

        public static PMS.Model.Context.PMS_APPRAISAL_KPI MapAppraisalKPIDTOToEntity(PMS.Model.DTO.Appraisal.KPI dto)
        {
            PMS.Model.Context.PMS_APPRAISAL_KPI obj_entity = new Context.PMS_APPRAISAL_KPI() 
            { 
                ID = dto.Id,
                APPRAISAL_ID = dto.Appraisal == null ? (int?)null : dto.Appraisal.Id,
                //SECTION_ID = dto.Section == null ? (int?)null : dto.Section.Id,
                BLOCK_MASTER_ID = dto.Block == null ? (int?)null : dto.Block.Id,
                DESCRIPTION = dto.Description,
                TARGET = dto.Target,
                PRIORITY_MASTER_ID = dto.Priority == null ? (int?)null : dto.Priority.Id,
                SELF_SCORE = dto.SelfScore,
                LEVEL_1_SCORE = dto.Level1Score,
                LEVEL_2_SCORE = dto.Level2Score,
                FINAL_SCORE = dto.FinalScore
            };
            return obj_entity;
        }

        public static PMS.Model.Context.PMS_APPRAISAL_KPI_COMMENT MapAppraisalKPICommentDTOToEntity(PMS.Model.DTO.Appraisal.KPIComment dto)
        {
            PMS.Model.Context.PMS_APPRAISAL_KPI_COMMENT obj_entity = new Context.PMS_APPRAISAL_KPI_COMMENT()
            {
                ID = dto.Id,
                ITEM_ID = dto.AppraisalKPI == null ? (int?)null : dto.AppraisalKPI.Id,
                COMMENT = dto.Comments,
                COMMENTED_BY_ID = dto.Commentor == null ? (int?)null : dto.Commentor.Id,
                COMMENTED_TIMESTAMP = dto.CommentedTimestamp,
                FORM_SAVE_ONLY = dto.FormSaveOnly
            };
            return obj_entity;
        }

        #endregion Appraisal KPI

        #region Appraisal Core Value

        public static List<PMS.Model.DTO.Appraisal.CoreValue> MapAppraisalCoreValueEntitiesToDTOs(List<PMS.Model.Context.PMS_APPRAISAL_CORE_VALUE> entities, bool mapChildEntities)
        {
            List<PMS.Model.DTO.Appraisal.CoreValue> lst_dtos = new List<PMS.Model.DTO.Appraisal.CoreValue>();
            foreach (PMS.Model.Context.PMS_APPRAISAL_CORE_VALUE entity in entities)
            {
                lst_dtos.Add(MapAppraisalCoreValueEntityToDTO(entity, mapChildEntities));
            }
            return lst_dtos;
        }

        public static PMS.Model.DTO.Appraisal.CoreValue MapAppraisalCoreValueEntityToDTO(PMS.Model.Context.PMS_APPRAISAL_CORE_VALUE entity, bool mapChildEntities)
        {
            PMS.Model.DTO.Appraisal.CoreValue obj_dto = new PMS.Model.DTO.Appraisal.CoreValue()
            {
                Id = entity.ID,
                //Section = MapSectionEntityToDTO(entity.PMS_MST_SECTION, true),
                Block = MapBlockEntityToDTO(entity.PMS_MST_BLOCK),
                //CoreValueCompetency = MapCoreValueCompetencyEntityToDTO(entity.PMS_MST_CORE_VALUE_COMPETENCY),
                Target = entity.TARGET,
                Progress = entity.PROGRESS_UPDATE,
                SelfScore = entity.SELF_SCORE,
                Level1Score = entity.LEVEL_1_SCORE,
                Level2Score = entity.LEVEL_2_SCORE,
                FinalScore = entity.FINAL_SCORE
            };

            obj_dto.Appraisal = entity.PMS_APPRAISAL == null ? null : MapAppraisalEntityToDTO(entity.PMS_APPRAISAL, false);

            if (mapChildEntities)
            {
                obj_dto.Comments = Lib.Utility.Common.IsNullOrEmptyList(entity.PMS_APPRAISAL_CORE_VALUE_COMMENT) == true ? null : MapAppraisalCoreValueCommentEntitiesToDTOs(entity.PMS_APPRAISAL_CORE_VALUE_COMMENT.ToList());
            }

            return obj_dto;
        }

        public static List<PMS.Model.DTO.Appraisal.CoreValueComment> MapAppraisalCoreValueCommentEntitiesToDTOs(List<PMS.Model.Context.PMS_APPRAISAL_CORE_VALUE_COMMENT> entities)
        {
            List<PMS.Model.DTO.Appraisal.CoreValueComment> lst_dtos = new List<PMS.Model.DTO.Appraisal.CoreValueComment>();
            foreach (PMS.Model.Context.PMS_APPRAISAL_CORE_VALUE_COMMENT entity in entities)
            {
                lst_dtos.Add(MapAppraisalCoreValueCommentEntityToDTO(entity));
            }
            return lst_dtos;
        }

        public static PMS.Model.DTO.Appraisal.CoreValueComment MapAppraisalCoreValueCommentEntityToDTO(PMS.Model.Context.PMS_APPRAISAL_CORE_VALUE_COMMENT entity)
        {
            PMS.Model.DTO.Appraisal.CoreValueComment obj_dto = new PMS.Model.DTO.Appraisal.CoreValueComment()
            {
                Id = entity.ID,
                Comments = entity.COMMENT,
                Commentor = CoreMapper.MapEmployeeEntityToDTO(entity.EMPLOYEE),
                CommentedTimestamp = entity.COMMENTED_TIMESTAMP,
                FormSaveOnly = entity.FORM_SAVE_ONLY
            };

            obj_dto.AppraisalCoreValue = entity.PMS_APPRAISAL_CORE_VALUE == null ? null : MapAppraisalCoreValueEntityToDTO(entity.PMS_APPRAISAL_CORE_VALUE, false);

            return obj_dto;
        }

        public static PMS.Model.Context.PMS_APPRAISAL_CORE_VALUE MapAppraisalCoreValueDTOToEntity(PMS.Model.DTO.Appraisal.CoreValue dto)
        {
            PMS.Model.Context.PMS_APPRAISAL_CORE_VALUE obj_entity = new Context.PMS_APPRAISAL_CORE_VALUE()
            {
                ID = dto.Id,
                APPRAISAL_ID = dto.Appraisal == null ? (int?)null : dto.Appraisal.Id,
                //SECTION_ID = dto.Section == null ? (int?)null : dto.Section.Id,
                BLOCK_MASTER_ID = dto.Block == null ? (int?)null : dto.Block.Id,
                //COMPETENCY_MASTER_ID = dto.CoreValueCompetency == null ? (int?)null : dto.CoreValueCompetency.Id,
                TARGET = dto.Target,
                SELF_SCORE = dto.SelfScore,
                LEVEL_1_SCORE = dto.Level1Score,
                LEVEL_2_SCORE = dto.Level2Score,
                FINAL_SCORE = dto.FinalScore
            };
            return obj_entity;
        }

        public static PMS.Model.Context.PMS_APPRAISAL_CORE_VALUE_COMMENT MapAppraisalCoreValueCommentDTOToEntity(PMS.Model.DTO.Appraisal.CoreValueComment dto)
        {
            PMS.Model.Context.PMS_APPRAISAL_CORE_VALUE_COMMENT obj_entity = new Context.PMS_APPRAISAL_CORE_VALUE_COMMENT()
            {
                ID = dto.Id,
                ITEM_ID = dto.AppraisalCoreValue == null ? (int?)null : dto.AppraisalCoreValue.Id,
                COMMENT = dto.Comments,
                COMMENTED_BY_ID = dto.Commentor == null ? (int?)null : dto.Commentor.Id,
                COMMENTED_TIMESTAMP = dto.CommentedTimestamp,
                FORM_SAVE_ONLY = dto.FormSaveOnly
            };
            return obj_entity;
        }

        #endregion Appraisal Core Value

        #region Appraisal Performance Coaching

        public static List<PMS.Model.DTO.Appraisal.PerformanceCoaching> MapAppraisalPerformanceCoachingEntitiesToDTOs(List<PMS.Model.Context.PMS_APPRAISAL_PERFORMANCE_COACHING> entities, bool mapChildEntities)
        {
            List<PMS.Model.DTO.Appraisal.PerformanceCoaching> lst_dtos = new List<PMS.Model.DTO.Appraisal.PerformanceCoaching>();
            foreach (PMS.Model.Context.PMS_APPRAISAL_PERFORMANCE_COACHING entity in entities)
            {
                lst_dtos.Add(MapAppraisalPerformanceCoachingEntityToDTO(entity, mapChildEntities));
            }
            return lst_dtos;
        }

        public static PMS.Model.DTO.Appraisal.PerformanceCoaching MapAppraisalPerformanceCoachingEntityToDTO(PMS.Model.Context.PMS_APPRAISAL_PERFORMANCE_COACHING entity, bool mapChildEntities)
        {
            PMS.Model.DTO.Appraisal.PerformanceCoaching obj_dto = new PMS.Model.DTO.Appraisal.PerformanceCoaching()
            {
                Id = entity.ID,
                //Section = MapSectionEntityToDTO(entity.PMS_MST_SECTION, true),
                AreasOfImprovement = entity.IMPROVEMENT_AREA,
                AreasOfStrength = entity.STRENGTH_AREA,
                Progress=entity.PROGRESS_UPDATE
            };

            obj_dto.Appraisal = entity.PMS_APPRAISAL == null ? null : MapAppraisalEntityToDTO(entity.PMS_APPRAISAL, false);

            if (mapChildEntities)
            {
                obj_dto.Comments = Lib.Utility.Common.IsNullOrEmptyList(entity.PMS_APPRAISAL_PERFORMANCE_COACHING_COMMENT) == true ? null : MapAppraisalPerformanceCoachingCommentEntitiesToDTOs(entity.PMS_APPRAISAL_PERFORMANCE_COACHING_COMMENT.ToList());
            }

            return obj_dto;
        }

        public static List<PMS.Model.DTO.Appraisal.PerformanceCoachingComment> MapAppraisalPerformanceCoachingCommentEntitiesToDTOs(List<PMS.Model.Context.PMS_APPRAISAL_PERFORMANCE_COACHING_COMMENT> entities)
        {
            List<PMS.Model.DTO.Appraisal.PerformanceCoachingComment> lst_dtos = new List<PMS.Model.DTO.Appraisal.PerformanceCoachingComment>();
            foreach (PMS.Model.Context.PMS_APPRAISAL_PERFORMANCE_COACHING_COMMENT entity in entities)
            {
                lst_dtos.Add(MapAppraisalPerformanceCoachingCommentEntityToDTO(entity));
            }
            return lst_dtos;
        }

        public static PMS.Model.DTO.Appraisal.PerformanceCoachingComment MapAppraisalPerformanceCoachingCommentEntityToDTO(PMS.Model.Context.PMS_APPRAISAL_PERFORMANCE_COACHING_COMMENT entity)
        {
            PMS.Model.DTO.Appraisal.PerformanceCoachingComment obj_dto = new PMS.Model.DTO.Appraisal.PerformanceCoachingComment()
            {
                Id = entity.ID,
                ItemId = entity.ITEM_ID,
                Comments = entity.COMMENT,
                Commentor = CoreMapper.MapEmployeeEntityToDTO(entity.EMPLOYEE),
                CommentedTimestamp = entity.COMMENTED_TIMESTAMP,
                FormSaveOnly = entity.FORM_SAVE_ONLY
            };

            obj_dto.AppraisalPerformanceCoaching = entity.PMS_APPRAISAL_PERFORMANCE_COACHING == null ? null : MapAppraisalPerformanceCoachingEntityToDTO(entity.PMS_APPRAISAL_PERFORMANCE_COACHING, false);

            return obj_dto;
        }

        public static PMS.Model.Context.PMS_APPRAISAL_PERFORMANCE_COACHING MapAppraisalPerformanceCoachingDTOToEntity(PMS.Model.DTO.Appraisal.PerformanceCoaching dto)
        {
            PMS.Model.Context.PMS_APPRAISAL_PERFORMANCE_COACHING obj_entity = new Context.PMS_APPRAISAL_PERFORMANCE_COACHING()
            {
                ID = dto.Id,
                APPRAISAL_ID = dto.Appraisal == null ? (int?)null : dto.Appraisal.Id,
                ///SECTION_ID = dto.Section == null ? (int?)null : dto.Section.Id,
                IMPROVEMENT_AREA = dto.AreasOfImprovement,
                STRENGTH_AREA = dto.AreasOfStrength
            };
            return obj_entity;
        }

        public static PMS.Model.Context.PMS_APPRAISAL_PERFORMANCE_COACHING_COMMENT MapAppraisalPerformanceCoachingCommentDTOToEntity(PMS.Model.DTO.Appraisal.PerformanceCoachingComment dto)
        {
            PMS.Model.Context.PMS_APPRAISAL_PERFORMANCE_COACHING_COMMENT obj_entity = new Context.PMS_APPRAISAL_PERFORMANCE_COACHING_COMMENT()
            {
                ID = dto.Id,
                ITEM_ID = dto.AppraisalPerformanceCoaching == null ? (int?)null : dto.AppraisalPerformanceCoaching.Id,
                COMMENT = dto.Comments,
                COMMENTED_BY_ID = dto.Commentor == null ? (int?)null : dto.Commentor.Id,
                COMMENTED_TIMESTAMP = dto.CommentedTimestamp,
                FORM_SAVE_ONLY = dto.FormSaveOnly
            };
            return obj_entity;
        }

        #endregion Appraisal Performance Coaching

        #region Appraisal Learning Development

        public static List<PMS.Model.DTO.Appraisal.CareerDevelopment> MapAppraisalCareerDevelopmentEntitiesToDTOs(List<PMS.Model.Context.PMS_APPRAISAL_CAREER_DEVELOPMENT> entities, bool mapChildEntities)
        {
            List<PMS.Model.DTO.Appraisal.CareerDevelopment> lst_dtos = new List<PMS.Model.DTO.Appraisal.CareerDevelopment>();
            foreach (PMS.Model.Context.PMS_APPRAISAL_CAREER_DEVELOPMENT entity in entities)
            {
                lst_dtos.Add(MapAppraisalCareerDevelopmentEntityToDTO(entity, mapChildEntities));
            }
            return lst_dtos;
        }

        public static PMS.Model.DTO.Appraisal.CareerDevelopment MapAppraisalCareerDevelopmentEntityToDTO(PMS.Model.Context.PMS_APPRAISAL_CAREER_DEVELOPMENT entity, bool mapChildEntities)
        {
            PMS.Model.DTO.Appraisal.CareerDevelopment obj_dto = new PMS.Model.DTO.Appraisal.CareerDevelopment()
            {
                Id = entity.ID,
                Progress=entity.PROGRESS_UPDATE,
//Section = MapSectionEntityToDTO(entity.PMS_MST_SECTION, true),
                CareerPlans = entity.CAREER_PLAN,
                ShortTermGoals = entity.SHORT_TERM_GOALS,
                LearningPlans = entity.LEARNING_PLAN
            };

            obj_dto.Appraisal = entity.PMS_APPRAISAL == null ? null : MapAppraisalEntityToDTO(entity.PMS_APPRAISAL, false);

            if (mapChildEntities)
            {
                obj_dto.Comments = Lib.Utility.Common.IsNullOrEmptyList(entity.PMS_APPRAISAL_CAREER_DEVELOPMENT_COMMENT) == true ? null : MapAppraisalCareerDevelopmentCommentEntitiesToDTOs(entity.PMS_APPRAISAL_CAREER_DEVELOPMENT_COMMENT.ToList());
            }

            return obj_dto;
        }

        public static List<PMS.Model.DTO.Appraisal.CareerDevelopmentComment> MapAppraisalCareerDevelopmentCommentEntitiesToDTOs(List<PMS.Model.Context.PMS_APPRAISAL_CAREER_DEVELOPMENT_COMMENT> entities)
        {
            List<PMS.Model.DTO.Appraisal.CareerDevelopmentComment> lst_dtos = new List<PMS.Model.DTO.Appraisal.CareerDevelopmentComment>();
            foreach (PMS.Model.Context.PMS_APPRAISAL_CAREER_DEVELOPMENT_COMMENT entity in entities)
            {
                lst_dtos.Add(MapAppraisalCareerDevelopmentCommentEntityToDTO(entity));
            }
            return lst_dtos;
        }

        public static PMS.Model.DTO.Appraisal.CareerDevelopmentComment MapAppraisalCareerDevelopmentCommentEntityToDTO(PMS.Model.Context.PMS_APPRAISAL_CAREER_DEVELOPMENT_COMMENT entity)
        {
            PMS.Model.DTO.Appraisal.CareerDevelopmentComment obj_dto = new PMS.Model.DTO.Appraisal.CareerDevelopmentComment()
            {
                Id = entity.ID,
                ItemId = entity.ITEM_ID,
                Comments = entity.COMMENT,
                Commentor = CoreMapper.MapEmployeeEntityToDTO(entity.EMPLOYEE),
                CommentedTimestamp = entity.COMMENTED_TIMESTAMP,
                FormSaveOnly = entity.FORM_SAVE_ONLY
            };

            obj_dto.AppraisalCareerDevelopment = entity.PMS_APPRAISAL_CAREER_DEVELOPMENT == null ? null : MapAppraisalCareerDevelopmentEntityToDTO(entity.PMS_APPRAISAL_CAREER_DEVELOPMENT, false);

            return obj_dto;
        }

        public static PMS.Model.Context.PMS_APPRAISAL_CAREER_DEVELOPMENT MapAppraisalCareerDevelopmentDTOToEntity(PMS.Model.DTO.Appraisal.CareerDevelopment dto)
        {
            PMS.Model.Context.PMS_APPRAISAL_CAREER_DEVELOPMENT obj_entity = new Context.PMS_APPRAISAL_CAREER_DEVELOPMENT()
            {
                ID = dto.Id,
                APPRAISAL_ID = dto.Appraisal == null ? (int?)null : dto.Appraisal.Id,
                //SECTION_ID = dto.Section == null ? (int?)null : dto.Section.Id,
                CAREER_PLAN = dto.CareerPlans,
                SHORT_TERM_GOALS = dto.ShortTermGoals,
                LEARNING_PLAN = dto.LearningPlans
            };
            return obj_entity;
        }

        public static PMS.Model.Context.PMS_APPRAISAL_CAREER_DEVELOPMENT_COMMENT MapAppraisalCareerDevelopmentCommentDTOToEntity(PMS.Model.DTO.Appraisal.CareerDevelopmentComment dto)
        {
            PMS.Model.Context.PMS_APPRAISAL_CAREER_DEVELOPMENT_COMMENT obj_entity = new Context.PMS_APPRAISAL_CAREER_DEVELOPMENT_COMMENT()
            {
                ID = dto.Id,
                ITEM_ID = dto.AppraisalCareerDevelopment == null ? (int?)null : dto.AppraisalCareerDevelopment.Id,
                COMMENT = dto.Comments,
                COMMENTED_BY_ID = dto.Commentor == null ? (int?)null : dto.Commentor.Id,
                COMMENTED_TIMESTAMP = dto.CommentedTimestamp,
                FORM_SAVE_ONLY = dto.FormSaveOnly
            };
            return obj_entity;
        }

        #endregion Appraisal Learning Development

        #region Approver

        public static PMS.Model.DTO.Appraisal.Approver MapEmployeeDTOToApproverDTO(PMS.Model.DTO.Core.Employee employeeDTO, int approvalLevel)
        {
            PMS.Model.DTO.Appraisal.Approver obj_dto = new PMS.Model.DTO.Appraisal.Approver()
            {
                EmployeeId = employeeDTO.Id,
                FirstName = employeeDTO.FirstName,
                LastName = employeeDTO.LastName,
                PreferredName = employeeDTO.PreferredName,
                OfficeEmailAddress = employeeDTO.OfficeEmailAddress,
                ApprovalLevel = approvalLevel
            };
            return obj_dto;
        }
        public static PMS.Model.DTO.Appraisal.Reviewer MapEmployeeDTOToReviewerDTO(PMS.Model.DTO.Core.Employee employeeDTO)
        {
            PMS.Model.DTO.Appraisal.Reviewer obj_dto = new PMS.Model.DTO.Appraisal.Reviewer()
            {
                EmployeeId = employeeDTO.Id,
                FirstName = employeeDTO.FirstName,
                LastName = employeeDTO.LastName,
                PreferredName = employeeDTO.PreferredName,
                OfficeEmailAddress = employeeDTO.OfficeEmailAddress,
            };
            return obj_dto;
        }
        

        public static List<PMS.Model.DTO.Appraisal.Approver> MapApproverEntitiesToDTOs(List<PMS.Model.Context.PMS_APPRAISAL_APPROVER> entities)
        {
            List<PMS.Model.DTO.Appraisal.Approver> lst_dtos = new List<PMS.Model.DTO.Appraisal.Approver>();
            foreach (PMS.Model.Context.PMS_APPRAISAL_APPROVER entity in entities)
            {
                lst_dtos.Add(MapApproverEntityToDTO(entity));
            }
            return lst_dtos;
        }

        public static PMS.Model.DTO.Appraisal.Approver MapApproverEntityToDTO(PMS.Model.Context.PMS_APPRAISAL_APPROVER entity)
        {
            PMS.Model.DTO.Appraisal.Approver obj_dto = new PMS.Model.DTO.Appraisal.Approver()
            {
                Id = entity.ID,
                EmployeeId = entity.EMPLOYEE.ID,
                ApprovalLevel = entity.APPROVAL_LEVEL,
                FirstName = entity.EMPLOYEE.FIRST_NAME,
                LastName = entity.EMPLOYEE.LAST_NAME,
                PreferredName = entity.EMPLOYEE.PREFERRED_NAME,
                DomainId = entity.EMPLOYEE.DOMAIN_ID,
                OfficeEmailAddress = entity.EMPLOYEE.OFFICE_EMAIL_ADDRESS
            };

            obj_dto.Appraisal = entity.PMS_APPRAISAL == null ? null : MapAppraisalEntityToDTO(entity.PMS_APPRAISAL, false);

            return obj_dto;
        }

        public static PMS.Model.Context.PMS_APPRAISAL_APPROVER MapApproverDTOToEntity(PMS.Model.DTO.Appraisal.Approver dto)
        {
            PMS.Model.Context.PMS_APPRAISAL_APPROVER obj_entity = new Context.PMS_APPRAISAL_APPROVER()
            {
                ID = dto.Id,
                APPRAISAL_ID = dto.Appraisal == null ? (int?)null : dto.Appraisal.Id,
                APPROVER_ID = dto.EmployeeId,
                APPROVAL_LEVEL = dto.ApprovalLevel
            };
            return obj_entity;
        }

        #endregion Approver

        #region Reviewer

        public static List<PMS.Model.DTO.Appraisal.Reviewer> MapReviewerEntitiesToDTOs(List<PMS.Model.Context.PMS_APPRAISAL_REVIEWER> entities)
        {
            List<PMS.Model.DTO.Appraisal.Reviewer> lst_dtos = new List<PMS.Model.DTO.Appraisal.Reviewer>();
            foreach (PMS.Model.Context.PMS_APPRAISAL_REVIEWER entity in entities)
            {
                lst_dtos.Add(MapReviewerEntityToDTO(entity));
            }
            return lst_dtos;
        }

        public static PMS.Model.DTO.Appraisal.Reviewer MapReviewerEntityToDTO(PMS.Model.Context.PMS_APPRAISAL_REVIEWER entity)
        {
            PMS.Model.DTO.Appraisal.Reviewer obj_dto = new PMS.Model.DTO.Appraisal.Reviewer()
            {
                Id = entity.ID,
                EmployeeId = entity.EMPLOYEE.ID,
                FirstName = entity.EMPLOYEE.FIRST_NAME,
                LastName = entity.EMPLOYEE.LAST_NAME,
                PreferredName = entity.EMPLOYEE.PREFERRED_NAME,
                DomainId = entity.EMPLOYEE.DOMAIN_ID,
                OfficeEmailAddress=entity.EMPLOYEE.OFFICE_EMAIL_ADDRESS,
                SMT=entity.IS_SMT
            };

            obj_dto.Appraisal = entity.PMS_APPRAISAL == null ? null : MapAppraisalEntityToDTO(entity.PMS_APPRAISAL, false);

            return obj_dto;
        }

        public static PMS.Model.Context.PMS_APPRAISAL_REVIEWER MapReviewerDTOTOEntity(PMS.Model.DTO.Appraisal.Reviewer dto)
        {
            PMS.Model.Context.PMS_APPRAISAL_REVIEWER obj_entity = new PMS.Model.Context.PMS_APPRAISAL_REVIEWER()
            {
                ID = dto.Id,
                APPRAISAL_ID = dto.Appraisal.Id,
                REVIEWER_ID = dto.EmployeeId,
                IS_SMT=dto.SMT
            };
            return obj_entity;
        }

        #endregion Reviewer

        #region Grade Competency

        public static List<PMS.Model.DTO.GradeCompetency> MapGradeCompetencyEntitiesToDTOs(List<PMS.Model.Context.PMS_GRADE_CORE_VALUE_COMPETENCY> entities)
        {
            List<PMS.Model.DTO.GradeCompetency> lst_dtos = new List<PMS.Model.DTO.GradeCompetency>();
            foreach (PMS.Model.Context.PMS_GRADE_CORE_VALUE_COMPETENCY entity in entities)
            {
                lst_dtos.Add(MapGradeCompetencyEntityToDTO(entity));
            }
            return lst_dtos;
        }

        public static PMS.Model.DTO.GradeCompetency MapGradeCompetencyEntityToDTO(PMS.Model.Context.PMS_GRADE_CORE_VALUE_COMPETENCY entity)
        {
            PMS.Model.DTO.GradeCompetency obj_dto = new PMS.Model.DTO.GradeCompetency()
            {
                Id = entity.ID,
                //CoreValueCompetencyId = entity.COMPETENCY_MASTER_ID,
                Block = MapBlockEntityToDTO(entity.PMS_MST_BLOCK),
                Grade = CoreMapper.MapGradeEntityToDTO(entity.MST_ACR_GRADE),
                Name = entity.PMS_MST_CORE_VALUE_COMPETENCY.NAME,
                Description = entity.PMS_MST_CORE_VALUE_COMPETENCY.DESCRIPTION,
                Active = entity.PMS_MST_CORE_VALUE_COMPETENCY.ACTIVE
            };

            return obj_dto;
        }
        
        #endregion

        #region Trail

        public static List<PMS.Model.DTO.Appraisal.Trail> MapAppraisalTrailEntitiesToDTOs(List<PMS.Model.Context.PMS_APPRAISAL_TRAIL> entities)
        {
            List<PMS.Model.DTO.Appraisal.Trail> lst_dtos = new List<PMS.Model.DTO.Appraisal.Trail>();
            foreach (PMS.Model.Context.PMS_APPRAISAL_TRAIL entity in entities)
            {
                lst_dtos.Add(MapAppraisalTrailEntityToDTO(entity));
            }
            return lst_dtos;
        }

        public static PMS.Model.DTO.Appraisal.Trail MapAppraisalTrailEntityToDTO(PMS.Model.Context.PMS_APPRAISAL_TRAIL entity)
        {
            PMS.Model.DTO.Appraisal.Trail obj_dto = new PMS.Model.DTO.Appraisal.Trail()
            {
                Id = entity.ID,
                Stage = entity.PMS_MST_STAGE == null ? null : MapStageEntityToDTO(entity.PMS_MST_STAGE),
                Action = entity.PMS_MST_ACTION == null ? null : MapActionEntityToDTO(entity.PMS_MST_ACTION),
                ActionTimestamp = entity.ACTION_TIMESTAMP,
                Actioner = entity.ACTION_BY_ID > 0 ? new Model.DTO.Core.Employee(){ Id = Convert.ToInt32(entity.ACTION_BY_ID) } : null
            };

            obj_dto.Appraisal = entity.PMS_APPRAISAL == null ? null : MapAppraisalEntityToDTO(entity.PMS_APPRAISAL, false);

            return obj_dto;
        }

        public static PMS.Model.Context.PMS_APPRAISAL_TRAIL MapAppraisalTrailDTOToEntity(PMS.Model.DTO.Appraisal.Trail dto)
        {
            PMS.Model.Context.PMS_APPRAISAL_TRAIL obj_entity = new Context.PMS_APPRAISAL_TRAIL() 
            { 
                ID = dto.Id,
                ACTION_BY_ID = dto.Actioner.Id,
                ACTION_ID = dto.Action.Id,
                ACTION_TIMESTAMP = dto.ActionTimestamp,
                STAGE_MASTER_ID = dto.Stage.Id,
                APPRAISAL_ID = dto.Appraisal.Id
            };
            return obj_entity;
        }


        #endregion Trail

        #region Master

        #region Stage

        public static PMS.Model.DTO.Master.Stage MapStageEntityToDTO(PMS.Model.Context.PMS_MST_STAGE entity)
        {
            PMS.Model.DTO.Master.Stage obj_dto = new DTO.Master.Stage()
            {
                Id = entity.ID,
                Name = entity.NAME,
                Description = entity.DESCRIPTION,
                SortOrder = entity.SORT_ORDER,
                Active = entity.ACTIVE
            };
            return obj_dto;
        }

        public static List<PMS.Model.DTO.Master.Stage> MapStageEntitiesToDTOs(List<PMS.Model.Context.PMS_MST_STAGE> entities)
        {
            List<PMS.Model.DTO.Master.Stage> lst_dtos = new List<PMS.Model.DTO.Master.Stage>();
            foreach (PMS.Model.Context.PMS_MST_STAGE entity in entities)
            {
                lst_dtos.Add(MapStageEntityToDTO(entity));
            }
            return lst_dtos;
        }

        public static PMS.Model.DTO.Master.Stage MapAppraisalStageDTOToStageDTO(PMS.Model.DTO.Appraisal.Stage dto)
        {
            PMS.Model.DTO.Master.Stage obj_stage = new DTO.Master.Stage() 
            { 
                Id = Convert.ToInt32(dto.StageId),
                Name = dto.Name,
                Description = dto.Description
            };
            return obj_stage;
        }

        #endregion Stage

        #region Status

        public static PMS.Model.DTO.Master.Status MapStatusEntityToDTO(PMS.Model.Context.PMS_MST_STATUS entity)
        {
            PMS.Model.DTO.Master.Status obj_dto = new DTO.Master.Status()
            {
                Id = entity.ID,
                Name = entity.NAME,
                Description = entity.DESCRIPTION,
                Active = entity.ACTIVE
            };
            return obj_dto;
        }

        #endregion Status

        #region Section

        public static List<PMS.Model.DTO.Master.Section> MapSectionEntitiesToDTOs(List<PMS.Model.Context.PMS_MST_SECTION> entities, bool mapChildEntities)
        {
            List<PMS.Model.DTO.Master.Section> lst_dtos = new List<PMS.Model.DTO.Master.Section>();
            foreach (PMS.Model.Context.PMS_MST_SECTION entity in entities)
            {
                lst_dtos.Add(MapSectionEntityToDTO(entity, mapChildEntities));
            }
            return lst_dtos;
        }

        public static PMS.Model.DTO.Master.Section MapSectionEntityToDTO(PMS.Model.Context.PMS_MST_SECTION entity, bool mapChildEntities)
        {
            PMS.Model.DTO.Master.Section obj_dto = new DTO.Master.Section()
            {
                Id = entity.ID,
                Name = entity.NAME,
                Description = entity.DESCRIPTION,
                SortOrder = entity.SORT_ORDER,
                Active = entity.ACTIVE
            };

            if (mapChildEntities)
            {
                obj_dto.Blocks = Lib.Utility.Common.IsNullOrEmptyList(entity.PMS_MST_BLOCK) == true ? null : MapBlockEntitiesToDTOs(entity.PMS_MST_BLOCK.ToList());
            }
            return obj_dto;
        }

        #endregion Section

        #region Block

        public static List<PMS.Model.DTO.Master.Block> MapBlockEntitiesToDTOs(List<PMS.Model.Context.PMS_MST_BLOCK> entities)
        {
            List<PMS.Model.DTO.Master.Block> lst_dtos = new List<PMS.Model.DTO.Master.Block>();
            foreach (PMS.Model.Context.PMS_MST_BLOCK entity in entities)
            {
                lst_dtos.Add(MapBlockEntityToDTO(entity));
            }
            return lst_dtos;
        }

        public static PMS.Model.DTO.Master.Block MapBlockEntityToDTO(PMS.Model.Context.PMS_MST_BLOCK entity)
        {
            PMS.Model.DTO.Master.Block obj_dto = new DTO.Master.Block()
            {
                Id = entity.ID,
                Name = entity.NAME,
                Description = entity.DESCRIPTION,
                SortOrder = entity.SORT_ORDER,
                Active = entity.ACTIVE
            };

            obj_dto.Section = entity.PMS_MST_SECTION == null ? null : MapSectionEntityToDTO(entity.PMS_MST_SECTION, false);

            return obj_dto;
        }

        #endregion Block

        #region Priority

        public static PMS.Model.DTO.Master.Priority MapPriorityEntityToDTO(PMS.Model.Context.PMS_MST_PRIORITY entity)
        {
            PMS.Model.DTO.Master.Priority obj_dto = new DTO.Master.Priority()
            {
                Id = entity.ID,
                Name = entity.NAME,
                Description = entity.DESCRIPTION,
                SortOrder = entity.SORT_ORDER,
                Active = entity.ACTIVE
            };
            return obj_dto;
        }

        public static List<PMS.Model.DTO.Master.Priority> MapPriorityEntitiesToDTOs(List<PMS.Model.Context.PMS_MST_PRIORITY> entities)
        {
            List<PMS.Model.DTO.Master.Priority> lst_dtos = new List<PMS.Model.DTO.Master.Priority>();
            foreach (PMS.Model.Context.PMS_MST_PRIORITY entity in entities)
            {
                lst_dtos.Add(MapPriorityEntityToDTO(entity));
            }
            return lst_dtos;
        }

        #endregion Priority

        #region Core Value Competency

        public static PMS.Model.DTO.Master.CoreValueCompetency MapCoreValueCompetencyEntityToDTO(PMS.Model.Context.PMS_MST_CORE_VALUE_COMPETENCY entity)
        {
            PMS.Model.DTO.Master.CoreValueCompetency obj_dto = new DTO.Master.CoreValueCompetency()
            {
                Id = entity.ID,
                Name = entity.NAME,
                Description = entity.DESCRIPTION,
                Active = entity.ACTIVE
            };
            return obj_dto;
        }

        #endregion Core Value Competency

        #region Action

        public static PMS.Model.DTO.Master.Action MapActionEntityToDTO(PMS.Model.Context.PMS_MST_ACTION entity)
        {
            PMS.Model.DTO.Master.Action obj_dto = new DTO.Master.Action()
            {
                Id = entity.ID,
                Name = entity.NAME,
                Description = entity.DESCRIPTION,
                Active = entity.ACTIVE
            };
            return obj_dto;
        }

        #endregion Action

        #endregion Master
    }
}
