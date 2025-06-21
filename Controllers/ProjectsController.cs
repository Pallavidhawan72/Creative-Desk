using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CreativeDesk.Data;
using CreativeDesk.Models;

namespace CreativeDesk.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjectsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // FRESH CRUD: Minimal, working CRUD for Project
        // GET: Projects
        public async Task<IActionResult> Index()
        {
            var projects = _context.Projects.Include(p => p.Client);
            return View(await projects.ToListAsync());
        }

        // GET: Projects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var project = await _context.Projects.Include(p => p.Client).FirstOrDefaultAsync(m => m.ProjectId == id);
            if (project == null) return NotFound();
            return View(project);
        }

        // GET: Projects/Create
        public IActionResult Create()
        {
            ViewBag.ClientId = new SelectList(_context.Clients, "ClientId", "Name");
            ViewBag.Designers = new MultiSelectList(_context.Designers, "DesignerId", "Name");
            return View();
        }

        // POST: Projects/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProjectId,Title,Deadline,TotalAmount,AmountPaid,ClientId")] Project project, int[] DesignerIds)
        {
            if (ModelState.IsValid)
            {
                project.RemainingAmount = project.TotalAmount - project.AmountPaid;
                _context.Add(project);
                await _context.SaveChangesAsync();
                System.Diagnostics.Debug.WriteLine($"[Create] ProjectId after save: {project.ProjectId}");
                // Clear navigation property to avoid EF Core tracking issues
                project.ProjectDesigners = null;
                System.Diagnostics.Debug.WriteLine("[Create] ProjectDesigners navigation property: " + (project.ProjectDesigners == null ? "null" : project.ProjectDesigners.Count.ToString()));
                System.Diagnostics.Debug.WriteLine("[Create] DesignerIds: " + (DesignerIds == null ? "null" : string.Join(",", DesignerIds)));
                if (DesignerIds == null)
                    System.Diagnostics.Debug.WriteLine("[Create] DesignerIds is null");
                else if (DesignerIds.Length == 0)
                    System.Diagnostics.Debug.WriteLine("[Create] DesignerIds is empty");
                if (DesignerIds != null && DesignerIds.Length > 0)
                {
                    foreach (var did in DesignerIds)
                    {
                        if (did <= 0)
                        {
                            System.Diagnostics.Debug.WriteLine($"[Create] Skipping invalid DesignerId: {did}");
                            continue;
                        }
                        System.Diagnostics.Debug.WriteLine($"[Create] Adding ProjectDesigner: ProjectId={project.ProjectId}, DesignerId={did}");
                        _context.Set<ProjectDesigner>().Add(new ProjectDesigner { ProjectId = project.ProjectId, DesignerId = did });
                    }
                    await _context.SaveChangesAsync();
                }
                System.Diagnostics.Debug.WriteLine("DesignerIds (Create): " + string.Join(",", DesignerIds ?? new int[0]));
                return RedirectToAction(nameof(Index));
            }
            ViewBag.ClientId = new SelectList(_context.Clients, "ClientId", "Name", project.ClientId);
            ViewBag.Designers = new MultiSelectList(_context.Designers, "DesignerId", "Name", DesignerIds);
            return View(project);
        }

        // GET: Projects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var project = await _context.Projects
                .Include(p => p.ProjectDesigners)
                .FirstOrDefaultAsync(p => p.ProjectId == id);
            if (project == null) return NotFound();
            ViewBag.ClientId = new SelectList(_context.Clients, "ClientId", "Name", project.ClientId);
            ViewBag.Designers = new MultiSelectList(_context.Designers, "DesignerId", "Name", project.ProjectDesigners.Select(pd => pd.DesignerId));
            return View(project);
        }

        // POST: Projects/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProjectId,Title,Deadline,TotalAmount,AmountPaid,ClientId")] Project project, int[] DesignerIds)
        {
            if (id != project.ProjectId) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    project.RemainingAmount = project.TotalAmount - project.AmountPaid;
                    _context.Update(project);
                    await _context.SaveChangesAsync();
                    System.Diagnostics.Debug.WriteLine($"[Edit] ProjectId after save: {project.ProjectId}");
                    if (DesignerIds == null)
                        System.Diagnostics.Debug.WriteLine("[Edit] DesignerIds is null");
                    else if (DesignerIds.Length == 0)
                        System.Diagnostics.Debug.WriteLine("[Edit] DesignerIds is empty");
                    // Update ProjectDesigners
                    var existing = _context.Set<ProjectDesigner>().Where(pd => pd.ProjectId == project.ProjectId);
                    _context.Set<ProjectDesigner>().RemoveRange(existing);
                    if (DesignerIds != null && DesignerIds.Length > 0)
                    {
                        foreach (var did in DesignerIds)
                        {
                            System.Diagnostics.Debug.WriteLine($"[Edit] Adding ProjectDesigner: ProjectId={project.ProjectId}, DesignerId={did}");
                            if (did > 0)
                            {
                                _context.Set<ProjectDesigner>().Add(new ProjectDesigner { ProjectId = project.ProjectId, DesignerId = did });
                            }
                        }
                        await _context.SaveChangesAsync();
                    }
                    System.Diagnostics.Debug.WriteLine("DesignerIds (Edit): " + string.Join(",", DesignerIds ?? new int[0]));
                }
                catch (Exception ex)
                {
                    // Log the error to the debug output and add to ModelState
                    System.Diagnostics.Debug.WriteLine("Edit Error: " + ex.ToString());
                    ModelState.AddModelError("", "Unable to save changes: " + ex.Message);
                }
                return RedirectToAction(nameof(Index));
            }
            // Output ModelState errors for debugging
            foreach (var key in ModelState.Keys)
            {
                var errors = ModelState[key].Errors;
                foreach (var error in errors)
                {
                    System.Diagnostics.Debug.WriteLine($"ModelState error for {key}: {error.ErrorMessage}");
                }
            }
            ViewBag.ClientId = new SelectList(_context.Clients, "ClientId", "Name", project.ClientId);
            ViewBag.Designers = new MultiSelectList(_context.Designers, "DesignerId", "Name", DesignerIds);
            return View(project);
        }

        // GET: Projects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var project = await _context.Projects.Include(p => p.Client).FirstOrDefaultAsync(m => m.ProjectId == id);
            if (project == null) return NotFound();
            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project != null)
            {
                _context.Projects.Remove(project);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.ProjectId == id);
        }
    }
}
    