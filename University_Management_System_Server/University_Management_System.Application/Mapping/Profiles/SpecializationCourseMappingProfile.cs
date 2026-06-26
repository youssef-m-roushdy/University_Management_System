using AutoMapper;
using University_Management_System.Application.Dtos.SpecializationCourseDtos;
using University_Management_System.Application.Commands.SpecializationCourses;
using University_Management_System.Domain.Entities.Models;

namespace University_Management_System.Application.Mapping.Profiles
{
    public class SpecializationCourseMappingProfile : Profile
    {
        public SpecializationCourseMappingProfile()
        {
            // ────────────────────────────────────────────────────────────────────────
            // ENTITY TO DTO
            // ────────────────────────────────────────────────────────────────────────

            CreateMap<SpecializationCourse, SpecializationCourseDto>()
                .ForMember(dest => dest.SpecializationName, opt => opt.MapFrom(src => 
                    src.Specialization != null ? src.Specialization.Name : string.Empty))
                .ForMember(dest => dest.CourseCode, opt => opt.MapFrom(src => 
                    src.Course != null ? src.Course.Code : string.Empty))
                .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => 
                    src.Course != null ? src.Course.Name : string.Empty))
                .ForMember(dest => dest.Credits, opt => opt.MapFrom(src => 
                    src.Course != null ? src.Course.Credits : 0))
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => 
                    src.Course != null && src.Course.Department != null ? src.Course.Department.Name : null))
                .ForMember(dest => dest.PrerequisitesCount, opt => opt.MapFrom(src => 
                    src.Course != null && src.Course.PrerequisiteFor != null ? src.Course.PrerequisiteFor.Count : 0));

            // ────────────────────────────────────────────────────────────────────────
            // DTO TO ENTITY
            // ────────────────────────────────────────────────────────────────────────

            CreateMap<CreateSpecializationCourseDto, SpecializationCourse>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Specialization, opt => opt.Ignore())
                .ForMember(dest => dest.Course, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            CreateMap<CreateSpecializationCourseBulkDto, SpecializationCourse>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Specialization, opt => opt.Ignore())
                .ForMember(dest => dest.Course, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CourseId, opt => opt.Ignore());
        }
    }
}