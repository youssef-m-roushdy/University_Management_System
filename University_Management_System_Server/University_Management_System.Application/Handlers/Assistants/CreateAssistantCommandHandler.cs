using AutoMapper;
using MediatR;
using University_Management_System.Application.Commands.Assistants;
using University_Management_System.Application.Contracts;
using University_Management_System.Application.Dtos.AssistantDtos;
using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Entities.Identity;
using University_Management_System.Shared.Exceptions;

namespace University_Management_System.Application.Handlers.Assistants
{
    public class CreateAssistantCommandHandler : IRequestHandler<CreateAssistantCommand, AssistantDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public CreateAssistantCommandHandler(IUnitOfWork unitOfWork, IUserService userService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
            _mapper = mapper;
        }

        public async Task<AssistantDto> Handle(CreateAssistantCommand request, CancellationToken cancellationToken)
        {
            // ─── 1. Validate Department ──────────────────────────────────────
            var department = await _unitOfWork.Departments
                .GetByIdAsync(request.Dto.DepartmentId);
            
            if (department == null)
                throw new NotFoundException($"Department with ID '{request.Dto.DepartmentId}' not found.");

            // ─── 2. Create User with "Assistant" role ──────────────────────
            var user = await _userService.CreateUserAsync(request.Dto, "Assistant");

            // ─── 3. Create Assistant Profile ──────────────────────────────────
            var assistant = new Assistant
            {
                Id = user.Id,
                DepartmentId = request.Dto.DepartmentId,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Assistants.AddAsync(assistant);
            await _unitOfWork.SaveChangesAsync();

            // ─── 4. Return mapped DTO ─────────────────────────────────────────
            return _mapper.Map<AssistantDto>(assistant);
        }
    }
}