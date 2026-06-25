using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using University_Management_System.Application.Commands.Admins;
using University_Management_System.Application.Dtos.AdminDtos;
using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Entities.Identity;
using University_Management_System.Shared.Exceptions;

namespace University_Management_System.Application.Handlers.Admins
{
    public class AddAdminToExistingUserCommandHandler : IRequestHandler<AddAdminToExistingUserCommand, AdminDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public AddAdminToExistingUserCommandHandler(
            IUnitOfWork unitOfWork,
            UserManager<User> userManager,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<AdminDto> Handle(AddAdminToExistingUserCommand request, CancellationToken cancellationToken)
        {
            // ─── 1. Find User by Email ──────────────────────────────────────
            var user = await _userManager.FindByEmailAsync(request.Dto.UserEmail);
            if (user == null)
                throw new NotFoundException($"User with email '{request.Dto.UserEmail}' not found.");

            // ─── 2. Check if User already has an Admin profile ──────────────
            var existingAdmin = await _unitOfWork.Admins
                .GetAdminByUserIdAsync(user.Id);

            if (existingAdmin != null)
                throw new ValidationException(new List<string> {
                    $"User '{request.Dto.UserEmail}' already has an Admin profile."
                });

            // ─── 3. Check if User already has "Admin" role ──────────────────
            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            if (isAdmin)
                throw new ValidationException(new List<string> {
                    $"User '{request.Dto.UserEmail}' already has the Admin role."
                });

            // ─── 4. Create Admin Profile ──────────────────────────────────────
            var admin = new Admin
            {
                Id = user.Id,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Admins.AddAsync(admin);
            await _unitOfWork.SaveChangesAsync();

            // ─── 5. Assign "Admin" role to User ──────────────────────────────
            var roleResult = await _userManager.AddToRoleAsync(user, "Admin");
            if (!roleResult.Succeeded)
            {
                var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                throw new Exception($"Failed to assign Admin role: {errors}");
            }

            // ─── 6. Return mapped DTO ─────────────────────────────────────────
            return _mapper.Map<AdminDto>(admin);
        }
    }
}