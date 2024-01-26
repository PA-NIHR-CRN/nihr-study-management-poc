namespace NIHR.StudyManagement.Api.Configuration
{
    public class StudyManagementApiConfiguration
    {
        public JwtTokenValidationConfiguration JwtTokenValidationConfiguration { get; set; }

        public StudyManagementApiConfiguration()
        {
            JwtTokenValidationConfiguration = new JwtTokenValidationConfiguration();
        }
    }

    public class JwtTokenValidationConfiguration
    {
        /// <summary>
        /// Gets or sets a property that, when true, will override and bypass Jwt token validation
        /// allowing for local development without dependency on authentication server.
        /// </summary>
        public bool OverrideJwtTokenValidation { get; set; }

        public string Audience { get; set; } = "";

        public string Authority { get; set; } = "";

        public List<ClaimsConfiguration> ClaimsConfigurations { get; set; } = new List<ClaimsConfiguration>();
    }

    public class ClaimsConfiguration
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
