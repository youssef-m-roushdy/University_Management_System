using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using University_Management_System.Application.Commands.Instructors;
using University_Management_System.Application.Dtos.InstructorDtos;
using University_Management_System.Application.Queries.Instructors;
using University_Management_System.Domain.Queries.InstructorQueries;
using University_Management_System.Shared.Responses;
using Microsoft.Extensions.Logging;
using AutoMapper;

namespace University_Management_System.Infrastructure.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class InstructorController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<InstructorController> _logger;
        private readonly IMapper _mapper;

        public InstructorController(IMediator mediator, ILogger<InstructorController> logger, IMapper mapper)
        {
            _mediator = mediator;
            _logger = logger;
            _mapper = mapper;
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET /api/instructors
        // ────────────────────────────────────────────────────────────────────────
        /// <summary>
        /// Get all instructors with filters and pagination (Admin only)
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<PagedResponse<InstructorDto>>> GetAllInstructors([FromQuery] InstructorFilterQueries query)
        {
            try
            {
                var result = await _mediator.Send(new GetInstructorsQuery { Query = query });

                var response = PagedResponse<InstructorDto>.SuccessResponse(
                    result.Data,
                    query.PageNumber,
                    query.PageSize,
                    result.TotalCount,
                    "Instructors retrieved successfully"
                );

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting instructors");
                return StatusCode(500, PagedResponse<InstructorDto>.ServerErrorResponse("An error occurred while retrieving instructors"));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET /api/instructors/{id}
        // ────────────────────────────────────────────────────────────────────────
        /// <summary>
        /// Get instructor by ID (Admin only)
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<InstructorDto>>> GetInstructorById(string id)
        {
            try
            {
                var result = await _mediator.Send(new GetInstructorQuery { Id = id });

                if (result == null)
                    return NotFound(ApiResponse<InstructorDto>.NotFoundResponse($"Instructor with ID '{id}' not found"));

                return Ok(ApiResponse<InstructorDto>.SuccessResponse(result, "Instructor retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting instructor by ID: {Id}", id);
                return StatusCode(500, ApiResponse<InstructorDto>.ServerErrorResponse("An error occurred while retrieving instructor"));
            }
        }
        
        // ────────────────────────────────────────────────────────────────────────
        // GET /api/instructors/department/{departmentId}
        // ────────────────────────────────────────────────────────────────────────
        /// <summary>
        /// Get instructors by department (Admin, Instructor, Assistant)
        /// </summary>
        [HttpGet("department/{departmentId}")]
        [Authorize(Roles = "Admin,Instructor,Assistant")]
        public async Task<ActionResult<PagedResponse<InstructorDto>>> GetDepartmentInstructors(
            int departmentId,
            [FromQuery] InstructorDepartmentQueries query)
        {
            try
            {
                var result = await _mediator.Send(new GetDepartmentInstructorsQuery
                {
                    DepartmentId = departmentId,
                    Query = query
                });

                var response = PagedResponse<InstructorDto>.SuccessResponse(
                    result.Data,
                    query.PageNumber,
                    query.PageSize,
                    result.TotalCount,
                    "Instructors retrieved successfully"
                );

                return Ok(response);
            }
            catch (Shared.Exceptions.NotFoundException ex)
            {
                return NotFound(PagedResponse<InstructorDto>.NotFoundResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting department instructors");
                return StatusCode(500, PagedResponse<InstructorDto>.ServerErrorResponse("An error occurred while retrieving instructors"));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // POST /api/instructors
        // ────────────────────────────────────────────────────────────────────────
        /// <summary>
        /// Create a new instructor (Admin only)
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ApiResponse<InstructorDto>>> CreateInstructor([FromBody] CreateInstructorDto dto)
        {
            try
            {
                var command = new CreateInstructorCommand { Dto = dto };
                var result = await _mediator.Send(command);

                return StatusCode(201, ApiResponse<InstructorDto>.SuccessResponse(result, "Instructor created successfully"));
            }
            catch (Shared.Exceptions.NotFoundException ex)
            {
                return NotFound(ApiResponse<InstructorDto>.NotFoundResponse(ex.Message));
            }
            catch (Shared.Exceptions.ValidationException ex)
            {
                return BadRequest(ApiResponse<InstructorDto>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating instructor");
                return StatusCode(500, ApiResponse<InstructorDto>.ServerErrorResponse("An error occurred while creating instructor"));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // PUT /api/instructors/{id}
        // ────────────────────────────────────────────────────────────────────────
        /// <summary>
        /// Update an instructor (Admin only)
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<InstructorDto>>> UpdateInstructor(string id, [FromBody] UpdateInstructorDto dto)
        {
            try
            {
                var command = new UpdateInstructorCommand
                {
                    Id = id,
                    Dto = dto
                };

                var result = await _mediator.Send(command);

                return Ok(ApiResponse<InstructorDto>.SuccessResponse(result, "Instructor updated successfully"));
            }
            catch (Shared.Exceptions.NotFoundException ex)
            {
                return NotFound(ApiResponse<InstructorDto>.NotFoundResponse(ex.Message));
            }
            catch (Shared.Exceptions.ValidationException ex)
            {
                return BadRequest(ApiResponse<InstructorDto>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating instructor: {Id}", id);
                return StatusCode(500, ApiResponse<InstructorDto>.ServerErrorResponse("An error occurred while updating instructor"));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // DELETE /api/instructors/{id}
        // ────────────────────────────────────────────────────────────────────────
        /// <summary>
        /// Delete an instructor (Admin only)
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteInstructor(string id)
        {
            try
            {
                var result = await _mediator.Send(new DeleteInstructorCommand { Id = id });

                return Ok(ApiResponse<object>.SuccessResponse("Instructor deleted successfully"));
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
                _logger.LogError(ex, "Error deleting instructor: {Id}", id);
                return StatusCode(500, ApiResponse<object>.ServerErrorResponse("An error occurred while deleting instructor"));
            }
        }
    }
}