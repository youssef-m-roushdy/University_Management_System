using AutoMapper;
using University_Management_System.Application.Dtos.AcademicSheduleDtos;
using University_Management_System.Application.Dtos.CourseDtos;
using University_Management_System.Application.Dtos.CourseUploadDtos;
using University_Management_System.Application.Dtos.DepartmentDtos;
using University_Management_System.Application.Dtos.DepartmentDtos.FeeDtos;
using University_Management_System.Application.Dtos.RegistrationDtos;
using University_Management_System.Application.Dtos.StudyYearDtos;
using University_Management_System.Application.Dtos.UserDtos;
using University_Management_System.Application.Dtos.StudentStudyYearDtos;
using University_Management_System.Domain.Entities.Identity;
using University_Management_System.Domain.Entities.Models;


namespace University_Management_System.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Department mappings
            CreateMap<Department, DepartmentDto>().ReverseMap();
            CreateMap<Department, DepartmentDetailsDto>().ReverseMap();
            CreateMap<CreateDepartmentDto, Department>();
            CreateMap<UpdateDepartmentDto, Department>();

            // AcademicSchedule mappings
            CreateMap<AcademicSchedule, AcademicSchedulesDto>().ReverseMap();
            CreateMap<CreateSemesterAcademicScheduleDto, AcademicSchedule>();

            // Fee mappings ✅ (only once, with full config)
            CreateMap<Fee, FeeDto>()
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department != null ? src.Department.Name : null))
                .ReverseMap();
            CreateMap<CreateFeeDto, Fee>();

            // Course mappings
            CreateMap<Course, CourseDto>().ReverseMap();
            CreateMap<Course, CourseWithDepartmentDto>()
                .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.Department.Code))
                .ReverseMap();
            CreateMap<CreateCourseDto, Course>();
            CreateMap<Course, DepartmentCourseDto>().ReverseMap();

            // CourseUpload mappings
            CreateMap<CourseUpload, CourseUploadDto>().ReverseMap();
            CreateMap<CreateCourseUploadDto, CourseUpload>();

            // Registration mappings
            CreateMap<Registration, RegistrationCourseDto>();
            CreateMap<Registration, RegistrationDto>()
                .ForMember(dest => dest.Student, opt => opt.MapFrom(src => src.Student.User))
                .ForMember(dest => dest.Course, opt => opt.MapFrom(src => src.Course))
                .ReverseMap();

            // StudyYear mappings
            CreateMap<StudyYear, StudyYearDto>().ReverseMap();

            // StudentStudyYear mappings
            CreateMap<StudentStudyYear, StudentStudyYearDetailsDto>();

            // User mappings
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src =>
                    src.UserRoles != null
                    ? src.UserRoles.Select(ur => ur.Role.Name).ToList()
                    : new List<string>()))
                .ReverseMap();

        }
    }
}
