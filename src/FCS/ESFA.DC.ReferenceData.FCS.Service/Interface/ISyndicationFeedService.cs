using System.ServiceModel.Syndication;
using System.Threading.Tasks;

namespace ESFA.DC.ReferenceData.FCS.Service.Interface
{
    public interface ISyndicationFeedService
    {
        Task<SyndicationFeed> LoadSyndicationFeedFromUriAsync(string uri);
    }
}
