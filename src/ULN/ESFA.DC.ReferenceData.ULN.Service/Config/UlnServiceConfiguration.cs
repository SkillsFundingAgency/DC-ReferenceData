using ESFA.DC.ReferenceData.ULN.Service.Config.Interface;

namespace ESFA.DC.ReferenceData.ULN.Service.Config
{
    public class UlnServiceConfiguration : IUlnServiceConfiguration
    {
        public string UlnStorageConnectionString { get; set; }

        public string UlnStorageContainerName { get; set; }

        public string UlnSqlConnectionString { get; set; }
    }
}
