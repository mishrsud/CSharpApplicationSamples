using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using FakeBusinessLogic;

namespace DbTransactionDapper
{
    class Program
    {
        static void Main(string[] args)
        {
            var appConfig = new AppConfiguration()
            {
                DbHostName = "localhost",
                DbPort = 1433,
                DbUserName = "sa",
                DbPassword = "G00d1!G00d1!",
                DbInitialCatalog = "Organisation"
            };
            var dapperConnectionManager = new DapperConnectionManager(appConfig);

            var connectionString = dapperConnectionManager.GetSqlConnectionString();
            
            IEnumerable<Account> accounts = GetRandomAccountsFromDatabase(connectionString);
            Guid accountIdUnderTest = accounts.FirstOrDefault().AccountId;
            Guid userGuid = accounts.FirstOrDefault().AccountUserId;

            /*
             * Read balance
             * Create Deal
             * Link Deal to Account
             * Update balance
             */

            int workerCount = 100;

            Parallel.For(0, workerCount, new ParallelOptions(){MaxDegreeOfParallelism = 3} , work =>
            {
                InitiateDealAndUpdateBalance(connectionString, userGuid, accountIdUnderTest);
            });

            Console.WriteLine("Finished, press any key to exit");
            Console.ReadKey();
        }

        private static void InitiateDealAndUpdateBalance(string connectionString, Guid userGuid, Guid accountIdUnderTest)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (var transaction =
                    sqlConnection.BeginTransaction(IsolationLevel.RepeatableRead, "UpdateBalanceTransaction"))
                {
                    try
                    {
                        // CREATE DEAL
                        decimal dealAmount = 1.0m;
                        Guid dealGuid = Guid.NewGuid();
                        CreateDeal(sqlConnection, dealGuid, dealAmount, userGuid, transaction);

                        // LINK TO VIRTUAL ACCOUNT
                        LinkDealToAccount(sqlConnection, transaction, accountIdUnderTest, dealGuid);

                        // READ BALANCE
                        var vaBalance = sqlConnection.Query<AccountBalance>(
                            "SELECT [Amount] FROM [dbo].[AccountBalance] WHERE [AccountId] = @AccountId",
                            new { AccountId = accountIdUnderTest }, transaction).FirstOrDefault();
                        decimal? currentBalance = vaBalance?.Amount;

                        // ADD OR UPDATE BALANCE
                        UpdateBalance(sqlConnection, accountIdUnderTest, dealAmount, currentBalance, transaction);
                        
                        transaction.Commit();
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine($"Exception in {nameof(InitiateDealAndUpdateBalance)}, Detail: {exception.Message}");
                    }
                }
            }
        }

        private static void UpdateBalance(SqlConnection sqlConnection, Guid accountIdUnderTest, decimal dealAmount,
            decimal? currentBalance, SqlTransaction transaction)
        {
            bool accountExists = sqlConnection.ExecuteScalar<bool>(
                "SELECT 1 FROM [dbo].[AccountBalance] WHERE [AccountId] = @AccountId",
                new { AccountId = accountIdUnderTest },
                transaction);

            if (accountExists)
            {
                try
                {
                    sqlConnection.Execute(
                                @"UPDATE [dbo].[AccountBalance] 
                      SET [Amount] = @Amount, [Updated] = @Updated, [UpdatedBy] = @UpdatedBy
                      WHERE [AccountId] = @AccountId",
                                new { Amount = (dealAmount + currentBalance), Updated = DateTime.UtcNow, UpdatedBy = 78934, AccountId = accountIdUnderTest },
                                transaction);
                    return;
                }
                catch (Exception exception)
                {
                    Console.WriteLine($"Exception: {exception.Message}");
                    return;
                }
            }

            // UPDATE Balance
            sqlConnection.Execute(
                @"INSERT INTO [dbo].[AccountBalance]
                       ([AccountBalanceId]
                       ,[AccountId]
                       ,[Amount]
                       ,[Entered]
                       ,[EnteredBy]
                       ,[Updated]
                       ,[UpdatedBy])
                        VALUES
                       (@AccountBalanceId
                       ,@AccountId
                       ,@Amount
                       ,@Entered
                       ,@EnteredBy
                       ,@Updated
                       ,@UpdatedBy)",
                new
                {
                    AccountBalanceId = Guid.NewGuid(),
                    AccountId = accountIdUnderTest,
                    Amount = (dealAmount + currentBalance),
                    Entered = 1,
                    EnteredBy = Guid.NewGuid(),
                    Updated = 2,
                    UpdatedBy = Guid.NewGuid(),
                }, transaction);
        }

        private static void LinkDealToAccount(SqlConnection sqlConnection, SqlTransaction transaction, Guid AccountId, Guid dealIdGuid)
        {
            int userId = 78934;
            sqlConnection.Execute(
                @"INSERT INTO [dbo].[U_AccountDeals]
                           ([UVAD_GUID]
                           ,[UVA_GUID]
                           ,[RD_GUID]
                           ,[UVAD_Entered]
                           ,[UVAD_EnteredBy]
                           ,[UVAD_Updated]
                           ,[UVAD_UpdatedBy]
                           ,[UVAD_Status])
                         VALUES
                           (
                            @UVAD_GUID
                           ,@UVA_GUID
                           ,@RD_GUID
                           ,@UVAD_Entered
                           ,@UVAD_EnteredBy
                           ,@UVAD_Updated
                           ,@UVAD_UpdatedBy
                           ,@UVAD_Status
                          )",
                new
                {
                    UVAD_GUID = Guid.NewGuid(),
                    UVA_GUID = AccountId,
                    RD_GUID = dealIdGuid,
                    UVAD_Entered = DateTime.UtcNow,
                    UVAD_EnteredBy = userId,
                    UVAD_Updated = DateTime.UtcNow,
                    UVAD_UpdatedBy = userId,
                    UVAD_Status = 1
                },
                transaction);
        }

        private static void CreateDeal(SqlConnection sqlConnection, Guid dealGuid, decimal dealAmount, Guid userGuid,
            SqlTransaction transaction)
        {
            sqlConnection.Execute(
                @"INSERT INTO [dbo].[Deal]
                           ([DealId]
                           ,[Amount]
                           ,[DealUserId]
                           ,[ReceivedDate]
                           ,[PaidDate])
                        VALUES
                           (@DealId
                           ,@Amount
                           ,@DealUserId
                           ,@ReceivedDate
                           ,@PaidDate)",
                new
                {
                    DealId = dealGuid,
                    Amount = dealAmount,
                    DealUserId = userGuid,
                    ReceivedDate = DateTime.UtcNow,
                    PaidDate = DateTime.UtcNow
                }, transaction);
        }

        private static IEnumerable<Account> GetRandomAccountsFromDatabase(string connectionString)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                return sqlConnection.GetList<Account>();
            }
        }

        private static Task SeedAccounts(string connectionString, int numberOfAccountsToInsert)
        {
            var task = Task.Run(() =>
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    using (var transaction = sqlConnection.BeginTransaction())
                    {
                        //Accounts = sqlConnection.GetList<Account>();
                        for (int i = 0; i < numberOfAccountsToInsert; i++)
                        {
                            sqlConnection.Execute(
                                @"INSERT INTO dbo.Account
                               (AccountId
                               ,AccountNumber
                               ,HeaderAccountId
                               ,AccountUserId
                               ,Active)
                            VALUES
                               (@AccountId
                               ,@AccountNumber
                               ,@HeaderAccountId
                               ,@AccountUserId
                               ,@Active)",
                                new
                                {
                                    AccountId = Guid.NewGuid(),
                                    AccountNumber = GetRandomString(),
                                    HeaderAccountId = Guid.NewGuid(),
                                    AccountUserId = Guid.NewGuid(),
                                    Active = true
                                },
                                transaction);
                        }

                        transaction.Commit();
                    }
                }
            });

            return task;
        }

        private static string GetRandomString()
        {
            var seedString = Guid.NewGuid().ToString("N").ToUpperInvariant();
            return seedString.Substring(1, 10);
        }
    }
}
