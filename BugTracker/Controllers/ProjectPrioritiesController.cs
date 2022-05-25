using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BugTracker.Data;
using BugTracker.Models;

namespace BugTracker.Controllers
{
    public class ProjectPrioritiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjectPrioritiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ProjectPriorities
        public async Task<IActionResult> Index()
        {
            return View(await _context.ProjectPriorities.ToListAsync());
        }

        // GET: ProjectPriorities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectPriorityModel = await _context.ProjectPriorities
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projectPriorityModel == null)
            {
                return NotFound();
            }

            return View(projectPriorityModel);
        }

        // GET: ProjectPriorities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProjectPriorities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] ProjectPriority projectPriorityModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(projectPriorityModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(projectPriorityModel);
        }

        // GET: ProjectPriorities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectPriorityModel = await _context.ProjectPriorities.FindAsync(id);
            if (projectPriorityModel == null)
            {
                return NotFound();
            }
            return View(projectPriorityModel);
        }

        // POST: ProjectPriorities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] ProjectPriority projectPriorityModel)
        {
            if (id != projectPriorityModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(projectPriorityModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectPriorityExists(projectPriorityModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(projectPriorityModel);
        }

        // GET: ProjectPriorities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectPriorityModel = await _context.ProjectPriorities
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projectPriorityModel == null)
            {
                return NotFound();
            }

            return View(projectPriorityModel);
        }

        // POST: ProjectPriorities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var projectPriorityModel = await _context.ProjectPriorities.FindAsync(id);
            _context.ProjectPriorities.Remove(projectPriorityModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectPriorityExists(int id)
        {
            return _context.ProjectPriorities.Any(e => e.Id == id);
        }
    }
}
