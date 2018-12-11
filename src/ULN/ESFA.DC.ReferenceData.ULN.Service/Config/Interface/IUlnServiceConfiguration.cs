namespace ESFA.DC.ReferenceData.ULN.Service.Config.Interface
{
    public interface IUlnServiceConfiguration
    {
        string ContainerName { get; }

        string UlnConnectionString { get; }
    }
}
