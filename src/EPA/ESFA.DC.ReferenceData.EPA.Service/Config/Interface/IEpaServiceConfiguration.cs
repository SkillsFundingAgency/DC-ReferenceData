namespace ESFA.DC.ReferenceData.EPA.Service.Config.Interface
{
    public interface IEpaServiceConfiguration
    {
        string EpaEndpointUri { get; }

        string EpaConnectionString { get; }
    }
}
