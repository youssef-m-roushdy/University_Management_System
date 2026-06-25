using AutoMapper;
using University_Management_System.Application.Dtos.AssistantDtos;
using University_Management_System.Domain.Entities.Identity;
using University_Management_System.Domain.Enums;

namespace University_Management_System.Application.Mapping.Profiles
{
    public class AssistantMappingProfile : Profile
    {
        public AssistantMappingProfile()
        {
            // ────────────────────────────────────────────────────────────────────────
            // ENTITY TO DTO
            // ────────────────────────────────────────────────────────────────────────

            CreateMap<Assistant, AssistantDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.User != null ? src.User.Name : string.Empty))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? src.User.UserName : string.Empty))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User != null ? src.User.Email : string.Empty))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.User != null ? src.User.PhoneNumber : null))
                .ForMember(dest => dest.ProfilePicture, opt => opt.MapFrom(src => src.User != null ? src.User.ProfilePicture : null))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.User != null ? src.User.Address : null))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.User != null ? src.User.Gender : Gender.Male))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.User != null ? src.User.IsActive : false))
                .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.DepartmentId))
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department != null ? src.Department.Name : string.Empty))
                .ForMember(dest => dest.DepartmentCode, opt => opt.MapFrom(src => src.Department != null ? src.Department.Code : string.Empty))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt));

            // ────────────────────────────────────────────────────────────────────────
            // CREATE ASSISTANT DTO → ASSISTANT
            // ────────────────────────────────────────────────────────────────────────

            CreateMap<CreateAssistantDto, Assistant>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Department, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CourseAssistants, opt => opt.Ignore())
                .ForMember(dest => dest.InstructorAssistants, opt => opt.Ignore())
                .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.DepartmentId));

            // ────────────────────────────────────────────────────────────────────────
            // UPDATE ASSISTANT DTO → ASSISTANT
            // ────────────────────────────────────────────────────────────────────────

            CreateMap<UpdateAssistantDto, Assistant>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Department, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CourseAssistants, opt => opt.Ignore())
                .ForMember(dest => dest.InstructorAssistants, opt => opt.Ignore())
                .ForMember(dest => dest.DepartmentId, opt => opt.Ignore());

            // AddAssistantToExistingUserDto → Assistant
            CreateMap<AddAssistantToExistingUserDto, Assistant>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Department, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CourseAssistants, opt => opt.Ignore())
                .ForMember(dest => dest.InstructorAssistants, opt => opt.Ignore())
                .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.DepartmentId));

        }
    }
}