using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Application.Commands.Semesters;
using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Entities.Models;
using MediatR;
using University_Management_System.Application.Dtos.SemesterDtos;
using University_Management_System.Shared.Exceptions;
using AutoMapper;

namespace University_Management_System.Application.Handlers.Semesters
{
    public class CreateSemesterCommandHandler : IRequestHandler<CreateSemesterCommand, SemesterDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateSemesterCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<SemesterDto> Handle(CreateSemesterCommand request, CancellationToken cancellationToken)
        {
            var studyYearExists = await _unitOfWork.StudyYears.AnyAsync(sy => sy.Id == request.StudyYearId);
            if (!studyYearExists)
                throw new NotFoundException($"StudyYear with id {request.StudyYearId} was not found.");

            var titleTaken = await _unitOfWork.Semesters.SemesterTitleExistsInStudyYearAsync(request.StudyYearId, request.SemesterDto.Title);
            if (titleTaken)
                throw new ConflictException($"A semester with title '{request.SemesterDto.Title}' already exists for this study year.");

            var semester = _mapper.Map<Semester>(request.SemesterDto);
            var created = await _unitOfWork.Semesters.CreateStudyYearSemesterAsync(request.StudyYearId, semester);

            return _mapper.Map<SemesterDto>(created);
        }
    }
}