using University_Management_System.Application.Commands.Fees;
using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Entities.Models;
using University_Management_System.Shared.Exceptions;
using MediatR;

namespace University_Management_System.Application.Handlers.Fees
{
    public class CreateFeeCommandHandler : IRequestHandler<CreateFeeCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateFeeCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreateFeeCommand request, CancellationToken cancellationToken)
        {
            var department = await _unitOfWork.Departments.GetByIdAsync(request.FeeDto.DepartmentId);
            Console.WriteLine($"Department ID: {request.FeeDto.DepartmentId}, Department Found: {department != null}");
            if (department is null)
                throw new NotFoundException($"Department with ID {request.FeeDto.DepartmentId} not found.");

            //check study year exists and is the current study year for the department to add fee for it
            var studyYear = await _unitOfWork.StudyYears.GetByIdAsync(request.FeeDto.StudyYearId);
            if (studyYear is null || !studyYear.IsCurrent) // check the study year is current and in exisxt
                throw new NotFoundException($"Study year with ID {request.FeeDto.StudyYearId} not found or not current.");

            var fee = new Fee
            {
                Amount = request.FeeDto.Amount,
                Type = request.FeeDto.Type,
                Level = request.FeeDto.Level,
                Description = request.FeeDto.Description,
                StudyYearId = request.FeeDto.StudyYearId,
                DepartmentId = request.FeeDto.DepartmentId
            };

            await _unitOfWork.Fees.AddAsync(fee);
            await _unitOfWork.SaveChangesAsync();

            return fee.Id;
        }
    }
}
