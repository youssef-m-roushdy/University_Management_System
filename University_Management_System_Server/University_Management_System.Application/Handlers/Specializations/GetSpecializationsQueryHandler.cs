using AutoMapper;
using MediatR;
using University_Management_System.Application.Dtos.SpecializationDtos;
using University_Management_System.Application.Queries.Specializations;
using University_Management_System.Domain.Contracts;

namespace University_Management_System.Application.Handlers.Specializations
{
    public class GetSpecializationsQueryHandler : IRequestHandler<GetSpecializationsQuery, (IEnumerable<SpecializationDto> Data, int TotalCount)>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetSpecializationsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<(IEnumerable<SpecializationDto> Data, int TotalCount)> Handle(
            GetSpecializationsQuery request,
            CancellationToken cancellationToken)
        {
            var (specializations, totalCount) = await _unitOfWork.Specializations
                .GetAllFilteredAsync(request.Query, cancellationToken);

            var dtos = _mapper.Map<IEnumerable<SpecializationDto>>(specializations);

            return (dtos, totalCount);
        }
    }
}