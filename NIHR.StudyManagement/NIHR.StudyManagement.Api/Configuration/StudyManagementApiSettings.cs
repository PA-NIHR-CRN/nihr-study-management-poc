namespace NIHR.StudyManagement.API.Configuration
{
    public class StudyManagementApiSettings
    {
        public JwtBearerSettings JwtBearer { get; set; }

        public StudyManagementApiSettings()
        {
            JwtBearer = new JwtBearerSettings();
        }
    }
}
