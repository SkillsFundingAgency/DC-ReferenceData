using System;
using System.Collections.Generic;

namespace ESFA.DC.ReferenceData.FCS.Model
{
    public partial class EsfEligibilityRuleEmploymentStatus
    {
        public string TenderSpecReference { get; set; }
        public string LotReference { get; set; }
        public int Code { get; set; }

        public virtual EsfEligibilityRule EsfEligibilityRule { get; set; }
    }
}
