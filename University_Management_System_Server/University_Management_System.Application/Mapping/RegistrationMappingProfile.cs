using AutoMapper;
using University_Management_System.Application.Dtos.RegistrationDtos;
using University_Management_System.Domain.Entities.Models;

namespace University_Management_System.Application.Mapping.Profiles
{
    public class RegistrationMappingProfile : Profile
    {
        public RegistrationMappingProfile()
        {
            CreateMap<Registration, RegistrationCourseDto>();
            
            CreateMap<Registration, RegistrationDto>()
                .ForMember(dest => dest.Student, opt => opt.MapFrom(src => src.Student.User))
                .ForMember(dest => dest.Course, opt => opt.MapFrom(src => src.Course))
                .ReverseMap();
        }
    }
}