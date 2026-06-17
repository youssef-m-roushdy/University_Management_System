namespace University_Management_System.Application.Contracts
{
    public interface IServiceManager
    {
        IAuthenticationService AuthenticationService { get; }
        IRoleService RoleService { get; }
        IUserService UserService { get; }
    }
}
