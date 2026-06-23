using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using University_Management_System.Application.Dtos.StudyYearDtos;
using University_Management_System.Domain.Queries;
using University_Management_System.Shared.Responses;
using Microsoft.Extensions.Logging;
using University_Management_System.Application.Dtos.DepartmentDtos.FeeDtos;
using University_Management_System.Application.Commands.StudyYears;
using University_Management_System.Application.Queries.StudyYears;
using University_Management_System.Application.Dtos.SemesterDtos;
using University_Management_System.Application.Commands.Semesters;
using University_Management_System.Application.Queries.Semesters;
using University_Management_System.Application.Queries.StudentStudyYears;
using University_Management_System.Application.Queries.Fees;
using University_Management_System.Domain.Queries.FeeQueries;
using University_Management_System.Domain.Queries.StudyYearQueries;

namespace University_Management_System.Infrastructure.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class StudyYearController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<StudyYearController> _logger;

        public StudyYearController(IMediator mediator, ILogger<StudyYearController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET /api/study-years
        // ────────────────────────────────────────────────────────────────────────
        [HttpGet]
        public async Task<ActionResult<PagedResponse<StudyYearDto>>> GetStudyYears([FromQuery] StudyYearFilterQueries query)
        {
            try
            {
                var result = await _mediator.Send(new GetStudyYearsQuery { Query = query });
                
                var response = PagedResponse<StudyYearDto>.SuccessResponse(
                    result.Data,
                    query.PageNumber,
                    query.PageSize,
                    result.TotalCount,
                    "Study years retrieved successfully"
                );
                
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting study years");
                return StatusCode(500, PagedResponse<StudyYearDto>.ServerErrorResponse("An error occurred while retrieving study years"));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET /api/study-years/current
        // ────────────────────────────────────────────────────────────────────────
        [HttpGet("current")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<StudyYearDto>>> GetCurrentStudyYear()
        {
            try
            {
                var result = await _mediator.Send(new GetCurrentStudyYearQuery());
                
                if (result == null)
                    return NotFound(ApiResponse<StudyYearDto>.NotFoundResponse("No current study year found"));
                
                return Ok(ApiResponse<StudyYearDto>.SuccessResponse(result, "Current study year retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting current study year");
                return StatusCode(500, ApiResponse<StudyYearDto>.ServerErrorResponse("An error occurred while retrieving current study year"));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET /api/study-years/{id}
        // ────────────────────────────────────────────────────────────────────────
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<StudyYearDto>>> GetStudyYearById(int id)
        {
            try
            {
                var result = await _mediator.Send(new GetStudyYearQuery { Id = id });
                
                return Ok(ApiResponse<StudyYearDto>.SuccessResponse(result, "Study year retrieved successfully"));
            }
            catch (Shared.Exceptions.NotFoundException ex)
            {
                return NotFound(ApiResponse<StudyYearDto>.NotFoundResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting study year by ID: {Id}", id);
                return StatusCode(500, ApiResponse<StudyYearDto>.ServerErrorResponse("An error occurred while retrieving study year"));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // POST /api/study-years
        // ────────────────────────────────────────────────────────────────────────
        [HttpPost]
        public async Task<ActionResult<ApiResponse<StudyYearDto>>> CreateStudyYear([FromBody] CreateStudyYearDto dto)
        {
            try
            {
                var command = new CreateStudyYearCommand
                {
                    StartYear = dto.StartYear,
                    EndYear = dto.EndYear,
                    IsCurrent = dto.IsCurrent
                };

                var result = await _mediator.Send(command);
                
                return CreatedAtAction(
                    nameof(GetStudyYearById),
                    new { id = result.Id },
                    ApiResponse<StudyYearDto>.SuccessResponse(result, "Study year created successfully")
                );
            }
            catch (Shared.Exceptions.ValidationException ex)
            {
                return BadRequest(ApiResponse<StudyYearDto>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating study year");
                return StatusCode(500, ApiResponse<StudyYearDto>.ServerErrorResponse("An error occurred while creating study year"));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // PUT /api/study-years/{id}
        // ────────────────────────────────────────────────────────────────────────
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<StudyYearDto>>> UpdateStudyYear(int id, [FromBody] UpdateStudyYearDto dto)
        {
            try
            {
                var command = new UpdateStudyYearCommand
                {
                    Id = id,
                    StartYear = dto.StartYear,
                    EndYear = dto.EndYear,
                    IsCurrent = dto.IsCurrent
                };

                var result = await _mediator.Send(command);
                
                return Ok(ApiResponse<StudyYearDto>.SuccessResponse(result, "Study year updated successfully"));
            }
            catch (Shared.Exceptions.NotFoundException ex)
            {
                return NotFound(ApiResponse<StudyYearDto>.NotFoundResponse(ex.Message));
            }
            catch (Shared.Exceptions.ValidationException ex)
            {
                return BadRequest(ApiResponse<StudyYearDto>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating study year: {Id}", id);
                return StatusCode(500, ApiResponse<StudyYearDto>.ServerErrorResponse("An error occurred while updating study year"));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // PATCH /api/study-years/{id}
        // ────────────────────────────────────────────────────────────────────────
        [HttpPatch("{id}")]
        public async Task<ActionResult<ApiResponse<StudyYearDto>>> PatchStudyYear(int id, [FromBody] PatchStudyYearDto dto)
        {
            try
            {
                var command = new PatchStudyYearCommand
                {
                    Id = id,
                    Dto = dto
                };

                var result = await _mediator.Send(command);
                
                return Ok(ApiResponse<StudyYearDto>.SuccessResponse(result, "Study year updated successfully"));
            }
            catch (Shared.Exceptions.NotFoundException ex)
            {
                return NotFound(ApiResponse<StudyYearDto>.NotFoundResponse(ex.Message));
            }
            catch (Shared.Exceptions.ValidationException ex)
            {
                return BadRequest(ApiResponse<StudyYearDto>.ErrorResponse(ex.Message));
            }
            catch (Shared.Exceptions.ConflictException ex)
            {
                return BadRequest(ApiResponse<StudyYearDto>.ConflictErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error patching study year: {Id}", id);
                return StatusCode(500, ApiResponse<StudyYearDto>.ServerErrorResponse("An error occurred while updating study year"));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // DELETE /api/study-years/{id}
        // ────────────────────────────────────────────────────────────────────────
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteStudyYear(int id)
        {
            try
            {
                var result = await _mediator.Send(new DeleteStudyYearCommand { Id = id });
                
                if (!result)
                    return NotFound(ApiResponse<object>.NotFoundResponse($"Study year with ID '{id}' not found"));
                
                return Ok(ApiResponse<object>.SuccessResponse("Study year deleted successfully"));
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
                _logger.LogError(ex, "Error deleting study year: {Id}", id);
                return StatusCode(500, ApiResponse<object>.ServerErrorResponse("An error occurred while deleting study year"));
            }
        }

        
    }
}