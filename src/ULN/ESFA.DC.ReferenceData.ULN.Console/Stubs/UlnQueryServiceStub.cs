using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ReferenceData.ULN.Service.Interface;

namespace ESFA.DC.ReferenceData.ULN.Console.Stubs
{
    public class UlnQueryServiceStub : IUlnQueryService
    {
        public Task<IEnumerable<string>> RetrieveNewFileNamesAsync(IEnumerable<string> fileNames, CancellationToken cancellationToken)
        {
            return Task.FromResult(fileNames);
        }

        public Task<IEnumerable<long>> RetrieveNewUlnsAsync(IEnumerable<long> ulns, CancellationToken cancellationToken)
        {
            return Task.FromResult(ulns);
        }
    }
}
