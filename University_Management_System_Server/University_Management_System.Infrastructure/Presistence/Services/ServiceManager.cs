using University_Management_System.Application.Contracts;
using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using University_Management_System.Shared.Settings;

namespace University_Management_System.Infrastructure.Presistence.Services
{
    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<IAuthenticationService> _authService;
        private readonly Lazy<IRoleService> _roleService;
        private readonly Lazy<IUserService> _userService;
        private readonly Lazy<IJwtService> _jwtService;
        private readonly Lazy<IEmailService> _emailService;

        public ServiceManager(
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            IJwtService jwtService,
            IOptions<JwtSettings> settings,
            IUserService userService,
            IUnitOfWork unitOfWork,
            IOptions<EmailSettings> emailSettings,
            IEmailService emailService,
            UniversityDbContext context)
        {
            _authService = new Lazy<IAuthenticationService>(
                () => new AuthenticationService(
                    userManager,
                    roleManager,
                    jwtService,
                    settings,
                    emailSettings,
                    unitOfWork,
                    emailService,
                    // ← Fixed: IOptions<EmailSettings> parameter
                    context));      // ← Fixed: UniversityDbContext parameter

            _roleService = new Lazy<IRoleService>(
                () => new RoleService(roleManager));
            _userService = new Lazy<IUserService>(
                () => userService);
            _jwtService = new Lazy<IJwtService>(
                () => jwtService);
            _emailService = new Lazy<IEmailService>(
                () => emailService);
        }

        public IAuthenticationService AuthenticationService => _authService.Value;
        public IRoleService RoleService => _roleService.Value;
        public IUserService UserService => _userService.Value;
        public IJwtService JwtService => _jwtService.Value;
        public IEmailService EmailService => _emailService.Value;
    }
}