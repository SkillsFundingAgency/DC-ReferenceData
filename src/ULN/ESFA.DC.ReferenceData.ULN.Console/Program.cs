using System.Threading;
using ESFA.DC.ReferenceData.ULN.Service;

namespace ESFA.DC.ReferenceData.ULN.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var ulnReferenceDataTask = new ULNReferenceDataTask(null, null, null, null, null);

            ulnReferenceDataTask.ExecuteAsync(CancellationToken.None).RunSynchronously();
        }
    }
}
