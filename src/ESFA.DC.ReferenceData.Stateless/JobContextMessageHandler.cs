using System;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.JobContext.Interface;
using ESFA.DC.ReferenceData.Stateless.Interfaces;

namespace ESFA.DC.ReferenceData.Stateless
{
    public class JobContextMessageHandler : IMessageHandler<IJobContextMessage>
    {
        public Task<bool> Handle(IJobContextMessage message, CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }
    }
}
