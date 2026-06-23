using AutoMapper;
using MediatR;
using University_Management_System.Application.Dtos.StudentStudyYearDtos;
using University_Management_System.Application.Queries.StudentStudyYears;
using University_Management_System.Domain.Contracts;

namespace University_Management_System.Application.Handlers.StudentStudyYears
{
    public class GetStudentCurrentStudyYearQueryHandler : IRequestHandler<GetStudentCurrentStudyYearQuery, StudentStudyYearDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetStudentCurrentStudyYearQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<StudentStudyYearDto?> Handle(
            GetStudentCurrentStudyYearQuery request, 
            CancellationToken cancellationToken)
        {
            var enrollment = await _unitOfWork.StudentStudyYears
                .GetCurrentByStudentIdAsync(request.StudentId);

            if (enrollment == null)
                return null;

            return _mapper.Map<StudentStudyYearDto>(enrollment);
        }
    }
}