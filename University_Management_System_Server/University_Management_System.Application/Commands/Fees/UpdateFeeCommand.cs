using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Application.Dtos.DepartmentDtos.FeeDtos;
using MediatR;

namespace University_Management_System.Application.Commands.Fees
{
    public class UpdateFeeCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public UpdateFeeDto FeeDto { get; set; }

        public UpdateFeeCommand(int id, UpdateFeeDto feeDto)
        {
            Id = id;
            FeeDto = feeDto;
        }
    }
}