using University_Management_System.Application.Commands.Departments;
using AutoMapper;
using University_Management_System.Domain.Entities.Models;
using MediatR;
using University_Management_System.Shared.Responses;
using University_Management_System.Application.Dtos.DepartmentDtos;
using University_Management_System.Domain.Contracts;

namespace University_Management_System.Application.Handlers.Departments
{
    public class CreateDepartmentCommandHandler : IRequestHandler<CreateDepartmentCommand, ApiResponse<DepartmentDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateDepartmentCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<DepartmentDto>> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
        {
            var department = _mapper.Map<Department>(request.Department);

            await _unitOfWork.Departments.AddAsync(department);
            await _unitOfWork.SaveChangesAsync();

            var result = _mapper.Map<DepartmentDto>(department);
            return ApiResponse<DepartmentDto>.SuccessResponse(result);
        }
    }
}