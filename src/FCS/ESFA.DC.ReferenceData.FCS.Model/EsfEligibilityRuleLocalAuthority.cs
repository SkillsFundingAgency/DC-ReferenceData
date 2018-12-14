namespace ESFA.DC.ReferenceData.FCS.Model
{
    public partial class EsfEligibilityRuleLocalAuthority
    {
        public string TenderSpecReference { get; set; }
        public string LotReference { get; set; }
        public string Code { get; set; }

        public virtual EsfEligibilityRule EsfEligibilityRule { get; set; }
    }
}
