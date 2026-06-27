using AutoMapper;
using University_Management_System.Application.Dtos.AcademicScheduleDtos;
using University_Management_System.Domain.Entities.Models;

namespace University_Management_System.Application.Mapping.Profiles
{
    public class AcademicScheduleMappingProfile : Profile
    {
        public AcademicScheduleMappingProfile()
        {
            // ────────────────────────────────────────────────────────────────────────
            // ENTITY TO DTO
            // ────────────────────────────────────────────────────────────────────────

            CreateMap<AcademicSchedule, AcademicScheduleDto>()
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => 
                    src.Department != null ? src.Department.Name : string.Empty))
                .ForMember(dest => dest.DepartmentCode, opt => opt.MapFrom(src => 
                    src.Department != null ? src.Department.Code : string.Empty))
                .ForMember(dest => dest.SemesterTitle, opt => opt.MapFrom(src => 
                    src.Semester != null ? src.Semester.Title.ToString().Replace("_", " ") : string.Empty))
                .ForMember(dest => dest.StudyYearRange, opt => opt.MapFrom(src => 
                    src.StudyYear != null ? $"{src.StudyYear.StartYear}-{src.StudyYear.EndYear}" : string.Empty))
                .ForMember(dest => dest.AdminName, opt => opt.MapFrom(src => 
                    src.Admin != null && src.Admin.User != null ? src.Admin.User.Name : string.Empty))
                .ForMember(dest => dest.ScheduleDateDisplay, opt => opt.MapFrom(src => 
                    src.ScheduleDate.ToString("yyyy-MM-dd")));

            // ────────────────────────────────────────────────────────────────────────
            // DTO TO ENTITY
            // ────────────────────────────────────────────────────────────────────────

            CreateMap<CreateAcademicScheduleDto, AcademicSchedule>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.AdminId, opt => opt.Ignore())
                .ForMember(dest => dest.Admin, opt => opt.Ignore())
                .ForMember(dest => dest.Department, opt => opt.Ignore())
                .ForMember(dest => dest.Semester, opt => opt.Ignore())
                .ForMember(dest => dest.StudyYear, opt => opt.Ignore())
                .ForMember(dest => dest.Url, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
        }
    }
}