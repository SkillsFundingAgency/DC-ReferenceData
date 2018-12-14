using System;
using ESFA.DC.Logging.Interfaces;

namespace ESFA.DC.ReferenceData.ULN.Console
{
    public class ConsoleLoggerStub : ILogger
    {
        public void Dispose()
        {
        }

        public void LogFatal(string message, Exception exception = null, object[] parameters = null, string callerMemberName = "", string callerFilePath = "", int callerLineNumber = 0)
        {
            System.Console.WriteLine(message);
        }

        public void LogError(string message, Exception exception = null, object[] parameters = null, string callerMemberName = "", string callerFilePath = "", int callerLineNumber = 0)
        {
            System.Console.WriteLine(message);
        }

        public void LogWarning(string message, object[] parameters = null, string callerMemberName = "", string callerFilePath = "", int callerLineNumber = 0)
        {
            System.Console.WriteLine(message);
        }

        public void LogDebug(string message, object[] parameters = null, string callerMemberName = "", string callerFilePath = "", int callerLineNumber = 0)
        {
            System.Console.WriteLine(message);
        }

        public void LogInfo(string message, object[] parameters = null, string callerMemberName = "", string callerFilePath = "", int callerLineNumber = 0)
        {
            System.Console.WriteLine(message);
        }

        public void LogVerbose(string message, object[] parameters = null, string callerMemberName = "", string callerFilePath = "", int callerLineNumber = 0)
        {
            System.Console.WriteLine(message);
        }
    }
}
