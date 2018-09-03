using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESFA.DC.ReferenceData.FCS.Model
{
    public class Contractor
    {
        public string OrganisationIdentifier { get; set; }

        public int Ukprn { get; set; }

        public string LegalName { get; set; }

        public List<Contract> Contracts { get; set; }
    }
}
