using AutoMapper;
using MediatR;
using University_Management_System.Application.Commands.Admins;
using University_Management_System.Application.Contracts;
using University_Management_System.Application.Dtos.AdminDtos;
using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Entities.Identity;
using University_Management_System.Shared.Exceptions;

namespace University_Management_System.Application.Handlers.Admins
{
    public class CreateAdminCommandHandler : IRequestHandler<CreateAdminCommand, AdminDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public CreateAdminCommandHandler(IUnitOfWork unitOfWork, IUserService userService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
            _mapper = mapper;
        }

        public async Task<AdminDto> Handle(CreateAdminCommand request, CancellationToken cancellationToken)
        {
            // ─── 1. Create User with "Admin" role ─────────────────────────────
            var user = await _userService.CreateUserAsync(request.Dto, "Admin");

            // ─── 2. Create Admin Profile ──────────────────────────────────────
            var admin = new Admin
            {
                Id = user.Id,
                User = user,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Admins.AddAsync(admin);
            await _unitOfWork.SaveChangesAsync();

            // ─── 3. Return mapped DTO ─────────────────────────────────────────
            return _mapper.Map<AdminDto>(admin);
        }
    }
}