using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Application.Commands.Courses;
using University_Management_System.Domain.Contracts;
using MediatR;
using University_Management_System.Shared.Exceptions;

namespace University_Management_System.Application.Handlers.Courses
{
    public class UpdateCourseStatusCommandHandler : IRequestHandler<UpdateCourseStatusCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCourseStatusCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateCourseStatusCommand request, CancellationToken cancellationToken)
        {
            var course = await _unitOfWork.Courses.GetByIdAsync(request.Dto.CourseId);
            if (course == null)
                throw new NotFoundException("Course not found");
            
            await _unitOfWork.Courses.UpdateCourseStatusAsync(request.Dto.CourseId, request.Dto.Status);
            await _unitOfWork.SaveChangesAsync();
            return Unit.Value;
        }
    }
}