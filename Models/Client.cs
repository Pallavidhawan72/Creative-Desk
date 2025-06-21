using System.Collections.Generic;

namespace CreativeDesk.Models
{
    public class Client
    {
        public int ClientId { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public ICollection<Project> Projects { get; set; } = new List<Project>();
    }
}
