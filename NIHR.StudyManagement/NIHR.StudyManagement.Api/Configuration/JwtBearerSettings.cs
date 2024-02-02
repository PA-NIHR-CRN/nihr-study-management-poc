namespace NIHR.StudyManagement.Api.Configuration
{
    public class JwtBearerSettings
    {
        public string Authority { get; set; } = "";

        public bool ValidateAudience { get; set; }

        public bool ValidateIssuerSigningKey { get; set; }

        public JwtBearerOverrideSettings JwtBearerOverrideSettings { get; set; } = new JwtBearerOverrideSettings();
    }
}
