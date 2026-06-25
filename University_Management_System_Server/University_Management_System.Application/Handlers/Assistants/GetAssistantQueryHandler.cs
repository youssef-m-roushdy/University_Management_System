using AutoMapper;
using MediatR;
using University_Management_System.Application.Dtos.AssistantDtos;
using University_Management_System.Application.Queries.Assistants;
using University_Management_System.Domain.Contracts;

namespace University_Management_System.Application.Handlers.Assistants
{
    public class GetAssistantQueryHandler : IRequestHandler<GetAssistantQuery, AssistantDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAssistantQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<AssistantDto?> Handle(GetAssistantQuery request, CancellationToken cancellationToken)
        {
            var assistant = await _unitOfWork.Assistants
                .GetAssistantByUserIdAsync(request.Id);
            
            if (assistant == null)
                return null;

            return _mapper.Map<AssistantDto>(assistant);
        }
    }
}