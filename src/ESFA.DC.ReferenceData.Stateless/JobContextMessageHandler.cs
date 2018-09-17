using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.JobContext;
using ESFA.DC.ReferenceData.Interfaces;
using ESFA.DC.ReferenceData.Stateless.Interfaces;

namespace ESFA.DC.ReferenceData.Stateless
{
    public class JobContextMessageHandler : IMessageHandler<JobContextMessage>
    {
        private readonly IEnumerable<IReferenceDataTask> _referenceDataTasks;

        public JobContextMessageHandler(IEnumerable<IReferenceDataTask> referenceDataTasks)
        {
            _referenceDataTasks = referenceDataTasks;
        }

        public async Task<bool> HandleAsync(JobContextMessage jobContextMessage, CancellationToken cancellationToken)
        {
            var taskNames = GetTaskNamesForTopicFromMessage(jobContextMessage);

            var tasks = _referenceDataTasks.Where(t => taskNames.Contains(t.TaskName));

            foreach (var task in tasks)
            {
                await task.ExecuteAsync(cancellationToken);
            }

            return true;
        }

        public IEnumerable<string> GetTaskNamesForTopicFromMessage(JobContextMessage jobContextMessage)
        {
            return jobContextMessage
                .Topics[jobContextMessage.TopicPointer]
                .Tasks
                .SelectMany(t => t.Tasks);
        }
    }
}
