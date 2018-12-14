using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using Autofac;
using Autofac.Integration.ServiceFabric;
using ESFA.DC.Auditing.Interface;
using ESFA.DC.DateTimeProvider.Interface;
using ESFA.DC.FileService;
using ESFA.DC.FileService.Config;
using ESFA.DC.FileService.Config.Interface;
using ESFA.DC.FileService.Interface;
using ESFA.DC.JobContext.Interface;
using ESFA.DC.JobContextManager;
using ESFA.DC.JobContextManager.Interface;
using ESFA.DC.JobContextManager.Model;
using ESFA.DC.JobStatus.Interface;
using ESFA.DC.Logging;
using ESFA.DC.Logging.Config;
using ESFA.DC.Logging.Config.Interfaces;
using ESFA.DC.Logging.Enums;
using ESFA.DC.Logging.Interfaces;
using ESFA.DC.Mapping.Interface;
using ESFA.DC.Queueing;
using ESFA.DC.Queueing.Interface;
using ESFA.DC.ReferenceData.EPA.Model;
using ESFA.DC.ReferenceData.EPA.Model.Interface;
using ESFA.DC.ReferenceData.EPA.Service;
using ESFA.DC.ReferenceData.EPA.Service.Config;
using ESFA.DC.ReferenceData.EPA.Service.Config.Interface;
using ESFA.DC.ReferenceData.EPA.Service.Interface;
using ESFA.DC.ReferenceData.FCS.Model;
using ESFA.DC.ReferenceData.FCS.Model.Interface;
using ESFA.DC.ReferenceData.FCS.Service;
using ESFA.DC.ReferenceData.FCS.Service.Config;
using ESFA.DC.ReferenceData.FCS.Service.Config.Interface;
using ESFA.DC.ReferenceData.FCS.Service.Interface;
using ESFA.DC.ReferenceData.Interfaces;
using ESFA.DC.ReferenceData.Stateless.Config;
using ESFA.DC.ReferenceData.Stateless.Config.Interfaces;
using ESFA.DC.ReferenceData.ULN.Console.Stubs;
using ESFA.DC.ReferenceData.ULN.Service;
using ESFA.DC.ReferenceData.ULN.Service.Config;
using ESFA.DC.ReferenceData.ULN.Service.Config.Interface;
using ESFA.DC.ReferenceData.ULN.Service.Interface;
using ESFA.DC.Serialization.Interfaces;
using ESFA.DC.Serialization.Json;
using ESFA.DC.Serialization.Xml;
using ESFA.DC.ServiceFabric.Helpers;
using Microsoft.EntityFrameworkCore;
using RestSharp;

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
                    container.Resolve<IEnumerable<IReferenceDataTask>>();

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
            var referenceDataConfiguration = GetReferenceDataConfiguration();
            var fcsServiceConfiguration = GetFcsServiceConfiguration();
            var epaServiceConfiguration = GetEpaServiceConfiguration();
            var ulnServiceConfiguration = GetUlnServiceConfiguration();

            return new ContainerBuilder()
                .RegisterJobContextManagementServices()
                .RegisterQueuesAndTopics(referenceDataConfiguration)
                .RegisterLogger(referenceDataConfiguration)
                .RegisterSerializers()
                .RegisterFcsServices(fcsServiceConfiguration)
                .RegisterEpaServices(epaServiceConfiguration)
                .RegisterUlnService(ulnServiceConfiguration);
        }

        private static ContainerBuilder RegisterSerializers(this ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<JsonSerializationService>().As<IJsonSerializationService>();
            containerBuilder.RegisterType<XmlSerializationService>().As<IXmlSerializationService>();

            return containerBuilder;
        }

        private static ContainerBuilder RegisterJobContextManagementServices(this ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<JobContextMessageHandler>().As<IMessageHandler<JobContextMessage>>();
            containerBuilder.RegisterType<JobContextManager<JobContextMessage>>().As<IJobContextManager<JobContextMessage>>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<DefaultJobContextMessageMapper<JobContextMessage>>().As<IMapper<JobContextMessage, JobContextMessage>>();
            containerBuilder.RegisterType<DateTimeProvider.DateTimeProvider>().As<IDateTimeProvider>();

            return containerBuilder;
        }

        private static ContainerBuilder RegisterLogger(this ContainerBuilder containerBuilder, ReferenceDataConfiguration referenceDataConfiguration)
        {
            containerBuilder.RegisterInstance(new LoggerOptions()
            {
                LoggerConnectionString = referenceDataConfiguration.LoggerConnectionString
            }).As<ILoggerOptions>().SingleInstance();

            containerBuilder.Register(c =>
            {
                var loggerOptions = c.Resolve<ILoggerOptions>();
                return new ApplicationLoggerSettings
                {
                    ApplicationLoggerOutputSettingsCollection = new List<IApplicationLoggerOutputSettings>()
                    {
                        new MsSqlServerApplicationLoggerOutputSettings()
                        {
                            MinimumLogLevel = LogLevel.Verbose,
                            ConnectionString = loggerOptions.LoggerConnectionString
                        },
                        new ConsoleApplicationLoggerOutputSettings()
                        {
                            MinimumLogLevel = LogLevel.Verbose
                        }
                    }
                };
            }).As<IApplicationLoggerSettings>().SingleInstance();

            containerBuilder.RegisterType<Logging.ExecutionContext>().As<IExecutionContext>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<SerilogLoggerFactory>().As<ISerilogLoggerFactory>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<SeriLogger>().As<ILogger>().InstancePerLifetimeScope();

            return containerBuilder;
        }

        private static ContainerBuilder RegisterQueuesAndTopics(this ContainerBuilder containerBuilder, ReferenceDataConfiguration referenceDataConfiguration)
        {
            containerBuilder.Register(c =>
            {
                var topicSubscriptionConfig = new TopicConfiguration(referenceDataConfiguration.ServiceBusConnectionString, referenceDataConfiguration.TopicName, referenceDataConfiguration.SubscriptionName, 1, maximumCallbackTimeSpan: TimeSpan.FromMinutes(40));

                return new TopicSubscriptionSevice<JobContextDto>(
                    topicSubscriptionConfig,
                    c.Resolve<IJsonSerializationService>(),
                    c.Resolve<ILogger>());
            }).As<ITopicSubscriptionService<JobContextDto>>();

            containerBuilder.RegisterType<TopicPublishServiceStub<JobContextDto>>().As<ITopicPublishService<JobContextDto>>();

            containerBuilder.Register(c =>
            {
                var auditPublishConfig = new QueueConfiguration(referenceDataConfiguration.ServiceBusConnectionString, referenceDataConfiguration.AuditQueueName, 1);

                return new QueuePublishService<AuditingDto>(
                    auditPublishConfig,
                    c.Resolve<IJsonSerializationService>());
            }).As<IQueuePublishService<AuditingDto>>();

            containerBuilder.Register(c =>
            {
                var jobStatusPublishConfig = new QueueConfiguration(referenceDataConfiguration.ServiceBusConnectionString, referenceDataConfiguration.JobStatusQueueName, 1);

                return new QueuePublishService<JobStatusDto>(
                    jobStatusPublishConfig,
                    c.Resolve<IJsonSerializationService>());
            }).As<IQueuePublishService<JobStatusDto>>();

            return containerBuilder;
        }

        private static ContainerBuilder RegisterFcsServices(this ContainerBuilder containerBuilder, IFcsServiceConfiguration fcsServiceConfiguration)
        {
            containerBuilder.RegisterInstance(fcsServiceConfiguration).As<IFcsServiceConfiguration>();
            containerBuilder.RegisterType<AccessTokenProvider>().As<IAccessTokenProvider>();
            containerBuilder.RegisterType<FcsHttpClientFactory>().As<IHttpClientFactory>();
            containerBuilder.Register(c => c.Resolve<IHttpClientFactory>().Create()).As<HttpClient>().InstancePerDependency();
            containerBuilder.RegisterType<SyndicationFeedService>().As<ISyndicationFeedService>();
            containerBuilder.RegisterType<FcsSyndicationFeedParserService>().As<IFcsSyndicationFeedParserService>();
            containerBuilder.RegisterType<ContractMappingService>().As<IContractMappingService>();
            containerBuilder.RegisterType<FcsFeedService>().As<IFcsFeedService>();
            containerBuilder.Register(c =>
            {
                DbContextOptions<FcsContext> options = new DbContextOptionsBuilder<FcsContext>().UseSqlServer(fcsServiceConfiguration.FcsConnectionString).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking).Options;
                return new FcsContext(options);
            }).As<IFcsContext>().InstancePerDependency();
            containerBuilder.RegisterType<FcsContractsPersistenceService>().As<IFcsContractsPersistenceService>();
            containerBuilder.RegisterType<FcsReferenceDataTask>().As<IReferenceDataTask>();

            return containerBuilder;
        }

        private static ContainerBuilder RegisterEpaServices(this ContainerBuilder containerBuilder, IEpaServiceConfiguration epaServiceConfiguration)
        {
            containerBuilder.RegisterInstance(epaServiceConfiguration).As<IEpaServiceConfiguration>();
            containerBuilder.Register(c => new RestClient(epaServiceConfiguration.EpaEndpointUri)).As<IRestClient>().SingleInstance();
            containerBuilder.RegisterType<ServicePointConfigurationService>().As<IServicePointConfigurationService>();
            containerBuilder.RegisterType<EpaRestClient>().As<IEpaRestClient>();
            containerBuilder.RegisterType<EpaFeedService>().As<IEpaFeedService>();
            containerBuilder.RegisterType<OrganisationMapper>().As<IOrganisationMapper>();
            containerBuilder.Register(c => new EpaContext(epaServiceConfiguration.EpaConnectionString)).As<IEpaContext>().InstancePerDependency();
            containerBuilder.RegisterType<EpaPersistenceService>().As<IEpaPersistenceService>();
            containerBuilder.RegisterType<EpaReferenceDataTask>().As<IReferenceDataTask>();

            return containerBuilder;
        }

        private static ContainerBuilder RegisterUlnService(this ContainerBuilder containerBuilder, IUlnServiceConfiguration ulnServiceConfiguration)
        {
            containerBuilder.RegisterInstance(ulnServiceConfiguration).As<IUlnServiceConfiguration>();

            var azureStorageFileServiceConfiguration = new AzureStorageFileServiceConfiguration()
            {
                ConnectionString = ulnServiceConfiguration.UlnStorageConnectionString
            };

            containerBuilder.RegisterInstance(azureStorageFileServiceConfiguration).As<IAzureStorageFileServiceConfiguration>();
            containerBuilder.RegisterType<AzureStorageFileService>().As<IFileService>();
            containerBuilder.RegisterType<UlnFileService>().As<IUlnFileService>();
            containerBuilder.RegisterType<UlnFileDeserializer>().As<IUlnFileDeserializer>();
            containerBuilder.RegisterType<UlnRepository>().As<IUlnRepository>();
            containerBuilder.RegisterType<UlnReferenceDataTask>().As<IReferenceDataTask>();

            return containerBuilder;
        }

        private static ReferenceDataConfiguration GetReferenceDataConfiguration()
        {
            var configHelper = new ConfigurationHelper();

            return configHelper.GetSectionValues<ReferenceDataConfiguration>("ReferenceDataConfiguration");
        }

        private static IFcsServiceConfiguration GetFcsServiceConfiguration()
        {
            var configHelper = new ConfigurationHelper();

            return configHelper.GetSectionValues<FcsServiceConfiguration>("FcsServiceConfiguration");
        }

        private static IEpaServiceConfiguration GetEpaServiceConfiguration()
        {
            var configHelper = new ConfigurationHelper();

            return configHelper.GetSectionValues<EpaServiceConfiguration>("EpaServiceConfiguration");
        }

        private static IUlnServiceConfiguration GetUlnServiceConfiguration()
        {
            var configHelper = new ConfigurationHelper();

            return configHelper.GetSectionValues<UlnServiceConfiguration>("UlnServiceConfiguration");
        }
    }
}
