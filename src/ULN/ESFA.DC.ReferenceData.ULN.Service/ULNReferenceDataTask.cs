using System;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ReferenceData.Interfaces;
using ESFA.DC.ReferenceData.Interfaces.Constants;
using ESFA.DC.ReferenceData.ULN.Service.Config.Interface;
using ESFA.DC.ReferenceData.ULN.Service.Interface;

namespace ESFA.DC.ReferenceData.ULN.Service
{
    public class ULNReferenceDataTask : IReferenceDataTask
    {
        private readonly IUlnServiceConfiguration _ulnServiceConfiguration;
        private readonly IUlnFileService _ulnFileService;
        private readonly IUlnQueryService _ulnQueryService;
        private readonly IUlnFileDeserializer _ulnFileDeserializer;
        private readonly IUlnPersistenceService _ulnPersistenceService;

        public string TaskName => TaskNameConstants.UlnReferenceDataTaskName;

        public ULNReferenceDataTask(
            IUlnServiceConfiguration ulnServiceConfiguration,
            IUlnFileService ulnFileService,
            IUlnQueryService ulnQueryService,
            IUlnFileDeserializer ulnFileDeserializer,
            IUlnPersistenceService ulnPersistenceService)
        {
            _ulnServiceConfiguration = ulnServiceConfiguration;
            _ulnFileService = ulnFileService;
            _ulnQueryService = ulnQueryService;
            _ulnFileDeserializer = ulnFileDeserializer;
            _ulnPersistenceService = ulnPersistenceService;
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var fileNames = await _ulnFileService.GetFilenamesAsync(_ulnServiceConfiguration.ContainerName, cancellationToken);

            var newFilenames = await _ulnQueryService.RetrieveNewFileNamesAsync(fileNames, cancellationToken);

            foreach (var fileName in newFilenames)
            {
                using (var stream = await _ulnFileService.GetStreamAsync(fileName, _ulnServiceConfiguration.ContainerName, cancellationToken))
                {
                    var ulnFile = _ulnFileDeserializer.Deserialize(stream);

                    await _ulnPersistenceService.PersistAsync(fileName, ulnFile.ULNs, cancellationToken);
                }
            }
        }
    }
}
