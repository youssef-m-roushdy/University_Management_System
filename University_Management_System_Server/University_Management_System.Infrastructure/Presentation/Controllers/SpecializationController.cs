using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using University_Management_System.Application.Commands.Specializations;
using University_Management_System.Application.Dtos.SpecializationDtos;
using University_Management_System.Application.Queries.Specializations;
using University_Management_System.Domain.Queries.SpecializationQueries;
using University_Management_System.Shared.Responses;
using Microsoft.Extensions.Logging;

namespace University_Management_System.Infrastructure.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpecializationController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<SpecializationController> _logger;

        public SpecializationController(IMediator mediator, ILogger<SpecializationController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET /api/specializations
        // ────────────────────────────────────────────────────────────────────────
        /// <summary>
        /// Get all specializations with filters and pagination (Admin only)
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<PagedResponse<SpecializationDto>>> GetAllSpecializations(
            [FromQuery] SpecializationFilterQueries query)
        {
            try
            {
                var result = await _mediator.Send(new GetSpecializationsQuery { Query = query });

                var response = PagedResponse<SpecializationDto>.SuccessResponse(
                    result.Data,
                    query.PageNumber,
                    query.PageSize,
                    result.TotalCount,
                    "Specializations retrieved successfully"
                );

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting specializations");
                return StatusCode(500, PagedResponse<SpecializationDto>.ServerErrorResponse(
                    "An error occurred while retrieving specializations"));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET /api/specializations/{id}
        // ────────────────────────────────────────────────────────────────────────
        /// <summary>
        /// Get specialization by ID (Admin only)
        /// </summary>
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<SpecializationDto>>> GetSpecializationById(int id)
        {
            try
            {
                var result = await _mediator.Send(new GetSpecializationQuery { Id = id });

                if (result == null)
                    return NotFound(ApiResponse<SpecializationDto>.NotFoundResponse($"Specialization with ID '{id}' not found"));

                return Ok(ApiResponse<SpecializationDto>.SuccessResponse(result, "Specialization retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting specialization by ID: {Id}", id);
                return StatusCode(500, ApiResponse<SpecializationDto>.ServerErrorResponse(
                    "An error occurred while retrieving specialization"));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET /api/specializations/department/{departmentId}
        // ────────────────────────────────────────────────────────────────────────
        /// <summary>
        /// Get specializations by department (Admin, Instructor, Assistant, Student)
        /// </summary>
        [HttpGet("department/{departmentId}")]
        [Authorize(Roles = "Admin,Instructor,Assistant,Student")]
        public async Task<ActionResult<PagedResponse<SpecializationDto>>> GetSpecializationsByDepartment(
            int departmentId,
            [FromQuery] SpecializationDepartmentQueries query)
        {
            try
            {
                var result = await _mediator.Send(new GetSpecializationsByDepartmentQuery
                {
                    DepartmentId = departmentId,
                    Query = query
                });

                var response = PagedResponse<SpecializationDto>.SuccessResponse(
                    result.Data,
                    query.PageNumber,
                    query.PageSize,
                    result.TotalCount,
                    "Specializations retrieved successfully"
                );

                return Ok(response);
            }
            catch (Shared.Exceptions.NotFoundException ex)
            {
                return NotFound(PagedResponse<SpecializationDto>.NotFoundResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting specializations by department");
                return StatusCode(500, PagedResponse<SpecializationDto>.ServerErrorResponse(
                    "An error occurred while retrieving specializations"));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // POST /api/specializations
        // ────────────────────────────────────────────────────────────────────────
        /// <summary>
        /// Create a new specialization (Admin only)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<SpecializationDto>>> CreateSpecialization(
            [FromBody] CreateSpecializationDto dto)
        {
            try
            {
                var command = new CreateSpecializationCommand { Dto = dto };
                var result = await _mediator.Send(command);

                return StatusCode(201, ApiResponse<SpecializationDto>.SuccessResponse(
                    result,
                    "Specialization created successfully"));
            }
            catch (Shared.Exceptions.NotFoundException ex)
            {
                return NotFound(ApiResponse<SpecializationDto>.NotFoundResponse(ex.Message));
            }
            catch (Shared.Exceptions.ValidationException ex)
            {
                return BadRequest(ApiResponse<SpecializationDto>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating specialization");
                return StatusCode(500, ApiResponse<SpecializationDto>.ServerErrorResponse(
                    "An error occurred while creating specialization"));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // PUT /api/specializations/{id}
        // ────────────────────────────────────────────────────────────────────────
        /// <summary>
        /// Update a specialization (Admin only)
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<SpecializationDto>>> UpdateSpecialization(
            int id,
            [FromBody] UpdateSpecializationDto dto)
        {
            try
            {
                var command = new UpdateSpecializationCommand
                {
                    Id = id,
                    Dto = dto
                };

                var result = await _mediator.Send(command);

                return Ok(ApiResponse<SpecializationDto>.SuccessResponse(
                    result,
                    "Specialization updated successfully"));
            }
            catch (Shared.Exceptions.NotFoundException ex)
            {
                return NotFound(ApiResponse<SpecializationDto>.NotFoundResponse(ex.Message));
            }
            catch (Shared.Exceptions.ValidationException ex)
            {
                return BadRequest(ApiResponse<SpecializationDto>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating specialization: {Id}", id);
                return StatusCode(500, ApiResponse<SpecializationDto>.ServerErrorResponse(
                    "An error occurred while updating specialization"));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // DELETE /api/specializations/{id}
        // ────────────────────────────────────────────────────────────────────────
        /// <summary>
        /// Delete a specialization (Admin only)
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteSpecialization(int id)
        {
            try
            {
                await _mediator.Send(new DeleteSpecializationCommand { Id = id });

                return Ok(ApiResponse<object>.SuccessResponse("Specialization deleted successfully"));
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
                _logger.LogError(ex, "Error deleting specialization: {Id}", id);
                return StatusCode(500, ApiResponse<object>.ServerErrorResponse(
                    "An error occurred while deleting specialization"));
            }
        }
    }
}