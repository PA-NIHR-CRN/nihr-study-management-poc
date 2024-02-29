
namespace NIHR.StudyManagement.Domain.Configuration
{
    public class S3Settings
    {
        public string BucketName { get; set; }

        public string S3FileKeyPrefix { get; set; }

        public S3Settings()
        {
            BucketName = "";
            S3FileKeyPrefix = "";
        }
    }
}
