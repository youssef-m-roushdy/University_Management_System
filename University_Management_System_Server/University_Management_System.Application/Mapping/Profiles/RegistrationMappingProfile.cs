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
                .ForMember(dest => dest.StudentId, opt => opt.MapFrom(src => src.StudentId))
                .ForMember(dest => dest.CourseId, opt => opt.MapFrom(src => src.CourseId))
                .ForMember(dest => dest.SemesterId, opt => opt.MapFrom(src => src.SemesterId))
                .ForMember(dest => dest.StudyYearId, opt => opt.MapFrom(src => src.StudyYearId))
                .ForMember(dest => dest.Grade, opt => opt.MapFrom(src => src.Grade))
                .ForMember(dest => dest.IsPassed, opt => opt.MapFrom(src => src.IsPassed))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.Progress, opt => opt.MapFrom(src => src.Progress))
                .ForMember(dest => dest.RegisteredAt, opt => opt.MapFrom(src => src.RegisteredAt));

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