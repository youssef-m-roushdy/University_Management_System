using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Application.Commands.StudyYears;
using University_Management_System.Domain.Entities.Models;
using University_Management_System.Domain.Contracts;
using MediatR;

namespace University_Management_System.Application.Handlers.StudyYears
{
    public class CreateStudyYearCommandHandler : IRequestHandler<CreateStudyYearCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateStudyYearCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreateStudyYearCommand request, CancellationToken cancellationToken)
        {
            // Map the CreateUserStudyYearDto to the StudyYear entity
            var studyYear = new StudyYear
            {
                StartYear = request.StudyYearDto.StartYear,
                EndYear = request.StudyYearDto.EndYear
            };

            // Add the new StudyYear to the repository
            _unitOfWork.StudyYears.AddAsync(studyYear);

            // Save changes to the database
            await _unitOfWork.SaveChangesAsync();

            // Return the ID of the newly created StudyYear
            return studyYear.Id;
        }
    }
}