using AutoMapper;
using University_Management_System.Application.Dtos.StudentStudyYearDtos;
using University_Management_System.Application.Commands.StudentStudyYears;
using University_Management_System.Domain.Entities.Models;

namespace University_Management_System.Application.Mapping.Profiles
{
    public class StudentStudyYearMappingProfile : Profile
    {
        public StudentStudyYearMappingProfile()
        {
            // ────────────────────────────────────────────────────────────────────────
            // ENTITY TO DTO
            // ────────────────────────────────────────────────────────────────────────

            // StudentStudyYear to StudentStudyYearDto
            CreateMap<StudentStudyYear, StudentStudyYearDto>()
                // ─── IDs ──────────────────────────────────────────────────────────────
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.StudentId, opt => opt.MapFrom(src => src.StudentId))
                .ForMember(dest => dest.StudyYearId, opt => opt.MapFrom(src => src.StudyYearId))
                .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src =>
                    src.Student != null && src.Student.Department != null
                        ? src.Student.Department.Id
                        : (int?)null))
                .ForMember(dest => dest.SpecializationId, opt => opt.MapFrom(src =>
                    src.Student != null && src.Student.Specialization != null
                        ? src.Student.Specialization.Id
                        : (int?)null))

                // ─── Student Info ──────────────────────────────────────────────────
                .ForMember(dest => dest.AcademicCode, opt => opt.MapFrom(src =>
                    src.Student != null ? src.Student.AcademicCode : string.Empty))
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src =>
                    src.Student != null && src.Student.User != null
                        ? src.Student.User.Name
                        : string.Empty))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src =>
                    src.Student != null && src.Student.User != null
                        ? src.Student.User.Email
                        : null))

                // ─── Department Info ──────────────────────────────────────────────────
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src =>
                    src.Student != null && src.Student.Department != null
                        ? src.Student.Department.Name
                        : null))
                .ForMember(dest => dest.DepartmentCode, opt => opt.MapFrom(src =>
                    src.Student != null && src.Student.Department != null
                        ? src.Student.Department.Code
                        : null))

                // ─── Specialization Info ──────────────────────────────────────────────
                .ForMember(dest => dest.SpecializationName, opt => opt.MapFrom(src =>
                    src.Student != null && src.Student.Specialization != null
                        ? src.Student.Specialization.Name
                        : null))

                // ─── Study Year Info ──────────────────────────────────────────────────
                .ForMember(dest => dest.StartYear, opt => opt.MapFrom(src =>
                    src.StudyYear != null ? src.StudyYear.StartYear : 0))
                .ForMember(dest => dest.EndYear, opt => opt.MapFrom(src =>
                    src.StudyYear != null ? src.StudyYear.EndYear : 0))
                .ForMember(dest => dest.IsCurrentStudyYear, opt => opt.MapFrom(src =>
                    src.StudyYear != null ? src.StudyYear.IsCurrent : false))

                // ─── Academic Info ──────────────────────────────────────────────────
                .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.Level))
                .ForMember(dest => dest.TotalGPA, opt => opt.MapFrom(src =>
                    src.Student != null ? src.Student.TotalGPA : (decimal?)null))
                .ForMember(dest => dest.TotalCredits, opt => opt.MapFrom(src =>
                    src.Student != null ? src.Student.TotalCredits : (int?)null))
                .ForMember(dest => dest.AllowedCredits, opt => opt.MapFrom(src =>
                    src.Student != null ? src.Student.AllowedCredits : (int?)null))

                // ─── Status ──────────────────────────────────────────────────────────
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))

                // ─── Dates ──────────────────────────────────────────────────────────
                .ForMember(dest => dest.EnrolledAt, opt => opt.MapFrom(src => src.EnrolledAt))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt));

            // ────────────────────────────────────────────────────────────────────────
            // DTO TO COMMAND
            // ────────────────────────────────────────────────────────────────────────

            // CreateStudentStudyYearDto to CreateStudentStudyYearCommand
            CreateMap<CreateStudentStudyYearDto, CreateStudentStudyYearCommand>();

            // UpdateStudentStudyYearDto to UpdateStudentStudyYearCommand
            CreateMap<UpdateStudentStudyYearDto, UpdateStudentStudyYearCommand>();

            // ────────────────────────────────────────────────────────────────────────
            // DTO TO ENTITY
            // ────────────────────────────────────────────────────────────────────────

            // CreateStudentStudyYearDto to StudentStudyYear
            CreateMap<CreateStudentStudyYearDto, StudentStudyYear>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Student, opt => opt.Ignore())
                .ForMember(dest => dest.StudyYear, opt => opt.Ignore())
                .ForMember(dest => dest.EnrolledAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            // UpdateStudentStudyYearDto to StudentStudyYear
            CreateMap<UpdateStudentStudyYearDto, StudentStudyYear>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.StudentId, opt => opt.Ignore())
                .ForMember(dest => dest.StudyYearId, opt => opt.Ignore())
                .ForMember(dest => dest.Student, opt => opt.Ignore())
                .ForMember(dest => dest.StudyYear, opt => opt.Ignore())
                .ForMember(dest => dest.EnrolledAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Level, opt => opt.Condition(src => src.Level.HasValue))
                .ForMember(dest => dest.IsActive, opt => opt.Condition(src => src.IsActive.HasValue));

            // ────────────────────────────────────────────────────────────────────────
            // COMMAND TO ENTITY
            // ────────────────────────────────────────────────────────────────────────

            // CreateStudentStudyYearCommand to StudentStudyYear
            CreateMap<CreateStudentStudyYearCommand, StudentStudyYear>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Student, opt => opt.Ignore())
                .ForMember(dest => dest.StudyYear, opt => opt.Ignore())
                .ForMember(dest => dest.EnrolledAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.StudentId, opt => opt.MapFrom(src => src.Dto.StudentId))
                .ForMember(dest => dest.StudyYearId, opt => opt.MapFrom(src => src.Dto.StudyYearId))
                .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.Dto.Level))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.Dto.IsActive));

            // UpdateStudentStudyYearCommand to StudentStudyYear
            CreateMap<UpdateStudentStudyYearCommand, StudentStudyYear>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.StudentId, opt => opt.Ignore())
                .ForMember(dest => dest.StudyYearId, opt => opt.Ignore())
                .ForMember(dest => dest.Student, opt => opt.Ignore())
                .ForMember(dest => dest.StudyYear, opt => opt.Ignore())
                .ForMember(dest => dest.EnrolledAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Level, opt => opt.Condition(src => src.Dto.Level.HasValue))
                .ForMember(dest => dest.IsActive, opt => opt.Condition(src => src.Dto.IsActive.HasValue));
        }
    }
}