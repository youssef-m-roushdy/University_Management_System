using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using University_Management_System.Application.Commands.Admins;
using University_Management_System.Application.Dtos.AdminDtos;
using University_Management_System.Application.Queries.Admins;
using University_Management_System.Domain.Queries.AdminQueries;
using University_Management_System.Shared.Responses;
using Microsoft.Extensions.Logging;
using AutoMapper;

namespace University_Management_System.Infrastructure.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AdminController> _logger;
        private readonly IMapper _mapper;

        public AdminController(IMediator mediator, ILogger<AdminController> logger, IMapper mapper)
        {
            _mediator = mediator;
            _logger = logger;
            _mapper = mapper;
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET /api/admins
        // ────────────────────────────────────────────────────────────────────────
        /// <summary>
        /// Get all admins with filters and pagination (Admin only)
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<PagedResponse<AdminDto>>> GetAllAdmins([FromQuery] AdminFilterQueries query)
        {
            try
            {
                var result = await _mediator.Send(new GetAdminsQuery { Query = query });

                var response = PagedResponse<AdminDto>.SuccessResponse(
                    result.Data,
                    query.PageNumber,
                    query.PageSize,
                    result.TotalCount,
                    "Admins retrieved successfully"
                );

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting admins");
                return StatusCode(500, PagedResponse<AdminDto>.ServerErrorResponse("An error occurred while retrieving admins"));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET /api/admins/{id}
        // ────────────────────────────────────────────────────────────────────────
        /// <summary>
        /// Get admin by ID (Admin only)
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<AdminDto>>> GetAdminById(string id)
        {
            try
            {
                var result = await _mediator.Send(new GetAdminQuery { Id = id });

                if (result == null)
                    return NotFound(ApiResponse<AdminDto>.NotFoundResponse($"Admin with ID '{id}' not found"));

                return Ok(ApiResponse<AdminDto>.SuccessResponse(result, "Admin retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting admin by ID: {Id}", id);
                return StatusCode(500, ApiResponse<AdminDto>.ServerErrorResponse("An error occurred while retrieving admin"));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // POST /api/admins
        // ────────────────────────────────────────────────────────────────────────
        /// <summary>
        /// Create a new admin (Admin only)
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ApiResponse<AdminDto>>> CreateAdmin([FromBody] CreateAdminDto dto)
        {
            try
            {
                var command = new CreateAdminCommand { Dto = dto };
                var result = await _mediator.Send(command);

                return StatusCode(201, ApiResponse<AdminDto>.SuccessResponse(result, "Admin created successfully"));
            }
            catch (Shared.Exceptions.NotFoundException ex)
            {
                return NotFound(ApiResponse<AdminDto>.NotFoundResponse(ex.Message));
            }
            catch (Shared.Exceptions.ValidationException ex)
            {
                return BadRequest(ApiResponse<AdminDto>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating admin");
                return StatusCode(500, ApiResponse<AdminDto>.ServerErrorResponse("An error occurred while creating admin"));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // DELETE /api/admins/{id}
        // ────────────────────────────────────────────────────────────────────────
        /// <summary>
        /// Delete an admin (Admin only)
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteAdmin(string id)
        {
            try
            {
                var result = await _mediator.Send(new DeleteAdminCommand { Id = id });

                return Ok(ApiResponse<object>.SuccessResponse("Admin deleted successfully"));
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
                _logger.LogError(ex, "Error deleting admin: {Id}", id);
                return StatusCode(500, ApiResponse<object>.ServerErrorResponse("An error occurred while deleting admin"));
            }
        }
    }
}