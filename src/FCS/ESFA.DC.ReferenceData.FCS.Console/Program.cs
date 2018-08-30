using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using ESFA.DC.ReferenceData.FCS.Model;
using ESFA.DC.ReferenceData.FCS.Service;
using ESFA.DC.ReferenceData.FCS.Service.Config;
using ESFA.DC.ReferenceData.FCS.Service.Config.Interface;
using ESFA.DC.Serialization.Json;
using ESFA.DC.Serialization.Xml;

namespace ESFA.DC.ReferenceData.FCS.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var fcsClientConfig = BuildConfig();   

            var accessTokenProvider = new AccessTokenProvider(fcsClientConfig);

            var httpClient = new FcsHttpClientFactory(accessTokenProvider).Create();

            var syndicationFeedService = new SyndicationFeedService(httpClient);

            var fcsSyndicationFeedParserService = new FcsSyndicationFeedParserService(new XmlSerializationService(), new JsonSerializationService());

            var fcsFeedService = new FcsFeedService(syndicationFeedService, fcsSyndicationFeedParserService);

            var startPage = fcsFeedService.FindFirstPageFromEntryPointAsync(fcsClientConfig.FeedUri + "/api/contracts/notifications/approval-onwards").Result;

            var contracts = fcsFeedService.LoadContractsFromFeedToEndAsync(startPage).Result.ToList();
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
