using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Application.Dtos.AcademicSheduleDtos;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace University_Management_System.Application.Commands.AcademicSchedules
{
    public class CreateSemesterAcademicScheduleCommand : IRequest<Unit>
    {
        public string UploadedByUserId { get; set; }
        public int StudyYearId { get; set; }
        public int DepartmentId { get; set; }
        public int SemesterId { get; set; }
        public CreateSemesterAcademicScheduleDto CreateAcademicScheduleDto { get; set; }
        public CreateSemesterAcademicScheduleCommand(
            string uploadedByUserId,
            int studyYearId,
            int departmentId,
            int semesterId,
            CreateSemesterAcademicScheduleDto createAcademicScheduleDto)
        {
            UploadedByUserId = uploadedByUserId;
            StudyYearId = studyYearId;
            DepartmentId = departmentId;
            SemesterId = semesterId;
            CreateAcademicScheduleDto = createAcademicScheduleDto;
        }
    }
}