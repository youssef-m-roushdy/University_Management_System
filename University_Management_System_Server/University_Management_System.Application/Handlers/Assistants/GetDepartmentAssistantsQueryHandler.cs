using AutoMapper;
using MediatR;
using University_Management_System.Application.Dtos.AssistantDtos;
using University_Management_System.Application.Queries.Assistants;
using University_Management_System.Domain.Contracts;
using University_Management_System.Shared.Exceptions;

namespace University_Management_System.Application.Handlers.Assistants
{
    public class GetDepartmentAssistantsQueryHandler : IRequestHandler<GetDepartmentAssistantsQuery, (IEnumerable<AssistantDto> Data, int TotalCount)>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetDepartmentAssistantsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<(IEnumerable<AssistantDto> Data, int TotalCount)> Handle(
            GetDepartmentAssistantsQuery request,
            CancellationToken cancellationToken)
        {
            // ─── Validate Department exists ──────────────────────────────────
            var department = await _unitOfWork.Departments
                .GetByIdAsync(request.DepartmentId);

            if (department == null)
                throw new NotFoundException($"Department with ID '{request.DepartmentId}' not found.");

            // ─── Get assistants in department ──────────────────────────────
            var (assistants, totalCount) = await _unitOfWork.Assistants
                .GetDepartmentAssistantsAsync(
                    request.DepartmentId,
                    request.Query,
                    cancellationToken);

            var assistantDtos = _mapper.Map<IEnumerable<AssistantDto>>(assistants);

            return (assistantDtos, totalCount);
        }
    }
}