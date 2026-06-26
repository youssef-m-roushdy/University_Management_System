using AutoMapper;
using MediatR;
using University_Management_System.Application.Dtos.SpecializationDtos;
using University_Management_System.Application.Queries.Specializations;
using University_Management_System.Domain.Contracts;

namespace University_Management_System.Application.Handlers.Specializations
{
    public class GetSpecializationQueryHandler : IRequestHandler<GetSpecializationQuery, SpecializationDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetSpecializationQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<SpecializationDto?> Handle(GetSpecializationQuery request, CancellationToken cancellationToken)
        {
            var specialization = await _unitOfWork.Specializations
                .GetByNameWithDetailsAsync(request.Id.ToString());
            
            if (specialization == null)
                return null;

            return _mapper.Map<SpecializationDto>(specialization);
        }
    }
}