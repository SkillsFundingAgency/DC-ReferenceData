using ESFA.DC.ReferenceData.ULN.Service.Config.Interface;

namespace ESFA.DC.ReferenceData.ULN.Service.Config
{
    public class UlnServiceConfiguration : IUlnServiceConfiguration
    {
        public string StorageConnectionString { get; set; }

        public string ContainerName { get; set; }

        public string UlnConnectionString { get; set; }
    }
}
