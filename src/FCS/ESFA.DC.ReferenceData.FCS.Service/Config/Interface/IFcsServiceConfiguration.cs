namespace ESFA.DC.ReferenceData.FCS.Service.Config.Interface
{
    public interface IFcsServiceConfiguration
    {
        string FeedUri { get; }

        string Authority { get; }

        string ResourceId { get; }

        string ClientId { get; }

        string AppKey { get; }

        string FcsConnectionString { get; }
    }
}
