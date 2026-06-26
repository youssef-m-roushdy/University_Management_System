using AutoMapper;
using MediatR;
using University_Management_System.Application.Dtos.SpecializationDtos;
using University_Management_System.Application.Queries.Specializations;
using University_Management_System.Domain.Contracts;
using University_Management_System.Shared.Exceptions;

namespace University_Management_System.Application.Handlers.Specializations
{
    public class GetSpecializationsByDepartmentQueryHandler : IRequestHandler<GetSpecializationsByDepartmentQuery, (IEnumerable<SpecializationDto> Data, int TotalCount)>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetSpecializationsByDepartmentQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<(IEnumerable<SpecializationDto> Data, int TotalCount)> Handle(
            GetSpecializationsByDepartmentQuery request,
            CancellationToken cancellationToken)
        {
            // ─── Validate Department exists ──────────────────────────────────
            var department = await _unitOfWork.Departments.GetByIdAsync(request.DepartmentId);
            if (department == null)
                throw new NotFoundException($"Department with ID '{request.DepartmentId}' not found.");

            // ─── Get specializations ──────────────────────────────────────────
            var (specializations, totalCount) = await _unitOfWork.Specializations
                .GetByDepartmentIdAsync(
                    request.DepartmentId,
                    request.Query,
                    cancellationToken);

            var dtos = _mapper.Map<IEnumerable<SpecializationDto>>(specializations);

            return (dtos, totalCount);
        }
    }
}