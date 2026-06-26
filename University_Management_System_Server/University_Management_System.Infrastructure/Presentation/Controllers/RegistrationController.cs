using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using University_Management_System.Application.Commands.Registrations;
using University_Management_System.Application.Dtos.RegistrationDtos;
using University_Management_System.Application.Queries.Registrations;
using University_Management_System.Domain.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using University_Management_System.Shared.Responses;
using University_Management_System.Domain.Queries.RegistrationQueries;
using University_Management_System.Application.Features.Registrations.Commands.UpdateRegistrationGrade;
using University_Management_System.Application.Features.Registrations.Commands.BulkUpdateRegistrationGrades;
using Microsoft.Extensions.Logging;

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

        // // ────────────────────────────────────────────────────────────────────────
        // // POST /api/registrations
        // // ────────────────────────────────────────────────────────────────────────
        // [HttpPost]
        // public async Task<ActionResult<ApiResponse<RegistrationDto>>> RegisterForCourse(
        //     [FromBody] CreateRegistrationDto registrationDto)
        // {
        //     try
        //     {
        //         var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //         if (string.IsNullOrEmpty(userId))
        //             return Unauthorized(ApiResponse<RegistrationDto>.UnauthorizedResponse("Invalid token"));

        //         var command = new CreateRegistrationCommand(registrationDto, userId);
        //         var result = await _mediator.Send(command);

        //         return StatusCode(201, ApiResponse<RegistrationDto>.SuccessResponse(
        //             result,
        //             "Registration created successfully"));
        //     }
        //     catch (Shared.Exceptions.NotFoundException ex)
        //     {
        //         return NotFound(ApiResponse<RegistrationDto>.NotFoundResponse(ex.Message));
        //     }
        //     catch (Shared.Exceptions.ValidationException ex)
        //     {
        //         return BadRequest(ApiResponse<RegistrationDto>.ErrorResponse(ex.Message));
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.LogError(ex, "Error creating registration");
        //         return StatusCode(500, ApiResponse<RegistrationDto>.ServerErrorResponse(
        //             "An error occurred while creating registration"));
        //     }
        // }

        // // ────────────────────────────────────────────────────────────────────────
        // // PUT /api/registrations/{id}
        // // ────────────────────────────────────────────────────────────────────────
        // [HttpPut("{id}")]
        // public async Task<ActionResult<ApiResponse<RegistrationDto>>> UpdateRegistration(
        //     int id,
        //     [FromBody] UpdateRegistrationDto updateDto)
        // {
        //     try
        //     {
        //         var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //         if (string.IsNullOrEmpty(userId))
        //             return Unauthorized(ApiResponse<RegistrationDto>.UnauthorizedResponse("Invalid token"));

        //         var command = new UpdateRegistrationCommand(id, updateDto, userId);
        //         var result = await _mediator.Send(command);

        //         return Ok(ApiResponse<RegistrationDto>.SuccessResponse(
        //             result,
        //             "Registration updated successfully"));
        //     }
        //     catch (Shared.Exceptions.NotFoundException ex)
        //     {
        //         return NotFound(ApiResponse<RegistrationDto>.NotFoundResponse(ex.Message));
        //     }
        //     catch (Shared.Exceptions.ValidationException ex)
        //     {
        //         return BadRequest(ApiResponse<RegistrationDto>.ErrorResponse(ex.Message));
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.LogError(ex, "Error updating registration: {Id}", id);
        //         return StatusCode(500, ApiResponse<RegistrationDto>.ServerErrorResponse(
        //             "An error occurred while updating registration"));
        //     }
        // }

        // // ────────────────────────────────────────────────────────────────────────
        // // DELETE /api/registrations/{id}
        // // ────────────────────────────────────────────────────────────────────────
        // [HttpDelete("{id}")]
        // public async Task<ActionResult<ApiResponse<object>>> DeleteRegistration(int id)
        // {
        //     try
        //     {
        //         var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //         if (string.IsNullOrEmpty(userId))
        //             return Unauthorized(ApiResponse<object>.UnauthorizedResponse("Invalid token"));

        //         await _mediator.Send(new DeleteRegistrationCommand(id, userId));

        //         return Ok(ApiResponse<object>.SuccessResponse("Registration deleted successfully"));
        //     }
        //     catch (Shared.Exceptions.NotFoundException ex)
        //     {
        //         return NotFound(ApiResponse<object>.NotFoundResponse(ex.Message));
        //     }
        //     catch (Shared.Exceptions.ValidationException ex)
        //     {
        //         return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.LogError(ex, "Error deleting registration: {Id}", id);
        //         return StatusCode(500, ApiResponse<object>.ServerErrorResponse(
        //             "An error occurred while deleting registration"));
        //     }
        // }

        // // ────────────────────────────────────────────────────────────────────────
        // // GET /api/registrations
        // // ────────────────────────────────────────────────────────────────────────
        // [HttpGet]
        // public async Task<ActionResult<ApiResponse<IEnumerable<RegistrationDto>>>> GetRegisteredCourses()
        // {
        //     try
        //     {
        //         var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //         if (string.IsNullOrEmpty(userId))
        //             return Unauthorized(ApiResponse<IEnumerable<RegistrationDto>>.UnauthorizedResponse("Invalid token"));

        //         var result = await _mediator.Send(new GetRegisteredCoursesQuery(userId));

        //         return Ok(ApiResponse<IEnumerable<RegistrationDto>>.SuccessResponse(
        //             result,
        //             "Registered courses retrieved successfully"));
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.LogError(ex, "Error getting registered courses");
        //         return StatusCode(500, ApiResponse<IEnumerable<RegistrationDto>>.ServerErrorResponse(
        //             "An error occurred while retrieving registered courses"));
        //     }
        // }

        // // ────────────────────────────────────────────────────────────────────────
        // // GET /api/registrations/{studyYearId}/year
        // // ────────────────────────────────────────────────────────────────────────
        // [HttpGet("{studyYearId}/year")]
        // public async Task<ActionResult<ApiResponse<IEnumerable<RegistrationDto>>>> GetRegisteredYearCourses(int studyYearId)
        // {
        //     try
        //     {
        //         var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //         if (string.IsNullOrEmpty(userId))
        //             return Unauthorized(ApiResponse<IEnumerable<RegistrationDto>>.UnauthorizedResponse("Invalid token"));

        //         var result = await _mediator.Send(new GetRegisteredYearCoursesQuery(userId, studyYearId));

        //         return Ok(ApiResponse<IEnumerable<RegistrationDto>>.SuccessResponse(
        //             result,
        //             "Year courses retrieved successfully"));
        //     }
        //     catch (Shared.Exceptions.NotFoundException ex)
        //     {
        //         return NotFound(ApiResponse<IEnumerable<RegistrationDto>>.NotFoundResponse(ex.Message));
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.LogError(ex, "Error getting registered year courses");
        //         return StatusCode(500, ApiResponse<IEnumerable<RegistrationDto>>.ServerErrorResponse(
        //             "An error occurred while retrieving year courses"));
        //     }
        // }

        // // ────────────────────────────────────────────────────────────────────────
        // // GET /api/registrations/student/{studyYearId}/year/{semesterId}/semester
        // // ────────────────────────────────────────────────────────────────────────
        // [HttpGet("student/{studyYearId}/year/{semesterId}/semester")]
        // public async Task<ActionResult<ApiResponse<IEnumerable<RegistrationDto>>>> GetRegisteredSemesterCourses(
        //     int studyYearId,
        //     int semesterId)
        // {
        //     try
        //     {
        //         var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //         if (string.IsNullOrEmpty(userId))
        //             return Unauthorized(ApiResponse<IEnumerable<RegistrationDto>>.UnauthorizedResponse("Invalid token"));

        //         var result = await _mediator.Send(new GetRegisteredSemesterCoursesQuery(studyYearId, semesterId, userId));

        //         return Ok(ApiResponse<IEnumerable<RegistrationDto>>.SuccessResponse(
        //             result,
        //             "Semester courses retrieved successfully"));
        //     }
        //     catch (Shared.Exceptions.NotFoundException ex)
        //     {
        //         return NotFound(ApiResponse<IEnumerable<RegistrationDto>>.NotFoundResponse(ex.Message));
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.LogError(ex, "Error getting registered semester courses");
        //         return StatusCode(500, ApiResponse<IEnumerable<RegistrationDto>>.ServerErrorResponse(
        //             "An error occurred while retrieving semester courses"));
        //     }
        // }

        // ────────────────────────────────────────────────────────────────────────
        // PATCH /api/registrations/{registrationId}/grade
        // ────────────────────────────────────────────────────────────────────────
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

        // ────────────────────────────────────────────────────────────────────────
        // PATCH /api/registrations/grades/bulk
        // ────────────────────────────────────────────────────────────────────────
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