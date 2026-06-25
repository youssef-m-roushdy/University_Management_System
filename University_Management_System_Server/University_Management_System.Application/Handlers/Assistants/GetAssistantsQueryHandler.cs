using AutoMapper;
using MediatR;
using University_Management_System.Application.Dtos.AssistantDtos;
using University_Management_System.Application.Queries.Assistants;
using University_Management_System.Domain.Contracts;

namespace University_Management_System.Application.Handlers.Assistants
{
    public class GetAssistantsQueryHandler : IRequestHandler<GetAssistantsQuery, (IEnumerable<AssistantDto> Data, int TotalCount)>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAssistantsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<(IEnumerable<AssistantDto> Data, int TotalCount)> Handle(
            GetAssistantsQuery request,
            CancellationToken cancellationToken)
        {
            var (assistants, totalCount) = await _unitOfWork.Assistants
                .GetAllFilteredAsync(request.Query, cancellationToken);

            var assistantDtos = _mapper.Map<IEnumerable<AssistantDto>>(assistants);

            return (assistantDtos, totalCount);
        }
    }
}