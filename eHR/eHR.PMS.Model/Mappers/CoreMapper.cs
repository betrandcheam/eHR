using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eHR.PMS.Model.Mappers
{
    public class CoreMapper
    {

        #region Employee

        public static PMS.Model.DTO.Core.Employee MapEmployeeEntityToDTO(PMS.Model.Context.EMPLOYEE entity)
        {
            PMS.Model.DTO.Core.Employee obj_dto = new DTO.Core.Employee()
            {
                Id = entity.ID,
                FirstName = entity.FIRST_NAME,
                LastName = entity.LAST_NAME,
                PreferredName = entity.PREFERRED_NAME,
                DomainId = entity.DOMAIN_ID,
                Department = entity.DEPARTMENT == null ? null : MapEmployeeDepartmentToDTO(entity.DEPARTMENT),
                ACRGrade = entity.MST_ACR_GRADE == null ? null : MapGradeEntityToDTO(entity.MST_ACR_GRADE),
                EmploymentType = entity.MST_EMPLOYMENT_TYPE == null ? null : MapEmploymentTypeEntityToDTO(entity.MST_EMPLOYMENT_TYPE),
                Active = entity.ACTIVE,
                OfficeEmailAddress = entity.OFFICE_EMAIL_ADDRESS,
                DateOfDeparture = entity.DATE_OF_DEPARTURE,
                DateOfHire = entity.DATE_OF_HIRE
            };
            return obj_dto;
        }

        public static PMS.Model.DTO.Core.Employee MapEmployeeEntityToDTO(PMS.Model.Context.EMPLOYEE entity, bool mapChildren)
        {
            PMS.Model.DTO.Core.Employee obj_dto = MapEmployeeEntityToDTO(entity);

            if (mapChildren)
            {
                obj_dto.Level1Approver = entity.PMS_LEVEL_1_APPROVER == null ? null : MapEmployeeEntityToDTO(entity.PMS_LEVEL_1_APPROVER, false);
                obj_dto.Level2Approver = entity.PMS_LEVEL_2_APPROVER == null ? null : MapEmployeeEntityToDTO(entity.PMS_LEVEL_2_APPROVER, false);
            }
            return obj_dto;
        }

        public static List<PMS.Model.DTO.Core.Employee> MapEmployeeEntitiesToDTOs(List<PMS.Model.Context.EMPLOYEE> entities, bool mapChildren)
        {
            List<PMS.Model.DTO.Core.Employee> lst_dtos = new List<DTO.Core.Employee>();
            foreach (PMS.Model.Context.EMPLOYEE entity in entities)
            {
                lst_dtos.Add(MapEmployeeEntityToDTO(entity, mapChildren));
            }
            return lst_dtos;
        }

        public static PMS.Model.DTO.Core.Security.User MapEmployeeEntityToUserDTO(PMS.Model.Context.EMPLOYEE entity, bool mapChildEntities)
        {
            PMS.Model.DTO.Core.Security.User obj_dto = new PMS.Model.DTO.Core.Security.User()
            {
                Id = entity.ID,
                FirstName = entity.FIRST_NAME,
                LastName = entity.LAST_NAME,
                PreferredName = entity.PREFERRED_NAME,
                DomainId = entity.DOMAIN_ID,
                ACRGrade = entity.MST_ACR_GRADE == null ? null : MapGradeEntityToDTO(entity.MST_ACR_GRADE),
                Active = entity.ACTIVE
            };

            if (mapChildEntities)
            {
                obj_dto.Roles = Lib.Utility.Common.IsNullOrEmptyList(entity.USER_ROLE) == true ? null : MapUserRoleEntitiesToDTOs(entity.USER_ROLE.ToList());
            }

            return obj_dto;
        }
     
        public static PMS.Model.DTO.Core.Employee MapUserDTOToEmployeeDTO(PMS.Model.DTO.Core.Security.User userDTO)
        {
            PMS.Model.DTO.Core.Employee obj_dto = new DTO.Core.Employee() 
            { 
                Id = userDTO.Id
            };
            return obj_dto;
        }

        #endregion Employee

        #region Department

        public static PMS.Model.DTO.Core.Department MapEmployeeDepartmentToDTO(PMS.Model.Context.DEPARTMENT entity)
        {
            PMS.Model.DTO.Core.Department obj_dto = new DTO.Core.Department()
            {
                Id = entity.ID,
                DepartmentId = entity.MST_DEPARTMENT.ID,
                Name = entity.MST_DEPARTMENT.NAME,
                Abbreviation = entity.MST_DEPARTMENT.ABBREVIATION,
                Description = entity.MST_DEPARTMENT.DESCRIPTION,
                Active = entity.MST_DEPARTMENT.ACTIVE
            };
            return obj_dto;
        }

        #endregion Department

        #region User

        public static PMS.Model.DTO.Core.Security.UserRole MapUserRoleEntityToDTO(PMS.Model.Context.USER_ROLE entity)
        {
            PMS.Model.DTO.Core.Security.UserRole obj_dto = new DTO.Core.Security.UserRole()
            {
                Id = entity.ID,
                RoleId = entity.MST_ROLE.ID,
                Name = entity.MST_ROLE.NAME,
                Code = entity.MST_ROLE.CODE,
                Description = entity.MST_ROLE.DESCRIPTION,
                Active = entity.MST_ROLE.ACTIVE
            };

            obj_dto.User = MapEmployeeEntityToUserDTO(entity.EMPLOYEE, false);

            return obj_dto;
        }

        public static List<PMS.Model.DTO.Core.Security.UserRole> MapUserRoleEntitiesToDTOs(List<PMS.Model.Context.USER_ROLE> entities)
        {
            List<PMS.Model.DTO.Core.Security.UserRole> lst_dtos = new List<DTO.Core.Security.UserRole>();
            foreach (PMS.Model.Context.USER_ROLE entity in entities)
            {
                lst_dtos.Add(MapUserRoleEntityToDTO(entity));
            }
            return lst_dtos;
        }

        #endregion User

        #region Task

        public static List<PMS.Model.DTO.Core.Task.Task> MapTaskEntitiesToDTOs(List<PMS.Model.Context.TASK> entities, bool mapChildren)
        {
            List<PMS.Model.DTO.Core.Task.Task> lst_dtos = new List<DTO.Core.Task.Task>();
            foreach (PMS.Model.Context.TASK entity in entities)
            {
                lst_dtos.Add(MapTaskEntityToDTO(entity, mapChildren));
            }
            return lst_dtos;
        }

        public static PMS.Model.DTO.Core.Task.Task MapTaskEntityToDTO(PMS.Model.Context.TASK entity, bool mapChildren)
        {
            PMS.Model.DTO.Core.Task.Task obj_dto = new PMS.Model.DTO.Core.Task.Task()
            {
                Id = entity.ID,
                RecordId = entity.RECORD_ID,
                Module = entity.MST_MODULE == null ? null : MapModuleEntityToDTO(entity.MST_MODULE),
                Status = entity.MST_STATUS == null ? null : MapStatusEntityToDTO(entity.MST_STATUS),
                Name = entity.NAME,
                Address = entity.ADDRESS,
                
            };

            if (mapChildren)
            { 
                obj_dto.Owners = Lib.Utility.Common.IsNullOrEmptyList(entity.TASK_OWNER) == true ? null :  MapOwnerEntitiesToDTOs(entity.TASK_OWNER.ToList());
            }

            return obj_dto;
        }

        public static List<PMS.Model.DTO.Core.Task.Owner> MapOwnerEntitiesToDTOs(List<PMS.Model.Context.TASK_OWNER> entities)
        {
            List<PMS.Model.DTO.Core.Task.Owner> lst_dtos = new List<DTO.Core.Task.Owner>();
            foreach (PMS.Model.Context.TASK_OWNER entity in entities)
            {
                lst_dtos.Add(MapOwnerEntityToDTO(entity));
            }
            return lst_dtos;
        }

        public static PMS.Model.DTO.Core.Task.Owner MapOwnerEntityToDTO(PMS.Model.Context.TASK_OWNER entity)
        {
            PMS.Model.DTO.Core.Task.Owner obj_dto = null;
            if (entity != null)
            {
                obj_dto = new PMS.Model.DTO.Core.Task.Owner()
                {
                    Id = entity.ID,
                    Task = MapTaskEntityToDTO(entity.TASK, false),
                    EmployeeId = entity.EMPLOYEE.ID,
                    PreferredName = entity.EMPLOYEE.PREFERRED_NAME
                };
            }
            return obj_dto;
        }

        public static Model.DTO.Core.Task.Owner MapApproverDTOToOwnerDTO(Model.DTO.Appraisal.Approver approver)
        {
            Model.DTO.Core.Task.Owner obj_owner = new DTO.Core.Task.Owner() 
            { 
                EmployeeId = approver.EmployeeId,
                PreferredName = approver.PreferredName,
                OfficeEmailAddress = approver.OfficeEmailAddress
            };
            return obj_owner;
        }

        public static Model.DTO.Core.Task.Owner MapEmployeeDTOToOwnerDTO(Model.DTO.Core.Employee employee)
        {
            Model.DTO.Core.Task.Owner obj_owner = new DTO.Core.Task.Owner()
            {
                EmployeeId = employee.Id,
                PreferredName = employee.PreferredName,
                OfficeEmailAddress = employee.OfficeEmailAddress
            };
            return obj_owner;
        }

        public static Model.Context.TASK MapTaskDTOToEntity(Model.DTO.Core.Task.Task dto)
        {
            Model.Context.TASK obj_entity = new Context.TASK() 
            { 
                ID = dto.Id,
                MODULE_ID = dto.Module.Id,
                RECORD_ID = dto.RecordId,
                STATUS_ID = dto.Status.Id,
                NAME = dto.Name,
                ADDRESS = dto.Address
            };
            return obj_entity;
        }

        public static Model.Context.TASK_OWNER MapTaskOwnerDTOToEntity(Model.DTO.Core.Task.Owner dto)
        {
            Model.Context.TASK_OWNER obj_entity = new Context.TASK_OWNER()
            {
                ID = dto.Id,
                TASK_ID = (dto.Task == null) ? (int?) null : dto.Task.Id,
                EMPLOYEE_ID = dto.EmployeeId
            };
            return obj_entity;
        }

        #endregion Task

        #region Master

        public static PMS.Model.DTO.Core.Master.Module MapModuleEntityToDTO(PMS.Model.Context.MST_MODULE entity)
        {
            PMS.Model.DTO.Core.Master.Module obj_dto = new DTO.Core.Master.Module()
            {
                Id = entity.ID,
                Name = entity.NAME,
                Description = entity.DESCRIPTION,
                Active = entity.ACTIVE
            };
            return obj_dto;
        }

        public static PMS.Model.DTO.Core.Master.Status MapStatusEntityToDTO(PMS.Model.Context.MST_STATUS entity)
        {
            PMS.Model.DTO.Core.Master.Status obj_dto = new DTO.Core.Master.Status()
            {
                Id = entity.ID,
                Name = entity.NAME,
                Description = entity.DESCRIPTION,
                Active = entity.ACTIVE
            };
            return obj_dto;
        }

        public static PMS.Model.DTO.Core.Master.Role MapStatusEntityToDTO(PMS.Model.Context.MST_ROLE entity)
        {
            PMS.Model.DTO.Core.Master.Role obj_dto = new DTO.Core.Master.Role()
            {
                Id = entity.ID,
                Name = entity.NAME,
                Code = entity.CODE,
                Description = entity.DESCRIPTION,
                Active = entity.ACTIVE
            };
            return obj_dto;
        }

        public static PMS.Model.DTO.Core.Master.Department MapDepartmentEntityToDTO(PMS.Model.Context.MST_DEPARTMENT entity)
        {
            PMS.Model.DTO.Core.Master.Department obj_dto = new DTO.Core.Master.Department()
            {
                Id = entity.ID,
                Name = entity.NAME,
                Abbreviation = entity.ABBREVIATION,
                Description = entity.DESCRIPTION,
                Active = entity.ACTIVE
            };
            return obj_dto;
        }

        public static PMS.Model.DTO.Core.Master.Grade MapGradeEntityToDTO(PMS.Model.Context.MST_ACR_GRADE entity)
        {
            PMS.Model.DTO.Core.Master.Grade obj_dto = new DTO.Core.Master.Grade()
            {
                Id = entity.ID,
                Name = entity.NAME,
                Description = entity.DESCRIPTION,
                Active = entity.ACTIVE
            };
            return obj_dto;
        }

        public static PMS.Model.DTO.Core.Master.EmploymentType MapEmploymentTypeEntityToDTO(PMS.Model.Context.MST_EMPLOYMENT_TYPE entity)
        {
            PMS.Model.DTO.Core.Master.EmploymentType obj_dto = new DTO.Core.Master.EmploymentType()
            {
                Id = entity.ID,
                Name = entity.NAME,
                Description = entity.DESCRIPTION,
                Active = entity.ACTIVE
            };
            return obj_dto;
        }

        public static PMS.Model.DTO.Core.Master.Department MapDepartmentDTOToMasterDepartmentDTO(PMS.Model.DTO.Core.Department departmentDTO)
        {
            PMS.Model.DTO.Core.Master.Department dto = new PMS.Model.DTO.Core.Master.Department()
            {
                Id = Convert.ToInt32(departmentDTO.DepartmentId)
            };
            return dto;
        }

        #endregion Master
    }
}
