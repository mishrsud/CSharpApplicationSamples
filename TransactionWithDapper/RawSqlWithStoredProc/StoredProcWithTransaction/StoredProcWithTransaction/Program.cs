using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace StoredProcWithTransaction
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionString = GetDbConnectionString();
            var dealUserId = Guid.Parse("55b6b37d-33d1-46b0-b296-a76539eb23e7");
            var virtualAccountId = Guid.Parse("468205EC-00E6-4426-9BD7-06D50048331D");
            var enteredByUserId = 578934;
            var updatedByUserId = enteredByUserId;

            int numberOfWorkerThreads = 1000;
            int degreeOfParallelism = 8;

            Parallel.For(0, numberOfWorkerThreads, new ParallelOptions { MaxDegreeOfParallelism = degreeOfParallelism },
                work =>
                {
                    CreateBalanceRecordIfNotFound(virtualAccountId, connectionString, enteredByUserId);
                    CreateHoldBalanceDealUsingStoredProc(connectionString, dealUserId, virtualAccountId, enteredByUserId, updatedByUserId);
                });

            Console.WriteLine("Finished, Press any key to exit");
            Console.ReadKey();
        }

        private static void CreateBalanceRecordIfNotFound(Guid virtualAccountId, string connectionString, int enteredByUserId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction(IsolationLevel.RepeatableRead, "InsertBalanceRecord"))
                {
                    using (var command = new SqlCommand())
                    {
                        var sql = 
                        @"
                            IF NOT EXISTS(SELECT 1 FROM [dbo].[VirtualAccountBalance] WHERE [VirtualAccountId] = @VirtualAccountId)
                            BEGIN
                                INSERT INTO dbo.VirtualAccountBalance 
                                ([VirtualAccountBalanceId], [VirtualAccountId], [Amount], [Entered], [EnteredBy], [Updated], [UpdatedBy])
                                SELECT NEWID(),@VirtualAccountId,0,SYSUTCDATETIME(),@EnteredBy,SYSUTCDATETIME(),@EnteredBy
                            END
                        ";
                        command.CommandText = sql;
                        command.Connection = connection;
                        command.Transaction = transaction;
                        command.Parameters.Add(new SqlParameter("@VirtualAccountId", virtualAccountId));
                        command.Parameters.Add(new SqlParameter("@EnteredBy", enteredByUserId));
                        command.ExecuteNonQuery();
                        transaction.Commit();
                    }
                }
            }
        }

        private static void CreateHoldBalanceDealUsingStoredProc(string connectionString, Guid dealUserId,
            Guid virtualAccountId, int enteredByUserId, int updatedByUserId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("dbo.CreateDealAndHoldBalance", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@Amount", 1.0m));
                    command.Parameters.Add(new SqlParameter("@DealUserId", dealUserId));
                    command.Parameters.Add(new SqlParameter("@ReceivedDate", DateTime.UtcNow));
                    command.Parameters.Add(new SqlParameter("@PaidDate", DateTime.UtcNow));
                    command.Parameters.Add(new SqlParameter("@UVA_GUID", virtualAccountId));
                    command.Parameters.Add(new SqlParameter("@UVAD_Entered", DateTime.UtcNow));
                    command.Parameters.Add(new SqlParameter("@UVAD_EnteredBy", enteredByUserId));
                    command.Parameters.Add(new SqlParameter("@UVAD_Updated", DateTime.UtcNow));
                    command.Parameters.Add(new SqlParameter("@UVAD_UpdatedBy", updatedByUserId));

                    command.ExecuteNonQuery();
                }
            }
        }

        private static string GetDbConnectionString()
        {
            var connectionStringBuilder = new SqlConnectionStringBuilder
            {
                ApplicationName = "POC",
                DataSource = $"localhost, 1433",
                UserID = "sa",
                Password = "G00d1!G00d1!",
                InitialCatalog = "Organisation",
                MultipleActiveResultSets = true
            };

            return connectionStringBuilder.ToString();
        }
    }
}
