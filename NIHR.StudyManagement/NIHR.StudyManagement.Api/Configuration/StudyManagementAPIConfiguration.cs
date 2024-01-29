namespace NIHR.StudyManagement.API.Configuration
{
    public class StudyManagementAPIConfiguration
    {
        public JwtTokenValidationConfiguration JwtTokenValidationConfiguration { get; set; }

        public StudyManagementAPIConfiguration()
        {
            JwtTokenValidationConfiguration = new JwtTokenValidationConfiguration();
        }
    }
}
