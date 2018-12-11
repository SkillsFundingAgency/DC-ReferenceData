using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ReferenceData.ULN.Model.Interface;
using ESFA.DC.ReferenceData.ULN.Service.Interface;
using FastMember;

namespace ESFA.DC.ReferenceData.ULN.Service
{
    public class UlnPersistenceService : IUlnPersistenceService
    {
        private readonly IUlnQueryService _ulnQueryService;
        private readonly Func<IUlnContext> _ulnContextFactory;

        public UlnPersistenceService(IUlnQueryService ulnQueryService, Func<IUlnContext> ulnContextFactory)
        {
            _ulnQueryService = ulnQueryService;
            _ulnContextFactory = ulnContextFactory;
        }

        public async Task PersistAsync(string fileName, IEnumerable<long> ulns, CancellationToken cancellationToken)
        {
            ulns = await _ulnQueryService.RetrieveNewUlnsAsync(ulns, cancellationToken);

            using (var ulnContext = _ulnContextFactory())
            {
                using (var sqlConnection = new SqlConnection(ulnContext.ConnectionString))
                {
                    await sqlConnection.OpenAsync(cancellationToken);

                    using (var sqlTransaction = sqlConnection.BeginTransaction())
                    {
                        using (var ulnSqlBulkCopy = new SqlBulkCopy(sqlConnection, SqlBulkCopyOptions.KeepIdentity, sqlTransaction))
                        {
                            ulnSqlBulkCopy.DestinationTableName = "UniqueLearnerNumber";
                            ulnSqlBulkCopy.BulkCopyTimeout = 120;
                            var ulnReader = ObjectReader.Create(ulns.Select(u => new { ULN = u }));

                            await ulnSqlBulkCopy.WriteToServerAsync(ulnReader, cancellationToken);
                        }

                        sqlTransaction.Commit();
                    }
                }
            }
        }
    }
}
