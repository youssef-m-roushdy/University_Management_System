using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using University_Management_System.Application.Contracts;
using University_Management_System.Application.Dtos.UserDtos;
using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Entities.Identity;
using University_Management_System.Domain.Enums;
using University_Management_System.Domain.Queries;
using University_Management_System.Shared.Exceptions;
using University_Management_System.Infrastructure.Presistence.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace University_Management_System.Infrastructure.Presistence.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly UniversityDbContext _dbContext;
        private readonly IMapper _mapper;

        public UserService(UserManager<User> userManager, ILogger<UserService> logger, ICloudinaryService cloudinaryService, IUnitOfWork unitOfWork, UniversityDbContext dbContext, IMapper mapper)
        {
            _userManager = userManager;
            _cloudinaryService = cloudinaryService;
            _unitOfWork = unitOfWork;
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserWithDepartmentDto>> GetAllUsers(string userId, UserQueries query)
        {
            
            var usersQuery = _dbContext.Users
                .Where(u => u.Id != userId)
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .Include(u => u.Student).ThenInclude(s => s.Department)
                .Include(u => u.Instructor).ThenInclude(i => i.Department)
                .Include(u => u.Assistant).ThenInclude(a => a.Department)
                .AsQueryable();

            // Filter by Role
            if (!string.IsNullOrEmpty(query.Role))
                usersQuery = usersQuery.Where(u => u.UserRoles.Any(ur => ur.Role.Name == query.Role));

            // Filter by Academic Code
            if (!string.IsNullOrEmpty(query.AcademicCode))
                usersQuery = usersQuery.Where(u => u.Student != null && u.Student.AcademicCode == query.AcademicCode);

            // Filter by Name
            if (!string.IsNullOrEmpty(query.Name))
                usersQuery = usersQuery.Where(u => u.Name.Contains(query.Name));

            // Filter by Gender
            if (query.Gender.HasValue)
                usersQuery = usersQuery.Where(u => u.Gender == query.Gender.Value);

            // Filter by Level
            if (query.Level.HasValue)
                usersQuery = usersQuery.Where(u => u.Student != null && u.Student.Level == query.Level.Value);

            // Filter by DepartmentId
            if (query.DepartmentId.HasValue)
                usersQuery = usersQuery.Where(u => (u.Student != null && u.Student.DepartmentId == query.DepartmentId.Value) || (u.Instructor != null && u.Instructor.DepartmentId == query.DepartmentId.Value) || (u.Assistant != null && u.Assistant.DepartmentId == query.DepartmentId.Value));

            var users = await usersQuery.ToListAsync();

            return _mapper.Map<IEnumerable<UserWithDepartmentDto>>(users);
        }

        public async Task<(IEnumerable<UserWithDepartmentDto> Data, int TotalCount)> GetAllUsersWithPaginationAsync(string userId, UserQueries query)
        {
            var usersQuery = _dbContext.Users
                .Where(u => u.Id != userId)
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .Include(u => u.Student).ThenInclude(s => s.Department)
                .Include(u => u.Instructor).ThenInclude(i => i.Department)
                .Include(u => u.Assistant).ThenInclude(a => a.Department)
                .AsNoTracking()
                .AsQueryable();

            // Filter by Role
            if (!string.IsNullOrEmpty(query.Role))
                usersQuery = usersQuery.Where(u => u.UserRoles.Any(ur => ur.Role.Name == query.Role));

            // Filter by Academic Code
            if (!string.IsNullOrEmpty(query.AcademicCode))
                usersQuery = usersQuery.Where(u => u.Student != null && u.Student.AcademicCode.Contains(query.AcademicCode));

            // Filter by Name
            if (!string.IsNullOrEmpty(query.Name))
                usersQuery = usersQuery.Where(u => u.Name.Contains(query.Name));

            // Filter by Gender
            if (query.Gender.HasValue)
                usersQuery = usersQuery.Where(u => u.Gender == query.Gender.Value);

            // Filter by Level
            if (query.Level.HasValue)
                usersQuery = usersQuery.Where(u => u.Student != null && u.Student.Level == query.Level.Value);


            // Filter by DepartmentId
            if (query.DepartmentId.HasValue)
                usersQuery = usersQuery.Where(u => (u.Student != null && u.Student.DepartmentId == query.DepartmentId.Value) || (u.Instructor != null && u.Instructor.DepartmentId == query.DepartmentId.Value) || (u.Assistant != null && u.Assistant.DepartmentId == query.DepartmentId.Value));

            // Apply sorting
            usersQuery = usersQuery.ApplySorting(query.SortBy ?? "Name", query.SortDirection);

            // Get total count before pagination
            var totalCount = await usersQuery.CountAsync();

            // Apply pagination
            usersQuery = usersQuery.ApplyPagination(query);

            var users = await usersQuery.ToListAsync();
            var result = _mapper.Map<IEnumerable<UserWithDepartmentDto>>(users);

            return (result, totalCount);
        }

        public async Task<IEnumerable<StudentUserDto>> GetUnGraduateStudentUsers(string userId, StudentQueries query)
        {
            
            var usersQuery = _dbContext.Users
                .Where(u => u.Id != userId && u.Student != null && u.Student.Level != Levels.Graduate)
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .Include(u => u.Student).ThenInclude(s => s.Department)
                .Include(u => u.Instructor).ThenInclude(i => i.Department)
                .Include(u => u.Assistant).ThenInclude(a => a.Department)
                .AsQueryable();

         
            usersQuery = usersQuery.Where(u => u.UserRoles.Any(ur => ur.Role.Name == "Student"));

            // Filter by Academic Code
            if (!string.IsNullOrEmpty(query.AcademicCode))
                usersQuery = usersQuery.Where(u => u.Student != null && u.Student.AcademicCode.Contains(query.AcademicCode));

            // Filter by Name
            if (!string.IsNullOrEmpty(query.Name))
                usersQuery = usersQuery.Where(u => u.Name.Contains(query.Name));

            // Filter by Gender
            if (query.Gender.HasValue)
                usersQuery = usersQuery.Where(u => u.Gender == query.Gender.Value);

            // Filter by Level
            if (query.Level.HasValue)
                usersQuery = usersQuery.Where(u => u.Student != null && u.Student.Level == query.Level.Value);

            // Filter by Min GPA
            if (query.MinGPA.HasValue)
                usersQuery = usersQuery.Where(u => u.Student != null && u.Student.TotalGPA >= query.MinGPA.Value);

            // Filter by Max GPA
            if (query.MaxGPA.HasValue)
                usersQuery = usersQuery.Where(u => u.Student != null && u.Student.TotalGPA <= query.MaxGPA.Value);

            // Filter by Min Credits
            if (query.MinCredits.HasValue)
                usersQuery = usersQuery.Where(u => u.Student != null && u.Student.TotalCredits >= query.MinCredits.Value);

            // Filter by Max Credits
            if (query.MaxCredits.HasValue)
                usersQuery = usersQuery.Where(u => u.Student != null && u.Student.TotalCredits <= query.MaxCredits.Value);

            // Filter by DepartmentId
            if (query.DepartmentId.HasValue)
                usersQuery = usersQuery.Where(u => (u.Student != null && u.Student.DepartmentId == query.DepartmentId.Value) || (u.Instructor != null && u.Instructor.DepartmentId == query.DepartmentId.Value) || (u.Assistant != null && u.Assistant.DepartmentId == query.DepartmentId.Value));

            var users = await usersQuery.ToListAsync();

            return _mapper.Map<IEnumerable<StudentUserDto>>(users);
        }

        public async Task<(IEnumerable<StudentUserDto> Data, int TotalCount)> GetUnGraduateStudentUsersWithPaginationAsync(string userId, StudentQueries query)
        {
            var usersQuery = _dbContext.Users
                .Where(u => u.Id != userId && u.Student != null && u.Student.Level != Levels.Graduate)
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .Include(u => u.Student).ThenInclude(s => s.Department)
                .Include(u => u.Instructor).ThenInclude(i => i.Department)
                .Include(u => u.Assistant).ThenInclude(a => a.Department)
                .AsNoTracking()
                .AsQueryable();

            usersQuery = usersQuery.Where(u => u.UserRoles.Any(ur => ur.Role.Name == "Student"));

            // Filter by Academic Code
            if (!string.IsNullOrEmpty(query.AcademicCode))
                usersQuery = usersQuery.Where(u => u.Student != null && u.Student.AcademicCode.Contains(query.AcademicCode));

            // Filter by Name
            if (!string.IsNullOrEmpty(query.Name))
                usersQuery = usersQuery.Where(u => u.Name.Contains(query.Name));

            // Filter by Gender
            if (query.Gender.HasValue)
                usersQuery = usersQuery.Where(u => u.Gender == query.Gender.Value);

            // Filter by Level
            if (query.Level.HasValue)
                usersQuery = usersQuery.Where(u => u.Student != null && u.Student.Level == query.Level.Value);

            // Filter by Min GPA
            if (query.MinGPA.HasValue)
                usersQuery = usersQuery.Where(u => u.Student != null && u.Student.TotalGPA >= query.MinGPA.Value);

            // Filter by Max GPA
            if (query.MaxGPA.HasValue)
                usersQuery = usersQuery.Where(u => u.Student != null && u.Student.TotalGPA <= query.MaxGPA.Value);

            // Filter by Min Credits
            if (query.MinCredits.HasValue)
                usersQuery = usersQuery.Where(u => u.Student != null && u.Student.TotalCredits >= query.MinCredits.Value);

            // Filter by Max Credits
            if (query.MaxCredits.HasValue)
                usersQuery = usersQuery.Where(u => u.Student != null && u.Student.TotalCredits <= query.MaxCredits.Value);

            // Filter by DepartmentId
            if (query.DepartmentId.HasValue)
                usersQuery = usersQuery.Where(u => (u.Student != null && u.Student.DepartmentId == query.DepartmentId.Value) || (u.Instructor != null && u.Instructor.DepartmentId == query.DepartmentId.Value) || (u.Assistant != null && u.Assistant.DepartmentId == query.DepartmentId.Value));

            // Apply sorting
            usersQuery = usersQuery.ApplySorting(query.SortBy ?? "Name", query.SortDirection);

            // Get total count before pagination
            var totalCount = await usersQuery.CountAsync();

            // Apply pagination
            usersQuery = usersQuery.ApplyPagination(query);

            var users = await usersQuery.ToListAsync();
            var result = _mapper.Map<IEnumerable<StudentUserDto>>(users);

            return (result, totalCount);
        }

        public async Task<IEnumerable<StudentUserDto>> GetAllStudentUsers(string userId, StudentQueries query)
        {
            
            var usersQuery = _dbContext.Users
                .Where(u => u.Id != userId)
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .Include(u => u.Student).ThenInclude(s => s.Department)
                .Include(u => u.Instructor).ThenInclude(i => i.Department)
                .Include(u => u.Assistant).ThenInclude(a => a.Department)
                .AsQueryable();

         
            usersQuery = usersQuery.Where(u => u.UserRoles.Any(ur => ur.Role.Name == "Student"));

            // Filter by Academic Code
            if (!string.IsNullOrEmpty(query.AcademicCode))
                usersQuery = usersQuery.Where(u => u.Student != null && u.Student.AcademicCode.Contains(query.AcademicCode));

            // Filter by Name
            if (!string.IsNullOrEmpty(query.Name))
                usersQuery = usersQuery.Where(u => u.Name.Contains(query.Name));

            // Filter by Gender
            if (query.Gender.HasValue)
                usersQuery = usersQuery.Where(u => u.Gender == query.Gender.Value);

            // Filter by Level
            if (query.Level.HasValue)
                usersQuery = usersQuery.Where(u => u.Student != null && u.Student.Level == query.Level.Value);

            // Filter by Min GPA
            if (query.MinGPA.HasValue)
                usersQuery = usersQuery.Where(u => u.Student != null && u.Student.TotalGPA >= query.MinGPA.Value);

            // Filter by Max GPA
            if (query.MaxGPA.HasValue)
                usersQuery = usersQuery.Where(u => u.Student != null && u.Student.TotalGPA <= query.MaxGPA.Value);

            // Filter by Min Credits
            if (query.MinCredits.HasValue)
                usersQuery = usersQuery.Where(u => u.Student != null && u.Student.TotalCredits >= query.MinCredits.Value);

            // Filter by Max Credits
            if (query.MaxCredits.HasValue)
                usersQuery = usersQuery.Where(u => u.Student != null && u.Student.TotalCredits <= query.MaxCredits.Value);

            // Filter by DepartmentId
            if (query.DepartmentId.HasValue)
                usersQuery = usersQuery.Where(u => (u.Student != null && u.Student.DepartmentId == query.DepartmentId.Value) || (u.Instructor != null && u.Instructor.DepartmentId == query.DepartmentId.Value) || (u.Assistant != null && u.Assistant.DepartmentId == query.DepartmentId.Value));

            var users = await usersQuery.ToListAsync();

            return _mapper.Map<IEnumerable<StudentUserDto>>(users);
        }

        public async Task<(IEnumerable<StudentUserDto> Data, int TotalCount)> GetAllStudentUsersWithPaginationAsync(string userId, StudentQueries query)
        {
            var usersQuery = _dbContext.Users
                .Where(u => u.Id != userId)
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .Include(u => u.Student).ThenInclude(s => s.Department)
                .Include(u => u.Instructor).ThenInclude(i => i.Department)
                .Include(u => u.Assistant).ThenInclude(a => a.Department)
                .AsNoTracking()
                .AsQueryable();

            usersQuery = usersQuery.Where(u => u.UserRoles.Any(ur => ur.Role.Name == "Student"));

            // Filter by Academic Code
            if (!string.IsNullOrEmpty(query.AcademicCode))
                usersQuery = usersQuery.Where(u => u.Student != null && u.Student.AcademicCode.Contains(query.AcademicCode));

            // Filter by Name
            if (!string.IsNullOrEmpty(query.Name))
                usersQuery = usersQuery.Where(u => u.Name.Contains(query.Name));

            // Filter by Gender
            if (query.Gender.HasValue)
                usersQuery = usersQuery.Where(u => u.Gender == query.Gender.Value);

            // Filter by Level
            if (query.Level.HasValue)
                usersQuery = usersQuery.Where(u => u.Student != null && u.Student.Level == query.Level.Value);

            // Filter by Min GPA
            if (query.MinGPA.HasValue)
                usersQuery = usersQuery.Where(u => u.Student != null && u.Student.TotalGPA >= query.MinGPA.Value);

            // Filter by Max GPA
            if (query.MaxGPA.HasValue)
                usersQuery = usersQuery.Where(u => u.Student != null && u.Student.TotalGPA <= query.MaxGPA.Value);

            // Filter by Min Credits
            if (query.MinCredits.HasValue)
                usersQuery = usersQuery.Where(u => u.Student != null && u.Student.TotalCredits >= query.MinCredits.Value);

            // Filter by Max Credits
            if (query.MaxCredits.HasValue)
                usersQuery = usersQuery.Where(u => u.Student != null && u.Student.TotalCredits <= query.MaxCredits.Value);

            // Filter by DepartmentId
            if (query.DepartmentId.HasValue)
                usersQuery = usersQuery.Where(u => (u.Student != null && u.Student.DepartmentId == query.DepartmentId.Value) || (u.Instructor != null && u.Instructor.DepartmentId == query.DepartmentId.Value) || (u.Assistant != null && u.Assistant.DepartmentId == query.DepartmentId.Value));

            // Apply sorting
            usersQuery = usersQuery.ApplySorting(query.SortBy ?? "Name", query.SortDirection);

            // Get total count before pagination
            var totalCount = await usersQuery.CountAsync();

            // Apply pagination
            usersQuery = usersQuery.ApplyPagination(query);

            var users = await usersQuery.ToListAsync();
            var result = _mapper.Map<IEnumerable<StudentUserDto>>(users);

            return (result, totalCount);
        }

        public async Task<userProfileDetailsDto> GetUserProfileByAcademicCodeAsync(string academicCode)
        {
            try
            {
                var user = await _userManager.Users
                    
                    .FirstOrDefaultAsync(u => u.Student != null && u.Student.AcademicCode == academicCode);

                if (user == null)
                    throw new NotFoundException($"User with academic code '{academicCode}' not found.");

                var department = (user.Student?.DepartmentId ?? user.Instructor?.DepartmentId ?? user.Assistant?.DepartmentId).HasValue
                    ? await _unitOfWork.Departments.GetByIdAsync((user.Student?.DepartmentId ?? user.Instructor?.DepartmentId ?? user.Assistant?.DepartmentId).Value)
                    : null;
                if(department == null)
                    throw new NotFoundException($"Department with ID '{(user.Student?.DepartmentId ?? user.Instructor?.DepartmentId ?? user.Assistant?.DepartmentId)}' not found.");

                var roles = await _userManager.GetRolesAsync(user);

                return new userProfileDetailsDto
                {
                    Id = user.Id,
                    DisplayName = user.Name,
                    Email = user.Email,
                    UserName = user.UserName,
                    PhoneNumber = user.PhoneNumber,
                    ProfilePicture = user.ProfilePicture,
                    AcademicCode = user.Student != null ? user.Student.AcademicCode : string.Empty,
                    Level = user.Student != null ? user.Student.Level : default(Levels),
                    TotalCredits = user.Student != null ? user.Student.TotalCredits : 0,
                    AllowedCredits = user.Student != null ? user.Student.AllowedCredits : 0,
                    TotalGPA = user.Student != null ? user.Student.TotalGPA : 0m,
                    DepartmentName = department.Name,
                    Role = roles.FirstOrDefault(),
                    
                };
            }
            catch (Exception ex)
            {
                throw new InternalServerErrorException($"An error occurred while retrieving the user profile for academic code '{academicCode}'.", ex);
            }
        }

        public async Task<string> UpdateProfilePictureAsync(string userId, UpdateProfilePictureDto dto)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    throw new NotFoundException($"User with ID '{userId}' not found.");

                user.ProfilePicture = await _cloudinaryService.UploadUserProfilePictureAsync(dto.ProfilePicture, userId);

                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(e => e.Description).ToList();
                    throw new ValidationException(errors);
                }

                return user.ProfilePicture;
            }
            catch (Exception ex)
            {
                throw new InternalServerErrorException($"An error occurred while updating the profile picture for user ID '{userId}'.", ex);
            }
        }

        public async Task DeleteProfilePictureAsync(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    throw new NotFoundException($"User with ID '{userId}' not found.");

                if (string.IsNullOrEmpty(user.ProfilePicture))
                    throw new ValidationException(new List<string> { "User does not have a profile picture to delete." });

                // Delete the image from Cloudinary using the user ID as the public ID
                await _cloudinaryService.DeleteImageAsync(user.Id);

                user.ProfilePicture = null;
                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(e => e.Description).ToList();
                    throw new ValidationException(errors);
                }
            }
            catch (Exception ex)
            {
                throw new InternalServerErrorException($"An error occurred while deleting the profile picture for user ID '{userId}'.", ex);
            }
        }
    }
}