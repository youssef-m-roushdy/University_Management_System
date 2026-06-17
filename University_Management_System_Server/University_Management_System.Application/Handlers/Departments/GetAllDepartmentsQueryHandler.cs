using University_Management_System.Application.Queries.Departments;
using AutoMapper;
using MediatR;
using University_Management_System.Shared.Respones;
using University_Management_System.Application.Dtos.DepartmentDtos;
using University_Management_System.Domain.Contracts;

namespace University_Management_System.Application.Handlers.Departments
{
    public class GetAllDepartmentsQueryHandler : IRequestHandler<GetAllDepartmentsQuery, Response<IEnumerable<DepartmentDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllDepartmentsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<DepartmentDto>>> Handle(GetAllDepartmentsQuery request, CancellationToken cancellationToken)
        {
            var departments = await _unitOfWork.Departments.GetAllAsync();
            var result = _mapper.Map<IEnumerable<DepartmentDto>>(departments);
            return Response<IEnumerable<DepartmentDto>>.SuccessResponse(result);
        }
    }
}
