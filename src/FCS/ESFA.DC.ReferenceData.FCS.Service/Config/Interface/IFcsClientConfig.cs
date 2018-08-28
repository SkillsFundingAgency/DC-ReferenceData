using System;
using System.Collections.Generic;
using System.Text;

namespace ESFA.DC.ReferenceData.FCS.Service.Config.Interface
{
    public interface IFcsClientConfig
    {
        string FeedUri { get; }

        string AADInstance { get; }

        string Tenant { get; }

        string ResourceId { get; }

        string ClientId { get; }

        string AppKey { get; }
    }
}
