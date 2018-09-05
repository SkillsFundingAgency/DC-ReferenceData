using ESFA.DC.ReferenceData.FCS.Model.Abstract;

namespace ESFA.DC.ReferenceData.FCS.Model
{
    public class ContractDeliverable : AbstractEntity
    {
        public string Description { get; set; }

        public int? DeliverableCode { get; set; }

        public decimal? UnitCost { get; set; }

        public int? PlannedVolume { get; set; }

        public decimal? PlannedValue { get; set; }

        public int ContractAllocationId { get; set; }

        public ContractAllocation ContractAllocation { get; set; }
    }
}
