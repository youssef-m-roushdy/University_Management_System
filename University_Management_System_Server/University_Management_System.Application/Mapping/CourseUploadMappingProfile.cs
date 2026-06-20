using AutoMapper;
using University_Management_System.Application.Dtos.CourseUploadDtos;
using University_Management_System.Domain.Entities.Models;

namespace University_Management_System.Application.Mapping.Profiles
{
    public class CourseUploadMappingProfile : Profile
    {
        public CourseUploadMappingProfile()
        {
            CreateMap<CourseUpload, CourseUploadDto>().ReverseMap();
            CreateMap<CreateCourseUploadDto, CourseUpload>();
        }
    }
}