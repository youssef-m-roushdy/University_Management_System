using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using University_Management_System.Application.Commands.Registrations;
using University_Management_System.Application.Dtos.RegistrationDtos;
using University_Management_System.Application.Features.Registrations.Commands.BulkUpdateRegistrationGrades;
using University_Management_System.Application.Features.Registrations.Commands.UpdateRegistrationGrade;
using University_Management_System.Application.Queries.Registrations;
using University_Management_System.Domain.Queries.RegistrationQueries;
using University_Management_System.Shared.Responses;

namespace University_Management_System.Infrastructure.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RegistrationController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<RegistrationController> _logger;

        public RegistrationController(IMediator mediator, ILogger<RegistrationController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // ────────────────────────────────────────────────────────────────────────
        // CREATE - Student
        // ────────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Register for a course (Student only)
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ApiResponse<RegistrationDto>>> RegisterForCourse(
            [FromBody] CreateRegistrationDto registrationDto)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(ApiResponse<RegistrationDto>.UnauthorizedResponse("Invalid token"));

                var command = new CreateRegistrationCommand
                {
                    Dto = registrationDto,
                    StudentId = userId
                };

                var result = await _mediator.Send(command);

                return StatusCode(201, ApiResponse<RegistrationDto>.SuccessResponse(
                    result,
                    "Registration created successfully"));
            }
            catch (Shared.Exceptions.NotFoundException ex)
            {
                return NotFound(ApiResponse<RegistrationDto>.NotFoundResponse(ex.Message));
            }
            catch (Shared.Exceptions.ValidationException ex)
            {
                return BadRequest(ApiResponse<RegistrationDto>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating registration");
                return StatusCode(500, ApiResponse<RegistrationDto>.ServerErrorResponse(
                    "An error occurred while creating registration"));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // UPDATE - Student
        // ────────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Update registration details (Student only)
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<RegistrationDto>>> UpdateRegistration(
            int id,
            [FromBody] UpdateRegistrationDto updateDto)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(ApiResponse<RegistrationDto>.UnauthorizedResponse("Invalid token"));

                var command = new UpdateRegistrationCommand
                {
                    Id = id,
                    Dto = updateDto,
                    StudentId = userId
                };

                var result = await _mediator.Send(command);

                return Ok(ApiResponse<RegistrationDto>.SuccessResponse(
                    result,
                    "Registration updated successfully"));
            }
            catch (Shared.Exceptions.NotFoundException ex)
            {
                return NotFound(ApiResponse<RegistrationDto>.NotFoundResponse(ex.Message));
            }
            catch (Shared.Exceptions.ValidationException ex)
            {
                return BadRequest(ApiResponse<RegistrationDto>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating registration: {Id}", id);
                return StatusCode(500, ApiResponse<RegistrationDto>.ServerErrorResponse(
                    "An error occurred while updating registration"));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // DELETE - Student
        // ────────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Delete registration (Student only)
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteRegistration(int id)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(ApiResponse<object>.UnauthorizedResponse("Invalid token"));

                await _mediator.Send(new DeleteRegistrationCommand
                {
                    Id = id,
                    StudentId = userId
                });

                return Ok(ApiResponse<object>.SuccessResponse("Registration deleted successfully"));
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
                _logger.LogError(ex, "Error deleting registration: {Id}", id);
                return StatusCode(500, ApiResponse<object>.ServerErrorResponse(
                    "An error occurred while deleting registration"));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET - Student (Self)
        // ────────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Get all registered courses for current student
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<RegistrationDto>>>> GetRegisteredCourses()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(ApiResponse<IEnumerable<RegistrationDto>>.UnauthorizedResponse("Invalid token"));

                var result = await _mediator.Send(new GetRegisteredCoursesQuery
                {
                    StudentId = userId
                });

                return Ok(ApiResponse<IEnumerable<RegistrationDto>>.SuccessResponse(
                    result,
                    "Registered courses retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting registered courses");
                return StatusCode(500, ApiResponse<IEnumerable<RegistrationDto>>.ServerErrorResponse(
                    "An error occurred while retrieving registered courses"));
            }
        }

        /// <summary>
        /// Get all registrations for current student with pagination
        /// </summary>
        [HttpGet("student/all")]
        public async Task<ActionResult<PagedResponse<RegistrationDto>>> GetStudentAllRegistrations(
            [FromQuery] RegistrationFilterQueries? filter = null)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(PagedResponse<RegistrationDto>.UnauthorizedResponse("Invalid token"));

                var result = await _mediator.Send(new GetStudentAllRegistrationsQuery
                {
                    StudentId = userId,
                    Filter = filter
                });

                var response = PagedResponse<RegistrationDto>.SuccessResponse(
                    result.Data,
                    filter?.PageNumber ?? 1,
                    filter?.PageSize ?? 10,
                    result.TotalCount,
                    "Your registrations retrieved successfully"
                );

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting student all registrations");
                return StatusCode(500, PagedResponse<RegistrationDto>.ServerErrorResponse(
                    "An error occurred while retrieving your registrations"));
            }
        }

        /// <summary>
        /// Get registered courses by study year for current student
        /// </summary>
        [HttpGet("{studyYearId}/year")]
        public async Task<ActionResult<ApiResponse<IEnumerable<RegistrationDto>>>> GetRegisteredYearCourses(int studyYearId)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(ApiResponse<IEnumerable<RegistrationDto>>.UnauthorizedResponse("Invalid token"));

                var result = await _mediator.Send(new GetRegisteredYearCoursesQuery
                {
                    StudentId = userId,
                    StudyYearId = studyYearId
                });

                return Ok(ApiResponse<IEnumerable<RegistrationDto>>.SuccessResponse(
                    result,
                    "Year courses retrieved successfully"));
            }
            catch (Shared.Exceptions.NotFoundException ex)
            {
                return NotFound(ApiResponse<IEnumerable<RegistrationDto>>.NotFoundResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting registered year courses");
                return StatusCode(500, ApiResponse<IEnumerable<RegistrationDto>>.ServerErrorResponse(
                    "An error occurred while retrieving year courses"));
            }
        }

        /// <summary>
        /// Get registered courses by semester for current student
        /// </summary>
        [HttpGet("student/{studyYearId}/year/{semesterId}/semester")]
        public async Task<ActionResult<ApiResponse<IEnumerable<RegistrationDto>>>> GetRegisteredSemesterCourses(
            int studyYearId,
            int semesterId)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(ApiResponse<IEnumerable<RegistrationDto>>.UnauthorizedResponse("Invalid token"));

                var result = await _mediator.Send(new GetRegisteredSemesterCoursesQuery
                {
                    StudentId = userId,
                    StudyYearId = studyYearId,
                    SemesterId = semesterId
                });

                return Ok(ApiResponse<IEnumerable<RegistrationDto>>.SuccessResponse(
                    result,
                    "Semester courses retrieved successfully"));
            }
            catch (Shared.Exceptions.NotFoundException ex)
            {
                return NotFound(ApiResponse<IEnumerable<RegistrationDto>>.NotFoundResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting registered semester courses");
                return StatusCode(500, ApiResponse<IEnumerable<RegistrationDto>>.ServerErrorResponse(
                    "An error occurred while retrieving semester courses"));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET - Admin/Instructor
        // ────────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Get all registrations for a semester (Admin/Instructor only)
        /// </summary>
        [HttpGet("semester/{semesterId}/study-year/{studyYearId}")]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<ActionResult<PagedResponse<RegistrationDto>>> GetSemesterRegistrations(
            int semesterId,
            int studyYearId,
            [FromQuery] RegistrationSemesterQueries? query = null)
        {
            try
            {
                var result = await _mediator.Send(new GetSemesterRegistrationsQuery
                {
                    SemesterId = semesterId,
                    StudyYearId = studyYearId,
                    RegistrationQuery = query
                });

                var response = PagedResponse<RegistrationDto>.SuccessResponse(
                    result.Data,
                    query?.PageNumber ?? 1,
                    query?.PageSize ?? 10,
                    result.TotalCount,
                    "Semester registrations retrieved successfully"
                );

                return Ok(response);
            }
            catch (Shared.Exceptions.NotFoundException ex)
            {
                return NotFound(PagedResponse<RegistrationDto>.NotFoundResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting semester registrations");
                return StatusCode(500, PagedResponse<RegistrationDto>.ServerErrorResponse(
                    "An error occurred while retrieving semester registrations"));
            }
        }

        /// <summary>
        /// Get all registrations for a study year (Admin only)
        /// </summary>
        [HttpGet("study-year/{studyYearId}/registrations")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<PagedResponse<RegistrationDto>>> GetStudyYearRegistrations(
            int studyYearId,
            [FromQuery] RegistrationStudyYearQueries? query = null)
        {
            try
            {
                var result = await _mediator.Send(new GetStudyYearRegistrationsQuery
                {
                    StudyYearId = studyYearId,
                    Query = query
                });

                var response = PagedResponse<RegistrationDto>.SuccessResponse(
                    result.Data,
                    query?.PageNumber ?? 1,
                    query?.PageSize ?? 10,
                    result.TotalCount,
                    "Study year registrations retrieved successfully"
                );

                return Ok(response);
            }
            catch (Shared.Exceptions.NotFoundException ex)
            {
                return NotFound(PagedResponse<RegistrationDto>.NotFoundResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting study year registrations");
                return StatusCode(500, PagedResponse<RegistrationDto>.ServerErrorResponse(
                    "An error occurred while retrieving study year registrations"));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // PATCH - Admin/Instructor (Status & Grade Management)
        // ────────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Update registration status (Admin/Instructor only)
        /// </summary>
        [HttpPatch("{registrationId}/status")]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<ActionResult<ApiResponse<RegistrationDto>>> UpdateRegistrationStatus(
            int registrationId,
            [FromBody] UpdateRegistrationStatusCommand command)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(ApiResponse<RegistrationDto>.UnauthorizedResponse("Invalid token"));

                command.RegistrationId = registrationId;
                command.StudentId = userId;

                var result = await _mediator.Send(command);

                return Ok(ApiResponse<RegistrationDto>.SuccessResponse(
                    result,
                    $"Registration status updated to {command.Status} successfully"));
            }
            catch (Shared.Exceptions.NotFoundException ex)
            {
                return NotFound(ApiResponse<RegistrationDto>.NotFoundResponse(ex.Message));
            }
            catch (Shared.Exceptions.ValidationException ex)
            {
                return BadRequest(ApiResponse<RegistrationDto>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating registration status: {RegistrationId}", registrationId);
                return StatusCode(500, ApiResponse<RegistrationDto>.ServerErrorResponse(
                    "An error occurred while updating registration status"));
            }
        }

        /// <summary>
        /// Update single registration grade (Admin/Instructor only)
        /// </summary>
        [HttpPatch("{registrationId}/grade")]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<ActionResult<ApiResponse<RegistrationDto>>> UpdateRegistrationGrade(
            int registrationId,
            [FromBody] UpdateRegistrationGradeCommand command)
        {
            try
            {
                command.RegistrationId = registrationId;

                var result = await _mediator.Send(command);

                return Ok(ApiResponse<RegistrationDto>.SuccessResponse(
                    result,
                    "Registration grade updated successfully"));
            }
            catch (Shared.Exceptions.NotFoundException ex)
            {
                return NotFound(ApiResponse<RegistrationDto>.NotFoundResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating registration grade: {RegistrationId}", registrationId);
                return StatusCode(500, ApiResponse<RegistrationDto>.ServerErrorResponse(
                    "An error occurred while updating registration grade"));
            }
        }

        /// <summary>
        /// Bulk update registration grades (Admin/Instructor only)
        /// </summary>
        [HttpPatch("grades/bulk")]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<ActionResult<ApiResponse<IEnumerable<RegistrationDto>>>> BulkUpdateGrades(
            [FromBody] BulkUpdateRegistrationGradesCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);

                return Ok(ApiResponse<IEnumerable<RegistrationDto>>.SuccessResponse(
                    result,
                    $"Successfully updated {result.Count()} registrations"));
            }
            catch (Shared.Exceptions.NotFoundException ex)
            {
                return NotFound(ApiResponse<IEnumerable<RegistrationDto>>.NotFoundResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error bulk updating grades");
                return StatusCode(500, ApiResponse<IEnumerable<RegistrationDto>>.ServerErrorResponse(
                    "An error occurred while updating grades"));
            }
        }
    }
}