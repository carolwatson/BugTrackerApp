//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
//using BugTracker.Data;
//using BugTracker.Models;

//namespace BugTracker.Controllers
//{
//    // use index
//    public class InvitesController : Controller
//    {
//        private readonly ApplicationDbContext _context;

//        public InvitesController(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        // GET: Invites
//        public async Task<IActionResult> Index()
//        {
//            var applicationDbContext = _context.Invites.Include(i => i.Company).Include(i => i.Invitee).Include(i => i.Invitor).Include(i => i.Project);
//            return View(await applicationDbContext.ToListAsync());
//        }

//        // GET: Invites/Details/5
//        public async Task<IActionResult> Details(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var inviteModel = await _context.Invites
//                .Include(i => i.Company)
//                .Include(i => i.Invitee)
//                .Include(i => i.Invitor)
//                .Include(i => i.Project)
//                .FirstOrDefaultAsync(m => m.Id == id);
//            if (inviteModel == null)
//            {
//                return NotFound();
//            }

//            return View(inviteModel);
//        }

//        // GET: Invites/Create
//        public IActionResult Create()
//        {
//            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Id");
//            ViewData["InviteeId"] = new SelectList(_context.Users, "Id", "Id");
//            ViewData["InvitorId"] = new SelectList(_context.Users, "Id", "Id");
//            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Id");
//            return View();
//        }

//        // POST: Invites/Create
//        // To protect from overposting attacks, enable the specific properties you want to bind to.
//        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create([Bind("Id,InviteDate,JoinDate,CompanyToken,InviteeId,InvitorId,ProjectId,CompanyId,InviteeFirstName,InviteeLastName,InviteeEmail,IsValid")] InviteModel inviteModel)
//        {
//            if (ModelState.IsValid)
//            {
//                _context.Add(inviteModel);
//                await _context.SaveChangesAsync();
//                return RedirectToAction(nameof(Index));
//            }
//            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Id", inviteModel.CompanyId);
//            ViewData["InviteeId"] = new SelectList(_context.Users, "Id", "Id", inviteModel.InviteeId);
//            ViewData["InvitorId"] = new SelectList(_context.Users, "Id", "Id", inviteModel.InvitorId);
//            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Id", inviteModel.ProjectId);
//            return View(inviteModel);
//        }

//        // GET: Invites/Edit/5
//        public async Task<IActionResult> Edit(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var inviteModel = await _context.Invites.FindAsync(id);
//            if (inviteModel == null)
//            {
//                return NotFound();
//            }
//            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Id", inviteModel.CompanyId);
//            ViewData["InviteeId"] = new SelectList(_context.Users, "Id", "Id", inviteModel.InviteeId);
//            ViewData["InvitorId"] = new SelectList(_context.Users, "Id", "Id", inviteModel.InvitorId);
//            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Id", inviteModel.ProjectId);
//            return View(inviteModel);
//        }

//        // POST: Invites/Edit/5
//        // To protect from overposting attacks, enable the specific properties you want to bind to.
//        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit(int id, [Bind("Id,InviteDate,JoinDate,CompanyToken,InviteeId,InvitorId,ProjectId,CompanyId,InviteeFirstName,InviteeLastName,InviteeEmail,IsValid")] InviteModel inviteModel)
//        {
//            if (id != inviteModel.Id)
//            {
//                return NotFound();
//            }

//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    _context.Update(inviteModel);
//                    await _context.SaveChangesAsync();
//                }
//                catch (DbUpdateConcurrencyException)
//                {
//                    if (!InviteModelExists(inviteModel.Id))
//                    {
//                        return NotFound();
//                    }
//                    else
//                    {
//                        throw;
//                    }
//                }
//                return RedirectToAction(nameof(Index));
//            }
//            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Id", inviteModel.CompanyId);
//            ViewData["InviteeId"] = new SelectList(_context.Users, "Id", "Id", inviteModel.InviteeId);
//            ViewData["InvitorId"] = new SelectList(_context.Users, "Id", "Id", inviteModel.InvitorId);
//            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Id", inviteModel.ProjectId);
//            return View(inviteModel);
//        }

//        // GET: Invites/Delete/5
//        public async Task<IActionResult> Delete(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var inviteModel = await _context.Invites
//                .Include(i => i.Company)
//                .Include(i => i.Invitee)
//                .Include(i => i.Invitor)
//                .Include(i => i.Project)
//                .FirstOrDefaultAsync(m => m.Id == id);
//            if (inviteModel == null)
//            {
//                return NotFound();
//            }

//            return View(inviteModel);
//        }

//        // POST: Invites/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteConfirmed(int id)
//        {
//            var inviteModel = await _context.Invites.FindAsync(id);
//            _context.Invites.Remove(inviteModel);
//            await _context.SaveChangesAsync();
//            return RedirectToAction(nameof(Index));
//        }

//        private bool InviteModelExists(int id)
//        {
//            return _context.Invites.Any(e => e.Id == id);
//        }
//    }
//}
