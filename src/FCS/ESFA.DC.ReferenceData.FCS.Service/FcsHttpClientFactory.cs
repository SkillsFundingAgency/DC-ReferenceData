using System.Net.Http;
using System.Net.Http.Headers;
using ESFA.DC.ReferenceData.FCS.Service.Interface;

namespace ESFA.DC.ReferenceData.FCS.Service
{
    public class FcsHttpClientFactory : IHttpClientFactory
    {
        private const string MediaType = @"application/vnd.sfa.contract.v1+atom+xml";

        private readonly IAccessTokenProvider _accessTokenProvider;

        public FcsHttpClientFactory(IAccessTokenProvider accessTokenProvider)
        {
            _accessTokenProvider = accessTokenProvider;
        }

        public HttpClient Create()
        {
            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessTokenProvider.ProvideAsync().Result);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaType));
            
            return httpClient;
        }
    }
}
