namespace CreativeDesk.Models
{
    public class ProjectDesigner
    {
        public int ProjectId { get; set; }
        public Project Project { get; set; }

        public int DesignerId { get; set; }
        public Designer Designer { get; set; }
    }
}
