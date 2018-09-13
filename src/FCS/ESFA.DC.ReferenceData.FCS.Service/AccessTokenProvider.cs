using System;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ReferenceData.FCS.Service.Config.Interface;
using ESFA.DC.ReferenceData.FCS.Service.Interface;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Polly;

namespace ESFA.DC.ReferenceData.FCS.Service
{
    public class AccessTokenProvider : IAccessTokenProvider
    {
        private readonly IFcsServiceConfiguration _fcsClientConfig;

        public AccessTokenProvider(IFcsServiceConfiguration fcsClientConfig)
        {
            _fcsClientConfig = fcsClientConfig;
        }

        public async Task<string> ProvideAsync()
        {
            var policy = Policy
                .Handle<AdalException>(ex => ex.ErrorCode == "temporarily_unavailable")
                .WaitAndRetryAsync(3, a => TimeSpan.FromSeconds(3));

            return await policy.ExecuteAsync(async () => await AcquireTokenAsync());
        }

        private async Task<string> AcquireTokenAsync()
        {
            var authContext = new AuthenticationContext(_fcsClientConfig.Authority);
            var clientCredential = new ClientCredential(_fcsClientConfig.ClientId, _fcsClientConfig.AppKey);

            var authResult = await authContext.AcquireTokenAsync(_fcsClientConfig.ResourceId, clientCredential);

            if (authResult == null)
            {
                throw new AuthenticationException("Could not authenticate with the OAUTH2 claims provider after several attempts");
            }

            return authResult.AccessToken;
        }
    }
}
