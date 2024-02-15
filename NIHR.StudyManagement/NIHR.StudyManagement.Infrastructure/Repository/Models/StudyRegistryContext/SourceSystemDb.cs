using System;
using System.Collections.Generic;

namespace NIHR.StudyManagement.Infrastructure.Repository.Models.StudyRegistryContext
{
    public partial class SourceSystemDb
    {
        public SourceSystemDb()
        {
            LocalSystemLinkedIdentifiers = new HashSet<SourceSystemLinkedIdentifierDb>();
        }

        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public virtual ICollection<SourceSystemLinkedIdentifierDb> LocalSystemLinkedIdentifiers { get; set; }
    }
}
