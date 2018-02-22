using System;

namespace FakeBusinessLogic
{
    public class AccountBalance
    {
        public Guid AccountBalanceId { get; set; }
        public Guid AccountId { get; set; }
        public decimal Amount { get; set; }
        public long Entered { get; set; }
        public Guid EnteredBy { get; set; }
        public long Updated { get; set; }
        public Guid UpdatedBy { get; set; }
    }
}
