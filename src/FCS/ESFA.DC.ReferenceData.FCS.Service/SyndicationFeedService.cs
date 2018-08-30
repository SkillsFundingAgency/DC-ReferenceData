using System.Net.Http;
using System.ServiceModel.Syndication;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using ESFA.DC.ReferenceData.FCS.Service.Interface;

namespace ESFA.DC.ReferenceData.FCS.Service
{
    public class SyndicationFeedService : ISyndicationFeedService
    {
        private readonly HttpClient _httpClient;

        public SyndicationFeedService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<SyndicationFeed> LoadSyndicationFeedFromUriAsync(string uri)
        {
            var response = await _httpClient.GetAsync(uri, CancellationToken.None);

            using (var contentStream = await response.Content.ReadAsStreamAsync())
            {
                using (var xmlReader = new XmlTextReader(contentStream))
                {
                    return SyndicationFeed.Load(xmlReader);
                }
            }
        }
    }
}
