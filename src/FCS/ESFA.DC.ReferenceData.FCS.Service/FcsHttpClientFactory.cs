using System.Net.Http;
using System.Net.Http.Headers;
using ESFA.DC.ReferenceData.FCS.Service.Config.Interface;
using ESFA.DC.ReferenceData.FCS.Service.Interface;
using RestSharp;

namespace ESFA.DC.ReferenceData.FCS.Service
{
    public class FcsHttpClientFactory : IHttpClientFactory
    {
        private readonly IAccessTokenProvider _accessTokenProvider;
        private readonly IFcsClientConfig _fcsClientConfig;

        public FcsHttpClientFactory(IAccessTokenProvider accessTokenProvider, IFcsClientConfig fcsClientConfig)
        {
            _accessTokenProvider = accessTokenProvider;
            _fcsClientConfig = fcsClientConfig;
        }

        public HttpClient Create()
        {
            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessTokenProvider.ProvideAsync().Result);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.sfa.contract.v1+atom+xml"));
            
            return httpClient;
        }
    }
}
