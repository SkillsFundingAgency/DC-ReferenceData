namespace ESFA.DC.ReferenceData.FCS.Model
{
    public partial class ContractDeliverableCodeMapping
    {
        public int Id { get; set; }
        public string FundingStreamPeriodCode { get; set; }
        public string ExternalDeliverableCode { get; set; }
        public string FcsdeliverableCode { get; set; }
        public string DeliverableName { get; set; }
        public bool? Claimable { get; set; }
    }
}
