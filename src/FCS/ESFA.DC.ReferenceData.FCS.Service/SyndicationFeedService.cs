using System;
using System.Net.Http;
using System.ServiceModel.Syndication;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using ESFA.DC.Logging.Interfaces;
using ESFA.DC.ReferenceData.FCS.Service.Interface;

namespace ESFA.DC.ReferenceData.FCS.Service
{
    public class SyndicationFeedService : ISyndicationFeedService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;

        public SyndicationFeedService(HttpClient httpClient, ILogger logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<SyndicationFeed> LoadSyndicationFeedFromUriAsync(string uri, CancellationToken cancellationToken)
        {
            var response = await _httpClient.GetAsync(uri, cancellationToken);

            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (Exception exception)
            {
                _logger.LogError($"Http Request for {uri} Failed", exception);
                throw;
            }

            using (var contentStream = await response.Content.ReadAsStreamAsync())
            {
                using (var xmlReader = new XmlTextReader(contentStream))
                {
                    try
                    {
                        return SyndicationFeed.Load(xmlReader);
                    }
                    catch (XmlException xmlException)
                    {
                        _logger.LogInfo(response.Content.ReadAsStringAsync().Result);
                        _logger.LogError($"Syndication Feed Load with Xml Exception {uri} Failed", xmlException);

                        throw;
                    }
                    catch (Exception exception)
                    {
                        _logger.LogError($"Syndication Feed Load for {uri} Failed", exception);

                        throw;
                    }
                }
            }
        }
    }
}
