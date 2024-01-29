namespace NIHR.StudyManagement.API.Configuration
{
    public class JwtTokenValidationConfiguration
    {
        /// <summary>
        /// Gets or sets a property that, when true, will override and bypass Jwt token validation
        /// allowing for local development without dependency on authentication server.
        /// </summary>
        public bool OverrideJwtTokenValidation { get; set; }

        public string Authority { get; set; } = "";

        public List<ClaimsConfiguration> OverrideClaimsConfigurations { get; set; } = new List<ClaimsConfiguration>();
    }
}
