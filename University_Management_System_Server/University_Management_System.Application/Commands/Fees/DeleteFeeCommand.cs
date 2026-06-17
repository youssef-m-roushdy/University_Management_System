using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace University_Management_System.Application.Commands.Fees
{
    public class DeleteFeeCommand : IRequest<Unit>
    {
        public int Id { get; set; }

        public DeleteFeeCommand(int id)
        {
            Id = id;
        }
    }
}