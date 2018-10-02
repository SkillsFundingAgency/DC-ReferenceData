using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ReferenceData.EPA.Model.EPA;
using ESFA.DC.ReferenceData.EPA.Service.Interface;
using Polly;
using Polly.Retry;
using RestSharp;

namespace ESFA.DC.ReferenceData.EPA.Service
{
    public class EpaRestClient : IEpaRestClient
    {
        private readonly IRestClient _restClient;

        private const string AssessmentOrganisationsResource = "assessment-organisations";
        private const string StandardsForAssessmentOrganisationResource = "assessment-organisations/{organisationId}/standards";

        private readonly RetryPolicy _retryPolicy = 
            Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(3, a => TimeSpan.FromSeconds(3));

        public EpaRestClient(IRestClient restClient)
        {
            _restClient = restClient;
        }

        public async Task<IEnumerable<Organisation>> GetOrganisationsAsync(CancellationToken cancellationToken)
        {
            var request = new RestRequest(AssessmentOrganisationsResource);

            return (await ExecuteTaskWithRetryPolicyAsync<List<Organisation>>(request, cancellationToken)).Data;
        }

        public async Task<IEnumerable<Standard>> GetStandardsForOrganisationAsync(string organisationId, CancellationToken cancellationToken)
        {
            var standardsRequest = new RestRequest(StandardsForAssessmentOrganisationResource);

            standardsRequest.AddUrlSegment("organisationId", organisationId);

            return (await ExecuteTaskWithRetryPolicyAsync<List<Standard>>(standardsRequest, cancellationToken)).Data;
        }

        private async Task<IRestResponse<T>> ExecuteTaskWithRetryPolicyAsync<T>(IRestRequest request, CancellationToken cancellationToken)
        {
            return await _retryPolicy.ExecuteAsync(async () => await _restClient.ExecuteTaskAsync<T>(request, cancellationToken));
        }

    }
}
