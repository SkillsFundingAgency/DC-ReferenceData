using System.Collections.Generic;

namespace ESFA.DC.ReferenceData.FCS.Model
{
    public partial class EsfEligibilityRule
    {
        public EsfEligibilityRule()
        {
            EsfEligibilityRuleEmploymentStatus = new HashSet<EsfEligibilityRuleEmploymentStatus>();
            EsfEligibilityRuleLocalAuthority = new HashSet<EsfEligibilityRuleLocalAuthority>();
            EsfEligibilityRuleLocalEnterprisePartnership = new HashSet<EsfEligibilityRuleLocalEnterprisePartnership>();
            EsfEligibilityRuleSectorSubjectAreaLevel = new HashSet<EsfEligibilityRuleSectorSubjectAreaLevel>();
        }

        public string TenderSpecReference { get; set; }
        public string LotReference { get; set; }
        public int? MinAge { get; set; }
        public int? MaxAge { get; set; }
        public int? MinLengthOfUnemployment { get; set; }
        public int? MaxLengthOfUnemployment { get; set; }
        public string MinPriorAttainment { get; set; }
        public string MaxPriorAttainment { get; set; }
        public int? CalcMethod { get; set; }
        public bool? Benefits { get; set; }

        public virtual ICollection<EsfEligibilityRuleEmploymentStatus> EsfEligibilityRuleEmploymentStatus { get; set; }
        public virtual ICollection<EsfEligibilityRuleLocalAuthority> EsfEligibilityRuleLocalAuthority { get; set; }
        public virtual ICollection<EsfEligibilityRuleLocalEnterprisePartnership> EsfEligibilityRuleLocalEnterprisePartnership { get; set; }
        public virtual ICollection<EsfEligibilityRuleSectorSubjectAreaLevel> EsfEligibilityRuleSectorSubjectAreaLevel { get; set; }
    }
}
