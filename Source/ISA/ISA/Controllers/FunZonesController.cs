using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ISA.DataAccess.Context;
using ISA.DataAccess.Models;
using Microsoft.AspNetCore.Authorization;

namespace ISA.Controllers
{
    public class FunZonesController : Controller
    {
        private readonly ISAContext _context;

        public FunZonesController(ISAContext context)
        {
            _context = context;
        }

        // GET: FunZones
        public async Task<IActionResult> Index()
        {
            return View(await _context.FunZone
                .Include(i => i.Cinema)
                .Include(i => i.Theater)
                .Include(i=> i.ThematicProps)
                .ToListAsync());
        }

        // GET: FunZones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var funZone = await _context.FunZone
                .Include(i => i.Cinema)
                .Include(i => i.Theater)
                .Include(i => i.ThematicProps)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (funZone == null)
            {
                return NotFound();
            }

            return View(funZone);
        }

        // GET: FunZones/Create
        [Authorize(Roles = "FunZoneAdmin,SuperAdmin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: FunZones/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "FunZoneAdmin,SuperAdmin")]
        public async Task<IActionResult> Create([Bind("Id")] FunZone funZone)
        {
            if (ModelState.IsValid)
            {
                _context.Add(funZone);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(funZone);
        }

        // GET: FunZones/Edit/5
        [Authorize(Roles = "FunZoneAdmin,SuperAdmin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var funZone = await _context.FunZone.SingleOrDefaultAsync(m => m.Id == id);
            if (funZone == null)
            {
                return NotFound();
            }
            return View(funZone);
        }

        // POST: FunZones/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "FunZoneAdmin,SuperAdmin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id")] FunZone funZone)
        {
            if (id != funZone.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(funZone);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FunZoneExists(funZone.Id))
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
            return View(funZone);
        }

        // GET: FunZones/Delete/5
        [Authorize(Roles = "FunZoneAdmin,SuperAdmin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var funZone = await _context.FunZone
                .SingleOrDefaultAsync(m => m.Id == id);
            if (funZone == null)
            {
                return NotFound();
            }

            return View(funZone);
        }

        // POST: FunZones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "FunZoneAdmin,SuperAdmin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var funZone = await _context.FunZone.SingleOrDefaultAsync(m => m.Id == id);
            _context.FunZone.Remove(funZone);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FunZoneExists(int id)
        {
            return _context.FunZone.Any(e => e.Id == id);
        }
    }
}
