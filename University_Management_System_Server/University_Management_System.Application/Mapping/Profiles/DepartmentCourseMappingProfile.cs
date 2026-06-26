using AutoMapper;
using University_Management_System.Application.Dtos.DepartmentCourseDtos;
using University_Management_System.Domain.Entities.Models;

namespace University_Management_System.Application.Mapping.Profiles
{
    public class DepartmentCourseMappingProfile : Profile
    {
        public DepartmentCourseMappingProfile()
        {
            // ────────────────────────────────────────────────────────────────────────
            // ENTITY TO DTO
            // ────────────────────────────────────────────────────────────────────────

            CreateMap<DepartmentCourse, DepartmentCourseDto>()
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => 
                    src.Department != null ? src.Department.Name : string.Empty))
                .ForMember(dest => dest.DepartmentCode, opt => opt.MapFrom(src => 
                    src.Department != null ? src.Department.Code : string.Empty))
                .ForMember(dest => dest.CourseCode, opt => opt.MapFrom(src => 
                    src.Course != null ? src.Course.Code : string.Empty))
                .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => 
                    src.Course != null ? src.Course.Name : string.Empty))
                .ForMember(dest => dest.Credits, opt => opt.MapFrom(src => 
                    src.Course != null ? src.Course.Credits : 0));

            // ────────────────────────────────────────────────────────────────────────
            // DTO TO ENTITY
            // ────────────────────────────────────────────────────────────────────────

            CreateMap<CreateDepartmentCourseDto, DepartmentCourse>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Department, opt => opt.Ignore())
                .ForMember(dest => dest.Course, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            CreateMap<CreateDepartmentCourseBulkDto, DepartmentCourse>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Department, opt => opt.Ignore())
                .ForMember(dest => dest.Course, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CourseId, opt => opt.Ignore());
        }
    }
}