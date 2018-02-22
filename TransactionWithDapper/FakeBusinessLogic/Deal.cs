using System;

namespace FakeBusinessLogic
{
    public class Deal
    {
        public Guid DealId { get; set; }
        public decimal Amount { get; set; }
        public Guid DealUserId { get; set; }
        public long ReceivedDate { get; set; }
        public long PaidDate { get; set; }
    }
}
