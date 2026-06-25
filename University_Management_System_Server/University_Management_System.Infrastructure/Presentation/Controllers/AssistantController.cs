using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using University_Management_System.Application.Commands.Assistants;
using University_Management_System.Application.Dtos.AssistantDtos;
using University_Management_System.Application.Queries.Assistants;
using University_Management_System.Domain.Queries.AssistantQueries;
using University_Management_System.Shared.Responses;
using Microsoft.Extensions.Logging;
using AutoMapper;

namespace University_Management_System.Infrastructure.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AssistantController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AssistantController> _logger;

        public AssistantController(IMediator mediator, ILogger<AssistantController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET /api/assistants
        // ────────────────────────────────────────────────────────────────────────
        [HttpGet]
        public async Task<ActionResult<PagedResponse<AssistantDto>>> GetAllAssistants([FromQuery] AssistantFilterQueries query)
        {
            try
            {
                var result = await _mediator.Send(new GetAssistantsQuery { Query = query });

                var response = PagedResponse<AssistantDto>.SuccessResponse(
                    result.Data,
                    query.PageNumber,
                    query.PageSize,
                    result.TotalCount,
                    "Assistants retrieved successfully"
                );

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting assistants");
                return StatusCode(500, PagedResponse<AssistantDto>.ServerErrorResponse("An error occurred while retrieving assistants"));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET /api/assistants/{id}
        // ────────────────────────────────────────────────────────────────────────
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<AssistantDto>>> GetAssistantById(string id)
        {
            try
            {
                var result = await _mediator.Send(new GetAssistantQuery { Id = id });

                if (result == null)
                    return NotFound(ApiResponse<AssistantDto>.NotFoundResponse($"Assistant with ID '{id}' not found"));

                return Ok(ApiResponse<AssistantDto>.SuccessResponse(result, "Assistant retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting assistant by ID: {Id}", id);
                return StatusCode(500, ApiResponse<AssistantDto>.ServerErrorResponse("An error occurred while retrieving assistant"));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET /api/assistants/department/{departmentId}
        // ────────────────────────────────────────────────────────────────────────
        [HttpGet("department/{departmentId}")]
        [Authorize(Roles = "Admin,Instructor,Assistant")]
        public async Task<ActionResult<PagedResponse<AssistantDto>>> GetDepartmentAssistants(
            int departmentId,
            [FromQuery] AssistantDepartmentQueries query)
        {
            try
            {
                var result = await _mediator.Send(new GetDepartmentAssistantsQuery
                {
                    DepartmentId = departmentId,
                    Query = query
                });

                var response = PagedResponse<AssistantDto>.SuccessResponse(
                    result.Data,
                    query.PageNumber,
                    query.PageSize,
                    result.TotalCount,
                    "Assistants retrieved successfully"
                );

                return Ok(response);
            }
            catch (Shared.Exceptions.NotFoundException ex)
            {
                return NotFound(PagedResponse<AssistantDto>.NotFoundResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting department assistants");
                return StatusCode(500, PagedResponse<AssistantDto>.ServerErrorResponse("An error occurred while retrieving assistants"));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // POST /api/assistants
        // ────────────────────────────────────────────────────────────────────────
        [HttpPost]
        public async Task<ActionResult<ApiResponse<AssistantDto>>> CreateAssistant([FromBody] CreateAssistantDto dto)
        {
            try
            {
                var command = new CreateAssistantCommand { Dto = dto };
                var result = await _mediator.Send(command);

                return StatusCode(201, ApiResponse<AssistantDto>.SuccessResponse(result, "Assistant created successfully"));
            }
            catch (Shared.Exceptions.NotFoundException ex)
            {
                return NotFound(ApiResponse<AssistantDto>.NotFoundResponse(ex.Message));
            }
            catch (Shared.Exceptions.ValidationException ex)
            {
                return BadRequest(ApiResponse<AssistantDto>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating assistant");
                return StatusCode(500, ApiResponse<AssistantDto>.ServerErrorResponse("An error occurred while creating assistant"));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // PUT /api/assistants/{id}
        // ────────────────────────────────────────────────────────────────────────
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<AssistantDto>>> UpdateAssistant(string id, [FromBody] UpdateAssistantDto dto)
        {
            try
            {
                var command = new UpdateAssistantCommand
                {
                    Id = id,
                    Dto = dto
                };

                var result = await _mediator.Send(command);

                return Ok(ApiResponse<AssistantDto>.SuccessResponse(result, "Assistant updated successfully"));
            }
            catch (Shared.Exceptions.NotFoundException ex)
            {
                return NotFound(ApiResponse<AssistantDto>.NotFoundResponse(ex.Message));
            }
            catch (Shared.Exceptions.ValidationException ex)
            {
                return BadRequest(ApiResponse<AssistantDto>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating assistant: {Id}", id);
                return StatusCode(500, ApiResponse<AssistantDto>.ServerErrorResponse("An error occurred while updating assistant"));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // DELETE /api/assistants/{id}
        // ────────────────────────────────────────────────────────────────────────
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteAssistant(string id)
        {
            try
            {
                var result = await _mediator.Send(new DeleteAssistantCommand { Id = id });

                return Ok(ApiResponse<object>.SuccessResponse("Assistant deleted successfully"));
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
                _logger.LogError(ex, "Error deleting assistant: {Id}", id);
                return StatusCode(500, ApiResponse<object>.ServerErrorResponse("An error occurred while deleting assistant"));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // POST /api/assistants/add-to-existing-user
        // ────────────────────────────────────────────────────────────────────────
        [HttpPost("add-to-existing-user")]
        public async Task<ActionResult<ApiResponse<AssistantDto>>> AddAssistantToExistingUser(
            [FromBody] AddAssistantToExistingUserCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                return StatusCode(201, ApiResponse<AssistantDto>.SuccessResponse(result, "Assistant profile added to existing user successfully"));
            }
            catch (Shared.Exceptions.NotFoundException ex)
            {
                return NotFound(ApiResponse<AssistantDto>.NotFoundResponse(ex.Message));
            }
            catch (Shared.Exceptions.ValidationException ex)
            {
                return BadRequest(ApiResponse<AssistantDto>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding assistant to existing user");
                return StatusCode(500, ApiResponse<AssistantDto>.ServerErrorResponse("An error occurred while adding assistant profile"));
            }
        }
    }
}