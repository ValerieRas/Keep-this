using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using sib_api_v3_sdk.Api;
using sib_api_v3_sdk.Client;
using sib_api_v3_sdk.Model;
using System.Diagnostics;
using System.Text;
using Task = System.Threading.Tasks.Task;

namespace API.KeepThis.Services
{
    public class BrevoService:IBrevoService
    {
        private readonly string? _brevoApiKey;
        private readonly string? _apiUrl;
        private readonly IAuthentificationService _authetificationService;

        public BrevoService(IConfiguration config, IAuthentificationService authetificationService)
        {
            _brevoApiKey = config["Brevo:ApiKey"];
            _apiUrl = config["KeepThisSettings:ApiUrl"];
            _authetificationService = authetificationService;
        }

        public async Task SendVerificationEmailAsync(string userEmail)
        {
            string verificationToken = _authetificationService.GenerateEmailVerificationToken(userEmail);

            string verificationLink = $"{_apiUrl}/Authentification/verify-email?token={verificationToken}";

            Configuration.Default.ApiKey.Add("api-key", _brevoApiKey);

            var apiInstance = new TransactionalEmailsApi();
            string SenderName = "KeepThis";
            string SenderEmail = "services.keepthis@gmail.com";
            SendSmtpEmailSender Email = new SendSmtpEmailSender(SenderName, SenderEmail);


            SendSmtpEmailTo smtpEmailTo = new SendSmtpEmailTo(userEmail);
            List<SendSmtpEmailTo> To = new List<SendSmtpEmailTo>();
            To.Add(smtpEmailTo);


            List<SendSmtpEmailBcc> Bcc = new List<SendSmtpEmailBcc>();

            List<SendSmtpEmailCc> Cc = new List<SendSmtpEmailCc>();

            string HtmlContent = $"<html><body><p>Bienvenue sur KeepThis! Cliquez sur ce lien pour valider votre email:</p><p><a href=\"{verificationLink}\">Valider email</a></p></body></html>";
            string TextContent = null;
            string Subject = "KeepThis : Validation email";

            try
            {
                var sendSmtpEmail = new SendSmtpEmail(Email, To, null, null, HtmlContent, TextContent, Subject);

                CreateSmtpEmail result = await Task.Run(() => apiInstance.SendTransacEmail(sendSmtpEmail));
                Debug.WriteLine(result.ToJson());
                Console.WriteLine(result.ToJson());
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                Console.WriteLine(e.Message);
            }
        }
    }
}

