using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using University_Management_System.Application.Contracts;
using University_Management_System.Application.Dtos.UserDtos;
using University_Management_System.Domain.Queries;
using University_Management_System.Shared.Responses;
using System.Security.Claims;
using Microsoft.Extensions.Logging;

namespace University_Management_System.Infrastructure.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET /api/user/profile
        // ────────────────────────────────────────────────────────────────────────
        /// <summary>
        /// Get current user's profile
        /// </summary>
        [HttpGet("profile")]
        public async Task<ActionResult<ApiResponse<UserProfileDetailsDto>>> GetMyProfile()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(ApiResponse<UserProfileDetailsDto>.UnauthorizedResponse("Invalid token"));

                var result = await _userService.GetUserProfileByIdAsync(userId);
                return Ok(ApiResponse<UserProfileDetailsDto>.SuccessResponse(result, "Profile retrieved successfully"));
            }
            catch (Shared.Exceptions.NotFoundException ex)
            {
                return NotFound(ApiResponse<UserProfileDetailsDto>.NotFoundResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user profile");
                return StatusCode(500, ApiResponse<UserProfileDetailsDto>.ServerErrorResponse("An error occurred while retrieving profile"));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET /api/user/profile/{userId}
        // ────────────────────────────────────────────────────────────────────────
        /// <summary>
        /// Get user profile by ID (Admin only)
        /// </summary>
        [HttpGet("profile/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<UserProfileDetailsDto>>> GetUserProfileById(string userId)
        {
            try
            {
                var result = await _userService.GetUserProfileByIdAsync(userId);
                return Ok(ApiResponse<UserProfileDetailsDto>.SuccessResponse(result, "Profile retrieved successfully"));
            }
            catch (Shared.Exceptions.NotFoundException ex)
            {
                return NotFound(ApiResponse<UserProfileDetailsDto>.NotFoundResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user profile for ID: {UserId}", userId);
                return StatusCode(500, ApiResponse<UserProfileDetailsDto>.ServerErrorResponse("An error occurred while retrieving profile"));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET /api/user
        // ────────────────────────────────────────────────────────────────────────
        /// <summary>
        /// Get all users with filters and pagination (Admin only)
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<PagedResponse<UserBasicDto>>> GetUsers([FromQuery] UserQueries query)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(PagedResponse<UserBasicDto>.UnauthorizedResponse("Invalid token"));

                // If no pagination params, return all
                if (query.PageNumber == 0 && query.PageSize == 0)
                {
                    var allUsers = await _userService.GetAllUsersAsync(userId, query);
                    var response = PagedResponse<UserBasicDto>.SuccessResponse(
                        allUsers,
                        1,
                        allUsers.Count(),
                        allUsers.Count(),
                        "Users retrieved successfully"
                    );
                    return Ok(response);
                }

                var (users, totalCount) = await _userService.GetUsersAsync(userId, query);
                
                var pagedResponse = PagedResponse<UserBasicDto>.SuccessResponse(
                    users,
                    query.PageNumber,
                    query.PageSize,
                    totalCount,
                    "Users retrieved successfully"
                );
                
                return Ok(pagedResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting users");
                return StatusCode(500, PagedResponse<UserBasicDto>.ServerErrorResponse("An error occurred while retrieving users"));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET /api/user/{userId}
        // ────────────────────────────────────────────────────────────────────────
        /// <summary>
        /// Get user by ID (Admin only)
        /// </summary>
        [HttpGet("{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<UserBasicDto>>> GetUserById(string userId)
        {
            try
            {
                var result = await _userService.GetUserByIdAsync(userId);
                return Ok(ApiResponse<UserBasicDto>.SuccessResponse(result, "User retrieved successfully"));
            }
            catch (Shared.Exceptions.NotFoundException ex)
            {
                return NotFound(ApiResponse<UserBasicDto>.NotFoundResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user by ID: {UserId}", userId);
                return StatusCode(500, ApiResponse<UserBasicDto>.ServerErrorResponse("An error occurred while retrieving user"));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET /api/user/by-email/{email}
        // ────────────────────────────────────────────────────────────────────────
        /// <summary>
        /// Get user by email (Admin only)
        /// </summary>
        [HttpGet("by-email/{email}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<UserBasicDto>>> GetUserByEmail(string email)
        {
            try
            {
                var result = await _userService.GetUserByEmailAsync(email);
                return Ok(ApiResponse<UserBasicDto>.SuccessResponse(result, "User retrieved successfully"));
            }
            catch (Shared.Exceptions.NotFoundException ex)
            {
                return NotFound(ApiResponse<UserBasicDto>.NotFoundResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user by email: {Email}", email);
                return StatusCode(500, ApiResponse<UserBasicDto>.ServerErrorResponse("An error occurred while retrieving user"));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // PUT /api/user
        // ────────────────────────────────────────────────────────────────────────
        /// <summary>
        /// Update current user's profile
        /// </summary>
        [HttpPut]
        public async Task<ActionResult<ApiResponse<object>>> UpdateUser([FromBody] UpdateUserDto dto)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(ApiResponse<object>.UnauthorizedResponse("Invalid token"));

                await _userService.UpdateUserAsync(userId, dto);
                return Ok(ApiResponse<object>.SuccessResponse("User updated successfully"));
            }
            catch (Shared.Exceptions.NotFoundException ex)
            {
                return NotFound(ApiResponse<object>.NotFoundResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user");
                return StatusCode(500, ApiResponse<object>.ServerErrorResponse("An error occurred while updating user"));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // ✅ POST /api/user/profile-picture (Upload - Correct HTTP Method)
        // ────────────────────────────────────────────────────────────────────────
        /// <summary>
        /// Upload or update current user's profile picture
        /// </summary>
        [HttpPost("profile-picture")]
        public async Task<ActionResult<ApiResponse<string>>> UploadProfilePicture([FromForm] UpdateProfilePictureDto dto)
        {
            try
            {
                if (dto.ProfilePicture == null || dto.ProfilePicture.Length == 0)
                    return BadRequest(ApiResponse<string>.ErrorResponse("Profile picture is required"));

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(ApiResponse<string>.UnauthorizedResponse("Invalid token"));

                var result = await _userService.UpdateProfilePictureAsync(userId, dto);
                return Ok(ApiResponse<string>.SuccessResponse(result, "Profile picture uploaded successfully"));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<string>.ErrorResponse(ex.Message));
            }
            catch (Shared.Exceptions.NotFoundException ex)
            {
                return NotFound(ApiResponse<string>.NotFoundResponse(ex.Message));
            }
            catch (Shared.Exceptions.ValidationException ex)
            {
                return BadRequest(ApiResponse<string>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading profile picture");
                return StatusCode(500, ApiResponse<string>.ServerErrorResponse("An error occurred while uploading profile picture"));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // DELETE /api/user/profile-picture
        // ────────────────────────────────────────────────────────────────────────
        /// <summary>
        /// Delete current user's profile picture
        /// </summary>
        [HttpDelete("profile-picture")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteProfilePicture()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(ApiResponse<object>.UnauthorizedResponse("Invalid token"));

                await _userService.DeleteProfilePictureAsync(userId);
                return Ok(ApiResponse<object>.SuccessResponse("Profile picture deleted successfully"));
            }
            catch (Shared.Exceptions.NotFoundException ex)
            {
                return NotFound(ApiResponse<object>.NotFoundResponse(ex.Message));
            }
            catch (Shared.Exceptions.ValidationException ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting profile picture");
                return StatusCode(500, ApiResponse<object>.ServerErrorResponse("An error occurred while deleting profile picture"));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // POST /api/user/activate/{userId}
        // ────────────────────────────────────────────────────────────────────────
        /// <summary>
        /// Activate a user account (Admin only)
        /// </summary>
        [HttpPost("activate/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<object>>> ActivateUser(string userId)
        {
            try
            {
                await _userService.ActivateUserAsync(userId);
                return Ok(ApiResponse<object>.SuccessResponse("User activated successfully"));
            }
            catch (Shared.Exceptions.NotFoundException ex)
            {
                return NotFound(ApiResponse<object>.NotFoundResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error activating user: {UserId}", userId);
                return StatusCode(500, ApiResponse<object>.ServerErrorResponse("An error occurred while activating user"));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // POST /api/user/deactivate/{userId}
        // ────────────────────────────────────────────────────────────────────────
        /// <summary>
        /// Deactivate a user account (Admin only)
        /// </summary>
        [HttpPost("deactivate/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<object>>> DeactivateUser(string userId)
        {
            try
            {
                await _userService.DeactivateUserAsync(userId);
                return Ok(ApiResponse<object>.SuccessResponse("User deactivated successfully"));
            }
            catch (Shared.Exceptions.NotFoundException ex)
            {
                return NotFound(ApiResponse<object>.NotFoundResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deactivating user: {UserId}", userId);
                return StatusCode(500, ApiResponse<object>.ServerErrorResponse("An error occurred while deactivating user"));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // DELETE /api/user/{userId}
        // ────────────────────────────────────────────────────────────────────────
        /// <summary>
        /// Delete a user account (Admin only)
        /// </summary>
        [HttpDelete("{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteUser(string userId)
        {
            try
            {
                // Prevent admin from deleting themselves
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (currentUserId == userId)
                    return BadRequest(ApiResponse<object>.ErrorResponse("You cannot delete your own account"));

                await _userService.DeleteUserAsync(userId);
                return Ok(ApiResponse<object>.SuccessResponse("User deleted successfully"));
            }
            catch (Shared.Exceptions.NotFoundException ex)
            {
                return NotFound(ApiResponse<object>.NotFoundResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user: {UserId}", userId);
                return StatusCode(500, ApiResponse<object>.ServerErrorResponse("An error occurred while deleting user"));
            }
        }
    }
}