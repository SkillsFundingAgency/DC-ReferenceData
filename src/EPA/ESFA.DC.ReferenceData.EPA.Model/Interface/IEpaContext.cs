using System.Data.Entity;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace ESFA.DC.ReferenceData.EPA.Model.Interface
{
    public interface IEpaContext
    {
        DbSet<Organisation> Organisations { get; set; }

        DbSet<Standard> Standards { get; set; }

        DbSet<Period> Periods { get; set; }

        DbContextTransaction BeginTransaction();
        
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
