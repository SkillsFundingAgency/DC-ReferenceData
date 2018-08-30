using System.Collections.Generic;
using System.Threading.Tasks;
using ESFA.DC.ReferenceData.FCS.Model;

namespace ESFA.DC.ReferenceData.FCS.Service.Interface
{
    public interface IFcsFeedService
    {
        Task<string> FindFirstPageFromEntryPointAsync(string uri);

        Task<IEnumerable<contract>> LoadContractsFromFeedToEndAsync(string uri);
    }
}
