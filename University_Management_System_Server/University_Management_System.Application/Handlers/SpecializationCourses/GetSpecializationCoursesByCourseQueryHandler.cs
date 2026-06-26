using AutoMapper;
using MediatR;
using University_Management_System.Application.Dtos.SpecializationCourseDtos;
using University_Management_System.Application.Queries.SpecializationCourses;
using University_Management_System.Domain.Contracts;
using University_Management_System.Shared.Exceptions;

namespace University_Management_System.Application.Handlers.SpecializationCourses
{
    public class GetSpecializationCoursesByCourseQueryHandler : IRequestHandler<GetSpecializationCoursesByCourseQuery, IEnumerable<SpecializationCourseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetSpecializationCoursesByCourseQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SpecializationCourseDto>> Handle(
            GetSpecializationCoursesByCourseQuery request,
            CancellationToken cancellationToken)
        {
            // ─── Validate Course exists ──────────────────────────────────
            var course = await _unitOfWork.Courses.GetByIdAsync(request.CourseId);
            if (course == null)
                throw new NotFoundException($"Course with ID '{request.CourseId}' not found.");

            // ─── Get specialization courses ──────────────────────────────────────
            var specializationCourses = await _unitOfWork.SpecializationCourses
                .GetByCourseIdWithDetailsAsync(request.CourseId);

            return _mapper.Map<IEnumerable<SpecializationCourseDto>>(specializationCourses);
        }
    }
}