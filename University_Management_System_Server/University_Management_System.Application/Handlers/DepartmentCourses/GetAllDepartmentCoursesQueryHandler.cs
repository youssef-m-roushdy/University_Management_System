using AutoMapper;
using MediatR;
using University_Management_System.Application.Dtos.DepartmentCourseDtos;
using University_Management_System.Application.Queries.DepartmentCourses;
using University_Management_System.Domain.Contracts;

namespace University_Management_System.Application.Handlers.DepartmentCourses
{
    public class GetAllDepartmentCoursesQueryHandler : IRequestHandler<GetAllDepartmentCoursesQuery, (IEnumerable<DepartmentCourseDto> Data, int TotalCount)>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllDepartmentCoursesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<(IEnumerable<DepartmentCourseDto> Data, int TotalCount)> Handle(
            GetAllDepartmentCoursesQuery request,
            CancellationToken cancellationToken)
        {
            // ─── Get all department courses ──────────────────────────────────
            var (departmentCourses, totalCount) = await _unitOfWork.DepartmentCourses
                .GetAllDepartmentCoursesAsync(
                    request.Query,
                    cancellationToken);

            var dtos = _mapper.Map<IEnumerable<DepartmentCourseDto>>(departmentCourses);

            return (dtos, totalCount);
        }
    }
}