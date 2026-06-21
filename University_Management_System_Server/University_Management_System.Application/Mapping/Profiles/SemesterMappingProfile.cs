using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using University_Management_System.Application.Dtos.SemesterDtos;
using University_Management_System.Domain.Entities.Models;

namespace University_Management_System.Application.Mapping.Profiles
{
    public class SemesterMappingProfile : Profile
    {
        public SemesterMappingProfile()
        {
            // Add your mapping configurations here
            CreateMap<Semester, SemesterDto>().ReverseMap();
        }
    }
}