using System;
using System.Collections.Generic;

namespace ESFA.DC.ReferenceData.FCS.Model
{
    public partial class Contractor
    {
        public Contractor()
        {
            Contract = new HashSet<Contract>();
        }

        public int Id { get; set; }
        public string OrganisationIdentifier { get; set; }
        public int? Ukprn { get; set; }
        public string LegalName { get; set; }
        public Guid? SyndicationItemId { get; set; }

        public virtual ICollection<Contract> Contract { get; set; }
    }
}
