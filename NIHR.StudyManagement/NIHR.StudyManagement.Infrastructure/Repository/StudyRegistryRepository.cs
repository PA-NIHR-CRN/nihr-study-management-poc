using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NIHR.StudyManagement.Domain.Abstractions;
using NIHR.StudyManagement.Domain.Configuration;
using NIHR.StudyManagement.Domain.Models;
using NIHR.StudyManagement.Infrastructure.Repository.Models.StudyRegistryContext;
using System;

/*
 * 
 * dotnet ef migrations add InitialDb --startup-project ..\NIHR.StudyManagement.Api\ --context StudyRegistryContext
 * dotnet ef database update --context StudyRegistryContext
 * 
 * */

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

            var researcherRole = await GetRoleAsync(request.RoleName) ?? throw new ArgumentNullException($"Researcher role {request.RoleName} not found in database.");

            var localSystem = await GetLocalSystemAsync(request.LocalSystemName) ?? throw new ArgumentNullException($"Local system {request.LocalSystemName} not found in database.");

            var chiefInvestigator = await GetPersonAsync(request.ChiefInvestigator) ?? new PersonDb
                {
                    Firstname = request.ChiefInvestigator.Firstname,
                    Lastname = request.ChiefInvestigator.Lastname,
                    PrimaryEmail = request?.ChiefInvestigator?.Email?.Address ?? ""
                };

            var team = new ResearchInitiativeTeamMemberDb
            {
                Person = chiefInvestigator,
                Role = researcherRole,
                EffectiveFrom = request.EffectiveFrom,
                EffectiveTo = request.EffectiveTo
            };

            var researchInitiative = new ResearchInitiativeDb
            {
                Identifier = request.Identifier,
                Name = request.ShortTitle,
                ProtocolId = request.ProtocolId,
                ResearchInitiativeTeams = new[] { team },
                LocalSystemLinkedIdentifiers = new SourceSystemLinkedIdentifierDb[]
                {
                        new SourceSystemLinkedIdentifierDb
                        {
                            LocalSystemIdentifier = request.ProjectId,
                            LocalSystems = localSystem
                        }
                }
            };

            _context.ResearchInitiatives.Add(researchInitiative);

            await _context.SaveChangesAsync();

            return await GetAsync(request.Identifier);
        }

        public async Task<GovernmentResearchIdentifier> AddStudyToIdentifierAsync(AddStudyToExistingIdentifierRequestWithContext request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            };

            var existingIdentifier = await GetResearchInitiative(request.Identifier);

            if (existingIdentifier == null)
            {
                throw new FileNotFoundException($"No identifier found matching '{request.Identifier}'");
            }

            var researcherRole = await GetRoleAsync(request.RoleName) ?? throw new ArgumentNullException($"Researcher role {request.RoleName} not found in database.");

            var localSystem = await GetLocalSystemAsync(request.LocalSystemName) ?? throw new ArgumentNullException($"Local system {request.LocalSystemName} not found in database.");

            var chiefInvestigator = await GetPersonAsync(request.ChiefInvestigator) ?? new PersonDb
            {
                Firstname = request.ChiefInvestigator.Firstname,
                Lastname = request.ChiefInvestigator.Lastname,
                PrimaryEmail = request?.ChiefInvestigator?.Email?.Address ?? ""
            };

            var chiefInvestigatorTeamMember = new ResearchInitiativeTeamMemberDb
            {
                Person = chiefInvestigator,
                Role = researcherRole,
                EffectiveFrom = request.EffectiveFrom,
                EffectiveTo = request.EffectiveTo
            };

            var linkedSystemIdentifier = new SourceSystemLinkedIdentifierDb
            {
                LocalSystemIdentifier = request.ProjectId,
                LocalSystems = localSystem
            };

            existingIdentifier.ResearchInitiativeTeams.Add(chiefInvestigatorTeamMember);
            existingIdentifier.LocalSystemLinkedIdentifiers.Add(linkedSystemIdentifier);

            await _context.SaveChangesAsync();

            return await GetAsync(request.Identifier);
        }

        public async Task<GovernmentResearchIdentifier> GetAsync(string identifier)
        {
            var identifierDb = await GetResearchInitiative(identifier);

            if(identifierDb == null)
            {
                return null;
            }

            var mappedIdentifier = Map(identifierDb);

            return mappedIdentifier;
        }

        private async Task<SourceSystemDb?> GetLocalSystemAsync(string code)
        {
            return await _context.SourceSystems.FirstOrDefaultAsync(system => system.Name == code);
        }

        private async Task<PersonDb?> GetPersonAsync(Domain.Models.Person person)
        {
            if (person == null)
            {
                return null;
            }

            return await _context.People.FirstOrDefaultAsync(dbPerson => dbPerson.Firstname == person.Firstname
                && dbPerson.Lastname == person.Lastname);
        }

        private async Task<ResearchInitiativeDb?> GetResearchInitiative(string identifier)
        {
            var identifierDb = await _context.ResearchInitiatives
                .Include(study => study.ResearchInitiativeTeams).ThenInclude(team => team.Role)
                .Include(study => study.ResearchInitiativeTeams).ThenInclude(team => team.Person)
                .Include(study => study.LocalSystemLinkedIdentifiers).ThenInclude(localSystem => localSystem.LocalSystems)
                .FirstOrDefaultAsync(study => study.Identifier == identifier);

            return identifierDb;
        }

        private async Task<RoleDb?> GetRoleAsync (string roleName)
        {
            return await _context.Roles.FirstOrDefaultAsync(role => role.Name == roleName);
        }

        private GovernmentResearchIdentifier Map(ResearchInitiativeDb researchInitiative)
        {
            var associatedSystemIdentifier = researchInitiative.LocalSystemLinkedIdentifiers.FirstOrDefault();

            return new GovernmentResearchIdentifier
            {
                Identifier = researchInitiative.Identifier,
                ShortTitle = researchInitiative.Name,
                Created = researchInitiative.Created,
                TeamMembers = Map(researchInitiative.ResearchInitiativeTeams),
                LinkedSystemIdentifiers = Map(researchInitiative.LocalSystemLinkedIdentifiers)
            };
        }

        private List<LinkedSystemIdentifier> Map(ICollection<SourceSystemLinkedIdentifierDb> localSystemLinkedIdentifiers)
        {
            var linkedIdentifiers = new List<LinkedSystemIdentifier>();

            foreach (var  localIdentifier in localSystemLinkedIdentifiers)
            {
                linkedIdentifiers.Add(new LinkedSystemIdentifier
                {
                 CreatedAt = localIdentifier.Created,
                 Identifier = localIdentifier.LocalSystemIdentifier,
                 SystemName = localIdentifier.LocalSystems.Name
                });
            }

            return linkedIdentifiers;
        }

        private List<TeamMember> Map(ICollection<ResearchInitiativeTeamMemberDb> researchInitiativeTeam)
        {
            var teamMembers = new List<TeamMember>();

            foreach (var researchTeamMember in researchInitiativeTeam)
            {
                teamMembers.Add(new TeamMember
                {
                    Person = new PersonWithPrimaryEmail {
                        Email = new Email { Address = researchTeamMember.Person.PrimaryEmail },
                        Firstname = researchTeamMember.Person.Firstname,
                        Lastname = researchTeamMember.Person.Lastname
                    },
                    Role = new Domain.Models.Role {
                        Name = researchTeamMember.Role.Name
                    },
                    EffectiveFrom = researchTeamMember.EffectiveFrom,
                    EffectiveTo = researchTeamMember.EffectiveTo
                });
            }

            return teamMembers;
        }
    }
}
