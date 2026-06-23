using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Application.Commands.Fees;
using University_Management_System.Application.Dtos.DepartmentDtos.FeeDtos;
using University_Management_System.Application.Queries.Fees;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using University_Management_System.Shared.Responses;
using University_Management_System.Domain.Queries.FeeQueries;
using Microsoft.Extensions.Logging;

namespace University_Management_System.Infrastructure.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeeController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<FeeController> _logger;

        public FeeController(IMediator mediator, ILogger<FeeController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateFee([FromBody] CreateFeeDto feeDto)
        {
            var command = new CreateFeeCommand(feeDto);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFee(int id)
        {            var command = new DeleteFeeCommand(id);
            await _mediator.Send(command);
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFee(int id, [FromBody] UpdateFeeDto feeDto)
        {            
            var command = new UpdateFeeCommand(id, feeDto);
            await _mediator.Send(command);
            return NoContent();
        }


        [HttpGet("study-year/{studyYearId}")]
        public async Task<ActionResult<PagedResponse<FeeDto>>> GetStudyYearFees(
            int studyYearId,
            [FromQuery] FeeStudyYearQueries query)
        {
            try
            {
                var result = await _mediator.Send(new GetStudyYearFeesQuery {Query = query, StudyYearId = studyYearId});

                var response = PagedResponse<FeeDto>.SuccessResponse(
                    result.Data,
                    query.PageNumber,
                    query.PageSize,
                    result.TotalCount,
                    "Fees retrieved successfully"
                );

                return Ok(response);
            }
            catch (Shared.Exceptions.NotFoundException ex)
            {
                return NotFound(PagedResponse<FeeDto>.NotFoundResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting fees for study year: {Id}", studyYearId);
                return StatusCode(500, PagedResponse<FeeDto>.ServerErrorResponse(
                    "An error occurred while retrieving fees"));
            }
        }   
    }
}