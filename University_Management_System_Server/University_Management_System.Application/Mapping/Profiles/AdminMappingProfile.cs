using AutoMapper;
using University_Management_System.Application.Dtos.AdminDtos;
using University_Management_System.Domain.Entities.Identity;
using University_Management_System.Domain.Enums;

namespace University_Management_System.Application.Mapping.Profiles
{
    public class AdminMappingProfile : Profile
    {
        public AdminMappingProfile()
        {
            // ────────────────────────────────────────────────────────────────────────
            // ENTITY TO DTO
            // ────────────────────────────────────────────────────────────────────────

            CreateMap<Admin, AdminDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.User != null ? src.User.Name : string.Empty))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? src.User.UserName : string.Empty))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User != null ? src.User.Email : string.Empty))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.User != null ? src.User.PhoneNumber : null))
                .ForMember(dest => dest.ProfilePicture, opt => opt.MapFrom(src => src.User != null ? src.User.ProfilePicture : null))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.User != null ? src.User.Address : null))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.User != null ? src.User.Gender : Gender.Male))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.User != null ? src.User.IsActive : false))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt));

            // ────────────────────────────────────────────────────────────────────────
            // CREATE ADMIN DTO → ADMIN
            // ────────────────────────────────────────────────────────────────────────

            CreateMap<CreateAdminDto, Admin>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.AcademicSchedules, opt => opt.Ignore());
        }
    }
}