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
}
