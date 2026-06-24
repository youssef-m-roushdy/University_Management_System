using AutoMapper;
using MediatR;
using University_Management_System.Application.Dtos.StudentDtos;
using University_Management_System.Application.Queries.Students;
using University_Management_System.Domain.Contracts;
using University_Management_System.Shared.Exceptions;

namespace University_Management_System.Application.Handlers.Students
{
    public class GetDepartmentStudentsQueryHandler : IRequestHandler<GetDepartmentStudentsQuery, (IEnumerable<StudentDto> Data, int TotalCount)>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetDepartmentStudentsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<(IEnumerable<StudentDto> Data, int TotalCount)> Handle(
            GetDepartmentStudentsQuery request, 
            CancellationToken cancellationToken)
        {
            // ─── Validate Department exists ──────────────────────────────────
            var department = await _unitOfWork.Departments
                .GetByIdAsync(request.DepartmentId);

            if (department == null)
                throw new NotFoundException($"Department with ID '{request.DepartmentId}' not found.");

            // ─── Get students in department ──────────────────────────────────
            var (students, totalCount) = await _unitOfWork.Students
                .GetDepartmentStudentsAsync(
                    request.DepartmentId,
                    request.Query,
                    cancellationToken);

            var studentDtos = _mapper.Map<IEnumerable<StudentDto>>(students);

            return (studentDtos, totalCount);
        }
    }
}