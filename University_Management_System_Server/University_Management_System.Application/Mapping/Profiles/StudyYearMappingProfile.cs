using AutoMapper;
using University_Management_System.Application.Commands.StudyYears;
using University_Management_System.Application.Dtos.StudyYearDtos;
using University_Management_System.Domain.Entities.Models;

namespace University_Management_System.Application.Mapping.Profiles
{
    public class StudyYearMappingProfile : Profile
    {
        public StudyYearMappingProfile()
        {
            // ────────────────────────────────────────────────────────────────────────
            // ENTITY TO DTO
            // ────────────────────────────────────────────────────────────────────────

            // StudyYear to StudyYearDto
            CreateMap<StudyYear, StudyYearDto>()
                .ForMember(dest => dest.SemesterCount, opt => opt.Ignore())
                .ForMember(dest => dest.StudentCount, opt => opt.Ignore())
                .ForMember(dest => dest.RegistrationCount, opt => opt.Ignore());

            // ────────────────────────────────────────────────────────────────────────
            // DTO TO COMMAND / ENTITY
            // ────────────────────────────────────────────────────────────────────────

            // CreateStudyYearDto to StudyYear
            CreateMap<CreateStudyYearDto, StudyYear>();

            // CreateStudyYearCommand to StudyYear
            CreateMap<CreateStudyYearCommand, StudyYear>();

            // UpdateStudyYearDto to StudyYear
            CreateMap<UpdateStudyYearDto, StudyYear>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Semesters, opt => opt.Ignore())
                .ForMember(dest => dest.Fees, opt => opt.Ignore())
                .ForMember(dest => dest.Registrations, opt => opt.Ignore())
                .ForMember(dest => dest.StudentStudyYears, opt => opt.Ignore())
                .ForMember(dest => dest.AcademicSchedules, opt => opt.Ignore())
                .ForMember(dest => dest.CourseAssistants, opt => opt.Ignore())
                .ForMember(dest => dest.CourseInstructors, opt => opt.Ignore())
                .ForMember(dest => dest.InstructorCourseUploads, opt => opt.Ignore())
                .ForMember(dest => dest.AssistantCourseUploads, opt => opt.Ignore())
                .ForMember(dest => dest.SemesterGPAs, opt => opt.Ignore());

            // UpdateStudyYearCommand to StudyYear
            CreateMap<UpdateStudyYearCommand, StudyYear>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Semesters, opt => opt.Ignore())
                .ForMember(dest => dest.Fees, opt => opt.Ignore())
                .ForMember(dest => dest.Registrations, opt => opt.Ignore())
                .ForMember(dest => dest.StudentStudyYears, opt => opt.Ignore())
                .ForMember(dest => dest.AcademicSchedules, opt => opt.Ignore())
                .ForMember(dest => dest.CourseAssistants, opt => opt.Ignore())
                .ForMember(dest => dest.CourseInstructors, opt => opt.Ignore())
                .ForMember(dest => dest.InstructorCourseUploads, opt => opt.Ignore())
                .ForMember(dest => dest.AssistantCourseUploads, opt => opt.Ignore())
                .ForMember(dest => dest.SemesterGPAs, opt => opt.Ignore());

            // PatchStudyYearDto to StudyYear (only update provided fields)
            CreateMap<PatchStudyYearDto, StudyYear>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.StartYear, opt => opt.Condition(src => src.StartYear.HasValue))
                .ForMember(dest => dest.EndYear, opt => opt.Condition(src => src.EndYear.HasValue))
                .ForMember(dest => dest.IsCurrent, opt => opt.Condition(src => src.IsCurrent.HasValue))
                .ForMember(dest => dest.Semesters, opt => opt.Ignore())
                .ForMember(dest => dest.Fees, opt => opt.Ignore())
                .ForMember(dest => dest.Registrations, opt => opt.Ignore())
                .ForMember(dest => dest.StudentStudyYears, opt => opt.Ignore())
                .ForMember(dest => dest.AcademicSchedules, opt => opt.Ignore())
                .ForMember(dest => dest.CourseAssistants, opt => opt.Ignore())
                .ForMember(dest => dest.CourseInstructors, opt => opt.Ignore())
                .ForMember(dest => dest.InstructorCourseUploads, opt => opt.Ignore())
                .ForMember(dest => dest.AssistantCourseUploads, opt => opt.Ignore())
                .ForMember(dest => dest.SemesterGPAs, opt => opt.Ignore());

            // PatchStudyYearCommand to StudyYear (only update provided fields)
            CreateMap<PatchStudyYearCommand, StudyYear>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.StartYear, opt => opt.Condition(src => src.StartYear.HasValue))
                .ForMember(dest => dest.EndYear, opt => opt.Condition(src => src.EndYear.HasValue))
                .ForMember(dest => dest.IsCurrent, opt => opt.Condition(src => src.IsCurrent.HasValue))
                .ForMember(dest => dest.Semesters, opt => opt.Ignore())
                .ForMember(dest => dest.Fees, opt => opt.Ignore())
                .ForMember(dest => dest.Registrations, opt => opt.Ignore())
                .ForMember(dest => dest.StudentStudyYears, opt => opt.Ignore())
                .ForMember(dest => dest.AcademicSchedules, opt => opt.Ignore())
                .ForMember(dest => dest.CourseAssistants, opt => opt.Ignore())
                .ForMember(dest => dest.CourseInstructors, opt => opt.Ignore())
                .ForMember(dest => dest.InstructorCourseUploads, opt => opt.Ignore())
                .ForMember(dest => dest.AssistantCourseUploads, opt => opt.Ignore())
                .ForMember(dest => dest.SemesterGPAs, opt => opt.Ignore());

            // ────────────────────────────────────────────────────────────────────────
            // DTO TO COMMAND
            // ────────────────────────────────────────────────────────────────────────

            // CreateStudyYearDto to CreateStudyYearCommand
            CreateMap<CreateStudyYearDto, CreateStudyYearCommand>();

            // UpdateStudyYearDto to UpdateStudyYearCommand
            CreateMap<UpdateStudyYearDto, UpdateStudyYearCommand>();

            // PatchStudyYearDto to PatchStudyYearCommand
            CreateMap<PatchStudyYearDto, PatchStudyYearCommand>();
        }
    }
}