using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.JobContext;
using ESFA.DC.Logging.Interfaces;
using ESFA.DC.ReferenceData.Interfaces;
using ESFA.DC.ReferenceData.Stateless.Interfaces;

namespace ESFA.DC.ReferenceData.Stateless
{
    public class JobContextMessageHandler : IMessageHandler<JobContextMessage>
    {
        private readonly IEnumerable<IReferenceDataTask> _referenceDataTasks;
        private readonly ILogger _logger;

        public JobContextMessageHandler(IEnumerable<IReferenceDataTask> referenceDataTasks, ILogger logger)
        {
            _referenceDataTasks = referenceDataTasks;
            _logger = logger;
        }

        public async Task<bool> HandleAsync(JobContextMessage jobContextMessage, CancellationToken cancellationToken)
        {
            try
            {
                var taskNames = GetTaskNamesForTopicFromMessage(jobContextMessage);

                var tasks = _referenceDataTasks.Where(t => taskNames.Contains(t.TaskName)).ToList();

                _logger.LogInfo($"Handling Reference Data Message - Message Tasks : {string.Join(", ", taskNames)} - Reference Data Tasks found in Registry : {string.Join(", ", tasks.Select(t => t.TaskName))}");

                foreach (var task in tasks)
                {
                    _logger.LogInfo($"Reference Data Task : {task.TaskName} Starting");

                    await task.ExecuteAsync(cancellationToken);

                    _logger.LogInfo($"Reference Data Task : {task.TaskName} Finished");
                }
            }
            catch (Exception exception)
            {
                _logger.LogError("Reference data Message Handler Failed", exception);
                throw;
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
