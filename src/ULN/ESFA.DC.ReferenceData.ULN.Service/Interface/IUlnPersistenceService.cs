using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ESFA.DC.ReferenceData.ULN.Service.Interface
{
    public interface IUlnPersistenceService
    {
        Task PersistAsync(string fileName, IEnumerable<long> ulns, CancellationToken cancellationToken);
    }
}
