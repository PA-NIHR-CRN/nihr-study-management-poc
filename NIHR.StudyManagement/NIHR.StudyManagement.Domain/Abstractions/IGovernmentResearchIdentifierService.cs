using NIHR.StudyManagement.Domain.Models;

namespace NIHR.StudyManagement.Domain.Abstractions
{
    public interface IGovernmentResearchIdentifierService
    {
        Task<GovernmentResearchIdentifier> RegisterStudyAsync(RegisterStudyRequest request);

        Task<GovernmentResearchIdentifier> RegisterStudyAsync(RegisterStudyToExistingIdentifierRequest request);

        Task<GovernmentResearchIdentifier> GetAsync(string identifier);
    }
}
