using System;
using System.Net;
using ESFA.DC.ReferenceData.EPA.Service.Config.Interface;
using ESFA.DC.ReferenceData.EPA.Service.Interface;

namespace ESFA.DC.ReferenceData.EPA.Service
{
    public class ServicePointConfigurationService : IServicePointConfigurationService
    {
        private readonly IEpaServiceConfiguration _epaServiceConfiguration;

        public ServicePointConfigurationService(IEpaServiceConfiguration epaServiceConfiguration)
        {
            _epaServiceConfiguration = epaServiceConfiguration;
        }

        public ServicePoint ConfigureServicePoint()
        {
            var servicePoint = ServicePointManager.FindServicePoint(new Uri(_epaServiceConfiguration.EpaEndpointUri));

            servicePoint.ConnectionLimit = 250;
            servicePoint.UseNagleAlgorithm = false;

            return servicePoint;
        }
    }
}
