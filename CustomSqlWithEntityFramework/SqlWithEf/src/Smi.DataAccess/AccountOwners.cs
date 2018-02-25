using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Smi.DataAccess
{
    [Table("AccountOwners")]
    public class AccountOwners
    {
        [Key]
        public int AccountId { get; set; }
        [Key]
        public int UserId { get; set; }
    }
}