using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Smi.DataAccess
{
    [Table("Users")]
    public class Users
    {
        [Key]
        public int UserId { get; set; }
        public Guid UserGuid { get; set; }
    }
}