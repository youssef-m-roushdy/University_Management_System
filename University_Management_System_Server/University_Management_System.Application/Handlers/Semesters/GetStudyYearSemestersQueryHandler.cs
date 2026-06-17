using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using University_Management_System.Application.Dtos.SemesterDtos;
using University_Management_System.Application.Queries.Semesters;
using University_Management_System.Domain.Contracts;
using MediatR;

namespace University_Management_System.Application.Handlers.Semesters
{
    public class GetStudyYearSemestersQueryHandler : IRequestHandler<GetStudyYearSemestersQuery, IEnumerable<SemesterDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetStudyYearSemestersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SemesterDto>> Handle(GetStudyYearSemestersQuery request, CancellationToken cancellationToken)
        {
            var semesters = await _unitOfWork.Semesters.GetByStudyYearIdAsync(request.StudyYearId);
            return _mapper.Map<IEnumerable<SemesterDto>>(semesters);
        }
    }
}