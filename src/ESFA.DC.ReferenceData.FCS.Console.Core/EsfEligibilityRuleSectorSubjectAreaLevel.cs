namespace ESFA.DC.ReferenceData.FCS.Model
{
    public partial class EsfEligibilityRuleSectorSubjectAreaLevel
    {
        public int Id { get; set; }
        public string TenderSpecReference { get; set; }
        public string LotReference { get; set; }
        public decimal? SectorSubjectAreaCode { get; set; }
        public string MinLevelCode { get; set; }
        public string MaxLevelCode { get; set; }

        public virtual EsfEligibilityRule EsfEligibilityRule { get; set; }
    }
}
