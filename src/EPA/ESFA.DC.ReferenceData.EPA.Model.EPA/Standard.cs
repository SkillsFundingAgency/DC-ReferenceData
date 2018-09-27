using System.Collections.Generic;

namespace ESFA.DC.ReferenceData.EPA.Model.EPA
{
    public class Standard
    {
        public string StandardCode { get; set; }

        public List<Period> Periods { get; set; }
    }
}
