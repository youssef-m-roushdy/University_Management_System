using University_Management_System.Application.Contracts;
using University_Management_System.Application.Dtos.StatisticsDtos;
using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Entities.Models;
using University_Management_System.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace University_Management_System.Infrastructure.Presistence.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;

        public StatisticsService(IUnitOfWork unitOfWork, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }


        // ────────────────────────────────────────────────────────────────────────
        // STUDY YEAR STATISTICS
        // ────────────────────────────────────────────────────────────────────────

        public async Task<StudyYearStatisticsResponseDto> GetStudyYearStatisticsAsync(int studyYearId)
        {
            // ✅ Get raw data from repositories
            var studyYear = await _unitOfWork.StudyYears
                .GetStudyYearWithFullDetailsAsync(studyYearId, cancellationToken: default);

            if (studyYear == null)
                throw new Exception($"Study year with ID {studyYearId} not found.");

            var students = studyYear.StudentStudyYears?.Select(ssy => ssy.Student).ToList() ?? new();
            var registrations = studyYear.Registrations?.ToList() ?? new();
            var semesters = studyYear.Semesters?.ToList() ?? new();

            // ✅ Calculate statistics in service layer
            return CalculateStudyYearStatistics(studyYear, students, registrations, semesters);
        }

        public async Task<StudyYearOverviewDto> GetStudyYearOverviewAsync(int studyYearId)
        {
            var studyYear = await _unitOfWork.StudyYears
                .GetStudyYearWithDetailsAsync(studyYearId, cancellationToken: default);

            if (studyYear == null)
                throw new Exception($"Study year with ID {studyYearId} not found.");

            var students = await _unitOfWork.StudentStudyYears
                .FindAsync(ssy => ssy.StudyYearId == studyYearId);

            var studentIds = students.Select(s => s.StudentId).ToList();
            var allStudents = await _unitOfWork.Students
                .FindAsync(s => studentIds.Contains(s.Id));

            return CalculateStudyYearOverview(studyYear, allStudents.ToList());
        }

        // ────────────────────────────────────────────────────────────────────────
        // SEMESTER STATISTICS
        // ────────────────────────────────────────────────────────────────────────

        public async Task<SemesterStatisticsResponseDto> GetSemesterStatisticsAsync(int semesterId)
        {
            var semester = await _unitOfWork.Semesters
                .GetSemesterWithFullDetailsAsync(semesterId, cancellationToken: default);

            if (semester == null)
                throw new Exception($"Semester with ID {semesterId} not found.");

            var registrations = semester.Registrations?.ToList() ?? new();
            var students = registrations
                .Select(r => r.Student)
                .Distinct()
                .ToList();

            return CalculateSemesterStatistics(semester, students, registrations);
        }

        public async Task<SemesterOverviewDto> GetSemesterOverviewAsync(int semesterId)
        {
            var semester = await _unitOfWork.Semesters
                .GetSemesterWithDetailsAsync(semesterId, cancellationToken: default);

            if (semester == null)
                throw new Exception($"Semester with ID {semesterId} not found.");

            var registrations = semester.Registrations?.ToList() ?? new();
            var students = registrations
                .Select(r => r.Student)
                .Distinct()
                .ToList();

            return CalculateSemesterOverview(semester, students);
        }

        // ────────────────────────────────────────────────────────────────────────
        // OVERALL STATISTICS
        // ────────────────────────────────────────────────────────────────────────

        public async Task<OverallStatisticsDto> GetOverallStatisticsAsync()
        {
            // ✅ Get raw data from repositories
            var users = await _userManager.Users.ToListAsync();
            var students = await _unitOfWork.Students.GetAllAsync();
            var instructors = await _unitOfWork.Instructors.GetAllAsync();
            var assistants = await _unitOfWork.Assistants.GetAllAsync();
            var admins = await _unitOfWork.Admins.GetAllAsync();
            var studyYears = await _unitOfWork.StudyYears.GetAllAsync();
            var semesters = await _unitOfWork.Semesters.GetAllAsync();
            var courses = await _unitOfWork.Courses.GetAllAsync();
            var departments = await _unitOfWork.Departments.GetAllAsync();
            var specializations = await _unitOfWork.Specializations.GetAllAsync();

            // ✅ Calculate statistics in service layer
            return CalculateOverallStatistics(
                users.ToList(),
                students.ToList(),
                instructors.ToList(),
                assistants.ToList(),
                admins.ToList(),
                studyYears.ToList(),
                semesters.ToList(),
                courses.ToList(),
                departments.ToList(),
                specializations.ToList()
            );
        }

        public async Task<DepartmentStatisticsDto> GetDepartmentStatisticsAsync(int departmentId)
        {
            var department = await _unitOfWork.Departments.GetByIdAsync(departmentId);
            if (department == null)
                throw new Exception($"Department with ID {departmentId} not found.");

            // ✅ Get raw data from repositories
            var students = await _unitOfWork.Students
                .FindAsync(s => s.DepartmentId == departmentId);

            var instructors = await _unitOfWork.Instructors
                .FindAsync(i => i.DepartmentId == departmentId);

            var assistants = await _unitOfWork.Assistants
                .FindAsync(a => a.DepartmentId == departmentId);

            var courses = await _unitOfWork.Courses
                .FindAsync(c => c.DepartmentId == departmentId);

            // ✅ Calculate statistics in service layer
            return CalculateDepartmentStatistics(
                department,
                students.ToList(),
                instructors.ToList(),
                assistants.ToList(),
                courses.ToList()
            );
        }

        // ────────────────────────────────────────────────────────────────────────
        // COMPARISON
        // ────────────────────────────────────────────────────────────────────────

        public async Task<StudyYearComparisonDto> CompareStudyYearsAsync(int year1Id, int year2Id)
        {
            var year1 = await GetStudyYearOverviewAsync(year1Id);
            var year2 = await GetStudyYearOverviewAsync(year2Id);

            return new StudyYearComparisonDto
            {
                Year1 = new StudyYearComparisonData
                {
                    StudyYearId = year1.StudyYearId,
                    YearRange = year1.YearRange,
                    TotalStudents = year1.TotalStudents,
                    AverageGPA = year1.OverallAverageGPA,
                    PassRate = year1.PassRate,
                    TotalCourses = year1.TotalCourses,
                    TotalRegistrations = 0,
                    TotalRevenue = year1.TotalRevenue
                },
                Year2 = new StudyYearComparisonData
                {
                    StudyYearId = year2.StudyYearId,
                    YearRange = year2.YearRange,
                    TotalStudents = year2.TotalStudents,
                    AverageGPA = year2.OverallAverageGPA,
                    PassRate = year2.PassRate,
                    TotalCourses = year2.TotalCourses,
                    TotalRegistrations = 0,
                    TotalRevenue = year2.TotalRevenue
                },
                Summary = new ComparisonSummary
                {
                    StudentChange = $"{year2.TotalStudents - year1.TotalStudents:+#;-#;0}",
                    GpaChange = $"{year2.OverallAverageGPA - year1.OverallAverageGPA:+#0.00;-#0.00;0.00}",
                    PassRateChange = $"{year2.PassRate - year1.PassRate:+#0.0;-#0.0;0.0}",
                    RegistrationChange = "0",
                    IsImprovement = year2.OverallAverageGPA > year1.OverallAverageGPA
                }
            };
        }

        public async Task<SemesterComparisonDto> CompareSemestersAsync(int semester1Id, int semester2Id)
        {
            var semester1 = await GetSemesterOverviewAsync(semester1Id);
            var semester2 = await GetSemesterOverviewAsync(semester2Id);

            return new SemesterComparisonDto
            {
                Semester1 = new SemesterComparisonData
                {
                    SemesterId = semester1.SemesterId,
                    SemesterTitle = semester1.SemesterTitle,
                    YearRange = semester1.YearRange,
                    TotalStudents = semester1.TotalStudents,
                    AverageGPA = semester1.AverageGPA,
                    PassRate = semester1.PassRate,
                    TotalCourses = semester1.TotalCourses,
                    TotalRegistrations = 0
                },
                Semester2 = new SemesterComparisonData
                {
                    SemesterId = semester2.SemesterId,
                    SemesterTitle = semester2.SemesterTitle,
                    YearRange = semester2.YearRange,
                    TotalStudents = semester2.TotalStudents,
                    AverageGPA = semester2.AverageGPA,
                    PassRate = semester2.PassRate,
                    TotalCourses = semester2.TotalCourses,
                    TotalRegistrations = 0
                },
                Summary = new ComparisonSummary
                {
                    StudentChange = $"{semester2.TotalStudents - semester1.TotalStudents:+#;-#;0}",
                    GpaChange = $"{semester2.AverageGPA - semester1.AverageGPA:+#0.00;-#0.00;0.00}",
                    PassRateChange = $"{semester2.PassRate - semester1.PassRate:+#0.0;-#0.0;0.0}",
                    RegistrationChange = "0",
                    IsImprovement = semester2.AverageGPA > semester1.AverageGPA
                }
            };
        }

        // ────────────────────────────────────────────────────────────────────────
        // CHART DATA
        // ────────────────────────────────────────────────────────────────────────

        public async Task<EnrollmentTrendDto> GetEnrollmentTrendAsync(int studyYearId)
        {
            var registrations = await _unitOfWork.Registrations
                .FindAsync(r => r.StudyYearId == studyYearId);

            var registrationsList = registrations.ToList();

            var monthlyData = registrationsList
                .GroupBy(r => new { r.RegisteredAt.Year, r.RegisteredAt.Month })
                .Select(g => new MonthlyEnrollmentData
                {
                    Month = $"{g.Key.Year}-{g.Key.Month:00}",
                    Registrations = g.Count(),
                    NewStudents = g.Select(r => r.StudentId).Distinct().Count()
                })
                .OrderBy(m => m.Month)
                .ToList();

            return new EnrollmentTrendDto
            {
                MonthlyData = monthlyData,
                WeeklyData = new List<WeeklyEnrollmentData>(),
                CourseEnrollment = new List<CourseEnrollmentData>()
            };
        }

        public async Task<GpaDistributionDto> GetGpaDistributionAsync(int studyYearId)
        {
            var studentStudyYears = await _unitOfWork.StudentStudyYears
                .FindAsync(ssy => ssy.StudyYearId == studyYearId);

            var studentIds = studentStudyYears.Select(ssy => ssy.StudentId).ToList();
            var students = await _unitOfWork.Students
                .FindAsync(s => studentIds.Contains(s.Id));

            var studentsList = students.ToList();

            return CalculateGpaDistribution(studentsList);
        }

        public async Task<DepartmentEnrollmentDto> GetDepartmentEnrollmentAsync(int studyYearId)
        {
            var studentStudyYears = await _unitOfWork.StudentStudyYears
                .FindAsync(ssy => ssy.StudyYearId == studyYearId);

            var studentIds = studentStudyYears.Select(ssy => ssy.StudentId).ToList();
            var students = await _unitOfWork.Students
                .FindAsync(s => studentIds.Contains(s.Id));

            var departments = await _unitOfWork.Departments.GetAllAsync();

            return CalculateDepartmentEnrollment(students.ToList(), departments.ToList());
        }

        // ────────────────────────────────────────────────────────────────────────
        // PRIVATE CALCULATION METHODS
        // ────────────────────────────────────────────────────────────────────────

        private StudyYearStatisticsResponseDto CalculateStudyYearStatistics(
            StudyYear studyYear,
            List<Student> students,
            List<Registration> registrations,
            List<Semester> semesters)
        {
            // Calculate GPA statistics
            var gpas = students.Where(s => s.TotalGPA > 0).Select(s => s.TotalGPA).ToList();
            decimal averageGpa = gpas.Any() ? gpas.Average() : 0;
            decimal highestGpa = gpas.Any() ? gpas.Max() : 0;
            decimal lowestGpa = gpas.Any() ? gpas.Min() : 0;

            // Level distribution
            var studentsByLevel = students
                .GroupBy(s => s.Level)
                .ToDictionary(g => g.Key.ToString(), g => g.Count());

            var averageGpaByLevel = students
                .Where(s => s.TotalGPA > 0)
                .GroupBy(s => s.Level)
                .ToDictionary(
                    g => g.Key.ToString(),
                    g => g.Average(s => s.TotalGPA)
                );

            // Department distribution
            var studentsByDepartment = students
                .GroupBy(s => s.Department?.Name ?? "Unknown")
                .ToDictionary(g => g.Key, g => g.Count());

            var coursesByDepartment = registrations
                .Where(r => r.Course != null && r.Course.Department != null)
                .GroupBy(r => r.Course.Department.Name)
                .ToDictionary(g => g.Key, g => g.Select(r => r.CourseId).Distinct().Count());

            // Semester distribution
            var now = DateTime.UtcNow;

            return new StudyYearStatisticsResponseDto
            {
                StudyYearId = studyYear.Id,
                YearRange = $"{studyYear.StartYear}-{studyYear.EndYear}",
                IsCurrent = studyYear.IsCurrent,

                TotalStudents = students.Count,
                TotalRegistrations = registrations.Count,
                TotalCourses = registrations.Select(r => r.CourseId).Distinct().Count(),
                TotalInstructors = 0, // Calculate from CourseInstructors
                TotalAssistants = 0, // Calculate from CourseAssistants

                AverageGPA = averageGpa,
                HighestGPA = highestGpa,
                LowestGPA = lowestGpa,
                TotalCreditsEarned = students.Sum(s => s.TotalCredits),

                TotalFees = 0, // Calculate from Fees
                TotalFeeAmount = 0, // Calculate from Fees
                AverageFeePerStudent = students.Count > 0 ? 0 : 0,

                TotalSemesters = semesters.Count,
                ActiveSemesters = semesters.Count(s => s.IsActive),
                CompletedSemesters = semesters.Count(s => s.EndDate < now),
                UpcomingSemesters = semesters.Count(s => s.StartDate > now),

                StudentsByLevel = studentsByLevel,
                AverageGpaByLevel = averageGpaByLevel,
                StudentsByDepartment = studentsByDepartment,
                CoursesByDepartment = coursesByDepartment,

                Status = CalculateStatus(studyYear),
                CalculatedAt = DateTime.UtcNow
            };
        }

        private StudyYearOverviewDto CalculateStudyYearOverview(StudyYear studyYear, List<Student> allStudents)
        {
            var stats = CalculateStudyYearStatistics(
                studyYear,
                allStudents,
                new List<Registration>(),
                studyYear.Semesters?.ToList() ?? new List<Semester>()
            );

            var passingStudents = allStudents.Count(s => s.TotalGPA >= 2.0m);
            var totalStudents = allStudents.Count();

            return new StudyYearOverviewDto
            {
                StudyYearId = studyYear.Id,
                YearRange = $"{studyYear.StartYear}-{studyYear.EndYear}",
                IsCurrent = studyYear.IsCurrent,
                Status = stats.Status,

                TotalStudents = stats.TotalStudents,
                TotalCourses = stats.TotalCourses,
                OverallAverageGPA = stats.AverageGPA,
                PassRate = totalStudents > 0 ? (decimal)passingStudents / totalStudents * 100 : 0,

                NewStudents = allStudents.Count(s => s.CreatedAt >= studyYear.CreatedAt),
                GraduatingStudents = allStudents.Count(s => s.Level == Levels.Graduate),
                ActiveSemesters = stats.ActiveSemesters,

                TotalRevenue = 0,
                TotalExpenses = 0,
                NetRevenue = 0
            };
        }

        private SemesterStatisticsResponseDto CalculateSemesterStatistics(
            Semester semester,
            List<Student> students,
            List<Registration> registrations)
        {
            // GPA distribution
            var gpaDistribution = students
                .Where(s => s.TotalGPA > 0)
                .GroupBy(s => GetGpaRange(s.TotalGPA))
                .ToDictionary(g => g.Key, g => g.Count());

            // Grade distribution
            var gradeDistribution = new Dictionary<string, int>
                {
                    { "A (3.7-4.0)", students.Count(s => s.TotalGPA >= 3.7m) },
                    { "B (3.0-3.69)", students.Count(s => s.TotalGPA >= 3.0m && s.TotalGPA < 3.7m) },
                    { "C (2.0-2.99)", students.Count(s => s.TotalGPA >= 2.0m && s.TotalGPA < 3.0m) },
                    { "D (1.0-1.99)", students.Count(s => s.TotalGPA >= 1.0m && s.TotalGPA < 2.0m) },
                    { "F (0-0.99)", students.Count(s => s.TotalGPA < 1.0m && s.TotalGPA > 0) }
                };


            // Popular courses
            var popularCourses = registrations
                .GroupBy(r => r.CourseId)
                .Select(g => new
                {
                    CourseId = g.Key,
                    CourseName = g.First().Course?.Name ?? "Unknown",
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Count)
                .Take(10)
                .ToDictionary(x => x.CourseName, x => x.Count);

            // Course average GPA
            var courseAverageGpa = registrations
                .Where(r => r.Student.TotalGPA > 0)
                .GroupBy(r => r.CourseId)
                .Select(g => new
                {
                    CourseId = g.Key,
                    CourseName = g.First().Course?.Name ?? "Unknown",
                    AverageGpa = g.Average(r => r.Student.TotalGPA)
                })
                .ToDictionary(x => x.CourseName, x => x.AverageGpa);

            // Department performance
            var departmentAverageGpa = students
                .Where(s => s.TotalGPA > 0)
                .GroupBy(s => s.Department?.Name ?? "Unknown")
                .ToDictionary(g => g.Key, g => g.Average(s => s.TotalGPA));

            // Calculate GPA statistics
            var gpas = students.Where(s => s.TotalGPA > 0).Select(s => s.TotalGPA).ToList();
            decimal averageGpa = gpas.Any() ? gpas.Average() : 0;
            decimal highestGpa = gpas.Any() ? gpas.Max() : 0;
            decimal lowestGpa = gpas.Any() ? gpas.Min() : 0;

            // Pass/Fail rate
            var passingStudents = students.Count(s => s.TotalGPA >= 2.0m);
            var totalStudents = students.Count;

            var now = DateTime.UtcNow;
            string status;
            if (now < semester.StartDate)
                status = "Upcoming";
            else if (now > semester.EndDate)
                status = "Completed";
            else
                status = "Ongoing";

            return new SemesterStatisticsResponseDto
            {
                SemesterId = semester.Id,
                SemesterTitle = semester.Title.ToString().Replace("_", " "),
                YearRange = semester.StudyYear != null
                    ? $"{semester.StudyYear.StartYear}-{semester.StudyYear.EndYear}"
                    : "N/A",
                StartDate = semester.StartDate,
                EndDate = semester.EndDate,
                IsActive = semester.IsActive,
                Status = status,

                TotalStudents = students.Count,
                TotalRegistrations = registrations.Count,
                TotalCourses = registrations.Select(r => r.CourseId).Distinct().Count(),
                AverageCoursesPerStudent = students.Count > 0
                    ? (double)registrations.Count / students.Count
                    : 0,

                SemesterAverageGPA = averageGpa,
                HighestGPA = highestGpa,
                LowestGPA = lowestGpa,
                TotalCreditsEarned = students.Sum(s => s.TotalCredits),
                PassRate = totalStudents > 0 ? (decimal)passingStudents / totalStudents * 100 : 0,
                FailRate = totalStudents > 0 ? (decimal)(totalStudents - passingStudents) / totalStudents * 100 : 0,

                GpaDistribution = gpaDistribution,
                GradeDistribution = gradeDistribution,
                PopularCourses = popularCourses,
                CourseAverageGpa = courseAverageGpa,

                StudentsOnProbation = students.Count(s => s.TotalGPA > 0 && s.TotalGPA < 2.0m),
                StudentsWithHonors = students.Count(s => s.TotalGPA >= 3.5m),
                TopStudentGPA = highestGpa,

                DepartmentAverageGpa = departmentAverageGpa,
                DailyRegistrations = new Dictionary<string, int>(),

                CalculatedAt = DateTime.UtcNow
            };
        }

        private SemesterOverviewDto CalculateSemesterOverview(Semester semester, List<Student> students)
        {
            var stats = CalculateSemesterStatistics(
                semester,
                students,
                semester.Registrations?.ToList() ?? new List<Registration>()
            );

            var now = DateTime.UtcNow;
            var daysRemaining = now < semester.EndDate
                ? (int)(semester.EndDate - now).TotalDays
                : 0;

            return new SemesterOverviewDto
            {
                SemesterId = semester.Id,
                SemesterTitle = semester.Title.ToString().Replace("_", " "),
                YearRange = semester.StudyYear != null
                    ? $"{semester.StudyYear.StartYear}-{semester.StudyYear.EndYear}"
                    : "N/A",
                Status = stats.Status,

                TotalStudents = stats.TotalStudents,
                TotalCourses = stats.TotalCourses,
                AverageGPA = stats.SemesterAverageGPA,
                PassRate = stats.PassRate,

                NewStudents = stats.StudentsOnProbation,
                GraduatingStudents = stats.StudentsWithHonors,
                DaysRemaining = daysRemaining
            };
        }

        private OverallStatisticsDto CalculateOverallStatistics(
            List<User> users,
            List<Student> students,
            List<Instructor> instructors,
            List<Assistant> assistants,
            List<Admin> admins,
            List<StudyYear> studyYears,
            List<Semester> semesters,
            List<Course> courses,
            List<Department> departments,
            List<Specialization> specializations)
        {
            var currentStudyYear = studyYears.FirstOrDefault(sy => sy.IsCurrent);
            var currentSemester = semesters.FirstOrDefault(s => s.IsActive);

            var studentGpas = students.Where(s => s.TotalGPA > 0).Select(s => s.TotalGPA).ToList();
            var averageGpa = studentGpas.Any() ? studentGpas.Average() : 0;
            var passingStudents = students.Count(s => s.TotalGPA >= 2.0m);
            var totalStudents = students.Count();

            return new OverallStatisticsDto
            {
                TotalUsers = users.Count,
                TotalStudents = totalStudents,
                TotalInstructors = instructors.Count,
                TotalAssistants = assistants.Count,
                TotalAdmins = admins.Count,

                TotalStudyYears = studyYears.Count,
                TotalSemesters = semesters.Count,
                TotalCourses = courses.Count,
                TotalDepartments = departments.Count,
                TotalSpecializations = specializations.Count,

                CurrentStudyYearId = currentStudyYear?.Id ?? 0,
                CurrentStudyYear = currentStudyYear != null
                    ? $"{currentStudyYear.StartYear}-{currentStudyYear.EndYear}"
                    : "N/A",
                CurrentSemesterId = currentSemester?.Id ?? 0,
                CurrentSemester = currentSemester != null
                    ? currentSemester.Title.ToString().Replace("_", " ")
                    : "N/A",

                OverallAverageGPA = averageGpa,
                OverallPassRate = totalStudents > 0
                    ? (decimal)passingStudents / totalStudents * 100
                    : 0,

                TotalRevenue = 0,
                TotalFeesCollected = 0,

                ActiveStudents = students.Count(s => s.Level != Levels.Graduate),
                InactiveStudents = students.Count(s => s.Level == Levels.Graduate),
                ActiveInstructors = instructors.Count,

                CalculatedAt = DateTime.UtcNow
            };
        }

        private DepartmentStatisticsDto CalculateDepartmentStatistics(
            Department department,
            List<Student> students,
            List<Instructor> instructors,
            List<Assistant> assistants,
            List<Course> courses)
        {
            var gpas = students.Where(s => s.TotalGPA > 0).Select(s => s.TotalGPA).ToList();
            var averageGpa = gpas.Any() ? gpas.Average() : 0;
            var passingStudents = students.Count(s => s.TotalGPA >= 2.0m);
            var totalStudents = students.Count;

            return new DepartmentStatisticsDto
            {
                DepartmentId = department.Id,
                DepartmentName = department.Name,

                TotalStudents = totalStudents,
                TotalInstructors = instructors.Count,
                TotalAssistants = assistants.Count,
                TotalCourses = courses.Count,

                AverageGPA = averageGpa,
                PassRate = totalStudents > 0 ? (decimal)passingStudents / totalStudents * 100 : 0,
                GraduatedStudents = students.Count(s => s.Level == Levels.Graduate),
                CurrentStudents = students.Count(s => s.Level != Levels.Graduate),

                StudentsByLevel = students
                .GroupBy(s => s.Level)
                .ToDictionary(g => (int)g.Key, g => g.Count()),

                AverageGpaByLevel = students
                    .Where(s => s.TotalGPA > 0)
                    .GroupBy(s => s.Level)
                    .ToDictionary(g => (int)g.Key, g => g.Average(s => s.TotalGPA)),

                StudentsBySpecialization = students
                    .Where(s => s.SpecializationId.HasValue)
                    .GroupBy(s => s.Specialization?.Name ?? "Unknown")
                    .ToDictionary(g => g.Key, g => g.Count())
            };
        }

        private GpaDistributionDto CalculateGpaDistribution(List<Student> students)
        {
            var gpaRanges = new List<GpaRangeData>
            {
                new() { Range = "4.0", Count = 0 },
                new() { Range = "3.5-3.99", Count = 0 },
                new() { Range = "3.0-3.49", Count = 0 },
                new() { Range = "2.5-2.99", Count = 0 },
                new() { Range = "2.0-2.49", Count = 0 },
                new() { Range = "1.5-1.99", Count = 0 },
                new() { Range = "1.0-1.49", Count = 0 },
                new() { Range = "0.0-0.99", Count = 0 }
            };

            foreach (var student in students)
            {
                var gpa = student.TotalGPA;
                var range = gpaRanges.FirstOrDefault(r =>
                    r.Range switch
                    {
                        "4.0" => gpa >= 3.99m,
                        "3.5-3.99" => gpa >= 3.5m && gpa < 4.0m,
                        "3.0-3.49" => gpa >= 3.0m && gpa < 3.5m,
                        "2.5-2.99" => gpa >= 2.5m && gpa < 3.0m,
                        "2.0-2.49" => gpa >= 2.0m && gpa < 2.5m,
                        "1.5-1.99" => gpa >= 1.5m && gpa < 2.0m,
                        "1.0-1.49" => gpa >= 1.0m && gpa < 1.5m,
                        "0.0-0.99" => gpa >= 0m && gpa < 1.0m,
                        _ => false
                    }
                );

                if (range != null)
                    range.Count++;
            }

            var totalStudents = students.Count;
            foreach (var range in gpaRanges)
            {
                range.Percentage = totalStudents > 0 ? (decimal)range.Count / totalStudents * 100 : 0;
            }

            var levelGpaData = students
                .Where(s => s.TotalGPA > 0)
                .GroupBy(s => s.Level)
                .Select(g => new LevelGpaData
                {
                    Level = (int)g.Key,
                    AverageGPA = g.Average(s => s.TotalGPA),
                    StudentCount = g.Count()
                })
                .OrderBy(l => l.Level)
                .ToList();

            return new GpaDistributionDto
            {
                GpaRanges = gpaRanges,
                LevelGpaData = levelGpaData,
                AverageGPA = students.Where(s => s.TotalGPA > 0).Select(s => s.TotalGPA).DefaultIfEmpty(0).Average(),
                TotalStudents = totalStudents
            };
        }

        private DepartmentEnrollmentDto CalculateDepartmentEnrollment(
            List<Student> students,
            List<Department> departments)
        {
            var departmentData = departments.Select(d => new DepartmentEnrollmentData
            {
                DepartmentName = d.Name,
                StudentCount = students.Count(s => s.DepartmentId == d.Id),
                CourseCount = 0,
                InstructorCount = 0,
                Percentage = 0
            }).ToList();

            var totalStudents = students.Count;
            foreach (var dept in departmentData)
            {
                dept.Percentage = totalStudents > 0 ? (decimal)dept.StudentCount / totalStudents * 100 : 0;
            }

            return new DepartmentEnrollmentDto
            {
                Departments = departmentData.OrderByDescending(d => d.StudentCount).ToList(),
                TotalStudents = totalStudents
            };
        }

        private string CalculateStatus(StudyYear studyYear)
        {
            var now = DateTime.UtcNow;
            var yearStart = new DateTime(studyYear.StartYear, 9, 1);
            var yearEnd = new DateTime(studyYear.EndYear, 6, 30);

            if (now < yearStart)
                return "Upcoming";
            else if (now > yearEnd)
                return "Completed";
            else
                return "Ongoing";
        }

        private string GetGpaRange(decimal gpa)
        {
            if (gpa >= 3.7m) return "3.7-4.0";
            if (gpa >= 3.3m) return "3.3-3.69";
            if (gpa >= 3.0m) return "3.0-3.29";
            if (gpa >= 2.7m) return "2.7-2.99";
            if (gpa >= 2.3m) return "2.3-2.69";
            if (gpa >= 2.0m) return "2.0-2.29";
            if (gpa >= 1.5m) return "1.5-1.99";
            return "0.0-1.49";
        }
    }
}