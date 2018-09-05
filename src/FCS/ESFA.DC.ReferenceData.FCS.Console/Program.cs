using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
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

          //  var startPage = fcsFeedService.FindFirstPageFromEntryPointAsync(fcsClientConfig.FeedUri + "/api/contracts/notifications/approval-onwards", CancellationToken.None).Result;

        //    var fcsContracts = fcsFeedService.LoadContractsFromFeedToEndAsync(startPage, CancellationToken.None).Result.ToList();

            //var fcsContracts = fcsFeedService.LoadContractsFromFeedToEndAsync(fcsClientConfig.FeedUri + "/api/contracts/notifications/approval-onwards", CancellationToken.None).Result.ToList();

           // var contractMappingService = new ContractMappingService();

            //var dcContracts = fcsContracts.Select(contractMappingService.Map).ToList();

            var dcContractors = BuildContractors(300);

            using (var fcsContext = new FcsContext("Server=(local);Database=ESFA.DC.ReferenceData.FCS.Database;Trusted_Connection=True;"))
            {
                fcsContext.Configuration.AutoDetectChangesEnabled = false;

                using (var transaction = fcsContext.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var contract in dcContractors)
                        {
                            fcsContext.Contractors.Add(contract);
                        }

                        fcsContext.SaveChanges();

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
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

        private static List<Contractor> BuildContractors(int count)
        {
            return Enumerable.Range(0, count).Select(BuildContractor).ToList();
        }

        private static Contractor BuildContractor(int iteration)
        {
            return new Contractor()
            {
                LegalName = "Name : " + iteration,
                OrganisationIdentifier = "OrganisationIdentifier : " + iteration,
                Ukprn = iteration,
                Contracts = new List<Contract>()
                {
                    BuildContract(iteration),
                    BuildContract(iteration),
                }
            };
        }


        private static Contract BuildContract(int iteration)
        {
            return new Contract()
            {
                ContractNumber = "ContractNumber : " + iteration,
                ContractVersionNumber = iteration,
                StartDate = new DateTime(2017, 1, 1),
                EndDate = new DateTime(2018, 1, 1),
                ContractAllocations = new List<ContractAllocation>()
                {
                    BuildContractAllocation(iteration),
                    BuildContractAllocation(iteration),
                    BuildContractAllocation(iteration),
                    BuildContractAllocation(iteration),
                }
            };
        }

        private static ContractAllocation BuildContractAllocation(int iteration)
        {
            return new ContractAllocation()
            {
                ContractAllocationNumber = "CANumber : " + iteration,
                FundingStreamCode = "FSCode" + iteration,
                StartDate = new DateTime(2017, 1, 1),
                EndDate = new DateTime(2018, 1, 1),
                Period = iteration.ToString(),
                PeriodTypeCode = iteration.ToString(),
                ContractDeliverables = new List<ContractDeliverable>()
                {
                    BuildContractDeliverable(iteration),
                    BuildContractDeliverable(iteration),
                    BuildContractDeliverable(iteration),
                    BuildContractDeliverable(iteration)
                }
            };
        }

        private static ContractDeliverable BuildContractDeliverable(int iteration)
        {
            return new ContractDeliverable()
            {
                DeliverableCode = iteration,
                PlannedValue = iteration,
                PlannedVolume = iteration,
                UnitCost = iteration,
            };
        }
    }
}
