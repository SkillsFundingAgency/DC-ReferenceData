using System;
using System.Collections.Generic;

namespace ESFA.DC.ReferenceData.FCS.Model
{
    public partial class Contract
    {
        public Contract()
        {
            ContractAllocation = new HashSet<ContractAllocation>();
        }

        public int Id { get; set; }
        public string ContractNumber { get; set; }
        public int? ContractVersionNumber { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int ContractorId { get; set; }

        public virtual Contractor Contractor { get; set; }
        public virtual ICollection<ContractAllocation> ContractAllocation { get; set; }
    }
}
