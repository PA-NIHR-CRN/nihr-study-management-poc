using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Mysqlx.Expr;
using NIHR.StudyManagement.Domain.Abstractions;
using NIHR.StudyManagement.Domain.Configuration;
using NIHR.StudyManagement.Domain.Exceptions;
using NIHR.StudyManagement.Domain.Models;
using NIHR.StudyManagement.Infrastructure.Repository.EnumsAndConstants;
using NIHR.StudyManagement.Infrastructure.Repository.Models;
using System;

using PersonDb = NIHR.StudyManagement.Infrastructure.Repository.Models.Person;

namespace NIHR.StudyManagement.Infrastructure.Repository
{
    public class StudyRegistryRepository : IStudyRegistryRepository
    {
        private readonly StudyRegistryContext _context;

        public StudyRegistryRepository(StudyRegistryContext context)
        {
            _context = context;
        }

        public async Task<GovernmentResearchIdentifier> CreateAsync(RegisterStudyRequestWithContext request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            };

            var studyType = await GetResearchInitiativeType(ResearchInitiativeTypes.Study) ?? throw new EntityNotFoundException(nameof(ResearchInitiativeType));

            var sourceSystem = await GetSourceSystem(SourceSystemNames.Edge) ?? throw new EntityNotFoundException(nameof(SourceSystem));

            var personTypeResearcher = _context.PersonTypes.FirstOrDefault(x => x.Description == PersonTypes.Researcher) ?? throw new EntityNotFoundException(nameof(PersonType));

            var personRoleCI = _context.PersonRoles.FirstOrDefault(x => x.Type == PersonRoles.ChiefInvestigator) ?? throw new EntityNotFoundException(nameof(PersonRole));

            var projectResearchInitiativeIdentifierType = await _context.ResearchInitiativeIdentifierTypes
                .FirstOrDefaultAsync(x => x.Description == ResearchInitiativeIdentifierTypes.Project) ?? throw new EntityNotFoundException(nameof(ResearchInitiativeIdentifierType));

            var protocolResearchInitiativeIdentifierType = await _context.ResearchInitiativeIdentifierTypes
                .FirstOrDefaultAsync(x => x.Description == ResearchInitiativeIdentifierTypes.Protocol) ?? throw new EntityNotFoundException(nameof(ResearchInitiativeIdentifierType));

            var researchInitiative = new ResearchInitiative
            {
                ResearchInitiativeType = studyType,
            };

            var researchStudy = new GriResearchStudy
            {
                ResearchInitiative = researchInitiative,
                ShortTitle = request.ShortTitle,
                Gri = request.Identifier ?? "",
                RequestSourceSystem = sourceSystem
            };

            var griMapping = new GriMapping
            {
                 GriResearchStudy = researchStudy,
                 ResearchInitiativeIdentifier = new ResearchInitiativeIdentifier
                 {
                     SourceSystem = sourceSystem,
                     Value = request.ProjectId,
                     ResearchInitiativeIdentifierType = projectResearchInitiativeIdentifierType
                 },
                 SourceSystem = sourceSystem
            };

            var griMappingForProtocol = new GriMapping
            {
                GriResearchStudy = researchStudy,
                ResearchInitiativeIdentifier = new ResearchInitiativeIdentifier
                {
                    SourceSystem = sourceSystem,
                    Value = request.ProtocolId,
                    ResearchInitiativeIdentifierType = protocolResearchInitiativeIdentifierType
                },
                SourceSystem = sourceSystem
            };

            var chiefInvestigator = await GetPersonAsync(request.ChiefInvestigator) ?? new PersonDb
            {
                PersonNames = new PersonName[] { new PersonName {
                        Given =request.ChiefInvestigator.Firstname,
                        Family = request.ChiefInvestigator.Lastname,
                        Email = request.ChiefInvestigator.Email.Address
                    } },
                PersonType = personTypeResearcher
            };

            var teamMember = new ResearchStudyTeamMember
            {
                GriMapping = researchStudy,
                Researcher = new Researcher
                {
                    Person = chiefInvestigator
                },
                PersonRole = personRoleCI
            };

            await _context.AddAsync(griMappingForProtocol);

            await _context.AddAsync(griMapping);

            await _context.AddAsync(teamMember);

            await _context.SaveChangesAsync();

            return await GetAsync(request.Identifier);
        }

        private async Task<SourceSystem?> GetSourceSystem(string code)
        {
            return await _context.SourceSystems.FirstOrDefaultAsync(system => system.Code == code);
        }

        private async Task<ResearchInitiativeType?> GetResearchInitiativeType(string code)
        {
            return await _context.ResearchInitiativeTypes.FirstOrDefaultAsync(x => x.Description == code);
        }

