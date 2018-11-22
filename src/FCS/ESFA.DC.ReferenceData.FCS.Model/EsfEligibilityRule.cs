using System.Collections.Generic;

namespace ESFA.DC.ReferenceData.FCS.Model
{
    public class EsfEligibilityRule
    {
        public string TenderSpecReference { get; set; }

        public string LotReference { get; set; }

        public int? MinAge { get; set; }

        public int? MaxAge { get; set; }

        public int? MinLengthOfUnemployment { get; set; }

        public int? MaxLengthOfUnemployment { get; set; }

        public string MinPriorAttainment { get; set; }

        public string MaxPriorAttainment { get; set; }

        public bool? Benefits { get; set; }

        public int? CalcMethod { get; set; }

        public List<EsfEligibilityRuleEmploymentStatus> EsfEligibilityRuleEmploymentStatuses { get; set; }

        public List<EsfEligibilityRuleLocalAuthority> EsfEligibilityRuleLocalAuthorities { get; set; }

        public List<EsfEligibilityRuleLocalEnterprisePartnership> EsfEligibilityRuleLocalEnterprisePartnerships { get; set; }

        public List<EsfEligibilityRuleSectorSubjectAreaLevel> EsfEligibilityRuleSectorSubjectAreaLevel { get; set; }
    }
}
