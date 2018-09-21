using ESFA.DC.ReferenceData.EPA.Service.Config.Interface;

namespace ESFA.DC.ReferenceData.EPA.Service.Config
{
    public class EpaServiceConfiguration : IEpaServiceConfiguration
    {
        public string EndpointUri { get; set; }
        public string ConnectionString { get; set; }
    }
}
