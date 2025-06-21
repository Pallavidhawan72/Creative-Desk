using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CreativeDesk.Data;
using CreativeDesk.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CreativeDesk.Controllers
{
    public class UpcomingProjectsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public UpcomingProjectsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var nextMonth = DateTime.Now.AddMonths(1);
            var projects = await _context.Projects
                .Include(p => p.Client)
                .Include(p => p.ProjectDesigners)
                    .ThenInclude(pd => pd.Designer)
                .Where(p => p.Deadline >= DateTime.Now && p.Deadline <= nextMonth)
                .ToListAsync();
            return View(projects);
        }
    }
} 