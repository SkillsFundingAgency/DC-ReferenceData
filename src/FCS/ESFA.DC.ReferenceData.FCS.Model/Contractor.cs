using System.Collections.Generic;
using ESFA.DC.ReferenceData.FCS.Model.Abstract;

namespace ESFA.DC.ReferenceData.FCS.Model
{
    public class Contractor : AbstractEntity
    {
        public string OrganisationIdentifier { get; set; }

        public int Ukprn { get; set; }

        public string LegalName { get; set; }

        public List<Contract> Contracts { get; set; }
    }
}
