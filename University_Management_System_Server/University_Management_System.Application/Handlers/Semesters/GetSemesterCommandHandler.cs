using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using University_Management_System.Application.Dtos.SemesterDtos;
using University_Management_System.Application.Queries.Semesters;
using University_Management_System.Domain.Contracts;
using University_Management_System.Shared.Exceptions;

namespace University_Management_System.Application.Handlers.Semesters
{
    public class GetSemesterCommandHandler : IRequestHandler<GetSemesterQuery, SemesterDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetSemesterCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<SemesterDto> Handle(GetSemesterQuery request, CancellationToken cancellationToken)
        {
            var semester = await _unitOfWork.Semesters.GetByIdAsync(request.Id);
            if (semester == null)
            {
                throw new NotFoundException($"Semester with ID {request.Id} not found.");
            }
            return _mapper.Map<SemesterDto>(semester);
        }
    }
}