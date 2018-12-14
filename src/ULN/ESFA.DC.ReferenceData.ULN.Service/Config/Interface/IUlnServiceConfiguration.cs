namespace ESFA.DC.ReferenceData.ULN.Service.Config.Interface
{
    public interface IUlnServiceConfiguration
    {
        string StorageConnectionString { get; }

        string ContainerName { get; }

        string UlnConnectionString { get; }
    }
}
