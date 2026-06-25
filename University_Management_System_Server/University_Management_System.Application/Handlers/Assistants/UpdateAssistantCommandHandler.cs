using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using University_Management_System.Application.Commands.Assistants;
using University_Management_System.Application.Dtos.AssistantDtos;
using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Entities.Identity;
using University_Management_System.Shared.Exceptions;

namespace University_Management_System.Application.Handlers.Assistants
{
    public class UpdateAssistantCommandHandler : IRequestHandler<UpdateAssistantCommand, AssistantDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public UpdateAssistantCommandHandler(IUnitOfWork unitOfWork, UserManager<User> userManager, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<AssistantDto> Handle(UpdateAssistantCommand request, CancellationToken cancellationToken)
        {
            // ─── 1. Check if Assistant exists ────────────────────────────────
            var assistant = await _unitOfWork.Assistants
                .GetAssistantByUserIdAsync(request.Id);
            
            if (assistant == null)
                throw new NotFoundException($"Assistant with ID '{request.Id}' not found.");

            // ─── 2. Get User ────────────────────────────────────────────────────
            var user = await _userManager.FindByIdAsync(request.Id);
            if (user == null)
                throw new NotFoundException($"User with ID '{request.Id}' not found.");

 
                var department = await _unitOfWork.Departments
                    .GetByIdAsync(request.Dto.DepartmentId);
                
                if (department == null)
                    throw new NotFoundException($"Department with ID '{request.Dto.DepartmentId}' not found.");
                
                assistant.DepartmentId = request.Dto.DepartmentId;
                
            // ─── 3. Update User Properties ─────────────────────────────────────
            user.UpdatedAt = DateTime.UtcNow;  
            assistant.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Assistants.UpdateAsync(assistant);
            await _unitOfWork.SaveChangesAsync();

            // ─── 4. Return mapped DTO ─────────────────────────────────────────
            return _mapper.Map<AssistantDto>(assistant);
        }
    }
}