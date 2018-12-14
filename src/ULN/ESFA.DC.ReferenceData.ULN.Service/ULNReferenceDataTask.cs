using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.DateTimeProvider.Interface;
using ESFA.DC.Logging.Interfaces;
using ESFA.DC.ReferenceData.Interfaces;
using ESFA.DC.ReferenceData.Interfaces.Constants;
using ESFA.DC.ReferenceData.ULN.Model;
using ESFA.DC.ReferenceData.ULN.Service.Config.Interface;
using ESFA.DC.ReferenceData.ULN.Service.Interface;

namespace ESFA.DC.ReferenceData.ULN.Service
{
    public class ULNReferenceDataTask : IReferenceDataTask
    {
        private readonly IUlnServiceConfiguration _ulnServiceConfiguration;
        private readonly IUlnFileService _ulnFileService;
        private readonly IUlnFileDeserializer _ulnFileDeserializer;
        private readonly IUlnRepository _ulnRepository;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ILogger _logger;

        public string TaskName => TaskNameConstants.UlnReferenceDataTaskName;

        public ULNReferenceDataTask(
            IUlnServiceConfiguration ulnServiceConfiguration,
            IUlnFileService ulnFileService,
            IUlnFileDeserializer ulnFileDeserializer,
            IUlnRepository ulnRepository,
            IDateTimeProvider dateTimeProvider,
            ILogger logger)
        {
            _ulnServiceConfiguration = ulnServiceConfiguration;
            _ulnFileService = ulnFileService;
            _ulnFileDeserializer = ulnFileDeserializer;
            _ulnRepository = ulnRepository;
            _dateTimeProvider = dateTimeProvider;
            _logger = logger;
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            try
            {
                var fileNames = await _ulnFileService.GetFilenamesAsync(_ulnServiceConfiguration.ContainerName, cancellationToken);
                _logger.LogInfo($"Found {fileNames.Count()} ULN Files.");

                var newFilenames = await _ulnRepository.RetrieveNewFileNamesAsync(fileNames, cancellationToken);
                _logger.LogInfo($"Found {newFilenames.Count()} new ULN Files.");

                foreach (var fileName in newFilenames)
                {
                    _logger.LogInfo($"Processing {fileName}.");
                    using (var stream = await _ulnFileService.GetStreamAsync(fileName, _ulnServiceConfiguration.ContainerName, cancellationToken))
                    {
                        var ulnFile = _ulnFileDeserializer.Deserialize(stream);
                       
                        _logger.LogInfo($"Found {ulnFile.ULNs.Count()} ULNs in {fileName}.");

                        var newUlnsInFile = await _ulnRepository.RetrieveNewUlnsAsync(ulnFile.ULNs, cancellationToken);

                        _logger.LogInfo($"Found {newUlnsInFile.Count()} new ULNs in {fileName}.");

                        var import = new Import()
                        {
                            Filename = fileName,
                            NewUlnsInFileCount = newUlnsInFile.Count(),
                            UlnsInFileCount = ulnFile.ULNs.Count(),
                            StartDateTime = _dateTimeProvider.GetNowUtc(),
                        };

                        _logger.LogInfo($"Starting Persisting ULNs from {fileName}");

                        await _ulnRepository.PersistAsync(import, newUlnsInFile, cancellationToken);

                        _logger.LogInfo($"Finished Persisting ULNs from {fileName}");
                    }
                }
            }
            catch (Exception exception)
            {
                _logger.LogError("ULN Reference Data Task Failed.", exception);
                throw;
            }
        }
    }
}
