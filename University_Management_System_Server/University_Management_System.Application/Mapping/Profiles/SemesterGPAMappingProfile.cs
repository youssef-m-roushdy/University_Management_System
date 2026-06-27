using AutoMapper;
using University_Management_System.Domain.Entities.Models;
using University_Management_System.Domain.Enums;
using University_Management_System.Application.Dtos.SemesterGPADtos;

namespace University_Management_System.Application.Mappings
{
    public class SemesterGPAMappingProfile : Profile
    {
        public SemesterGPAMappingProfile()
        {
            // Entity to DTO mapping
            CreateMap<SemesterGPA, SemesterGPADto>()
                .ForMember(dest => dest.StudentName, 
                    opt => opt.MapFrom(src => src.Student != null && src.Student.User != null 
                        ? src.Student.User.Name 
                        : "Unknown"))
                .ForMember(dest => dest.AcademicCode, 
                    opt => opt.MapFrom(src => src.Student != null 
                        ? src.Student.AcademicCode 
                        : "Unknown"))
                .ForMember(dest => dest.SemesterTitle, 
                    opt => opt.MapFrom(src => src.Semester != null 
                        ? src.Semester.Title.ToString() 
                        : "Unknown"))
                .ForMember(dest => dest.DepartmentName, 
                    opt => opt.MapFrom(src => src.Student != null && src.Student.Department != null 
                        ? src.Student.Department.Name 
                        : "Unknown"))    
                .ForMember(dest => dest.TotalCreditHours, 
                    opt => opt.MapFrom(src => src.TotalCreditHours));             
        }
    }
}