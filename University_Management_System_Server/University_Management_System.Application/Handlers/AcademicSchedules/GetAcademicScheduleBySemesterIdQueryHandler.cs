using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using University_Management_System.Application.Dtos.AcademicSheduleDtos;
using University_Management_System.Application.Queries.AcademicSchedules;
using University_Management_System.Domain.Contracts;
using University_Management_System.Shared.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace University_Management_System.Application.Handlers.AcademicSchedules
{
    public class GetAcademicSchedulesBySemesterIdQueryHandler : IRequestHandler<GetAcademicSchedulesBySemesterIdQuery, IEnumerable<AcademicScheduleDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAcademicSchedulesBySemesterIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
    }

        public async Task<IEnumerable<AcademicScheduleDto>> Handle(GetAcademicSchedulesBySemesterIdQuery request, CancellationToken cancellationToken)
        {
            // check if semester exists
            var semester = await _unitOfWork.Semesters.GetByIdAsync(request.SemesterId);
            if (semester is null)                
                throw new NotFoundException("Semester not found");
            
            var schedules = await _unitOfWork.AcademicSchedules.GetBySemesterIdAsync(request.SemesterId);
            return _mapper.Map<IEnumerable<AcademicScheduleDto>>(schedules);
        }
    }
}