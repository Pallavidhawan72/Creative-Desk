using System;
using System.Collections.Generic;

namespace CreativeDesk.Models
{
    public class DesignerAvailabilityViewModel
    {
        public int DesignerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<ProjectAssignment> Projects { get; set; } = new();
        public List<ProjectInfo> AllProjects { get; set; } = new();
    }

    public class ProjectAssignment
    {
        public int ProjectId { get; set; }
        public string ProjectTitle { get; set; } = string.Empty;
        public DateTime Deadline { get; set; }
    }

    public class ProjectInfo
    {
        public int ProjectId { get; set; }
        public string Title { get; set; } = string.Empty;
    }
} 