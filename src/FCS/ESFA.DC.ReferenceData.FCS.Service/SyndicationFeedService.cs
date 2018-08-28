using System;
using System.IO;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Xml;
using ESFA.DC.ReferenceData.FCS.Service.Interface;
using RestSharp;

namespace ESFA.DC.ReferenceData.FCS.Service
{
    public class SyndicationFeedService : ISyndicationFeedService
    {
        private readonly IRestClient _restClient;

        public SyndicationFeedService(IRestClient restClient)
        {
            _restClient = restClient;
        }

        public async Task<SyndicationFeed> LoadFromUriAsync(string uri)
        {
            var request = new RestRequest(uri);

            var response = await _restClient.ExecuteTaskAsync(request);

            using (var xmlReader = new XmlTextReader(new StringReader(response.Content)))
            {
                return SyndicationFeed.Load(xmlReader);
            }
        }
    }
}
