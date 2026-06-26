using AutoMapper;
using MediatR;
using University_Management_System.Application.Commands.Specializations;
using University_Management_System.Application.Dtos.SpecializationDtos;
using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Entities.Models;
using University_Management_System.Shared.Exceptions;

namespace University_Management_System.Application.Handlers.Specializations
{
    public class CreateSpecializationCommandHandler : IRequestHandler<CreateSpecializationCommand, SpecializationDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateSpecializationCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<SpecializationDto> Handle(CreateSpecializationCommand request, CancellationToken cancellationToken)
        {
            // ─── 1. Validate Department exists ──────────────────────────────────
            var department = await _unitOfWork.Departments.GetByIdAsync(request.Dto.DepartmentId);
            if (department == null)
                throw new NotFoundException($"Department with ID '{request.Dto.DepartmentId}' not found.");

            // ─── 2. Check if specialization already exists ────────────────────
            var exists = await _unitOfWork.Specializations.ExistsAsync(request.Dto.Name);
            if (exists)
                throw new ValidationException(new List<string> {
                    $"Specialization '{request.Dto.Name}' already exists."
                });

            // ─── 3. Check if specialization exists in same department ──────────
            var existsInDepartment = await _unitOfWork.Specializations
                .ExistsByNameAndDepartmentAsync(request.Dto.Name, request.Dto.DepartmentId);
            if (existsInDepartment)
                throw new ValidationException(new List<string> {
                    $"Specialization '{request.Dto.Name}' already exists in department '{department.Name}'."
                });

            // ─── 4. Create Specialization ──────────────────────────────────────
            var specialization = new Specialization
            {
                Name = request.Dto.Name,
                Description = request.Dto.Description,
                DepartmentId = request.Dto.DepartmentId,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Specializations.AddAsync(specialization);
            await _unitOfWork.SaveChangesAsync();

            // ─── 5. Get full details ──────────────────────────────────────────
            var created = await _unitOfWork.Specializations
                .GetByNameWithDetailsAsync(specialization.Name);

            return _mapper.Map<SpecializationDto>(created);
        }
    }
}