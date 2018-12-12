using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.DateTimeProvider.Interface;
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

        public string TaskName => TaskNameConstants.UlnReferenceDataTaskName;

        public ULNReferenceDataTask(
            IUlnServiceConfiguration ulnServiceConfiguration,
            IUlnFileService ulnFileService,
            IUlnFileDeserializer ulnFileDeserializer,
            IUlnRepository ulnRepository,
            IDateTimeProvider dateTimeProvider)
        {
            _ulnServiceConfiguration = ulnServiceConfiguration;
            _ulnFileService = ulnFileService;
            _ulnFileDeserializer = ulnFileDeserializer;
            _ulnRepository = ulnRepository;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var fileNames = await _ulnFileService.GetFilenamesAsync(_ulnServiceConfiguration.ContainerName, cancellationToken);

            var newFilenames = await _ulnRepository.RetrieveNewFileNamesAsync(fileNames, cancellationToken);

            foreach (var fileName in newFilenames)
            {
                using (var stream = await _ulnFileService.GetStreamAsync(fileName, _ulnServiceConfiguration.ContainerName, cancellationToken))
                {
                    var ulnFile = _ulnFileDeserializer.Deserialize(stream);

                    var newUlnsInFile = await _ulnRepository.RetrieveNewUlnsAsync(ulnFile.ULNs, cancellationToken);

                    var import = new Import()
                    {
                        Filename = fileName,
                        NewUlnsInFileCount = newUlnsInFile.Count(),
                        UlnsInFileCount = ulnFile.ULNs.Count(),
                        StartDateTime = _dateTimeProvider.GetNowUtc(),
                    };

                    await _ulnRepository.PersistAsync(import, newUlnsInFile, cancellationToken);
                }
            }
        }
    }
}
