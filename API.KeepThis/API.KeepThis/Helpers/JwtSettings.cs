namespace API.KeepThis.Helpers
{
    public class JwtSettings: IJwtSettings
    {
        public string SecretKey { get; set; }
    }
}
