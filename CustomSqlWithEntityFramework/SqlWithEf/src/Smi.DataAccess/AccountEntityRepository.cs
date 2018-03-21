using System.Collections.Generic;
using System.Linq;

namespace Smi.DataAccess
{
    public class AccountEntityRepository : IRepository<Account>
    {
        private readonly IAccountDbContext _accountDbContext;

        public AccountEntityRepository(IAccountDbContext accountDbContext)
        {
            _accountDbContext = accountDbContext;
        }

        public IEnumerable<Account> GetAllRecords()
        {
            return _accountDbContext.Account;
        }

        public IEnumerable<Account> GetAccountsForUser(int userId, bool activeOnly)
        {
            var accounts = from account in _accountDbContext.Account
                join accountOwners in _accountDbContext.AccountOwners
                    on account.AccountId equals accountOwners.AccountId
                join users in _accountDbContext.Users
                    on accountOwners.UserId equals users.UserId
                where users.UserId == userId && account.AccountStatus == activeOnly
                select account;

            return accounts;
        }
    }
}