using System.Collections.Generic;

namespace ESFA.DC.ReferenceData.FCS.Model.DC
{
    public class Contractor
    {
        public string OrganisationIdentifier { get; set; }

        public int Ukprn { get; set; }

        public string LegalName { get; set; }

        public List<Contract> Contracts { get; set; }
    }
}
