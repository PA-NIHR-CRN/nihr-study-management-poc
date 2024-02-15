using System;
using System.Collections.Generic;

namespace NIHR.StudyManagement.Infrastructure.Repository.Models.StudyRegistryContext
{
    public partial class PersonDb : DbEntity
    {
        public PersonDb()
        {
            ResearchInitiativeTeams = new HashSet<ResearchInitiativeTeamMemberDb>();
        }

        public int Id { get; set; }
        
        public string? Firstname { get; set; } = "";

        public string Lastname {get;set;} = "";

        public string PrimaryEmail {get;set;} = "";

        public virtual ICollection<ResearchInitiativeTeamMemberDb> ResearchInitiativeTeams { get; set; }
    }
}
