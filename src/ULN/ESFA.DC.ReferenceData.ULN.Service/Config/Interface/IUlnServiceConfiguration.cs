namespace ESFA.DC.ReferenceData.ULN.Service.Config.Interface
{
    public interface IUlnServiceConfiguration
    {
        string UlnStorageConnectionString { get; }

        string UlnStorageContainerName { get; }

        string UlnSqlConnectionString { get; }
    }
}
