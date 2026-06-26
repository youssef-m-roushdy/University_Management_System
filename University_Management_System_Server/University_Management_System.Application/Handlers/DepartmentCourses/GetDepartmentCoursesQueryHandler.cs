using AutoMapper;
using MediatR;
using University_Management_System.Application.Dtos.DepartmentCourseDtos;
using University_Management_System.Application.Queries.DepartmentCourses;
using University_Management_System.Domain.Contracts;
using University_Management_System.Shared.Exceptions;

namespace University_Management_System.Application.Handlers.DepartmentCourses
{
    public class GetDepartmentCoursesQueryHandler : IRequestHandler<GetDepartmentCoursesQuery, (IEnumerable<DepartmentCourseDto> Data, int TotalCount)>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetDepartmentCoursesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<(IEnumerable<DepartmentCourseDto> Data, int TotalCount)> Handle(
            GetDepartmentCoursesQuery request,
            CancellationToken cancellationToken)
        {
            // ─── Validate Department exists ──────────────────────────────────
            var department = await _unitOfWork.Departments.GetByIdAsync(request.DepartmentId);
            if (department == null)
                throw new NotFoundException($"Department with ID '{request.DepartmentId}' not found.");

            // ─── Get department courses ──────────────────────────────────────
            var (departmentCourses, totalCount) = await _unitOfWork.DepartmentCourses
                .GetCoursesByDepartmentIdAsync(
                    request.DepartmentId,
                    request.Query,
                    cancellationToken);

            var dtos = _mapper.Map<IEnumerable<DepartmentCourseDto>>(departmentCourses);

            return (dtos, totalCount);
        }
    }
}