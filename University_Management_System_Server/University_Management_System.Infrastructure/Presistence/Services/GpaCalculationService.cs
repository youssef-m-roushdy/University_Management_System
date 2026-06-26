using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Application.Contracts;
using University_Management_System.Domain.Entities.Models;
using University_Management_System.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace University_Management_System.Infrastructure.Presistence.Services
{
    public class GpaCalculationService : IGpaCalculationService
    {
        private readonly UniversityDbContext _dbContext;

        public GpaCalculationService(UniversityDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Get grade point value for a grade (4.0 scale)
        /// </summary>
        public decimal GetGradePoint(Grades grade)
        {
            return grade switch
            {
                Grades.A_Plus => 4.0m,
                Grades.A => 4.0m,
                Grades.A_Minus => 3.7m,
                Grades.B_Plus => 3.3m,
                Grades.B => 3.0m,
                Grades.B_Minus => 2.7m,
                Grades.C_Plus => 2.3m,
                Grades.C => 2.0m,
                Grades.C_Minus => 1.7m,
                Grades.D_Plus => 1.3m,
                Grades.D => 1.0m,
                Grades.D_Minus => 0.7m,
                Grades.F => 0.0m,
                _ => 0.0m
            };
        }

        /// <summary>
        /// Calculate GPA for a student in a specific semester and study year
        /// Formula: GPA = Σ(Grade Points × Course Credits) / Σ(Course Credits)
        /// </summary>
        public async Task<SemesterGPA?> CalculateAndSaveSemesterGPAAsync(string userId, int semesterId, int studyYearId)
        {
            try
            {
                var registrations = await _dbContext.Registrations
                    .Where(r => r.StudentId == userId && r.SemesterId == semesterId && r.StudyYearId == studyYearId)
                    .Include(r => r.Course)
                    .ToListAsync();

                if (!registrations.Any())
                {
                    return null; // No registrations found
                }

                int totalCreditHours = 0;
                decimal totalWeightedPoints = 0;

                foreach (var registration in registrations)
                {
                    if (registration.Grade is null) continue;
                    decimal gradePoint = GetGradePoint(registration.Grade.Value);
                    int credits = registration.Course.Credits;

                    totalWeightedPoints += gradePoint * credits;
                    totalCreditHours += credits;
                }

                decimal gpa = totalCreditHours > 0 ? (totalWeightedPoints / totalCreditHours) : 0;
                gpa = Math.Round(gpa, 2);

                var existingGPA = await _dbContext.SemesterGPAs
                    .FirstOrDefaultAsync(g => g.StudentId == userId && g.SemesterId == semesterId && g.StudyYearId == studyYearId);

                if (existingGPA != null)
                {
                    existingGPA.GPA = gpa;
                    existingGPA.TotalCreditHours = totalCreditHours;
                    existingGPA.CalculatedAt = DateTime.UtcNow;
                    _dbContext.SemesterGPAs.Update(existingGPA);
                }
                else
                {
                    var semesterGPA = new SemesterGPA
                    {
                        StudentId = userId,
                        SemesterId = semesterId,
                        StudyYearId = studyYearId,
                        GPA = gpa,
                        TotalCreditHours = totalCreditHours,
                        CalculatedAt = DateTime.UtcNow
                    };

                    _dbContext.SemesterGPAs.Add(semesterGPA);
                }

                await _dbContext.SaveChangesAsync();

                return await GetSemesterGPAAsync(userId, semesterId, studyYearId);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error calculating GPA for user {userId}: {ex.Message}", ex);
            }
        }

        public async Task<SemesterGPA?> GetSemesterGPAAsync(string userId, int semesterId, int studyYearId)
        {
            return await _dbContext.SemesterGPAs
                .Include(g => g.Semester)
                .Include(g => g.StudyYear)
                .FirstOrDefaultAsync(g => g.StudentId == userId && g.SemesterId == semesterId && g.StudyYearId == studyYearId);
        }

        public async Task<IEnumerable<SemesterGPA>> GetAllStudentGPAsAsync(string userId)
        {
            return await _dbContext.SemesterGPAs
                .Where(g => g.StudentId == userId)
                .Include(g => g.Semester)
                .Include(g => g.StudyYear)
                .OrderBy(g => g.StudyYear.StartYear)
                .ThenBy(g => g.Semester.Title)
                .ToListAsync();
        }

        /// <summary>
        /// Calculate cumulative GPA for a student across all semesters
        /// </summary>
        public async Task<decimal> CalculateCumulativeGPAAsync(string userId)
        {
            var semesterGPAs = await GetAllStudentGPAsAsync(userId);

            if (!semesterGPAs.Any())
            {
                return 0;
            }

            decimal totalWeightedPoints = 0;
            int totalCreditHours = 0;

            foreach (var semesterGPA in semesterGPAs)
            {
                totalWeightedPoints += semesterGPA.GPA * semesterGPA.TotalCreditHours;
                totalCreditHours += semesterGPA.TotalCreditHours;
            }

            decimal cumulativeGPA = totalCreditHours > 0 ? (totalWeightedPoints / totalCreditHours) : 0;
            return Math.Round(cumulativeGPA, 2);
        }

        /// <summary>
        /// Recalculates cumulative GPA and writes it onto Student.TotalGPA
        /// </summary>
        public async Task UpdateStudentCumulativeGpaAsync(string studentId)
        {
            var cumulativeGpa = await CalculateCumulativeGPAAsync(studentId);

            var student = await _dbContext.Students
                .FirstOrDefaultAsync(s => s.Id == studentId);

            if (student is null)
            {
                throw new InvalidOperationException($"Student with id {studentId} was not found while updating cumulative GPA.");
            }

            student.TotalGPA = cumulativeGpa;
            _dbContext.Students.Update(student);

            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Cascade entry point: call after Grade/IsPassed changes on one or many
        /// registrations have already been saved. Recalculates every affected
        /// SemesterGPA, then every affected student's cumulative GPA.
        /// </summary>
        public async Task RecalculateGpaForRegistrationsAsync(IEnumerable<Registration> updatedRegistrations)
        {
            var registrations = updatedRegistrations?.ToList() ?? new List<Registration>();
            if (!registrations.Any()) return;

            // 1. Recalculate SemesterGPA for every distinct (Student, Semester, StudyYear) touched
            var semesterGroups = registrations
                .Select(r => new { r.StudentId, r.SemesterId, r.StudyYearId })
                .Distinct();

            foreach (var group in semesterGroups)
            {
                await CalculateAndSaveSemesterGPAAsync(group.StudentId, group.SemesterId, group.StudyYearId);
            }

            // 2. Refresh cumulative GPA for every distinct student touched
            var studentIds = registrations
                .Select(r => r.StudentId)
                .Distinct();

            foreach (var studentId in studentIds)
            {
                await UpdateStudentCumulativeGpaAsync(studentId);
            }
        }
    }
}