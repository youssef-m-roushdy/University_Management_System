using University_Management_System.Domain.Enums;

namespace University_Management_System.Domain.Queries.SemesterGPAQueries
{
    public class semesterGPAStudyYearQueries : SearchablePaginationQuery
    {
        public decimal? MinGPA { get; set; }
        public decimal? MaxGPA { get; set; }
        public int? MinCreditHours { get; set; }
        public int? MaxCreditHours { get; set; }
        public int? DepartmentId { get; set; }
        public DateTime? CalculatedFrom { get; set; }
        public DateTime? CalculatedTo { get; set; }
        public SemesterEnum? SemesterTitle { get; set; }
    }
}