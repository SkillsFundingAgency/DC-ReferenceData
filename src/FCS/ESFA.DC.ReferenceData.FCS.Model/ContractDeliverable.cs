namespace ESFA.DC.ReferenceData.FCS.Model
{
    public partial class ContractDeliverable
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int? DeliverableCode { get; set; }
        public decimal? UnitCode { get; set; }
        public decimal? UnitCost { get; set; }
        public int? PlannedVolume { get; set; }
        public decimal? PlannedValue { get; set; }
        public int? ContractAllocationId { get; set; }

        public virtual ContractAllocation ContractAllocation { get; set; }
    }
}
