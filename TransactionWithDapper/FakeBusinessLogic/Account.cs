using System;
using Dapper;

namespace FakeBusinessLogic
{
    [Table("Account")]
    public class Account
    {
        [Key]
        public Guid AccountId { get; set; }
        public string AccountNumber { get; set; }
        public Guid HeaderAccountId { get; set; }
        public Guid AccountUserId { get; set; }
        public bool Active { get; set; }
    }
}
