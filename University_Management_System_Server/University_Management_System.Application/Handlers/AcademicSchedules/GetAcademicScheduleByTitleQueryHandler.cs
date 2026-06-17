using University_Management_System.Application.Queries.AcademicSchedules;
using AutoMapper;
using University_Management_System.Shared.Exceptions;
using MediatR;
using University_Management_System.Application.Dtos.AcademicSheduleDtos;
using University_Management_System.Domain.Contracts;

namespace University_Management_System.Application.Handlers.AcademicSchedules
{
    public class GetAcademicScheduleByTitleQueryHandler : IRequestHandler<GetAcademicScheduleByTitleQuery, AcademicSchedulesDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAcademicScheduleByTitleQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<AcademicSchedulesDto> Handle(GetAcademicScheduleByTitleQuery request, CancellationToken cancellationToken)
        {
            var schedule = await _unitOfWork.AcademicSchedules.GetByTitleAsync(request.ScheduleTitle);

            if (schedule is null)
                throw new NotFoundException($"Academic schedule with title '{request.ScheduleTitle}' not found.");

            return _mapper.Map<AcademicSchedulesDto>(schedule);
        }
    }
}
