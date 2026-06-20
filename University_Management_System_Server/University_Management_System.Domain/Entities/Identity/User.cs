// User.cs — base, lives in AspNetUsers
using Microsoft.AspNetCore.Identity;
using University_Management_System.Domain.Entities.Identity;
using University_Management_System.Domain.Enums;

namespace University_Management_System.Domain.Entities.Identity
{
    public class User : IdentityUser
    {
        public string Name { get; set; } = string.Empty;
        public string ProfilePicture { get; set; } = string.Empty;
        public string Address { get; set; }
        public Gender Gender { get; set; }

        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

        // navigation to the extended profiles
        public Student? Student { get; set; }
        public Admin? Admin { get; set; }
        public Instructor? Instructor { get; set; }
        public Assistant? Assistant { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}