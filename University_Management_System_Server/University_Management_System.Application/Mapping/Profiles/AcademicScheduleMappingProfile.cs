using AutoMapper;
using University_Management_System.Application.Dtos.AcademicSheduleDtos;
using University_Management_System.Domain.Entities.Models;

namespace University_Management_System.Application.Mapping.Profiles
{
    public class AcademicScheduleMappingProfile : Profile
    {
        public AcademicScheduleMappingProfile()
        {
            CreateMap<AcademicSchedule, AcademicSchedulesDto>().ReverseMap();
            CreateMap<CreateSemesterAcademicScheduleDto, AcademicSchedule>();
        }
    }
}