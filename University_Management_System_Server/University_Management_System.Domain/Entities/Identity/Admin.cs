// Admin.cs — own table, one-to-one with User
using University_Management_System.Domain.Entities.Models;

public class Admin
{
    public string UserId { get; set; } = string.Empty; // FK + PK
    public User User { get; set; } = null!;

    public ICollection<AcademicSchedule> AcademicSchedules { get; set; } = new List<AcademicSchedule>();

    // add admin-specific props here later
}