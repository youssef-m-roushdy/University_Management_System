using AutoMapper;
using University_Management_System.Application.Commands.Courses;
using University_Management_System.Application.Dtos.CourseDtos;
using University_Management_System.Domain.Entities.Models;

namespace University_Management_System.Application.Mapping.Profiles
{
    public class CourseMappingProfile : Profile
    {
        public CourseMappingProfile()
        {
            // ────────────────────────────────────────────────────────────────────────
            // ENTITY TO DTO
            // ────────────────────────────────────────────────────────────────────────

            CreateMap<Course, CourseDto>()
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => 
                    src.Department != null ? src.Department.Name : string.Empty))
                .ForMember(dest => dest.PrerequisitesCount, opt => opt.Ignore())
                .ForMember(dest => dest.DependenciesCount, opt => opt.Ignore());

            // ────────────────────────────────────────────────────────────────────────
            // DTO TO ENTITY
            // ────────────────────────────────────────────────────────────────────────

            CreateMap<CreateCourseDto, Course>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Department, opt => opt.Ignore())
                .ForMember(dest => dest.Registrations, opt => opt.Ignore())
                .ForMember(dest => dest.CourseUpload, opt => opt.Ignore())
                .ForMember(dest => dest.PrerequisiteFor, opt => opt.Ignore())
                .ForMember(dest => dest.DependentCourses, opt => opt.Ignore())
                .ForMember(dest => dest.DepartmentCourses, opt => opt.Ignore())
                .ForMember(dest => dest.SpecializationCourses, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            CreateMap<UpdateCourseDto, Course>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Department, opt => opt.Ignore())
                .ForMember(dest => dest.Registrations, opt => opt.Ignore())
                .ForMember(dest => dest.CourseUpload, opt => opt.Ignore())
                .ForMember(dest => dest.PrerequisiteFor, opt => opt.Ignore())
                .ForMember(dest => dest.DependentCourses, opt => opt.Ignore())
                .ForMember(dest => dest.DepartmentCourses, opt => opt.Ignore())
                .ForMember(dest => dest.SpecializationCourses, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Code, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Code)))
                .ForMember(dest => dest.Name, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Name)))
                .ForMember(dest => dest.Credits, opt => opt.Condition(src => src.Credits.HasValue))
                .ForMember(dest => dest.Status, opt => opt.Condition(src => src.Status.HasValue))
                .ForMember(dest => dest.DepartmentId, opt => opt.Condition(src => src.DepartmentId.HasValue));

            // ────────────────────────────────────────────────────────────────────────
            // COMMAND TO ENTITY
            // ────────────────────────────────────────────────────────────────────────

            CreateMap<CreateCourseCommand, Course>();
            CreateMap<UpdateCourseCommand, Course>();
            CreateMap<UpdateCourseStatusCommand, Course>();
        }
    }
}