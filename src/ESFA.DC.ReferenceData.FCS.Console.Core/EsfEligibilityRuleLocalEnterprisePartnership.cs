using System;
using System.Collections.Generic;

namespace ESFA.DC.ReferenceData.FCS.Model
{
    public partial class EsfEligibilityRuleLocalEnterprisePartnership
    {
        public string TenderSpecReference { get; set; }
        public string LotReference { get; set; }
        public string Code { get; set; }

        public virtual EsfEligibilityRule EsfEligibilityRule { get; set; }
    }
}
