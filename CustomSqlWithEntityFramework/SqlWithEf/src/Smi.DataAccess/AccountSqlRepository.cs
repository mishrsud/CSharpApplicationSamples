using System.Collections.Generic;
using System.Data.SqlClient;

namespace Smi.DataAccess
{
    public class AccountSqlRepository : IRepository<Account> 
    {
        private readonly IAccountDbContext _accountDbContext;

        public AccountSqlRepository(IAccountDbContext accountDbContext)
        {
            _accountDbContext = accountDbContext;
        }

        public IEnumerable<Account> GetAllRecords()
        {
            var accountsRawSqlQuery = _accountDbContext.Database.SqlQuery<Account>(
                "SELECT [AccountId], [AccountGuid], [AccountNumber], [AccountNickname], [AccountStatus] " +
                "FROM Accounts");
            return accountsRawSqlQuery;
        }

        public IEnumerable<Account> GetAccountsForUser(int userId, bool activeOnly)
        {
            var parameters = new List<object>();
            parameters.Add(new SqlParameter("@UserId", userId));
            parameters.Add(new SqlParameter("@ActiveOnly", userId));

            var parameterNames = string.Join(",", parameters);

            var accounts = _accountDbContext.Database.SqlQuery<Account>(
                $"dbo.GetAccountsForUser {parameterNames}",
                parameters.ToArray());

            return accounts;
        }
    }
}