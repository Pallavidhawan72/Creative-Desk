namespace CreativeDesk.Models
{

    // Designer.cs
    public class Designer
    {
        public int DesignerId { get; set; }

        // Add required or initialize
        public required string Name { get; set; }
        public required string AvailabilityStatus { get; set; }

        // Initialize collection to avoid null warning
        public ICollection<ProjectDesigner> ProjectDesigns { get; set; } = new List<ProjectDesigner>();
    }

}