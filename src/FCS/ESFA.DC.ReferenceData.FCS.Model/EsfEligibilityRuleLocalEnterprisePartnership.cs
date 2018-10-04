namespace ESFA.DC.ReferenceData.FCS.Model
{
    public class EsfEligibilityRuleLocalEnterprisePartnership
    {
        public string Code { get; set; }

        public string TenderSpecReference { get; set; }

        public string LotReference { get; set; }

        public EsfEligibilityRule EsfEligibilityRule { get; set; }
    }
}
