using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Application.Dtos.SpecializationDtos;
using MediatR;

namespace University_Management_System.Application.Commands.Specializations
{
    public class UpdateSpecializationCommand : IRequest<int>
    {
        public int Id { get; set; }
        public UpdateSpecializationDto UpdateSpecializationDto { get; set; }

        public UpdateSpecializationCommand(int id, UpdateSpecializationDto updateSpecializationDto)
        {
            Id = id;
            UpdateSpecializationDto = updateSpecializationDto;
        }
    }
}