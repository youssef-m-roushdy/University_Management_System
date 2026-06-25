using AutoMapper;
using MediatR;
using University_Management_System.Application.Dtos.AdminDtos;
using University_Management_System.Application.Queries.Admins;
using University_Management_System.Domain.Contracts;

namespace University_Management_System.Application.Handlers.Admins
{
    public class GetAdminQueryHandler : IRequestHandler<GetAdminQuery, AdminDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAdminQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<AdminDto?> Handle(GetAdminQuery request, CancellationToken cancellationToken)
        {
            var admin = await _unitOfWork.Admins.GetAdminByUserIdAsync(request.Id);
            
            if (admin == null)
                return null;

            return _mapper.Map<AdminDto>(admin);
        }
    }
}