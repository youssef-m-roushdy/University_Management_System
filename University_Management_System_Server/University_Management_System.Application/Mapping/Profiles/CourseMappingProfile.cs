using AutoMapper;
using University_Management_System.Application.Dtos.CourseDtos;
using University_Management_System.Domain.Entities.Models;

namespace University_Management_System.Application.Mapping.Profiles
{
    public class CourseMappingProfile : Profile
    {
        public CourseMappingProfile()
        {
            CreateMap<Course, CourseDto>().ReverseMap();
            
            CreateMap<Course, CourseWithDepartmentDto>()
                .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.Department.Code))
                .ReverseMap();
            
            CreateMap<CreateCourseDto, Course>();
            CreateMap<Course, DepartmentCourseDto>().ReverseMap();
        }
    }
}