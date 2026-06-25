using AutoMapper;
using MediatR;
using University_Management_System.Application.Dtos.CourseDtos;
using University_Management_System.Application.Queries.Courses;
using University_Management_System.Domain.Contracts;
using University_Management_System.Shared.Exceptions;

namespace University_Management_System.Application.Handlers.Courses
{
    public class GetCoursePrerequisitesQueryHandler : IRequestHandler<GetCoursePrerequisitesQuery, IEnumerable<CourseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetCoursePrerequisitesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CourseDto>> Handle(
            GetCoursePrerequisitesQuery request,
            CancellationToken cancellationToken)
        {
            // ─── Check if Course exists ──────────────────────────────────────
            var course = await _unitOfWork.Courses
                .GetByIdAsync(request.CourseId);
            
            if (course == null)
                throw new NotFoundException($"Course with ID '{request.CourseId}' not found.");

            // ─── Get Prerequisites ─────────────────────────────────────────────
            var prerequisites = await _unitOfWork.Courses
                .GetPrerequisitesAsync(request.CourseId);

            return _mapper.Map<IEnumerable<CourseDto>>(prerequisites);
        }
    }
}