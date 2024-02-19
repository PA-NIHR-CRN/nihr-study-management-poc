using NIHR.StudyManagement.Domain.Models;

namespace NIHR.StudyManagement.Domain.Abstractions
{
    public interface IGovernmentResearchIdentifierService
    {
        Task<GovernmentResearchIdentifier> RegisterStudy(RegisterStudyRequest request);

        Task<GovernmentResearchIdentifier> RegisterStudy(RegisterStudyToExistingIdentifierRequest request);

        Task<GovernmentResearchIdentifier> GetAsync(string identifier);
    }
}
