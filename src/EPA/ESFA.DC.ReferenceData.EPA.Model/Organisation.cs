using System.Collections.Generic;

namespace ESFA.DC.ReferenceData.EPA.Model
{
    public class Organisation
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Ukprn { get; set; }

        public List<Standard> Standards { get; set; }
    }
}
