using AutoMapper;
using MediatR;
using University_Management_System.Application.Dtos.InstructorDtos;
using University_Management_System.Application.Queries.Instructors;
using University_Management_System.Domain.Contracts;
using University_Management_System.Shared.Exceptions;

namespace University_Management_System.Application.Handlers.Instructors
{
    public class GetDepartmentInstructorsQueryHandler : IRequestHandler<GetDepartmentInstructorsQuery, (IEnumerable<InstructorDto> Data, int TotalCount)>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetDepartmentInstructorsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<(IEnumerable<InstructorDto> Data, int TotalCount)> Handle(
            GetDepartmentInstructorsQuery request,
            CancellationToken cancellationToken)
        {
            // ─── Validate Department exists ──────────────────────────────────
            var department = await _unitOfWork.Departments
                .GetByIdAsync(request.DepartmentId);

            if (department == null)
                throw new NotFoundException($"Department with ID '{request.DepartmentId}' not found.");

            // ─── Get instructors in department ──────────────────────────────
            var (instructors, totalCount) = await _unitOfWork.Instructors
                .GetDepartmentInstructorsAsync(
                    request.DepartmentId,
                    request.Query,
                    cancellationToken);

            var instructorDtos = _mapper.Map<IEnumerable<InstructorDto>>(instructors);

            return (instructorDtos, totalCount);
        }
    }
}