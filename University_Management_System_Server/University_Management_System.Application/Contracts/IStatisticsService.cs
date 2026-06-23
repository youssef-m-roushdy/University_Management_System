using University_Management_System.Application.Dtos.StatisticsDtos;

namespace University_Management_System.Application.Contracts
{
    public interface IStatisticsService
    {
        // ─── Study Year Statistics ────────────────────────────────────────────
        Task<StudyYearStatisticsResponseDto> GetStudyYearStatisticsAsync(int studyYearId);
        Task<StudyYearOverviewDto> GetStudyYearOverviewAsync(int studyYearId);
        
        // ─── Semester Statistics ──────────────────────────────────────────────
        Task<SemesterStatisticsResponseDto> GetSemesterStatisticsAsync(int semesterId);
        Task<SemesterOverviewDto> GetSemesterOverviewAsync(int semesterId);
        
        // ─── Overall Statistics ───────────────────────────────────────────────
        Task<OverallStatisticsDto> GetOverallStatisticsAsync();
        Task<DepartmentStatisticsDto> GetDepartmentStatisticsAsync(int departmentId);
        
        // ─── Comparison ────────────────────────────────────────────────────────
        Task<StudyYearComparisonDto> CompareStudyYearsAsync(int year1Id, int year2Id);
        Task<SemesterComparisonDto> CompareSemestersAsync(int semester1Id, int semester2Id);
        
        // ─── Charts Data ──────────────────────────────────────────────────────
        Task<EnrollmentTrendDto> GetEnrollmentTrendAsync(int studyYearId);
        Task<GpaDistributionDto> GetGpaDistributionAsync(int studyYearId);
        Task<DepartmentEnrollmentDto> GetDepartmentEnrollmentAsync(int studyYearId);
    }
}