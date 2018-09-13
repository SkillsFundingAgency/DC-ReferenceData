using ESFA.DC.ReferenceData.Stateless.Config.Interfaces;

namespace ESFA.DC.ReferenceData.Stateless.Config
{
    public class LoggerOptions : ILoggerOptions
    {
        public string LoggerConnectionString { get; set; }
    }
}
