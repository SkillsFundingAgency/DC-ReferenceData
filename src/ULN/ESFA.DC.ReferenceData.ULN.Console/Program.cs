using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ReferenceData.ULN.Service;

namespace ESFA.DC.ReferenceData.ULN.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var ulnReferenceDataTask = new ULNReferenceDataTask();

            ulnReferenceDataTask.ExecuteAsync(CancellationToken.None);
        }
    }
}
