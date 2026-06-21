using AutoMapper;
using University_Management_System.Application.Dtos.DepartmentDtos.FeeDtos;
using University_Management_System.Domain.Entities.Models;

namespace University_Management_System.Application.Mapping.Profiles
{
    public class FeeMappingProfile : Profile
    {
        public FeeMappingProfile()
        {
            CreateMap<Fee, FeeDto>()
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department != null ? src.Department.Name : null))
                .ReverseMap();
            CreateMap<CreateFeeDto, Fee>();
        }
    }
}