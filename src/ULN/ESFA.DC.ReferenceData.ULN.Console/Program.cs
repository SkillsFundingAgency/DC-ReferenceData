using System;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ReferenceData.ULN.Console.Stubs;
using ESFA.DC.ReferenceData.ULN.Model;
using ESFA.DC.ReferenceData.ULN.Model.Interface;
using ESFA.DC.ReferenceData.ULN.Service;
using ESFA.DC.ReferenceData.ULN.Service.Config;
using ESFA.DC.Serialization.Json;

namespace ESFA.DC.ReferenceData.ULN.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var ulnServiceConfiguration = new UlnServiceConfiguration()
            {
                ContainerName = "Files",
                UlnConnectionString = @"Server=.\\;Database=ESFA.DC.ReferenceData.ULN.Database;Trusted_Connection=True;"
            };
            
            var ulnQueryService = new UlnQueryService(new JsonSerializationService(), ulnServiceConfiguration);

            var ulnReferenceDataTask = new ULNReferenceDataTask(
                ulnServiceConfiguration,
                new UlnFileServiceStub(),
                ulnQueryService, 
                new UlnFileDeserializer(), 
                new UlnPersistenceService(ulnQueryService, ulnServiceConfiguration));

            ulnReferenceDataTask.ExecuteAsync(CancellationToken.None).Wait();


        }
    }
}
