using AutoMapper;
using University_Management_System.Application.Commands.Students;
using University_Management_System.Application.Dtos.StudentDtos;
using University_Management_System.Domain.Entities.Identity;
using University_Management_System.Domain.Entities.Models;
using University_Management_System.Domain.Enums;

namespace University_Management_System.Application.Mapping.Profiles
{
    public class StudentMappingProfile : Profile
    {
        public StudentMappingProfile()
        {
            // ────────────────────────────────────────────────────────────────────────
            // ENTITY TO DTO
            // ────────────────────────────────────────────────────────────────────────

            CreateMap<Student, StudentDto>()
                // ─── From Student (Id is the same as User.Id) ────────────────────
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.AcademicCode, opt => opt.MapFrom(src => src.AcademicCode))
                .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.Level))
                .ForMember(dest => dest.TotalCredits, opt => opt.MapFrom(src => src.TotalCredits))
                .ForMember(dest => dest.AllowedCredits, opt => opt.MapFrom(src => src.AllowedCredits))
                .ForMember(dest => dest.TotalGPA, opt => opt.MapFrom(src => src.TotalGPA))

                // ─── From User (navigation property) ──────────────────────────────
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => 
                    src.User != null ? src.User.Name : string.Empty))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => 
                    src.User != null ? src.User.UserName : string.Empty))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => 
                    src.User != null ? src.User.PhoneNumber : string.Empty))
                .ForMember(dest => dest.ProfilePicture, opt => opt.MapFrom(src => 
                    src.User != null ? src.User.ProfilePicture : string.Empty))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => 
                    src.User != null ? src.User.Address : string.Empty))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => 
                    src.User != null ? src.User.Gender : Gender.Male))

                // ─── From Department ──────────────────────────────────────────────
                .ForMember(dest => dest.Department, opt => opt.MapFrom(src => 
                    src.Department != null ? src.Department.Name : string.Empty))

                // ─── From Specialization ──────────────────────────────────────────
                .ForMember(dest => dest.Specialization, opt => opt.MapFrom(src => 
                    src.Specialization != null ? src.Specialization.Name : null));

            // ────────────────────────────────────────────────────────────────────────
            // DTO TO ENTITY
            // ────────────────────────────────────────────────────────────────────────

            // CreateStudentDto → Student
            CreateMap<CreateStudentDto, Student>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())           // Set in handler (same as User.Id)
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Department, opt => opt.Ignore())
                .ForMember(dest => dest.Specialization, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Registrations, opt => opt.Ignore())
                .ForMember(dest => dest.AcademicSchedules, opt => opt.Ignore())
                .ForMember(dest => dest.CourseUploads, opt => opt.Ignore())
                .ForMember(dest => dest.SemesterGPAs, opt => opt.Ignore())
                .ForMember(dest => dest.StudentStudyYears, opt => opt.Ignore())
                .ForMember(dest => dest.AcademicCode, opt => opt.MapFrom(src => src.AcademicCode))
                .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.Level))
                .ForMember(dest => dest.TotalCredits, opt => opt.MapFrom(src => src.TotalCredits))
                .ForMember(dest => dest.AllowedCredits, opt => opt.MapFrom(src => src.AllowedCredits))
                .ForMember(dest => dest.TotalGPA, opt => opt.MapFrom(src => src.TotalGPA))
                .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.DepartmentId))
                .ForMember(dest => dest.SpecializationId, opt => opt.MapFrom(src => src.SpecializationId));

            // CreateStudentCommand → Student
            CreateMap<CreateStudentCommand, Student>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Department, opt => opt.Ignore())
                .ForMember(dest => dest.Specialization, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Registrations, opt => opt.Ignore())
                .ForMember(dest => dest.AcademicSchedules, opt => opt.Ignore())
                .ForMember(dest => dest.CourseUploads, opt => opt.Ignore())
                .ForMember(dest => dest.SemesterGPAs, opt => opt.Ignore())
                .ForMember(dest => dest.StudentStudyYears, opt => opt.Ignore())
                .ForMember(dest => dest.AcademicCode, opt => opt.MapFrom(src => src.Dto.AcademicCode))
                .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.Dto.Level))
                .ForMember(dest => dest.TotalCredits, opt => opt.MapFrom(src => src.Dto.TotalCredits))
                .ForMember(dest => dest.AllowedCredits, opt => opt.MapFrom(src => src.Dto.AllowedCredits))
                .ForMember(dest => dest.TotalGPA, opt => opt.MapFrom(src => src.Dto.TotalGPA))
                .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.Dto.DepartmentId))
                .ForMember(dest => dest.SpecializationId, opt => opt.MapFrom(src => src.Dto.SpecializationId));

            // UpdateStudentDto → Student
            CreateMap<UpdateStudentDto, Student>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Department, opt => opt.Ignore())
                .ForMember(dest => dest.Specialization, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Registrations, opt => opt.Ignore())
                .ForMember(dest => dest.AcademicSchedules, opt => opt.Ignore())
                .ForMember(dest => dest.CourseUploads, opt => opt.Ignore())
                .ForMember(dest => dest.SemesterGPAs, opt => opt.Ignore())
                .ForMember(dest => dest.StudentStudyYears, opt => opt.Ignore())
                .ForMember(dest => dest.Level, opt => opt.Ignore())
                .ForMember(dest => dest.DepartmentId, opt => opt.Ignore())
                .ForMember(dest => dest.SpecializationId, opt => opt.Condition(src => src.SpecializationId.HasValue));
        }
    }
}