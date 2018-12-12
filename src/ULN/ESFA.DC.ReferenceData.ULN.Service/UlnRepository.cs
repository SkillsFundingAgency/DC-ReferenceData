using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using ESFA.DC.DateTimeProvider.Interface;
using ESFA.DC.ReferenceData.ULN.Model;
using ESFA.DC.ReferenceData.ULN.Service.Config.Interface;
using ESFA.DC.ReferenceData.ULN.Service.Interface;
using ESFA.DC.Serialization.Interfaces;
using FastMember;

namespace ESFA.DC.ReferenceData.ULN.Service
{
    public class UlnRepository : IUlnRepository
    {
        private readonly IUlnServiceConfiguration _ulnServiceConfiguration;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IJsonSerializationService _jsonSerializationService;

        public UlnRepository(IUlnServiceConfiguration ulnServiceConfiguration, IDateTimeProvider dateTimeProvider, IJsonSerializationService jsonSerializationService)
        {
            _ulnServiceConfiguration = ulnServiceConfiguration;
            _dateTimeProvider = dateTimeProvider;
            _jsonSerializationService = jsonSerializationService;
        }
        
        public async Task PersistAsync(Import import, IEnumerable<long> ulns, CancellationToken cancellationToken)
        {
            using (var sqlConnection = new SqlConnection(_ulnServiceConfiguration.UlnConnectionString))
            {
                await sqlConnection.OpenAsync(cancellationToken);

                using (var sqlTransaction = sqlConnection.BeginTransaction())
                {
                    await InsertImport(sqlConnection, sqlTransaction, import, cancellationToken);
                    await InsertUlns(sqlConnection, sqlTransaction, ProjectUniqueLearnerNumbers(ulns, import.Id), cancellationToken);
                    await UpdateImport(sqlConnection, sqlTransaction, import.Id, _dateTimeProvider.GetNowUtc(), cancellationToken);

                    sqlTransaction.Commit();
                }
            }
        }

        public async Task<IEnumerable<string>> RetrieveNewFileNamesAsync(IEnumerable<string> fileNames, CancellationToken cancellationToken)
        {
            var sql =
                @"SELECT New.Filename  FROM OPENJSON(@json)
                    WITH (Filename nvarchar(255) '$') New
                    WHERE New.Filename NOT IN (SELECT [Filename] FROM [Import])";

            var json = _jsonSerializationService.Serialize(fileNames);

            var commandDefinition = new CommandDefinition(sql, new { json }, cancellationToken: cancellationToken);

            using (var sqlConnection = new SqlConnection(_ulnServiceConfiguration.UlnConnectionString))
            {
                return await sqlConnection.QueryAsync<string>(commandDefinition);
            }
        }

        public async Task<IEnumerable<long>> RetrieveNewUlnsAsync(IEnumerable<long> ulns, CancellationToken cancellationToken)
        {
            var sql =
                @"SELECT New.ULN  FROM OPENJSON(@json)
                    WITH (ULN bigint '$') New
                    WHERE New.ULN NOT IN (SELECT [ULN] FROM [UniqueLearnerNumber])";

            var json = _jsonSerializationService.Serialize(ulns);

            using (var sqlConnection = new SqlConnection(_ulnServiceConfiguration.UlnConnectionString))
            {
                var commandDefinition = new CommandDefinition(sql, new { json }, cancellationToken: cancellationToken);
                return await sqlConnection.QueryAsync<long>(commandDefinition);
            }
        }

        private IEnumerable<UniqueLearnerNumber> ProjectUniqueLearnerNumbers(IEnumerable<long> ulns, int importId)
        {
            return ulns.Select(u => new UniqueLearnerNumber() { ImportId = importId, Uln = u });
        }

        private SqlBulkCopy BuildUlnBulkCopy(SqlConnection sqlConnection, SqlTransaction sqlTransaction)
        {
            var sqlBulkCopy = new SqlBulkCopy(sqlConnection, SqlBulkCopyOptions.KeepIdentity, sqlTransaction)
            {
                BatchSize = 5000,
                BulkCopyTimeout = 300,
                DestinationTableName = "UniqueLearnerNumber",
                EnableStreaming = true,
            };
            
            sqlBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("Uln", "Uln"));
            sqlBulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("ImportId", "ImportId"));

            return sqlBulkCopy;
        }

        private async Task InsertUlns(SqlConnection sqlConnection, SqlTransaction sqlTransaction, IEnumerable<UniqueLearnerNumber> ulns, CancellationToken cancellationToken)
        {
            var ulnReader = ObjectReader.Create(ulns);

            using (var ulnSqlBulkCopy = BuildUlnBulkCopy(sqlConnection, sqlTransaction))
            {
                await ulnSqlBulkCopy.WriteToServerAsync(ulnReader, cancellationToken);
            }
        }

        private async Task<int> InsertImport(SqlConnection sqlConnection, SqlTransaction sqlTransaction, Import import, CancellationToken cancellationToken)
        {
            var insertImportSql = @"INSERT INTO [Import] ([Filename], [UlnsInFileCount], [NewUlnsInFileCount], [StartDateTime])
                                VALUES (@Filename, @UlnsInFileCount, @NewUlnsInFileCount, @StartDateTime);
                                SELECT CAST(SCOPE_IDENTITY() AS INT)";

            var insertImportCommandDefinition = new CommandDefinition(insertImportSql, import, sqlTransaction, cancellationToken: cancellationToken);

            import.Id = await sqlConnection.QueryFirstAsync<int>(insertImportCommandDefinition);

            return import.Id;
        }

        private async Task UpdateImport(SqlConnection sqlConnection, SqlTransaction sqlTransaction, int id, DateTime endDateTime, CancellationToken cancellationToken)
        {
            var updateImportSql = @"UPDATE [Import] SET [EndDateTime] = @EndDateTime WHERE [Id] = @Id";

            var updateImportCommandDefinition = new CommandDefinition(updateImportSql, new { Id = id, EndDateTime = endDateTime }, sqlTransaction, cancellationToken: cancellationToken);

            await sqlConnection.ExecuteAsync(updateImportCommandDefinition);
        }
    }
}
