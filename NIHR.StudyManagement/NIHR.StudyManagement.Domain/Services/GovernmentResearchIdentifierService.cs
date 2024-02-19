using Microsoft.Extensions.Options;
using NIHR.StudyManagement.Domain.Abstractions;
using NIHR.StudyManagement.Domain.Configuration;
using NIHR.StudyManagement.Domain.Exceptions;
using NIHR.StudyManagement.Domain.Models;

namespace NIHR.StudyManagement.Domain.Services
{
    public class GovernmentResearchIdentifierService : IGovernmentResearchIdentifierService
    {
        private readonly IStudyRegistryRepository _governmentResearchIdentifierRepository;

        private readonly StudyManagementSettings _settings;

        public GovernmentResearchIdentifierService(IStudyRegistryRepository governmentResearchIdentifierRepository,
            IOptions<StudyManagementSettings> settings)
        {
            this._governmentResearchIdentifierRepository = governmentResearchIdentifierRepository;
            this._settings = settings.Value;

            if (string.IsNullOrEmpty(this._settings.DefaultRoleName)) throw new ArgumentNullException(nameof(_settings.DefaultRoleName));

            if (string.IsNullOrEmpty(this._settings.DefaultLocalSystemName)) throw new ArgumentNullException(nameof(_settings.DefaultLocalSystemName));
        }

        public async Task<GovernmentResearchIdentifier> RegisterStudy(RegisterStudyRequest request)
        {
            // Check if request has specified an existing identifier
            if(!string.IsNullOrEmpty(request.Identifier))
            {
                return await AddNewStudyToExistingIdentifierAsync(request);
            }

            return await RegisterNewStudyWithNewIdentifierAsync(request);
        }

        private async Task<GovernmentResearchIdentifier> AddNewStudyToExistingIdentifierAsync(RegisterStudyRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Identifier)) throw new ArgumentNullException(nameof(request));

            // Verify that the GRI exists
            var existingIdentifier = await GetByIdentifierAsync(request.Identifier);

            if(existingIdentifier == null)
            {
                throw new GriNotFoundException($"Could not find an existing GRI with the specified identifier '{request.Identifier}'");
            }

            // Ensure no existing link exists for the same local system record
            if(existingIdentifier.LinkedSystemIdentifiers.Any(linked => linked.Identifier == request.ProjectId
                && linked.SystemName == _settings.DefaultLocalSystemName
                // Todo: need to add effective from/to dates on this record for evaluation here.
                ))
            {
                throw new GriAlreadyExistsException($"A local system identifier already exists for GRI '{request.Identifier}' and '{_settings.DefaultLocalSystemName}'");
            }

            var domainRequest = new AddStudyToExistingIdentifierRequestWithContext
            {
                ChiefInvestigator = request.ChiefInvestigator,
                ProjectId = request.ProjectId,
                Identifier = request.Identifier,
                Sponsor = request.Sponsor,
                ShortTitle = request.ShortTitle,
                LocalSystemName = _settings.DefaultLocalSystemName,
                RoleName = _settings.DefaultRoleName,
                ProtocolId = request.ProtocolId
            };

            return await _governmentResearchIdentifierRepository.AddStudyToIdentifierAsync(domainRequest);
        }

        private RegisterStudyRequestWithContext Map(RegisterStudyRequest request)
        {
            return new RegisterStudyRequestWithContext
            {
                ChiefInvestigator = request.ChiefInvestigator,
                ProjectId = request.ProjectId,
                Identifier = request.Identifier,
                Sponsor = request.Sponsor,
                ShortTitle = request.ShortTitle,
                LocalSystemName = _settings.DefaultLocalSystemName,
                RoleName = _settings.DefaultRoleName,
                ProtocolId = request.ProtocolId
            };
        }

        private async Task<GovernmentResearchIdentifier> RegisterNewStudyWithNewIdentifierAsync(RegisterStudyRequest request)
        {
            // Generate the GRI
            var gri = GenerateGrisPostcode();

            var domainRequest =  Map(request);

            domainRequest.Identifier = gri;

            return await _governmentResearchIdentifierRepository.CreateAsync(domainRequest);
        }

        public async Task<GovernmentResearchIdentifier> GetAsync(string identifier)
        {
            var existingIdentifier = await _governmentResearchIdentifierRepository.GetAsync(identifier);

            if (existingIdentifier == null)
            {
                throw new GriNotFoundException();
            }

            return existingIdentifier;
        }

        private async Task<GovernmentResearchIdentifier> GetByIdentifierAsync(string identifier)
        {
            return await _governmentResearchIdentifierRepository.GetAsync(identifier);
        }

        /// <summary>
        /// This is a temp implementation for proof of concept to generate a government research identifier.
        /// The implementation simply generates a random sequence of 3 capital letters, 3 numbers and 3 capital letters.
        /// </summary>
        /// <returns></returns>
        private string GenerateGrisPostcode()
        {
            int asciiA = 65;
            int asciiX = 90;
            int ascii1 = 48;
            int ascii9 = 57;
            int maxCharacters = 3;

            var identifier = $"{GetRandomSting(asciiA,asciiX, maxCharacters)}{GetRandomSting(ascii1, ascii9, maxCharacters)}{GetRandomSting(asciiA, asciiX, maxCharacters)}";

            return identifier;
        }

        private static string GetRandomSting(int minAsciiValue, int maxAsciiValue, int numberCharacters)
        {
            var identifier = "";
            var random = new Random();

            for (int i = 0; i < numberCharacters; i++)
            {
                var randomCharacterAscii = random.Next(minAsciiValue, maxAsciiValue);

                var randomCharacter = ((char)randomCharacterAscii).ToString();
                identifier = identifier + randomCharacter;
            }

            return identifier;
        }
    }
}
