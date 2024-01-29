namespace NIHR.StudyManagement.API.Configuration
{
    public class StudyManagementAPIConfigurationx
    {
        public JwtTokenValidationConfiguration JwtTokenValidationConfiguration { get; set; }

        public StudyManagementAPIConfigurationx()
        {
            JwtTokenValidationConfiguration = new JwtTokenValidationConfiguration();
        }
    }
}
