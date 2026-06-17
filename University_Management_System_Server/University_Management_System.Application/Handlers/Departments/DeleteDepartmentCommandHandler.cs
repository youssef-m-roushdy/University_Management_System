using University_Management_System.Application.Commands.Departments;
using University_Management_System.Domain.Contracts;
using University_Management_System.Shared.Exceptions;
using MediatR;

namespace University_Management_System.Application.Handlers.Departments
{
    public class DeleteDepartmentCommandHandler : IRequestHandler<DeleteDepartmentCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteDepartmentCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteDepartmentCommand request, CancellationToken cancellationToken)
        {
            var department = await _unitOfWork.Departments.GetByIdAsync(request.Id);

            if (department is null)
                throw new NotFoundException($"Department with ID {request.Id} not found.");

            await _unitOfWork.Departments.Delete(department);
            await _unitOfWork.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
