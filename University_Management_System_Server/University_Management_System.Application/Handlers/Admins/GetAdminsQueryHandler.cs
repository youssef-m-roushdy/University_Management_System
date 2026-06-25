using AutoMapper;
using MediatR;
using University_Management_System.Application.Dtos.AdminDtos;
using University_Management_System.Application.Queries.Admins;
using University_Management_System.Domain.Contracts;

namespace University_Management_System.Application.Handlers.Admins
{
    public class GetAdminsQueryHandler : IRequestHandler<GetAdminsQuery, (IEnumerable<AdminDto> Data, int TotalCount)>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAdminsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<(IEnumerable<AdminDto> Data, int TotalCount)> Handle(
            GetAdminsQuery request,
            CancellationToken cancellationToken)
        {
            var (admins, totalCount) = await _unitOfWork.Admins
                .GetAllFilteredAsync(request.Query, cancellationToken);

            var adminDtos = _mapper.Map<IEnumerable<AdminDto>>(admins);

            return (adminDtos, totalCount);
        }
    }
}