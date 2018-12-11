using System;
using System.Collections.Generic;

namespace ESFA.DC.ReferenceData.ULN.Model
{
    public partial class UniqueLearnerNumber
    {
        public long Uln { get; set; }
        public int ImportId { get; set; }

        public virtual Import Import { get; set; }
    }
}
