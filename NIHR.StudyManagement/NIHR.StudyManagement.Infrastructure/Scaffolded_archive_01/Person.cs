using System;
using System.Collections.Generic;

namespace NIHR.StudyManagement.Infrastructure.Scaffolded_archived_01
{
    public partial class Person
    {
        public Person()
        {
            PersonNames = new HashSet<PersonName>();
            Researchers = new HashSet<Researcher>();
        }

        public int Id { get; set; }
        public int PersonTypeId { get; set; }

        public virtual PersonType PersonType { get; set; } = null!;
        public virtual ICollection<PersonName> PersonNames { get; set; }
        public virtual ICollection<Researcher> Researchers { get; set; }
    }
}
