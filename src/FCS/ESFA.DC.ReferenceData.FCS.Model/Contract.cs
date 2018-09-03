using System;
using System.Collections.Generic;

namespace ESFA.DC.ReferenceData.FCS.Model
{
    public class Contract
    {
        public string ContractNumber { get; set; }

        public int ContractVersionNumber { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public List<ContractAllocation> ContractAllocations { get; set; }
    }
}
