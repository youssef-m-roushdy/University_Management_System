using University_Management_System.Application.Queries.AcademicSchedules;
using AutoMapper;
using University_Management_System.Shared.Exceptions;
using MediatR;
using University_Management_System.Domain.Contracts;
using University_Management_System.Application.Dtos.AcademicSheduleDtos;

namespace University_Management_System.Application.Handlers.AcademicSchedules
{
    public class GetAllAcademicSchedulesQueryHandler : IRequestHandler<GetAllAcademicSchedulesQuery, List<AcademicSchedulesDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllAcademicSchedulesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<AcademicSchedulesDto>> Handle(GetAllAcademicSchedulesQuery request, CancellationToken cancellationToken)
        {
            var schedules = await _unitOfWork.AcademicSchedules.GetAllWithDetailsAsync();

            var result = _mapper.Map<List<AcademicSchedulesDto>>(schedules);

            return result;
        }
    }
}
