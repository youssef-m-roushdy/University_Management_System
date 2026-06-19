using University_Management_System.Application.Queries.Departments;
using AutoMapper;
using University_Management_System.Shared.Exceptions;
using MediatR;
using University_Management_System.Shared.Responses;
using University_Management_System.Application.Dtos.DepartmentDtos;
using University_Management_System.Domain.Contracts;

namespace University_Management_System.Application.Handlers.Departments
{
    public class GetDepartmentByIdQueryHandler : IRequestHandler<GetDepartmentByIdQuery, ApiResponse<DepartmentDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetDepartmentByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<DepartmentDto>> Handle(GetDepartmentByIdQuery request, CancellationToken cancellationToken)
        {
            var department = await _unitOfWork.Departments.GetByIdWithDetailsAsync(request.Id);
            
            if (department == null)
                throw new NotFoundException($"Department with ID {request.Id} not found");

            var result = _mapper.Map<DepartmentDto>(department);
            return ApiResponse<DepartmentDto>.SuccessResponse(result);
        }
    }
}
