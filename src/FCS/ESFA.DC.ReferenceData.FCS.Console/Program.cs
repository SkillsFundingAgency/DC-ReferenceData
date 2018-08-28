using System.Configuration;
using ESFA.DC.ReferenceData.FCS.Service;
using ESFA.DC.ReferenceData.FCS.Service.Config;
using ESFA.DC.ReferenceData.FCS.Service.Config.Interface;

namespace ESFA.DC.ReferenceData.FCS.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var fcsClientConfig = BuildConfig();   

            var syndicationFeedService = new SyndicationFeedService(null);
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
