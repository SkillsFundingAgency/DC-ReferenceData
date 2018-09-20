using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ReferenceData.EPA.Model;
using RestSharp;
using Organisation = ESFA.DC.ReferenceData.EPA.Model.EPA.Organisation;
using Period = ESFA.DC.ReferenceData.EPA.Model.EPA.Period;
using Standard = ESFA.DC.ReferenceData.EPA.Model.EPA.Standard;

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

            var mappedOrganisations = organisationsPopulated.Select(
                o =>
                    new Model.Organisation()
                    {
                        Id = o.Id,
                        Name = o.Name,
                        Ukprn = o.Ukprn,
                        Standards = o.Standards.Select(
                            s =>
                                new Model.Standard()
                                {
                                    StandardCode = s.StandardCode,
                                    Periods = s.Periods.Select(
                                        p => new Model.Period()
                                        {
                                            EffectiveFrom = p.EffectiveFrom,
                                            EffectiveTo = p.EffectiveTo,
                                        }).ToList()
                                }).ToList()
                    }
            );

            using (var epaContext = new EpaContext("Server=(local);Database=ESFA.DC.ReferenceData.EPA.Database;Trusted_Connection=True;"))
            {
                using (var transaction = epaContext.Database.BeginTransaction())
                {
                    try
                    {
                        epaContext.Organisations.RemoveRange(epaContext.Organisations);

                        epaContext.SaveChangesAsync(CancellationToken.None).Wait();

                        epaContext.Organisations.AddRange(mappedOrganisations);

                        epaContext.SaveChangesAsync(CancellationToken.None).Wait();

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
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