        public async Task<GovernmentResearchIdentifier> AddStudyToIdentifierAsync(AddStudyToExistingIdentifierRequestWithContext request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var griResearchStudy = await GetGriResearchStudyByIdentifierAsync(request.Identifier) ?? throw new GriNotFoundException();

            var studyType = await GetResearchInitiativeType(ResearchInitiativeTypes.Study) ?? throw new EntityNotFoundException(nameof(ResearchInitiativeType));

            var sourceSystem = await GetSourceSystem(SourceSystemNames.Edge) ?? throw new EntityNotFoundException(nameof(SourceSystem));

            var personTypeResearcher = _context.PersonTypes.FirstOrDefault(x => x.Description == PersonTypes.Researcher) ?? throw new EntityNotFoundException(nameof(PersonType));

            var personRoleCI = _context.PersonRoles.FirstOrDefault(x => x.Type == PersonRoles.ChiefInvestigator) ?? throw new EntityNotFoundException(nameof(PersonRole));

            var projectResearchInitiativeIdentifierType = await _context.ResearchInitiativeIdentifierTypes
                .FirstOrDefaultAsync(x => x.Description == ResearchInitiativeIdentifierTypes.Project) ?? throw new EntityNotFoundException(nameof(ResearchInitiativeIdentifierType));

            var protocolResearchInitiativeIdentifierType = await _context.ResearchInitiativeIdentifierTypes
                .FirstOrDefaultAsync(x => x.Description == ResearchInitiativeIdentifierTypes.Protocol) ?? throw new EntityNotFoundException(nameof(ResearchInitiativeIdentifierType));

            var griMapping = new GriMapping
            {
                GriResearchStudy = griResearchStudy,
                ResearchInitiativeIdentifier = new ResearchInitiativeIdentifier
                {
                    SourceSystem = sourceSystem,
                    Value = request.ProjectId,
                    ResearchInitiativeIdentifierType = projectResearchInitiativeIdentifierType
                },
                SourceSystem = sourceSystem
            };

            var chiefInvestigator = await GetPersonAsync(request.ChiefInvestigator) ?? new PersonDb
            {
                PersonNames = new PersonName[] { new PersonName {
                        Given = request.ChiefInvestigator.Firstname,
                        Family = request.ChiefInvestigator.Lastname,
                        Email = request.ChiefInvestigator.Email.Address
                    } },
                PersonType = personTypeResearcher
            };

            var teamMember = new ResearchStudyTeamMember
            {
                GriMapping = griResearchStudy,
                Researcher = new Researcher
                {
                    Person = chiefInvestigator
                },
                PersonRole = personRoleCI
            };

            await _context.AddAsync(teamMember);
            await _context.AddAsync(griMapping);

            await _context.SaveChangesAsync();

            return await GetAsync(request.Identifier);
        }

        public async Task<GovernmentResearchIdentifier> GetAsync(string identifier)
        {
            var griResearchStudy = await GetGriResearchStudyByIdentifierAsync(identifier) ?? throw new GriNotFoundException();

            var linkedSystemIdentifiers = new List<LinkedSystemIdentifier>();

            foreach (var x in griResearchStudy.GriMappings)
            {
                linkedSystemIdentifiers.Add(new LinkedSystemIdentifier {
                    CreatedAt = x.Created,
                    SystemName = x.SourceSystem.Description,
                    Identifier = x.ResearchInitiativeIdentifier.Value
                });
            }

            return new GovernmentResearchIdentifier {
                Created = griResearchStudy.Created,
                LinkedSystemIdentifiers = linkedSystemIdentifiers,
                Identifier = identifier,
                ShortTitle = griResearchStudy.ShortTitle,

                TeamMembers = Map(griResearchStudy.ResearchStudyTeamMembers)
            };
        }

        private List<TeamMember> Map(ICollection<ResearchStudyTeamMember> researchStudyTeamMembers)
        {
            var teamMembers = new List<TeamMember>();

            foreach (var teamMember in researchStudyTeamMembers)
            {
                teamMembers.Add(new TeamMember
                {
                    Role = new Role
                    {
                        Description = teamMember.PersonRole.Description,
                        Name = teamMember.PersonRole.Type
                    },
                    Person = new PersonWithPrimaryEmail
                    {
                        Email = new Email
                        {
                            Address = teamMember.Researcher.Person.PersonNames.First().Email
                        },
                        Firstname = teamMember.Researcher.Person.PersonNames.First().Given,
                        Lastname = teamMember.Researcher.Person.PersonNames.First().Family
                    }
                });
            }

            return teamMembers;
        }

        private async Task<GriResearchStudy?> GetGriResearchStudyByIdentifierAsync(string? identifier)
        {
            var griResearchStudy = await _context.GriResearchStudies
                .Include(context => context.ResearchStudyTeamMembers)
                    .ThenInclude(x => x.Researcher)
                    .ThenInclude(researcher => researcher.Person)
                    .ThenInclude(person => person.PersonNames)
                 .Include(context => context.ResearchStudyTeamMembers).ThenInclude(x => x.PersonRole)
                 .Include(study => study.GriMappings).ThenInclude(mapping => mapping.SourceSystem)
                 .Include(study => study.GriMappings).ThenInclude(mapping => mapping.ResearchInitiativeIdentifier)
                .FirstOrDefaultAsync(x => x.Gri == identifier);

            return griResearchStudy;
        }

        private async Task<PersonDb?> GetPersonAsync(PersonWithPrimaryEmail person)
        {
            if (person == null)
            {
                return null;
            }

            var personFromDb = await _context.PersonNames.Include(personName => personName.Person)
                .FirstOrDefaultAsync(personName => personName.Given == person.Firstname
                && personName.Family == person.Lastname
                && personName.Email == person.Email.Address);

            return personFromDb?.Person;
        }
    }
}
