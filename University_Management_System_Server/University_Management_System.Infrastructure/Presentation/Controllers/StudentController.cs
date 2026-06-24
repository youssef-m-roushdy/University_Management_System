using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using University_Management_System.Application.Commands.Students;
using University_Management_System.Application.Dtos.StudentDtos;
using University_Management_System.Application.Queries.Students;
using University_Management_System.Domain.Queries.StudentQueries;
using University_Management_System.Shared.Responses;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using AutoMapper;

namespace University_Management_System.Infrastructure.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class StudentController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<StudentController> _logger;
        private readonly IMapper _mapper;

        public StudentController(IMediator mediator, ILogger<StudentController> logger, IMapper mapper)
        {
            _mediator = mediator;
            _logger = logger;
            _mapper = mapper;
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET /api/students
        // ────────────────────────────────────────────────────────────────────────
        /// <summary>
        /// Get all students with filters and pagination (Admin only)
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<PagedResponse<StudentDto>>> GetAllStudents([FromQuery] StudentFilterQueries query)
        {
            try
            {
                var result = await _mediator.Send(new GetStudentsQuery { Query = query });

                var response = PagedResponse<StudentDto>.SuccessResponse(
                    result.Data,
                    query.PageNumber,
                    query.PageSize,
                    result.TotalCount,
                    "Students retrieved successfully"
                );

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting students");
                return StatusCode(500, PagedResponse<StudentDto>.ServerErrorResponse("An error occurred while retrieving students"));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET /api/students/department/{departmentId}
        // ────────────────────────────────────────────────────────────────────────
        /// <summary>
        /// Get students by department (Admin, Instructor, Assistant)
        /// </summary>
        [HttpGet("department/{departmentId}")]
        [Authorize(Roles = "Admin,Instructor,Assistant")]
        public async Task<ActionResult<PagedResponse<StudentDto>>> GetDepartmentStudents(
            int departmentId,
            [FromQuery] StudentDepartmentQueries query)
        {
            try
            {
                var result = await _mediator.Send(new GetDepartmentStudentsQuery
                {
                    DepartmentId = departmentId,
                    Query = query
                });

                var response = PagedResponse<StudentDto>.SuccessResponse(
                    result.Data,
                    query.PageNumber,
                    query.PageSize,
                    result.TotalCount,
                    "Students retrieved successfully"
                );

                return Ok(response);
            }
            catch (Shared.Exceptions.NotFoundException ex)
            {
                return NotFound(PagedResponse<StudentDto>.NotFoundResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting department students");
                return StatusCode(500, PagedResponse<StudentDto>.ServerErrorResponse("An error occurred while retrieving students"));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET /api/students/{id}
        // ────────────────────────────────────────────────────────────────────────
        /// <summary>
        /// Get student by ID (Admin only)
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<StudentDto>>> GetStudentById(string id)
        {
            try
            {
                var result = await _mediator.Send(new GetStudentQuery { Id = id });

                if (result == null)
                    return NotFound(ApiResponse<StudentDto>.NotFoundResponse($"Student with ID '{id}' not found"));


                return Ok(ApiResponse<StudentDto>.SuccessResponse(result, "Student retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting student by ID: {Id}", id);
                return StatusCode(500, ApiResponse<StudentDto>.ServerErrorResponse("An error occurred while retrieving student"));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET /api/students/academic-code/{academicCode}
        // ────────────────────────────────────────────────────────────────────────
        /// <summary>
        /// Get student by academic code (Admin only)
        /// </summary>
        [HttpGet("academic-code/{academicCode}")]
        public async Task<ActionResult<ApiResponse<StudentDto>>> GetStudentByAcademicCode(string academicCode)
        {
            try
            {
                var result = await _mediator.Send(new GetStudentByAcademicCodeQuery { AcademicCode = academicCode });

                if (result == null)
                    return NotFound(ApiResponse<StudentDto>.NotFoundResponse($"Student with academic code '{academicCode}' not found"));

                return Ok(ApiResponse<StudentDto>.SuccessResponse(result, "Student retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting student by academic code: {AcademicCode}", academicCode);
                return StatusCode(500, ApiResponse<StudentDto>.ServerErrorResponse("An error occurred while retrieving student"));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // POST /api/students
        // ────────────────────────────────────────────────────────────────────────
        /// <summary>
        /// Create a new student (Admin only)
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ApiResponse<StudentDto>>> CreateStudent([FromBody] CreateStudentDto dto)
        {
            try
            {
                var command = new CreateStudentCommand { Dto = dto };
                var result = await _mediator.Send(command);

                return StatusCode(201, ApiResponse<StudentDto>.SuccessResponse(result, "Student created successfully"));
            }
            catch (Shared.Exceptions.NotFoundException ex)
            {
                return NotFound(ApiResponse<StudentDto>.NotFoundResponse(ex.Message));
            }
            catch (Shared.Exceptions.ValidationException ex)
            {
                return BadRequest(ApiResponse<StudentDto>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating student");
                return StatusCode(500, ApiResponse<StudentDto>.ServerErrorResponse("An error occurred while creating student"));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // PUT /api/students/{id}
        // ────────────────────────────────────────────────────────────────────────
        /// <summary>
        /// Update a student (Admin only)
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<StudentDto>>> UpdateStudent(string id, [FromBody] UpdateStudentDto dto)
        {
            try
            {
                var command = new UpdateStudentCommand
                {
                    Id = id,
                    Dto = dto
                };

                var result = await _mediator.Send(command);

                var studentDto = _mapper.Map<StudentDto>(result);

                return Ok(ApiResponse<StudentDto>.SuccessResponse(studentDto, "Student updated successfully"));
            }
            catch (Shared.Exceptions.NotFoundException ex)
            {
                return NotFound(ApiResponse<StudentDto>.NotFoundResponse(ex.Message));
            }
            catch (Shared.Exceptions.ValidationException ex)
            {
                return BadRequest(ApiResponse<StudentDto>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating student: {Id}", id);
                return StatusCode(500, ApiResponse<StudentDto>.ServerErrorResponse("An error occurred while updating student"));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // DELETE /api/students/{id}
        // ────────────────────────────────────────────────────────────────────────
        /// <summary>
        /// Delete a student (Admin only)
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteStudent(string id)
        {
            try
            {
                var result = await _mediator.Send(new DeleteStudentCommand { Id = id });

                return Ok(ApiResponse<object>.SuccessResponse("Student deleted successfully"));
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
                _logger.LogError(ex, "Error deleting student: {Id}", id);
                return StatusCode(500, ApiResponse<object>.ServerErrorResponse("An error occurred while deleting student"));
            }
        }
    }
}