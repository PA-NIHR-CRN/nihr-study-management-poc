using NIHR.StudyManagement.Domain.Models;

namespace NIHR.StudyManagement.Domain.Abstractions
{
    public interface IStudyRegistryRepository
    {
        Task<GovernmentResearchIdentifier> CreateAsync(RegisterStudyRequestWithContext request);

        Task<GovernmentResearchIdentifier> AddStudyToIdentifierAsync(AddStudyToExistingIdentifierRequestWithContext request);

        Task<GovernmentResearchIdentifier> GetAsync(string identifier);
    }
}
