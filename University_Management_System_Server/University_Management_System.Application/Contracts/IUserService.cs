using Microsoft.AspNetCore.Http;
using University_Management_System.Application.Dtos.UserDtos;
using University_Management_System.Domain.Entities.Identity;
using University_Management_System.Domain.Queries;

namespace University_Management_System.Application.Contracts
{
    public interface IUserService
    {
        // Profile
        Task<UserProfileDetailsDto> GetUserProfileByIdAsync(string userId);
        Task<UserProfileDetailsDto> GetUserProfileByEmailAsync(string email);
        
        // Profile Picture
        Task<string> UpdateProfilePictureAsync(string userId, UpdateProfilePictureDto dto);
        Task DeleteProfilePictureAsync(string userId);
        
        // User Management
        Task<IEnumerable<UserBasicDto>> GetAllUsersAsync(string userId, UserQueries query);
        Task<(IEnumerable<UserBasicDto> Data, int TotalCount)> GetUsersAsync(string userId, UserQueries query);
        
        Task<UserBasicDto> GetUserByIdAsync(string userId);
        Task<UserBasicDto> GetUserByEmailAsync(string email);
        Task<User> CreateUserAsync(CreateUserDto dto, string role);
        Task UpdateUserAsync(string userId, UpdateUserDto dto);
        Task ActivateUserAsync(string userId);
        Task DeactivateUserAsync(string userId);
        Task DeleteUserAsync(string userId);
    }
}