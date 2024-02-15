using System;
using System.Collections.Generic;

namespace NIHR.StudyManagement.Infrastructure.Repository.Models.StudyRegistryContext
{
    public partial class RoleDb : DbEntity
    {
        public RoleDb()
        {
            ResearchInitiativeTeams = new HashSet<ResearchInitiativeTeamMemberDb>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<ResearchInitiativeTeamMemberDb> ResearchInitiativeTeams { get; set; }
    }
}
