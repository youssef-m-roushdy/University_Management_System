using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Application.Dtos.DepartmentDtos.FeeDtos;
using MediatR;
namespace University_Management_System.Application.Commands.Fees
{
    public class CreateFeeCommand : IRequest<int>
    {
        public CreateFeeDto FeeDto { get; init; }

        public CreateFeeCommand(CreateFeeDto feeDto)
        {
            FeeDto = feeDto;
        }
    }
}