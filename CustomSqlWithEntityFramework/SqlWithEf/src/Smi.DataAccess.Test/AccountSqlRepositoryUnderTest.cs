using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Moq;
using Moq.AutoMock;
using Shouldly;
using Xunit;

namespace Smi.DataAccess.Test
{
    public class AccountSqlRepositoryUnderTest
    {
        private readonly AutoMocker _autoMocker;

        public AccountSqlRepositoryUnderTest()
        {
            _autoMocker = new AutoMocker();
        }

        [Fact, Trait("Category", "Unit")]
        public void ShouldFetchExpectedRecords_WhenCalled()
        {
            var repoInstance = _autoMocker.CreateInstance<AccountSqlRepository>();
            
            var mockDatabase = _autoMocker.GetMock<IDatabase>();
            mockDatabase
                .Setup(database => database.SqlQuery<Account>(It.IsAny<string>()))
                .Returns(GetSeededDataForAccounts);
            
            _autoMocker.GetMock<IAccountDbContext>()
                .Setup(context => context.Database)
                .Returns(() => mockDatabase.Object);

            var allRecords = repoInstance.GetAllRecords();

            allRecords.Count().ShouldBe(2);
        }

        [Fact, Trait("Category", "Integration")]
        public void ShouldFetchExpectedRecordsFromDb_WhenCalled()
        {
            IAccountDbContext dbContext = new AccountDbContext(
                GetTestDatabaseConnectionString());
            var repoInstance = new AccountSqlRepository(dbContext);

            var allRecords = repoInstance.GetAllRecords();

            allRecords.Count().ShouldBe(1);
        }

        
        [Fact, Trait("Category", "Integration")]
        public void ShouldFetchExpectedRecordsFromDbUsingStoredProc_WhenCalled()
        {
            IAccountDbContext dbContext = new AccountDbContext(
                GetTestDatabaseConnectionString());
            var repoInstance = new AccountSqlRepository(dbContext);

            var allRecords = repoInstance.GetAccountsForUser(1, true);

            allRecords.Count().ShouldBe(1);
        }

        private static List<Account> GetSeededDataForAccounts()
        {
            return new List<Account>
            {
                new Account
                {
                    AccountId = 1,
                    AccountGuid = Guid.NewGuid(),
                    AccountNickname = "Shoe Account",
                    AccountNumber = "SAV1293812938",
                    AccountStatus = true
                },
                new Account
                {
                    AccountId = 2,
                    AccountGuid = Guid.NewGuid(),
                    AccountNickname = "Sandal Account",
                    AccountNumber = "SAV1293818945",
                    AccountStatus = true
                },

            };
        }

        private static string GetTestDatabaseConnectionString()
        {
            var connectionStringBuilder = new SqlConnectionStringBuilder
            {
                DataSource = "localhost,1433",
                InitialCatalog = "Bank",
                UserID = "sa",
                Password = "G00don3!",
                MultipleActiveResultSets = true
            };

            return connectionStringBuilder.ToString();
        }
    }
}
