using AutoMapper;
using University_Management_System.Application.Dtos.RegistrationDtos;
using University_Management_System.Domain.Entities.Models;

namespace University_Management_System.Application.Mapping.Profiles
{
    public class RegistrationMappingProfile : Profile
    {
        public RegistrationMappingProfile()
        {
            // ────────────────────────────────────────────────────────────────────────
            // ENTITY TO DTO
            // ────────────────────────────────────────────────────────────────────────

            CreateMap<Registration, RegistrationDto>()
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => 
                    src.Student != null && src.Student.User != null 
                        ? src.Student.User.Name 
                        : string.Empty))
                .ForMember(dest => dest.AcademicCode, opt => opt.MapFrom(src => 
                    src.Student != null 
                        ? src.Student.AcademicCode 
                        : string.Empty))
                .ForMember(dest => dest.CourseCode, opt => opt.MapFrom(src => 
                    src.Course != null 
                        ? src.Course.Code 
                        : string.Empty))
                .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => 
                    src.Course != null 
                        ? src.Course.Name 
                        : string.Empty))
                .ForMember(dest => dest.Credits, opt => opt.MapFrom(src => 
                    src.Course != null 
                        ? src.Course.Credits 
                        : 0))
                .ForMember(dest => dest.SemesterTitle, opt => opt.MapFrom(src => 
                    src.Semester != null 
                        ? src.Semester.Title.ToString().Replace("_", " ") 
                        : string.Empty))
                .ForMember(dest => dest.StudyYearRange, opt => opt.MapFrom(src => 
                    src.StudyYear != null 
                        ? $"{src.StudyYear.StartYear}-{src.StudyYear.EndYear}" 
                        : string.Empty));

            CreateMap<CreateRegistrationDto, Registration>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.StudentId, opt => opt.Ignore())
                .ForMember(dest => dest.Student, opt => opt.Ignore())
                .ForMember(dest => dest.Course, opt => opt.Ignore())
                .ForMember(dest => dest.Semester, opt => opt.Ignore())
                .ForMember(dest => dest.StudyYear, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.Progress, opt => opt.Ignore())
                .ForMember(dest => dest.Grade, opt => opt.Ignore())
                .ForMember(dest => dest.IsPassed, opt => opt.Ignore())
                .ForMember(dest => dest.Reason, opt => opt.Ignore())
                .ForMember(dest => dest.RegisteredAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            CreateMap<UpdateRegistrationDto, Registration>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.StudentId, opt => opt.Ignore())
                .ForMember(dest => dest.CourseId, opt => opt.Ignore())
                .ForMember(dest => dest.SemesterId, opt => opt.Ignore())
                .ForMember(dest => dest.StudyYearId, opt => opt.Ignore())
                .ForMember(dest => dest.Student, opt => opt.Ignore())
                .ForMember(dest => dest.Course, opt => opt.Ignore())
                .ForMember(dest => dest.Semester, opt => opt.Ignore())
                .ForMember(dest => dest.StudyYear, opt => opt.Ignore())
                .ForMember(dest => dest.Grade, opt => opt.Ignore())
                .ForMember(dest => dest.IsPassed, opt => opt.Ignore())
                .ForMember(dest => dest.RegisteredAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Condition(src => src.Status.HasValue))
                .ForMember(dest => dest.Progress, opt => opt.Condition(src => src.Progress.HasValue))
                .ForMember(dest => dest.Reason, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Reason)));

            // ────────────────────────────────────────────────────────────────────────
            // COMMAND TO ENTITY (if needed)
            // ────────────────────────────────────────────────────────────────────────

            CreateMap<UpdateRegistrationGradeDto, Registration>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.StudentId, opt => opt.Ignore())
                .ForMember(dest => dest.CourseId, opt => opt.Ignore())
                .ForMember(dest => dest.SemesterId, opt => opt.Ignore())
                .ForMember(dest => dest.StudyYearId, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.Progress, opt => opt.Ignore())
                .ForMember(dest => dest.RegisteredAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Student, opt => opt.Ignore())
                .ForMember(dest => dest.Course, opt => opt.Ignore())
                .ForMember(dest => dest.Semester, opt => opt.Ignore())
                .ForMember(dest => dest.StudyYear, opt => opt.Ignore());
        }
    }
}