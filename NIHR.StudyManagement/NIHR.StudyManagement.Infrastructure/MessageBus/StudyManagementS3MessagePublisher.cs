using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NIHR.StudyManagement.Domain.Abstractions;
using NIHR.StudyManagement.Domain.Configuration;
using NIHR.StudyManagement.Domain.Models;

namespace NIHR.StudyManagement.Infrastructure.MessageBus
{
    /// <summary>
    /// Implementation of IStudyEventMessagePublisher that writes the content to an S3 bucket.
    /// </summary>
    public class StudyManagementS3MessagePublisher : IStudyEventMessagePublisher
    {
        private readonly ILogger<StudyManagementS3MessagePublisher> _logger;
        private readonly S3Settings _s3Settings;

        public StudyManagementS3MessagePublisher(ILogger<StudyManagementS3MessagePublisher> logger,
            IOptions<S3Settings> s3Settings)
        {
            this._logger = logger;
            this._s3Settings = s3Settings.Value;
        }

        public async Task PublishAsync(string eventType,
            string sourceSystemName,
            GovernmentResearchIdentifier governmentResearchIdentifier,
            CancellationToken cancellationToken)
        {
            PutObjectRequest request = new PutObjectRequest()
            {
                StorageClass = new S3StorageClass(S3StorageClass.Standard),
                ContentBody = System.Text.Json.JsonSerializer.Serialize(governmentResearchIdentifier),
                BucketName = _s3Settings.BucketName,
                Key = $"{_s3Settings.S3FileKeyPrefix}{governmentResearchIdentifier.Identifier}-{DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss")}"
            };

            _logger.LogInformation($"Publishing message to S3 bucket '{request.BucketName}' with key '{request.Key}'");

            // Assuming lambda execution role policy, therefore no credentials initialised.
            var _client = new AmazonS3Client(Amazon.RegionEndpoint.EUWest2);

            var putResponse = await _client.PutObjectAsync(request, cancellationToken);

            if (putResponse.HttpStatusCode != System.Net.HttpStatusCode.OK)
            {
                _logger.LogError($"Failed to upload '{request.Key} to S3. HttpStatusCode: {putResponse.HttpStatusCode.ToString()}'");

                throw new Exception("Error uploading");
            }

            _logger.LogInformation($"Successfully put object into S3");
        }
    }
}
