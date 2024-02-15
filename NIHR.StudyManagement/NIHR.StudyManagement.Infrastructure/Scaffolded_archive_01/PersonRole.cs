using System;
using System.Collections.Generic;

namespace NIHR.StudyManagement.Infrastructure.Scaffolded_archived_01
{
    public partial class PersonRole
    {
        public PersonRole()
        {
            ResearchStudyTeamMembers = new HashSet<ResearchStudyTeamMember>();
        }

        public int Id { get; set; }
        public string Type { get; set; } = null!;
        public string? Description { get; set; }

        public virtual ICollection<ResearchStudyTeamMember> ResearchStudyTeamMembers { get; set; }
    }
}
