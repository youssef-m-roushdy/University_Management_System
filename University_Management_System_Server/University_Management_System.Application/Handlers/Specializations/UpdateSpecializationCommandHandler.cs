using AutoMapper;
using MediatR;
using University_Management_System.Application.Commands.Specializations;
using University_Management_System.Application.Dtos.SpecializationDtos;
using University_Management_System.Domain.Contracts;
using University_Management_System.Shared.Exceptions;

namespace University_Management_System.Application.Handlers.Specializations
{
    public class UpdateSpecializationCommandHandler : IRequestHandler<UpdateSpecializationCommand, SpecializationDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateSpecializationCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<SpecializationDto> Handle(UpdateSpecializationCommand request, CancellationToken cancellationToken)
        {
            // ─── 1. Check if Specialization exists ────────────────────────────
            var specialization = await _unitOfWork.Specializations
                .GetByIdAsync(request.Id);
            
            if (specialization == null)
                throw new NotFoundException($"Specialization with ID '{request.Id}' not found.");

            // ─── 2. Validate Department (if provided) ──────────────────────────
            if (request.Dto.DepartmentId.HasValue)
            {
                var department = await _unitOfWork.Departments
                    .GetByIdAsync(request.Dto.DepartmentId.Value);
                
                if (department == null)
                    throw new NotFoundException($"Department with ID '{request.Dto.DepartmentId}' not found.");
            }

            // ─── 3. Check if name is taken (if provided) ──────────────────────
            if (!string.IsNullOrEmpty(request.Dto.Name) && request.Dto.Name != specialization.Name)
            {
                var exists = await _unitOfWork.Specializations.ExistsAsync(request.Dto.Name);
                if (exists)
                    throw new ValidationException(new List<string> {
                        $"Specialization '{request.Dto.Name}' already exists."
                    });
            }

            // ─── 4. Update Specialization ──────────────────────────────────────
            if (!string.IsNullOrEmpty(request.Dto.Name))
                specialization.Name = request.Dto.Name;

            if (!string.IsNullOrEmpty(request.Dto.Description))
                specialization.Description = request.Dto.Description;

            if (request.Dto.DepartmentId.HasValue)
                specialization.DepartmentId = request.Dto.DepartmentId.Value;

            specialization.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Specializations.UpdateAsync(specialization);
            await _unitOfWork.SaveChangesAsync();

            // ─── 5. Get full details ──────────────────────────────────────────
            var updated = await _unitOfWork.Specializations
                .GetByNameWithDetailsAsync(specialization.Name);

            return _mapper.Map<SpecializationDto>(updated);
        }
    }
}