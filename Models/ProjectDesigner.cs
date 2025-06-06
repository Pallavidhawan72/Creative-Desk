namespace Creative_Desk.Models
{
    public class ProjectDesigner
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }

        public int DesignerId { get; set; }
        public User Designer { get; set; }
    }
}