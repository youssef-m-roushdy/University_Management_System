using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Logging;
using University_Management_System.Application.Contracts;
using University_Management_System.Application.Dtos.AuthDtos;
using University_Management_System.Shared.Responses;

namespace University_Management_System.Infrastructure.Presentation.Controllers
{
    [ApiController]
    [Route("api/auth")]
    [EnableRateLimiting("PolicyLimitRate")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(IServiceManager serviceManager, ILogger<AuthenticationController> logger)
        {
            _serviceManager = serviceManager;
            _logger = logger;
        }
        // ────────────────────────────────────────────────────────────────────────
        // POST /api/auth/login
        // ────────────────────────────────────────────────────────────────────────
        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Login([FromBody] LoginDto dto)
        {
            try
            {
                var result = await _serviceManager.AuthenticationService.LoginAsync(dto);
                return Ok(ApiResponse<AuthResponseDto>.SuccessResponse(
                    result, 
                    "Login successful"
                ));
            }
            catch (Shared.Exceptions.NotFoundException ex)
            {
                return NotFound(ApiResponse<AuthResponseDto>.NotFoundResponse(ex.Message));
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ApiResponse<AuthResponseDto>.UnauthorizedResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<AuthResponseDto>.ServerErrorResponse(
                    "An error occurred during login"
                ));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // POST /api/auth/logout - ✅ UserId from token
        // ────────────────────────────────────────────────────────────────────────
        [Authorize]
        [HttpPost("logout")]
        public async Task<ActionResult<ApiResponse<object>>> Logout([FromBody] LogoutDto dto)
        {
            try
            {
                // ✅ Extract UserId from JWT token
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(ApiResponse<object>.UnauthorizedResponse("Invalid token"));

                await _serviceManager.AuthenticationService.LogoutAsync(userId, dto.RefreshToken);
                return Ok(ApiResponse<object>.SuccessResponse("Logged out successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // POST /api/auth/refresh-token - ✅ UserId from expired token
        // ────────────────────────────────────────────────────────────────────────
        [HttpPost("refresh-token")]
        public async Task<ActionResult<ApiResponse<AuthResponseDto>>> RefreshToken([FromBody] RefreshTokenDto dto)
        {
            try
            {
                // ✅ Extract UserId from expired token
                var userId = _serviceManager.JwtService.GetUserIdFromExpiredToken(dto.AccessToken);
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(ApiResponse<AuthResponseDto>.UnauthorizedResponse("Invalid token"));

                var result = await _serviceManager.AuthenticationService.RefreshTokenAsync(
                    dto.RefreshToken, 
                    userId
                );
                
                return Ok(ApiResponse<AuthResponseDto>.SuccessResponse(
                    result, 
                    "Token refreshed successfully"
                ));
            }
            catch (Shared.Exceptions.NotFoundException ex)
            {
                return NotFound(ApiResponse<AuthResponseDto>.NotFoundResponse(ex.Message));
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ApiResponse<AuthResponseDto>.UnauthorizedResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Refresh token failed");
                return StatusCode(500, ApiResponse<AuthResponseDto>.ServerErrorResponse(
                    "An error occurred while refreshing token"
                ));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // POST /api/auth/revoke-token - ✅ UserId from token
        // ────────────────────────────────────────────────────────────────────────
        [Authorize]
        [HttpPost("revoke-token")]
        public async Task<ActionResult<ApiResponse<object>>> RevokeToken([FromBody] RevokeTokenDto dto)
        {
            try
            {
                // ✅ Extract UserId from JWT token (verify ownership)
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(ApiResponse<object>.UnauthorizedResponse("Invalid token"));

                // Verify the refresh token belongs to this user
                var refreshToken = await _serviceManager.JwtService.GetRefreshTokenAsync(dto.RefreshToken);
                if (refreshToken == null || refreshToken.UserId != userId)
                    return BadRequest(ApiResponse<object>.ErrorResponse("Invalid refresh token"));

                await _serviceManager.AuthenticationService.RevokeTokenAsync(dto.RefreshToken);
                return Ok(ApiResponse<object>.SuccessResponse("Token revoked successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // POST /api/auth/revoke-all-tokens - ✅ UserId from token
        // ────────────────────────────────────────────────────────────────────────
        [Authorize]
        [HttpPost("revoke-all-tokens")]
        public async Task<ActionResult<ApiResponse<object>>> RevokeAllTokens()
        {
            try
            {
                // ✅ Extract UserId from JWT token
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(ApiResponse<object>.UnauthorizedResponse("Invalid token"));

                await _serviceManager.AuthenticationService.RevokeAllTokensAsync(userId);
                return Ok(ApiResponse<object>.SuccessResponse("All tokens revoked successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // POST /api/auth/forgot-password
        // ────────────────────────────────────────────────────────────────────────
        [HttpPost("forgot-password")]
        public async Task<ActionResult<ApiResponse<object>>> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            try
            {
                await _serviceManager.AuthenticationService.ForgotPasswordAsync(dto.Email);
                
                return Ok(ApiResponse<object>.SuccessResponse(
                    "Password reset link sent to your email if the account exists"
                ));
            }
            catch (Exception)
            {
                return Ok(ApiResponse<object>.SuccessResponse(
                    "Password reset link sent to your email if the account exists"
                ));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // POST /api/auth/reset-password
        // ────────────────────────────────────────────────────────────────────────
        [HttpPost("reset-password")]
        public async Task<ActionResult<ApiResponse<object>>> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            try
            {
                if (dto.NewPassword != dto.ConfirmPassword)
                {
                    return BadRequest(ApiResponse<object>.ErrorResponse(
                        "Passwords do not match.",
                        400,
                        new List<ApiError>
                        {
                            new() { 
                                Code = "PASSWORD_MISMATCH", 
                                Message = "New password and confirm password do not match",
                                Field = "ConfirmPassword"
                            }
                        }
                    ));
                }

                var result = await _serviceManager.AuthenticationService.ResetPasswordAsync(
                    dto.Email, 
                    dto.Token, 
                    dto.NewPassword
                );

                return Ok(ApiResponse<object>.SuccessResponse(result));
            }
            catch (Shared.Exceptions.NotFoundException ex)
            {
                return NotFound(ApiResponse<object>.NotFoundResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(
                    "Password reset failed. Please try again or request a new reset link."
                ));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // POST /api/auth/change-password - ✅ UserId from token
        // ────────────────────────────────────────────────────────────────────────
        [Authorize]
        [HttpPost("change-password")]
        public async Task<ActionResult<ApiResponse<object>>> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            try
            {
                // ✅ Extract Email from JWT claims
                var email = User.FindFirstValue(ClaimTypes.Email);
                if (string.IsNullOrEmpty(email))
                {
                    return Unauthorized(ApiResponse<object>.UnauthorizedResponse(
                        "Invalid token. Email claim not found."
                    ));
                }

                var result = await _serviceManager.AuthenticationService.ChangePasswordAsync(
                    email, 
                    dto.CurrentPassword, 
                    dto.NewPassword
                );

                return Ok(ApiResponse<object>.SuccessResponse(result));
            }
            catch (Shared.Exceptions.NotFoundException ex)
            {
                return NotFound(ApiResponse<object>.NotFoundResponse(ex.Message));
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ApiResponse<object>.UnauthorizedResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // POST /api/auth/verify-email
        // ────────────────────────────────────────────────────────────────────────
        [HttpPost("verify-email")]
        public async Task<ActionResult<ApiResponse<object>>> VerifyEmail([FromBody] VerifyEmailDto dto)
        {
            try
            {
                var result = await _serviceManager.AuthenticationService.VerifyEmailAsync(
                    dto.Email, 
                    dto.Token
                );
                
                return Ok(ApiResponse<object>.SuccessResponse(result));
            }
            catch (Shared.Exceptions.NotFoundException ex)
            {
                return NotFound(ApiResponse<object>.NotFoundResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // POST /api/auth/resend-verification
        // ────────────────────────────────────────────────────────────────────────
        [HttpPost("resend-verification")]
        public async Task<ActionResult<ApiResponse<object>>> ResendVerificationEmail([FromBody] ResendVerificationDto dto)
        {
            try
            {
                await _serviceManager.AuthenticationService.ResendVerificationEmailAsync(dto.Email);
                
                return Ok(ApiResponse<object>.SuccessResponse(
                    "Verification email sent if the account exists"
                ));
            }
            catch (Exception)
            {
                return Ok(ApiResponse<object>.SuccessResponse(
                    "Verification email sent if the account exists"
                ));
            }
        }
    }
}