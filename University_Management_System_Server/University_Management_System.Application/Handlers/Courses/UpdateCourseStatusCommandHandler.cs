using AutoMapper;
using MediatR;
using University_Management_System.Application.Commands.Courses;
using University_Management_System.Application.Dtos.CourseDtos;
using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Enums;
using University_Management_System.Shared.Exceptions;

namespace University_Management_System.Application.Handlers.Courses
{
    public class UpdateCourseStatusCommandHandler : IRequestHandler<UpdateCourseStatusCommand, CourseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateCourseStatusCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CourseDto> Handle(UpdateCourseStatusCommand request, CancellationToken cancellationToken)
        {
            // ─── 1. Check if Course exists ──────────────────────────────────
            var course = await _unitOfWork.Courses
                .GetByIdAsync(request.Id);
            
            if (course == null)
                throw new NotFoundException($"Course with ID '{request.Id}' not found.");

            // ─── 2. Update Status ─────────────────────────────────────────────
            course.Status = request.Status;
            course.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Courses.UpdateAsync(course);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CourseDto>(course);
        }
    }
}