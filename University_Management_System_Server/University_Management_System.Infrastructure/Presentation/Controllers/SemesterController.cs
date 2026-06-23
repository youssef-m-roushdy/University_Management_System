using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using University_Management_System.Application.Commands.Semesters;
using University_Management_System.Application.Dtos.SemesterDtos;
using University_Management_System.Application.Queries.Semesters;
using University_Management_System.Domain.Queries;
using University_Management_System.Shared.Responses;
using Microsoft.Extensions.Logging;
using University_Management_System.Domain.Queries.SemesterQueries;

namespace University_Management_System.Infrastructure.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class SemesterController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<SemesterController> _logger;

        public SemesterController(IMediator mediator, ILogger<SemesterController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET /api/semesters
        // ────────────────────────────────────────────────────────────────────────
        /// <summary>
        /// Get all semesters with pagination and filters (Admin only)
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<PagedResponse<SemesterDto>>> GetSemesters([FromQuery] SemesterFilterQueries query)
        {
            try
            {
                var result = await _mediator.Send(new GetSemestersQuery { Query = query });
                
                var response = PagedResponse<SemesterDto>.SuccessResponse(
                    result.Data,
                    query.PageNumber,
                    query.PageSize,
                    result.TotalCount,
                    "Semesters retrieved successfully"
                );
                
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting semesters");
                return StatusCode(500, PagedResponse<SemesterDto>.ServerErrorResponse("An error occurred while retrieving semesters"));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET /api/semesters/{id}
        // ────────────────────────────────────────────────────────────────────────
        /// <summary>
        /// Get semester by ID (Admin only)
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<SemesterDto>>> GetSemesterById(int id)
        {
            try
            {
                var result = await _mediator.Send(new GetSemesterQuery { Id = id });
                
                return Ok(ApiResponse<SemesterDto>.SuccessResponse(result, "Semester retrieved successfully"));
            }
            catch (Shared.Exceptions.NotFoundException ex)
            {
                return NotFound(ApiResponse<SemesterDto>.NotFoundResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting semester by ID: {Id}", id);
                return StatusCode(500, ApiResponse<SemesterDto>.ServerErrorResponse("An error occurred while retrieving semester"));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // POST /api/semesters
        // ────────────────────────────────────────────────────────────────────────
        /// <summary>
        /// Create a new semester (Admin only)
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ApiResponse<SemesterDto>>> CreateSemester([FromBody] CreateSemesterDto command)
        {
            try
            {
                var result = await _mediator.Send(new CreateSemesterCommand { SemesterDto = command });

                return CreatedAtAction(
                    nameof(GetSemesterById),
                    new { id = result.Id },
                    ApiResponse<SemesterDto>.SuccessResponse(result, "Semester created successfully")
                );
            }
            catch (Shared.Exceptions.NotFoundException ex)
            {
                return NotFound(ApiResponse<SemesterDto>.NotFoundResponse(ex.Message));
            }
            catch (Shared.Exceptions.ConflictException ex)
            {
                return Conflict(ApiResponse<SemesterDto>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating semester");
                return StatusCode(500, ApiResponse<SemesterDto>.ServerErrorResponse("An error occurred while creating semester"));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // PUT /api/semesters/{id}
        // ────────────────────────────────────────────────────────────────────────
        /// <summary>
        /// Update a semester (Admin only)
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<SemesterDto>>> UpdateSemester(int id, [FromBody] UpdateSemesterDto dto)
        {
            try
            {
                var command = new UpdateSemesterCommand
                {
                    Id = id,
                    SemesterDto = dto
                };

                var result = await _mediator.Send(command);
                
                return Ok(ApiResponse<SemesterDto>.SuccessResponse(result, "Semester updated successfully"));
            }
            catch (Shared.Exceptions.NotFoundException ex)
            {
                return NotFound(ApiResponse<SemesterDto>.NotFoundResponse(ex.Message));
            }
            catch (Shared.Exceptions.ConflictException ex)
            {
                return Conflict(ApiResponse<SemesterDto>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating semester: {Id}", id);
                return StatusCode(500, ApiResponse<SemesterDto>.ServerErrorResponse("An error occurred while updating semester"));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // DELETE /api/semesters/{id}
        // ────────────────────────────────────────────────────────────────────────
        /// <summary>
        /// Delete a semester (Admin only)
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteSemester(int id)
        {
            try
            {
                await _mediator.Send(new DeleteSemesterCommand { Id = id });
                
                return Ok(ApiResponse<object>.SuccessResponse("Semester deleted successfully"));
            }
            catch (Shared.Exceptions.NotFoundException ex)
            {
                return NotFound(ApiResponse<object>.NotFoundResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting semester: {Id}", id);
                return StatusCode(500, ApiResponse<object>.ServerErrorResponse("An error occurred while deleting semester"));
            }
        }

        [HttpGet("study-year/{studyYearId}")]
        public async Task<ActionResult<PagedResponse<SemesterDto>>> GetStudyYearSemesters(int studyYearId)
        {
            try
            {
                var result = await _mediator.Send(new GetStudyYearSemestersQuery { StudyYearId = studyYearId });

                // the returned semesters now in one page is 2 or 3 semesters at most
                var response = PagedResponse<SemesterDto>.SuccessResponse(
                    result.Data,
                    1, // always return page 1 for nested routes
                    result.TotalCount, // total count is the count of semesters for this study year
                    result.TotalCount, // total count is the count of semesters for this study year
                    "Semesters retrieved successfully"
                );

                return Ok(response);
            }
            catch (Shared.Exceptions.NotFoundException ex)
            {
                return NotFound(PagedResponse<SemesterDto>.NotFoundResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting semesters for study year: {Id}", studyYearId);
                return StatusCode(500, PagedResponse<SemesterDto>.ServerErrorResponse(
                    "An error occurred while retrieving semesters"));
            }
        }

        [HttpPost("study-year/{studyYearId}")]
        public async Task<ActionResult<PagedResponse<SemesterDto>>> CreateStudyYearSemester(int studyYearId, [FromBody] CreateSemesterDto dto)
        {
            try
            {
                var result = await _mediator.Send(new CreateStudyYearSemesterCommand { StudyYearId = studyYearId, SemesterDto = dto });

                return CreatedAtAction(
                    nameof(GetStudyYearSemesters),
                    new { id = studyYearId },
                    ApiResponse<SemesterDto>.SuccessResponse(result, "Semester created successfully")
                );
            }
            catch (Shared.Exceptions.NotFoundException ex)
            {
                return NotFound(PagedResponse<SemesterDto>.NotFoundResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating semester for study year: {Id}", studyYearId);
                return StatusCode(500, PagedResponse<SemesterDto>.ServerErrorResponse(
                    "An error occurred while creating semester"));
            }
        }
    }
}