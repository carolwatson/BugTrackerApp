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
    public class TicketPrioritiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TicketPrioritiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TicketPriorities
        public async Task<IActionResult> Index()
        {
            return View(await _context.TicketPriorities.ToListAsync());
        }

        // GET: TicketPriorities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketPriorityModel = await _context.TicketPriorities
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticketPriorityModel == null)
            {
                return NotFound();
            }

            return View(ticketPriorityModel);
        }

        // GET: TicketPriorities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TicketPriorities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] TicketPriority ticketPriorityModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ticketPriorityModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ticketPriorityModel);
        }

        // GET: TicketPriorities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketPriorityModel = await _context.TicketPriorities.FindAsync(id);
            if (ticketPriorityModel == null)
            {
                return NotFound();
            }
            return View(ticketPriorityModel);
        }

        // POST: TicketPriorities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] TicketPriority ticketPriorityModel)
        {
            if (id != ticketPriorityModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticketPriorityModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketPriorityExists(ticketPriorityModel.Id))
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
            return View(ticketPriorityModel);
        }

        // GET: TicketPriorities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketPriorityModel = await _context.TicketPriorities
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticketPriorityModel == null)
            {
                return NotFound();
            }

            return View(ticketPriorityModel);
        }

        // POST: TicketPriorities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticketPriorityModel = await _context.TicketPriorities.FindAsync(id);
            _context.TicketPriorities.Remove(ticketPriorityModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketPriorityExists(int id)
        {
            return _context.TicketPriorities.Any(e => e.Id == id);
        }
    }
}
