using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CreativeDesk.Data;
using CreativeDesk.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CreativeDesk.Controllers
{
    public class DesignersAvailabilityController : Controller
    {
        private readonly ApplicationDbContext _context;
        public DesignersAvailabilityController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var designers = await _context.Designers
                .Include(d => d.ProjectDesigns)
                    .ThenInclude(pd => pd.Project)
                .ToListAsync();
            var allProjects = await _context.Projects.ToListAsync();

            var model = designers.Select(d => new DesignerAvailabilityViewModel
            {
                DesignerId = d.DesignerId,
                Name = d.Name,
                Projects = d.ProjectDesigns.Select(pd => new ProjectAssignment
                {
                    ProjectId = pd.Project.ProjectId,
                    ProjectTitle = pd.Project.Title,
                    Deadline = pd.Project.Deadline
                }).ToList(),
                AllProjects = allProjects.Select(p => new ProjectInfo
                {
                    ProjectId = p.ProjectId,
                    Title = p.Title
                }).ToList()
            }).ToList();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ReassignDesigner(int DesignerId, int CurrentProjectId, int NewProjectId)
        {
            var assignment = await _context.Set<ProjectDesigner>()
                .FirstOrDefaultAsync(pd => pd.DesignerId == DesignerId && pd.ProjectId == CurrentProjectId);
            if (assignment != null)
            {
                _context.Set<ProjectDesigner>().Remove(assignment);
                await _context.SaveChangesAsync();

                // Add new assignment
                var newAssignment = new ProjectDesigner
                {
                    DesignerId = DesignerId,
                    ProjectId = NewProjectId
                };
                _context.Set<ProjectDesigner>().Add(newAssignment);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }
} 