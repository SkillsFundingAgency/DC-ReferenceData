using System.Configuration;
using System.Threading.Tasks;
using ESFA.DC.ReferenceData.FCS.Service;
using ESFA.DC.ReferenceData.FCS.Service.Config;
using ESFA.DC.ReferenceData.FCS.Service.Config.Interface;
using RestSharp;

namespace ESFA.DC.ReferenceData.FCS.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var fcsClientConfig = BuildConfig();   

            var accessTokenProvider = new AccessTokenProvider(fcsClientConfig);

            var httpClient = new FcsHttpClientFactory(accessTokenProvider, fcsClientConfig).Create();

            var syndicationFeedService = new SyndicationFeedService(httpClient);

            var syndicationFeed = syndicationFeedService.LoadFromUriAsync(fcsClientConfig.FeedUri + "/api/contracts/notifications/approval-onwards").Result;
        }

        private static IFcsClientConfig BuildConfig()
        {
            var appSettings = ConfigurationManager.AppSettings;

            return new FcsClientConfig()
            {
                Authority = appSettings["Authority"],
                AppKey = appSettings["AppKey"],
                ClientId = appSettings["ClientId"],
                FeedUri = appSettings["FeedUri"],
                ResourceId = appSettings["ResourceId"],
            };
        }
    }
}
