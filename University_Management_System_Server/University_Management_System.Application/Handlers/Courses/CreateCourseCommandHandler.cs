using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using University_Management_System.Application.Commands.Courses;
using University_Management_System.Application.Dtos.CourseDtos;
using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Entities.Models;
using MediatR;
using University_Management_System.Shared.Responses;

namespace University_Management_System.Application.Handlers.Courses
{
    public class CreateCourseCommandHandler : IRequestHandler<CreateCourseCommand, ApiResponse<CourseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CreateCourseCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<CourseDto>> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
        {
            var course = _mapper.Map<Course>(request.Course);

            var department = await _unitOfWork.Departments.GetByIdAsync(request.Course.DepartmentId);
            if (department is null)                
                throw new Exception($"Department with ID {request.Course.DepartmentId} not found.");

            await _unitOfWork.Courses.AddAsync(course);
            await _unitOfWork.SaveChangesAsync();

            var result = _mapper.Map<CourseDto>(course);
            return ApiResponse<CourseDto>.SuccessResponse(result);
        }
    }
}