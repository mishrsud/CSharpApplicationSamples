using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Smi.DataAccess
{
    [Table("Accounts")]
    public class Account
    {
        [Key]
        public int AccountId { get; set; }
        public Guid AccountGuid { get; set; }
        public string AccountNumber { get; set; }
        public string AccountNickname { get; set; }
        public bool AccountStatus { get; set; }
    }
}