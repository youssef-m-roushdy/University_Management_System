using AutoMapper;
using MediatR;
using University_Management_System.Application.Dtos.AcademicScheduleDtos;
using University_Management_System.Application.Queries.AcademicSchedules;
using University_Management_System.Domain.Contracts;
using University_Management_System.Shared.Exceptions;

namespace University_Management_System.Application.Handlers.AcademicSchedules
{
    public class GetAcademicSchedulesByDepartmentQueryHandler : IRequestHandler<GetAcademicSchedulesByDepartmentQuery, (IEnumerable<AcademicScheduleDto> Data, int TotalCount)>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAcademicSchedulesByDepartmentQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<(IEnumerable<AcademicScheduleDto> Data, int TotalCount)> Handle(
            GetAcademicSchedulesByDepartmentQuery request,
            CancellationToken cancellationToken)
        {
            // ─── Validate Department ──────────────────────────────────────
            var department = await _unitOfWork.Departments.GetByIdAsync(request.DepartmentId);
            if (department == null)
                throw new NotFoundException($"Department with ID '{request.DepartmentId}' not found.");

            // ─── Get schedules ──────────────────────────────────────────
            var (schedules, totalCount) = await _unitOfWork.AcademicSchedules
                .GetByDepartmentIdAsync(
                    request.DepartmentId,
                    request.Filter,
                    cancellationToken);

            return (_mapper.Map<IEnumerable<AcademicScheduleDto>>(schedules), totalCount);
        }
    }
}