using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ESFA.DC.Queueing.Interface;

namespace ESFA.DC.ReferenceData.Stateless
{
    public class TopicPublishServiceStub<T> : ITopicPublishService<T>
        where T : new()
    {
        public Task PublishAsync(T obj, IDictionary<string, object> properties, string messageLabel)
        {
            throw new NotImplementedException();
        }
    }
}
