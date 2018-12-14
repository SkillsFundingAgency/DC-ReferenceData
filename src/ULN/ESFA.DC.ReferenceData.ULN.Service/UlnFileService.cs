using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.FileService.Interface;
using ESFA.DC.ReferenceData.ULN.Service.Config.Interface;
using ESFA.DC.ReferenceData.ULN.Service.Interface;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.RetryPolicies;

namespace ESFA.DC.ReferenceData.ULN.Console.Stubs
{
    public class UlnFileService : IUlnFileService
    {
        private readonly IUlnServiceConfiguration _ulnServiceConfiguruation;
        private readonly IFileService _fileService;

        public UlnFileService(IUlnServiceConfiguration ulnServiceConfiguruation, IFileService fileService)
        {
            _ulnServiceConfiguruation = ulnServiceConfiguruation;
            _fileService = fileService;
        }

        public Task<Stream> GetStreamAsync(string filename, string container, CancellationToken cancellationToken)
        {
            return _fileService.OpenReadStreamAsync(filename, container, cancellationToken);
        }

        public async Task<IEnumerable<string>> GetFilenamesAsync(string container, CancellationToken cancellationToken)
        {
            var cloudBlobContainer = GetCloudBlobContainer(container);

            BlobContinuationToken blobContinuationToken = null;
            BlobRequestOptions _requestOptions = new BlobRequestOptions() { RetryPolicy = new LinearRetry(TimeSpan.FromSeconds(5), 3) };

            var fileNames = new List<string>();

            do
            {
                var blobs = await cloudBlobContainer.ListBlobsSegmentedAsync(string.Empty, true, BlobListingDetails.None, null, blobContinuationToken, _requestOptions, null, cancellationToken);

                var names = blobs.Results.OfType<CloudBlockBlob>().Select(b => b.Name);

                fileNames.AddRange(names);
            } while (blobContinuationToken != null);

            return fileNames;
        }
            
        private CloudBlobContainer GetCloudBlobContainer(string container)
        {
            var cloudStorageAccount = CloudStorageAccount.Parse(_ulnServiceConfiguruation.StorageConnectionString);
            var cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            return cloudBlobClient.GetContainerReference(container);
        }
    }
}
