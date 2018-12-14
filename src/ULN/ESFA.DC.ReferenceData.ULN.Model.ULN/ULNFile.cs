using System.Collections.Generic;

namespace ESFA.DC.ReferenceData.ULN.Model.ULN
{
    public class UlnFile
    {
        public long Count { get; set; }

        public IEnumerable<long> ULNs { get; set; }
    }
}
