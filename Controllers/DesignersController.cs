using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CreativeDesk.Data;
using CreativeDesk.Models;

namespace CreativeDesk.Controllers
{
    public class DesignersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DesignersController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var designers = await _context.Designers
                .Include(d => d.ProjectDesigns)
                .ThenInclude(pd => pd.Project)
                .ToListAsync();

            return View(designers);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var designer = await _context.Designers
                .Include(d => d.ProjectDesigns)
                .ThenInclude(pd => pd.Project)
                .FirstOrDefaultAsync(m => m.DesignerId == id);

            if (designer == null) return NotFound();

            return View(designer);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DesignerId,Name,AvailabilityStatus")] Designer designer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(designer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(designer);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var designer = await _context.Designers.FindAsync(id);
            if (designer == null) return NotFound();

            return View(designer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DesignerId,Name,AvailabilityStatus")] Designer designer)
        {
            if (id != designer.DesignerId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(designer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DesignerExists(designer.DesignerId)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(designer);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var designer = await _context.Designers
                .Include(d => d.ProjectDesigns)
                .ThenInclude(pd => pd.Project)
                .FirstOrDefaultAsync(m => m.DesignerId == id);

            if (designer == null) return NotFound();

            return View(designer);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var designer = await _context.Designers.FindAsync(id);
            if (designer != null)
            {
                _context.Designers.Remove(designer);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool DesignerExists(int id)
        {
            return _context.Designers.Any(e => e.DesignerId == id);
        }
    }
}
