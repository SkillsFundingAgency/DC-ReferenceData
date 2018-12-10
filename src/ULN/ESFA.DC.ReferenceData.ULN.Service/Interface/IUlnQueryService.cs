using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ESFA.DC.ReferenceData.ULN.Service.Interface
{
    public interface IUlnQueryService
    {
        Task<IEnumerable<string>> RetrieveNewFileNamesAsync(IEnumerable<string> fileNames, CancellationToken cancellationToken);

        Task<IEnumerable<long>> RetrieveNewUlnsAsync(IEnumerable<long> ulns, CancellationToken cancellationToken);
    }
}
