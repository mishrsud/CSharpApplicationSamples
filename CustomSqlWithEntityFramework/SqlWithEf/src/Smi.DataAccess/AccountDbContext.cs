using System.Data.Entity;

namespace Smi.DataAccess
{
    public class AccountDbContext : DbContext, IAccountDbContext
    {
        public AccountDbContext(string connectionString) 
            : base(connectionString)
        {
            Database = new SourceDatabase(base.Database);
        }

        public DbSet<Account> Account { get; set; }
        public DbSet<AccountOwners> AccountOwners { get; set; }
        public DbSet<Users> Users { get; set; }

        public new IDatabase Database { get; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<AccountOwners>()
                .HasKey(pk => new {pk.AccountId, pk.UserId});
        }
    }
}