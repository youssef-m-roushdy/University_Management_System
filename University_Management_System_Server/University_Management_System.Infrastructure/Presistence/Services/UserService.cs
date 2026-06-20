using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using University_Management_System.Application.Contracts;
using University_Management_System.Application.Dtos.UserDtos;
using University_Management_System.Domain.Entities.Identity;
using University_Management_System.Domain.Queries;
using University_Management_System.Infrastructure.Presistence.Extensions;
using University_Management_System.Shared.Exceptions;

namespace University_Management_System.Infrastructure.Presistence.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly UniversityDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;

        public UserService(
            UserManager<User> userManager,
            ICloudinaryService cloudinaryService,
            UniversityDbContext dbContext,
            IMapper mapper,
            ILogger<UserService> logger)
        {
            _userManager = userManager;
            _cloudinaryService = cloudinaryService;
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET USER PROFILE BY ID
        // ────────────────────────────────────────────────────────────────────────
        public async Task<UserProfileDetailsDto> GetUserProfileByIdAsync(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    throw new NotFoundException($"User with ID '{userId}' not found.");

                var roles = await _userManager.GetRolesAsync(user);

                // ✅ Use AutoMapper
                var userProfile = _mapper.Map<UserProfileDetailsDto>(user);
                userProfile.Roles = roles.ToList();

                return userProfile;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user profile by ID: {UserId}", userId);
                throw;
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET USER PROFILE BY EMAIL
        // ────────────────────────────────────────────────────────────────────────
        public async Task<UserProfileDetailsDto> GetUserProfileByEmailAsync(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                    throw new NotFoundException($"User with email '{email}' not found.");

                var roles = await _userManager.GetRolesAsync(user);

                // ✅ Use AutoMapper
                var userProfile = _mapper.Map<UserProfileDetailsDto>(user);
                userProfile.Roles = roles.ToList();

                return userProfile;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user profile by email: {Email}", email);
                throw;
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET ALL USERS (Without Pagination)
        // ────────────────────────────────────────────────────────────────────────
        public async Task<IEnumerable<UserBasicDto>> GetAllUsersAsync(string userId, UserQueries query)
        {
            try
            {
                var usersQuery = BuildUserQuery(userId, query);
                var users = await usersQuery.ToListAsync();
                
                // ✅ Use AutoMapper
                var userDtos = _mapper.Map<IEnumerable<UserBasicDto>>(users);
                
                // Get roles for each user
                foreach (var userDto in userDtos)
                {
                    var user = users.First(u => u.Id == userDto.Id);
                    var roles = await _userManager.GetRolesAsync(user);
                    userDto.Roles = roles.ToList();
                }

                return userDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all users");
                throw;
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET USERS (With Pagination)
        // ────────────────────────────────────────────────────────────────────────
        public async Task<(IEnumerable<UserBasicDto> Data, int TotalCount)> GetUsersAsync(
            string userId,
            UserQueries query)
        {
            try
            {
                var usersQuery = BuildUserQuery(userId, query);

                var totalCount = await usersQuery.CountAsync();

                var users = await usersQuery
                    .Skip((query.PageNumber - 1) * query.PageSize)
                    .Take(query.PageSize)
                    .ToListAsync();

                // ✅ Use AutoMapper
                var userDtos = _mapper.Map<IEnumerable<UserBasicDto>>(users);

                // Get roles for each user
                foreach (var userDto in userDtos)
                {
                    var user = users.First(u => u.Id == userDto.Id);
                    var roles = await _userManager.GetRolesAsync(user);
                    userDto.Roles = roles.ToList();
                }

                return (userDtos, totalCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting users with pagination");
                throw;
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET USER BY ID (Basic)
        // ────────────────────────────────────────────────────────────────────────
        public async Task<UserBasicDto> GetUserByIdAsync(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    throw new NotFoundException($"User with ID '{userId}' not found.");

                var roles = await _userManager.GetRolesAsync(user);

                // ✅ Use AutoMapper
                var userDto = _mapper.Map<UserBasicDto>(user);
                userDto.Roles = roles.ToList();

                return userDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user by ID: {UserId}", userId);
                throw;
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET USER BY EMAIL (Basic)
        // ────────────────────────────────────────────────────────────────────────
        public async Task<UserBasicDto> GetUserByEmailAsync(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                    throw new NotFoundException($"User with email '{email}' not found.");

                var roles = await _userManager.GetRolesAsync(user);

                // ✅ Use AutoMapper
                var userDto = _mapper.Map<UserBasicDto>(user);
                userDto.Roles = roles.ToList();

                return userDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user by email: {Email}", email);
                throw;
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // UPDATE USER
        // ────────────────────────────────────────────────────────────────────────
        public async Task UpdateUserAsync(string userId, UpdateUserDto dto)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    throw new NotFoundException($"User with ID '{userId}' not found.");

                // ✅ Use AutoMapper to update user from dto
                _mapper.Map(dto, user);
                user.UpdatedAt = DateTime.UtcNow;

                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new Exception($"Failed to update user: {errors}");
                }

                _logger.LogInformation("User updated successfully: {UserId}", userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user: {UserId}", userId);
                throw;
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // ACTIVATE USER
        // ────────────────────────────────────────────────────────────────────────
        public async Task ActivateUserAsync(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    throw new NotFoundException($"User with ID '{userId}' not found.");

                user.IsActive = true;
                user.UpdatedAt = DateTime.UtcNow;

                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new Exception($"Failed to activate user: {errors}");
                }

                _logger.LogInformation("User activated successfully: {UserId}", userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error activating user: {UserId}", userId);
                throw;
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // DEACTIVATE USER
        // ────────────────────────────────────────────────────────────────────────
        public async Task DeactivateUserAsync(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    throw new NotFoundException($"User with ID '{userId}' not found.");

                user.IsActive = false;
                user.UpdatedAt = DateTime.UtcNow;

                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new Exception($"Failed to deactivate user: {errors}");
                }

                _logger.LogInformation("User deactivated successfully: {UserId}", userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deactivating user: {UserId}", userId);
                throw;
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // DELETE USER
        // ────────────────────────────────────────────────────────────────────────
        public async Task DeleteUserAsync(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    throw new NotFoundException($"User with ID '{userId}' not found.");

                // Delete profile picture from Cloudinary if exists
                if (!string.IsNullOrEmpty(user.ProfilePicture))
                {
                    try
                    {
                        var publicId = _cloudinaryService.ExtractPublicIdFromUrl(user.ProfilePicture);
                        await _cloudinaryService.DeleteImageAsync(publicId);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Failed to delete profile picture from Cloudinary for user: {UserId}", userId);
                        // Continue with user deletion even if image deletion fails
                    }
                }

                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new Exception($"Failed to delete user: {errors}");
                }

                _logger.LogInformation("User deleted successfully: {UserId}", userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user: {UserId}", userId);
                throw;
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // UPDATE PROFILE PICTURE
        // ────────────────────────────────────────────────────────────────────────
        public async Task<string> UpdateProfilePictureAsync(string userId, UpdateProfilePictureDto dto)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    throw new NotFoundException($"User with ID '{userId}' not found.");

                // Delete old profile picture if exists
                if (!string.IsNullOrEmpty(user.ProfilePicture))
                {
                    try
                    {
                        var publicId = _cloudinaryService.ExtractPublicIdFromUrl(user.ProfilePicture);
                        await _cloudinaryService.DeleteImageAsync(publicId);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Failed to delete old profile picture from Cloudinary for user: {UserId}", userId);
                        // Continue with upload even if deletion fails
                    }
                }

                user.ProfilePicture = await _cloudinaryService.UploadUserProfilePictureAsync(dto.ProfilePicture, userId);
                user.UpdatedAt = DateTime.UtcNow;

                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(e => e.Description).ToList();
                    throw new ValidationException(errors);
                }

                _logger.LogInformation("Profile picture updated successfully for user: {UserId}", userId);
                return user.ProfilePicture;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating profile picture for user: {UserId}", userId);
                throw;
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // DELETE PROFILE PICTURE
        // ────────────────────────────────────────────────────────────────────────
        public async Task DeleteProfilePictureAsync(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    throw new NotFoundException($"User with ID '{userId}' not found.");

                if (string.IsNullOrEmpty(user.ProfilePicture))
                    throw new ValidationException(new List<string> { "User does not have a profile picture to delete." });

                // Delete the image from Cloudinary
                var publicId = _cloudinaryService.ExtractPublicIdFromUrl(user.ProfilePicture);
                await _cloudinaryService.DeleteImageAsync(publicId);

                user.ProfilePicture = null;
                user.UpdatedAt = DateTime.UtcNow;

                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(e => e.Description).ToList();
                    throw new ValidationException(errors);
                }

                _logger.LogInformation("Profile picture deleted successfully for user: {UserId}", userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting profile picture for user: {UserId}", userId);
                throw;
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // PRIVATE HELPERS
        // ────────────────────────────────────────────────────────────────────────
        private IQueryable<User> BuildUserQuery(string userId, UserQueries query)
        {
            var usersQuery = _dbContext.Users
                .Where(u => u.Id != userId)
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .AsQueryable();

            // Filter by Role
            if (!string.IsNullOrEmpty(query.Role))
                usersQuery = usersQuery.Where(u => u.UserRoles.Any(ur => ur.Role.Name == query.Role));

            // Filter by Name
            if (!string.IsNullOrEmpty(query.Name))
                usersQuery = usersQuery.Where(u => u.Name.Contains(query.Name));

            // Filter by Email
            if (!string.IsNullOrEmpty(query.Email))
                usersQuery = usersQuery.Where(u => u.Email.Contains(query.Email));

            // Filter by Gender
            if (query.Gender.HasValue)
                usersQuery = usersQuery.Where(u => u.Gender == query.Gender.Value);

            // Filter by IsActive
            if (query.IsActive.HasValue)
                usersQuery = usersQuery.Where(u => u.IsActive == query.IsActive.Value);

            // Apply SearchTerm
            if (!string.IsNullOrEmpty(query.SearchTerm))
            {
                var searchTerm = query.SearchTerm.ToLower();
                usersQuery = usersQuery.Where(u =>
                    u.Name.ToLower().Contains(searchTerm) ||
                    u.Email.ToLower().Contains(searchTerm));
            }

            // Apply sorting
            usersQuery = query.SortBy?.ToLower() switch
            {
                "name" => query.SortDirection == SortDirection.Ascending
                    ? usersQuery.OrderBy(u => u.Name)
                    : usersQuery.OrderByDescending(u => u.Name),
                "email" => query.SortDirection == SortDirection.Ascending
                    ? usersQuery.OrderBy(u => u.Email)
                    : usersQuery.OrderByDescending(u => u.Email),
                "createdat" => query.SortDirection == SortDirection.Ascending
                    ? usersQuery.OrderBy(u => u.CreatedAt)
                    : usersQuery.OrderByDescending(u => u.CreatedAt),
                _ => query.SortDirection == SortDirection.Ascending
                    ? usersQuery.OrderBy(u => u.Name)
                    : usersQuery.OrderByDescending(u => u.Name)
            };

            return usersQuery;
        }
    }
}