using System;
using System.Diagnostics;
using System.Fabric;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Autofac.Integration.ServiceFabric;
using ESFA.DC.JobContext;
using ESFA.DC.JobContextManager;
using ESFA.DC.JobContextManager.Interface;
using Microsoft.ServiceFabric.Services.Runtime;

namespace ESFA.DC.ReferenceData.Stateless
{
    internal static class Program
    {
        /// <summary>
        /// This is the entry point of the service host process.
        /// </summary>
        private static void Main()
        {
            try
            {
                // The ServiceManifest.XML file defines one or more service type names.
                // Registering a service maps a service type name to a .NET type.
                // When Service Fabric creates an instance of this service type,
                // an instance of the class is created in this host process.

                var builder = BuildContainer();

                builder.RegisterServiceFabricSupport();

                builder.RegisterStatelessService<Stateless>("ESFA.DC.ReferenceData.StatelessType");

                using (var container = builder.Build())
                {
                    ServiceEventSource.Current.ServiceTypeRegistered(Process.GetCurrentProcess().Id, typeof(Stateless).Name);

                    // Prevents this host process from terminating so services keep running.
                    Thread.Sleep(Timeout.Infinite);
                }
            }
            catch (Exception e)
            {
                ServiceEventSource.Current.ServiceHostInitializationFailed(e.ToString());
                throw;
            }
        }

        private static ContainerBuilder BuildContainer()
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterType<JobContextManagerForQueue<JobContextMessage>>().As<IJobContextManager<JobContextMessage>>().InstancePerLifetimeScope();

            return containerBuilder;
        }
    }
}
