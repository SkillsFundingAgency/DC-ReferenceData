using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using ESFA.DC.ReferenceData.ULN.Model.Interface;
using ESFA.DC.ReferenceData.ULN.Service.Config.Interface;
using ESFA.DC.ReferenceData.ULN.Service.Interface;
using ESFA.DC.Serialization.Interfaces;

namespace ESFA.DC.ReferenceData.ULN.Service
{
    public class UlnQueryService : IUlnQueryService
    {
        private readonly IJsonSerializationService _jsonSerializationService;
        private readonly IUlnServiceConfiguration _ulnServiceConfiguration;

        public UlnQueryService(IJsonSerializationService jsonSerializationService, IUlnServiceConfiguration ulnServiceConfiguration)
        {
            _jsonSerializationService = jsonSerializationService;
            _ulnServiceConfiguration = ulnServiceConfiguration;
        }

        public Task<IEnumerable<string>> RetrieveNewFileNamesAsync(IEnumerable<string> fileNames, CancellationToken cancellationToken)
        {
            return Task.FromResult(fileNames);
        }

        public async Task<IEnumerable<long>> RetrieveNewUlnsAsync(IEnumerable<long> ulns, CancellationToken cancellationToken)
        {
            string sql = 
                @"SELECT New.ULN  FROM OPENJSON(@ulnjson)
                    WITH (ULN bigint '$') New
                    WHERE New.ULN NOT IN (SELECT ULN FROM UniqueLearnerNumber)";

            using (var sqlConnection = new SqlConnection(_ulnServiceConfiguration.UlnConnectionString))
            {
                return await sqlConnection.QueryAsync<long>(sql, new {ulnjson = _jsonSerializationService.Serialize(ulns)});
            }
        }
    }
}
