using System.Data.Entity;

namespace Smi.DataAccess
{
    public interface IAccountDbContext
    {
        IDatabase Database { get; }
        DbSet<Account> Account { get; set; }
        DbSet<AccountOwners> AccountOwners { get; set; }
        DbSet<Users> Users { get; set; }
    }
}