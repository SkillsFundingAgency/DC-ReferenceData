using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;

namespace ESFA.DC.ReferenceData.FCS.Model.Interface
{
    public interface IFcsContext
    {
        DbSet<Contractor> Contractors { get; set; }

        DbSet<Contract> Contracts { get; set; }

        DbSet<ContractAllocation> ContractAllocations { get; set; }

        DbSet<ContractDeliverable> ContractDeliverables { get; set; }

        Database Database { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
