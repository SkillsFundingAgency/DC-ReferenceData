using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using ESFA.DC.ReferenceData.FCS.Model;
using ESFA.DC.ReferenceData.FCS.Service;
using ESFA.DC.ReferenceData.FCS.Service.Config;
using ESFA.DC.ReferenceData.FCS.Service.Config.Interface;
using ESFA.DC.ReferenceData.FCS.Service.Model;
using ESFA.DC.Serialization.Json;
using ESFA.DC.Serialization.Xml;

namespace ESFA.DC.ReferenceData.FCS.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var stopwatch = new Stopwatch();

            var logFile = "log.txt";

            stopwatch.Start();
            
            var fcsClientConfig = BuildConfig();   

            var accessTokenProvider = new AccessTokenProvider(fcsClientConfig);

            var httpClient = new FcsHttpClientFactory(accessTokenProvider).Create();

            var syndicationFeedService = new SyndicationFeedService(httpClient);

            var fcsSyndicationFeedParserService = new FcsSyndicationFeedParserService(new XmlSerializationService());

            var contractMappingService = new ContractMappingService();

            var fcsFeedService = new FcsFeedService(syndicationFeedService, fcsSyndicationFeedParserService, contractMappingService);
            
            var existingSyndicationItemIds = new List<Guid>();

            using (var fcsContext = new FcsContext("Server=(local);Database=ESFA.DC.ReferenceData.FCS.Database;Trusted_Connection=True;"))
            {
                existingSyndicationItemIds = new FcsContractsPersistenceService(fcsContext).GetExistingSyndicationItemIds(CancellationToken.None).Result.ToList();
            }

            var fcsContracts = fcsFeedService.GetNewContractorsFromFeedAsync(fcsClientConfig.FeedUri + "/api/contracts/notifications/approval-onwards", existingSyndicationItemIds, CancellationToken.None).Result.ToList();
            
            File.AppendAllText(logFile, stopwatch.ElapsedMilliseconds + " ms - got FCS Contracts" + fcsContracts.Count);

            //var fcsContracts = fcsFeedService.LoadContractsFromFeedToEndAsync(fcsClientConfig.FeedUri + "/api/contracts/notifications/approval-onwards", CancellationToken.None).Result.ToList();
            
            File.AppendAllText(logFile, stopwatch.ElapsedMilliseconds + " ms - map to DC Contracts - " + fcsContracts.Count);

            //   var dcContracts = BuildMasterContracts(300);

            using (var fcsContext = new FcsContext("Server=(local);Database=ESFA.DC.ReferenceData.FCS.Database;Trusted_Connection=True;"))
            {
                fcsContext.Configuration.AutoDetectChangesEnabled = false;

                var fcsContractsPersistenceService = new FcsContractsPersistenceService(fcsContext);

                fcsContractsPersistenceService.PersistContracts(fcsContracts, CancellationToken.None).Wait();
            }

            File.AppendAllText(logFile, stopwatch.ElapsedMilliseconds + " ms - Persisted DC Contracts - " + fcsContracts.Count);
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

        private static List<Contractor> BuildMasterContracts(int count)
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
