using Creative_Desk.Models;

namespace Creative_Desk.Services
{
    public class ProjectService : IProjectService
    {
        private readonly WorkTrackingDbContext _context;

        public ProjectService(WorkTrackingDbContext context)
        {
            _context = context;
        }

        public async Task<List<Project>> GetAllProjectsAsync()
        {
            return await _context.Projects.Include(p => p.Designer).ToListAsync();
        }

        public async Task<Project> GetProjectByIdAsync(int id)
        {
            return await _context.Projects.Include(p => p.Designer)
                                          .FirstOrDefaultAsync(p => p.ProjectId == id);
        }

        public async Task CreateProjectAsync(Project project)
        {
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
        }

        public async Task<Project> UpdateProjectAsync(int id, Project project)
        {
            var existingProject = await _context.Projects.FindAsync(id);
            if (existingProject == null) return null;

            existingProject.Name = project.Name;
            existingProject.Deadline = project.Deadline;
            existingProject.Status = project.Status;
            existingProject.TotalPayment = project.TotalPayment;
            existingProject.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return existingProject;
        }

        public async Task<bool> DeleteProjectAsync(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null) return false;

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}

