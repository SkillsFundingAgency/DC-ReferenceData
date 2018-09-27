using System;
using System.Runtime.CompilerServices;
using ESFA.DC.Logging.Interfaces;

namespace ESFA.DC.ReferenceData.EPA.Console
{
    public class LoggerStub : ILogger
    {
        public void Dispose()
        {
        }

        public void LogDebug(string message, object[] parameters = null, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
        {
            System.Console.WriteLine(message);
        }

        public void LogError(string message, Exception exception = null, object[] parameters = null, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
        {
            System.Console.WriteLine(message);
        }

        public void LogFatal(string message, Exception exception = null, object[] parameters = null, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
        {
            System.Console.WriteLine(message);
        }

        public void LogInfo(string message, object[] parameters = null, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
        {
            System.Console.WriteLine(message);
        }

        public void LogVerbose(string message, object[] parameters = null, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
        {
            System.Console.WriteLine(message);
        }

        public void LogWarning(string message, object[] parameters = null, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
        {
            System.Console.WriteLine(message);
        }
    }
}
