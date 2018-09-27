using System.Collections.Generic;

namespace ESFA.DC.ReferenceData.EPA.Model
{
    public class Standard
    {
        public string OrganisationId { get; set; }

        public string StandardCode { get; set; }

        public List<Period> Periods { get; set; }

        public Organisation Organisation { get; set; }
    }
}
