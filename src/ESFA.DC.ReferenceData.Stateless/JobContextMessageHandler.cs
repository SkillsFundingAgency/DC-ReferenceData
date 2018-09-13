using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.JobContext;
using ESFA.DC.ReferenceData.Stateless.Interfaces;

namespace ESFA.DC.ReferenceData.Stateless
{
    public class JobContextMessageHandler : IMessageHandler<JobContextMessage>
    {
        public Task<bool> HandleAsync(JobContextMessage message, CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }
    }
}
