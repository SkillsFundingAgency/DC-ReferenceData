using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ESFA.DC.ReferenceData.EPA.Model.EPA;
using RestSharp;

namespace ESFA.DC.ReferenceData.EPA.Console
{
    class Program
    {
        static IRestClient _client = new RestClient("https://findapprenticeshiptraining-api.sfa.bis.gov.uk/");

        static void Main(string[] args)
        {
            ServicePoint servicePoint = ServicePointManager.FindServicePoint(new Uri("https://findapprenticeshiptraining-api.sfa.bis.gov.uk/"));
            servicePoint.ConnectionLimit = 250;
            servicePoint.UseNagleAlgorithm = false;
            
            var request = new RestRequest("assessment-organisations");

            var organisations = _client.Execute<List<Organisation>>(request).Data;
            
            var organisationsPopulated = Task.WhenAll(organisations.Select(GetStandardsAndUpdateOrganisation)).Result;
        }

        public async static Task<Organisation> GetStandardsAndUpdateOrganisation(Organisation organisation)
        {
            var standardsRequest = new RestRequest("assessment-organisations/{organisationId}/standards");

            standardsRequest.AddUrlSegment("organisationId", organisation.Id);

            var standardsResponseTask = await _client.ExecuteTaskAsync<List<Standard>>(standardsRequest).ConfigureAwait(false);

            organisation.Standards = standardsResponseTask.Data;

            return organisation;
        }
    }
}
