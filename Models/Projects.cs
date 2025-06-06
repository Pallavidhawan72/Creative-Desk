namespace Creative_Desk.Models
{
    public class Project
    {
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public DateTime Deadline { get; set; }
        public string Status { get; set; }
        public decimal TotalPayment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public int DesignerId { get; set; }
        public User Designer { get; set; }

        public ICollection<ProjectDesigner> ProjectDesigners { get; set; }
        public ICollection<Payment> Payments { get; set; }
    }

}