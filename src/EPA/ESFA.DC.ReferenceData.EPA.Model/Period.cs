using System;

namespace ESFA.DC.ReferenceData.EPA.Model
{
    public class Period
    {
        public DateTime EffectiveFrom { get; set; }

        public DateTime? EffectiveTo { get; set; }

        public string OrganisationId { get; set; }

        public string StandardCode { get; set; }

        public Standard Standard { get; set; }
    }
}
