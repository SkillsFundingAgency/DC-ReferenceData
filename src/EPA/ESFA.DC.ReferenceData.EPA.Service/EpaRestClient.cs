using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ReferenceData.EPA.Model.EPA;
using ESFA.DC.ReferenceData.EPA.Service.Interface;
using RestSharp;

namespace ESFA.DC.ReferenceData.EPA.Service
{
    public class EpaRestClient : IEpaRestClient
    {
        private readonly IRestClient _restClient;

        private const string AssessmentOrganisationsResource = "assessment-organisations";
        private const string StandardsForAssessmentOrganisationResource = "assessment-organisations/{organisationId}/standards";

        public EpaRestClient(IRestClient restClient)
        {
            _restClient = restClient;
        }

        public async Task<IEnumerable<Organisation>> GetOrganisationsAsync(CancellationToken cancellationToken)
        {
            var request = new RestRequest(AssessmentOrganisationsResource);

            return (await _restClient.ExecuteTaskAsync<List<Organisation>>(request, cancellationToken)).Data;
        }

        public async Task<IEnumerable<Standard>> GetStandardsForOrganisationAsync(string organisationId, CancellationToken cancellationToken)
        {
            var standardsRequest = new RestRequest(StandardsForAssessmentOrganisationResource);

            standardsRequest.AddUrlSegment("organisationId", organisationId);

            return (await _restClient.ExecuteTaskAsync<List<Standard>>(standardsRequest, cancellationToken)).Data;
        }
    }
}
