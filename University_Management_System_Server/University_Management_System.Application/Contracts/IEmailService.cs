namespace University_Management_System.Application.Contracts
{
    public interface IEmailService
    {
        Task SendPasswordResetEmailAsync(string to, string displayName, string resetLink);
        Task SendPasswordChangedConfirmationAsync(string to, string displayName);
    }
}
