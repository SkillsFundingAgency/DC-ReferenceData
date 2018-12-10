using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ESFA.DC.ReferenceData.ULN.Service.Interface
{
    public interface IUlnFileService
    {
        Task<Stream> GetStreamAsync(string filename, string container, CancellationToken cancellationToken);

        Task<IEnumerable<string>> GetFilenamesAsync(string container, CancellationToken cancellationToken);
    }
}
