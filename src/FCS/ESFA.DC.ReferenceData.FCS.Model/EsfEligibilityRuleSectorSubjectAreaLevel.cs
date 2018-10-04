namespace ESFA.DC.ReferenceData.FCS.Model
{
    public class EsfEligibilityRuleSectorSubjectAreaLevel
    {
        public decimal? SectorSubjectAreaCode { get; set; }

        public string MinLevelCode { get; set; }

        public string MaxLevelCode { get; set; }

        public string TenderSpecReference { get; set; }

        public string LotReference { get; set; }

        public EsfEligibilityRule EsfEligibilityRule { get; set; }
    }
}
