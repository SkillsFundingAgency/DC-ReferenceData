using ESFA.DC.ReferenceData.FCS.Service.Config.Interface;

namespace ESFA.DC.ReferenceData.FCS.Service.Config
{
    public class FcsClientConfig : IFcsClientConfig
    {
        public string FeedUri { get; set; }
        public string AADInstance { get; set; }
        public string Tenant { get; set; }
        public string ResourceId { get; set; }
        public string ClientId { get; set; }
        public string AppKey { get; set; }
    }
}
