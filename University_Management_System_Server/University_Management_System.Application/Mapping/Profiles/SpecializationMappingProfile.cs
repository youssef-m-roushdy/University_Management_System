using AutoMapper;
using University_Management_System.Application.Dtos.SpecializationDtos;
using University_Management_System.Domain.Entities.Models;

namespace University_Management_System.Application.Mapping.Profiles
{
    public class SpecializationMappingProfile : Profile
    {
        public SpecializationMappingProfile()
        {
            // ────────────────────────────────────────────────────────────────────────
            // ENTITY TO DTO
            // ────────────────────────────────────────────────────────────────────────

            CreateMap<Specialization, SpecializationDto>()
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => 
                    src.Department != null ? src.Department.Name : string.Empty))
                .ForMember(dest => dest.DepartmentCode, opt => opt.MapFrom(src => 
                    src.Department != null ? src.Department.Code : string.Empty))
                .ForMember(dest => dest.StudentCount, opt => opt.MapFrom(src => 
                    src.Students != null ? src.Students.Count : 0))
                .ForMember(dest => dest.CourseCount, opt => opt.MapFrom(src => 
                    src.SpecializationCourses != null ? src.SpecializationCourses.Count : 0));

            // ────────────────────────────────────────────────────────────────────────
            // DTO TO ENTITY
            // ────────────────────────────────────────────────────────────────────────

            CreateMap<CreateSpecializationDto, Specialization>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Department, opt => opt.Ignore())
                .ForMember(dest => dest.SpecializationCourses, opt => opt.Ignore())
                .ForMember(dest => dest.Students, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            CreateMap<UpdateSpecializationDto, Specialization>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Department, opt => opt.Ignore())
                .ForMember(dest => dest.SpecializationCourses, opt => opt.Ignore())
                .ForMember(dest => dest.Students, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Name, opt => opt.Ignore())
                .ForMember(dest => dest.Description, opt => opt.Ignore())
                .ForMember(dest => dest.DepartmentId, opt => opt.Ignore());
        }
    }
}