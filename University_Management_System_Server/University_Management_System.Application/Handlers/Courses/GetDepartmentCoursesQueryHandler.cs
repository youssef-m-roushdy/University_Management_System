using AutoMapper;
using MediatR;
using University_Management_System.Application.Dtos.CourseDtos;
using University_Management_System.Application.Queries.Courses;
using University_Management_System.Domain.Contracts;
using University_Management_System.Shared.Exceptions;

namespace University_Management_System.Application.Handlers.Courses
{
    public class GetDepartmentCoursesQueryHandler : IRequestHandler<GetDepartmentCoursesQuery, (IEnumerable<CourseDto> Data, int TotalCount)>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetDepartmentCoursesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<(IEnumerable<CourseDto> Data, int TotalCount)> Handle(
            GetDepartmentCoursesQuery request,
            CancellationToken cancellationToken)
        {
            // ─── Validate Department ──────────────────────────────────────────
            var department = await _unitOfWork.Departments
                .GetByIdAsync(request.DepartmentId);
            
            if (department == null)
                throw new NotFoundException($"Department with ID '{request.DepartmentId}' not found.");

            // ─── Get Courses ──────────────────────────────────────────────────
            var (courses, totalCount) = await _unitOfWork.Courses
                .GetByDepartmentAsync(request.DepartmentId, request.Query, cancellationToken);

            var courseDtos = _mapper.Map<IEnumerable<CourseDto>>(courses);

            return (courseDtos, totalCount);
        }
    }
}