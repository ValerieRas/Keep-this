namespace API.KeepThis.Services
{
    public interface IBrevoService
    {
        Task SendVerificationEmailAsync(string userEmail);
    }
}
