using AutoMapper;
using University_Management_System.Application.Dtos.StudentStudyYearDtos;
using University_Management_System.Application.Dtos.StudyYearDtos;
using University_Management_System.Domain.Entities.Models;

namespace University_Management_System.Application.Mapping.Profiles
{
    public class StudyYearMappingProfile : Profile
    {
        public StudyYearMappingProfile()
        {
            CreateMap<StudyYear, StudyYearDto>().ReverseMap();
            CreateMap<StudentStudyYear, StudentStudyYearDetailsDto>();
        }
    }
}