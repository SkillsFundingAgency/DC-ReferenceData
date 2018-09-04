using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ReferenceData.FCS.Model.FCS;

namespace ESFA.DC.ReferenceData.FCS.Service.Interface
{
    public interface IFcsFeedService
    {
        Task<string> FindFirstPageFromEntryPointAsync(string uri, CancellationToken cancellationToken);

        Task<IEnumerable<contract>> LoadContractsFromFeedToEndAsync(string uri, CancellationToken cancellationToken);
    }
}
