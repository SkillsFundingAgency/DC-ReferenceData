using ESFA.DC.ReferenceData.EPA.Service.Config.Interface;

namespace ESFA.DC.ReferenceData.EPA.Service.Config
{
    public class EpaServiceConfiguration : IEpaServiceConfiguration
    {
        public string EpaEndpointUri { get; set; }
        public string EpaConnectionString { get; set; }
    }
}
