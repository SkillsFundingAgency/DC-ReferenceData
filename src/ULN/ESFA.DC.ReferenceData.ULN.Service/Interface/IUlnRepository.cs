using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ReferenceData.ULN.Model;

namespace ESFA.DC.ReferenceData.ULN.Service.Interface
{
    public interface IUlnRepository
    {
        Task PersistAsync(Import import, IEnumerable<long> ulns, CancellationToken cancellationToken);

        Task<IEnumerable<string>> RetrieveNewFileNamesAsync(IEnumerable<string> fileNames, CancellationToken cancellationToken);

        Task<IEnumerable<long>> RetrieveNewUlnsAsync(IEnumerable<long> ulns, CancellationToken cancellationToken);
    }
}
