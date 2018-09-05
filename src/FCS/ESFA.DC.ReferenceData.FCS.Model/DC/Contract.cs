using System;
using System.Collections.Generic;
using ESFA.DC.ReferenceData.FCS.Model.DC.Abstract;

namespace ESFA.DC.ReferenceData.FCS.Model.DC
{
    public class Contract : AbstractEntity
    {
        public string ContractNumber { get; set; }

        public int ContractVersionNumber { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public List<ContractAllocation> ContractAllocations { get; set; }

        public int ContractorId { get; set; }

        public Contractor Contractor { get; set; }
    }
}
