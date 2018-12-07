using System;
using System.Collections.Generic;

namespace ESFA.DC.ReferenceData.ULN.Model.ULN
{
    public class ULNFile
    {
        public long Count { get; set; }

        public IEnumerable<long> ULNs { get; set; }
    }
}
