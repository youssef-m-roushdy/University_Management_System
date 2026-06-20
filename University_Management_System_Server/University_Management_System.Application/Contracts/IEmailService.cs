namespace University_Management_System.Application.Contracts
{
    public interface IEmailService
    {
        // Core email methods
        Task SendEmailVerificationAsync(string to, string displayName, string verificationLink);
        Task SendPasswordResetEmailAsync(string to, string displayName, string resetLink);
        Task SendPasswordChangedConfirmationAsync(string to, string displayName);
        Task SendWelcomeEmailAsync(string to, string displayName);
    }
}