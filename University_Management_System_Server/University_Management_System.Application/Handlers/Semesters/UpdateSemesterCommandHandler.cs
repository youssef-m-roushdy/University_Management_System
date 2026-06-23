using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using University_Management_System.Application.Commands.Semesters;
using University_Management_System.Application.Dtos.SemesterDtos;
using University_Management_System.Domain.Contracts;
using University_Management_System.Shared.Exceptions;

namespace University_Management_System.Application.Handlers.Semesters
{
    public class UpdateSemesterCommandHandler : IRequestHandler<UpdateSemesterCommand, SemesterDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateSemesterCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<SemesterDto> Handle(UpdateSemesterCommand request, CancellationToken cancellationToken)
        {
            var semester = await _unitOfWork.Semesters.GetByIdAsync(request.Id);
            if (semester == null)
            {
                throw new NotFoundException($"Semester with ID {request.Id} not found.");
            }

            // Update the semester properties
            semester.StudyYearId = request.SemesterDto.StudyYearId;
            semester.Title = request.SemesterDto.Title;
            semester.StartDate = request.SemesterDto.StartDate;
            semester.EndDate = request.SemesterDto.EndDate;

            await _unitOfWork.Semesters.UpdateAsync(semester);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<SemesterDto>(semester);
        }
    }
}