using System;
using System.Collections.Generic;

namespace NIHR.StudyManagement.Infrastructure.Scaffolded_archived_01
{
    public partial class Researcher
    {
        public Researcher()
        {
            ResearchStudyTeamMembers = new HashSet<ResearchStudyTeamMember>();
        }

        public int Id { get; set; }
        public int PersonId { get; set; }

        public virtual Person Person { get; set; } = null!;
        public virtual ICollection<ResearchStudyTeamMember> ResearchStudyTeamMembers { get; set; }
    }
}
