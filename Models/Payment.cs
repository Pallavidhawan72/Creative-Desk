namespace Creative_Desk.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public int ProjectId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime PaymentDate { get; set; }

        public Project Project { get; set; }
    }
}
