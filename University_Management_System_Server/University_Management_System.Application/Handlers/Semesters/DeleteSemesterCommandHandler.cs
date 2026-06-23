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
    public class DeleteSemesterCommandHandler : IRequestHandler<DeleteSemesterCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DeleteSemesterCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(DeleteSemesterCommand request, CancellationToken cancellationToken)
        {
            var semester = await _unitOfWork.Semesters.GetByIdAsync(request.Id);
            if (semester == null)
            {
                throw new NotFoundException($"Semester with ID {request.Id} not found.");
            }

            await _unitOfWork.Semesters.DeleteAsync(semester);
            await _unitOfWork.SaveChangesAsync();
            
            return Unit.Value;
        }
    }
}