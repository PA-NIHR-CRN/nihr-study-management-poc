using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Mysqlx.Expr;
using NIHR.StudyManagement.Domain.Abstractions;
using NIHR.StudyManagement.Domain.Configuration;
using NIHR.StudyManagement.Domain.Models;
using NIHR.StudyManagement.Infrastructure.Repository.Models.StudyRegistryContext;
using NIHR.StudyManagement.Infrastructure.Scaffolded02;
using System;

using PersonDb = NIHR.StudyManagement.Infrastructure.Scaffolded02.Person;

/*
 * 
 * dotnet ef migrations add InitialDb --startup-project ..\NIHR.StudyManagement.Api\ --context StudyRegistryContext
 * dotnet ef database update --context StudyRegistryContext
 * 
 * */

namespace NIHR.StudyManagement.Infrastructure.Repository
{
    public class StudyRegistryRepositoryNew : IStudyRegistryRepository
    {
        private readonly study_managementContext2 _context;

        public StudyRegistryRepositoryNew(study_managementContext2 context)
        {
            _context = context;
        }

        //private async Task<ResearchInitiativeType?> GetR

        public async Task<GovernmentResearchIdentifier> CreateAsync(RegisterStudyRequestWithContext request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            };

            var studyType = await GetResearchInitiativeType("study") ?? new ResearchInitiativeType 
                {
                    Description = "study"
                };

            var sourceSystem = await GetSourceSystem("EDGE") ?? new SourceSystem
                {
                    Code = "EDGE", Description = "Edge"
                }; //await GetSourceSystem("EDGE") ?? throw new Exception("not found");

            var personTypeResearcher = _context.PersonTypes.FirstOrDefault(x => x.Description == "researcher") ?? new PersonType
                {
                    Description = "researcher"
                };

            var personRoleCI = _context.PersonRoles.FirstOrDefault(x => x.Type == "CHIEF_INVESTIGATOR") ?? new PersonRole
                {
                    Type = "CHIEF_INVESTIGATOR"
                };

            var projectResearchInitiativeIdentifierType = await _context.ResearchInitiativeIdentifierTypes
                .FirstOrDefaultAsync(x => x.Description == "project") ?? new ResearchInitiativeIdentifierType
                {
                    Description = "project"
                };

            var protocolResearchInitiativeIdentifierType = await _context.ResearchInitiativeIdentifierTypes
                .FirstOrDefaultAsync(x => x.Description == "protocol") ?? new ResearchInitiativeIdentifierType
                {
                    Description = "protocol"
                };

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
            throw new NotImplementedException();
        }

        public async Task<GovernmentResearchIdentifier> GetAsync(string identifier)
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

            var chiefInvestigatorPerson = griResearchStudy.ResearchStudyTeamMembers
                .First();

            var chiefInvestigator = chiefInvestigatorPerson.Researcher.Person.PersonNames.First();

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
                
                TeamMembers = new List<TeamMember> {
                    new TeamMember{
                        Role = new Role{
                            Description = chiefInvestigatorPerson.PersonRole.Type,
                            Name = chiefInvestigatorPerson.PersonRole.Type
                        },
                        Person = new PersonWithPrimaryEmail{
                            Email = new Email{
                                Address = chiefInvestigator.Email,
                            },
                            Firstname = chiefInvestigator.Given,
                            Lastname = chiefInvestigator.Family
                        },
                        EffectiveFrom = chiefInvestigatorPerson.EffectiveFrom
                    }
                }
            };
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


        private GovernmentResearchIdentifier Map(ResearchInitiativeDb researchInitiative)
        {
            throw new NotImplementedException();
        }

        private List<LinkedSystemIdentifier> Map(ICollection<SourceSystemLinkedIdentifierDb> localSystemLinkedIdentifiers)
        {
            throw new NotImplementedException();
        }

        private List<TeamMember> Map(ICollection<ResearchInitiativeTeamMemberDb> researchInitiativeTeam)
        {
            throw new NotImplementedException();
        }
    }
}
