using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ReferenceData.ULN.Service.Interface;

namespace ESFA.DC.ReferenceData.ULN.Console.Stubs
{
    public class UlnFileServiceStub : IUlnFileService
    {
        public Task<Stream> GetStreamAsync(string filename, string container, CancellationToken cancellationToken)
        {
            return Task.FromResult(File.OpenRead(filename) as Stream);
        }

        public Task<IEnumerable<string>> GetFilenamesAsync(string container, CancellationToken cancellationToken)
        {
            return Task.FromResult(Directory.EnumerateFiles(container));
        }
    }
}
