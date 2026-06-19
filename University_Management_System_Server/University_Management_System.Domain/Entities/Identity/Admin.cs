// Admin.cs — own table, one-to-one with User
using University_Management_System.Domain.Entities.Models;
namespace University_Management_System.Domain.Entities.Identity
{
    public class Admin : BaseEntities<string>
    {
        public User User { get; set; } = null!;
        public ICollection<AcademicSchedule> AcademicSchedules { get; set; } = new List<AcademicSchedule>();

        // add admin-specific props here later
    }
}