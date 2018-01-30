using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ISA.DataAccess.Context;
using ISA.DataAccess.Models;

namespace ISA.Controllers
{
    public class RepertoiresController : Controller
    {
        private readonly ISAContext _context;

        public RepertoiresController(ISAContext context)
        {
            _context = context;
        }

        // GET: Repertoires
        public async Task<IActionResult> Index()
        {
            return View(await _context.Repertoires.ToListAsync());
        }

        // GET: Repertoires/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var repertoire = await _context.Repertoires
                .SingleOrDefaultAsync(m => m.Id == id);
            if (repertoire == null)
            {
                return NotFound();
            }

            return View(repertoire);
        }

        // GET: Repertoires/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Repertoires/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StartDate,EndDate,Id")] Repertoire repertoire)
        {
            if (ModelState.IsValid)
            {
                _context.Add(repertoire);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(repertoire);
        }

        // GET: Repertoires/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var repertoire = await _context.Repertoires.SingleOrDefaultAsync(m => m.Id == id);
            if (repertoire == null)
            {
                return NotFound();
            }
            return View(repertoire);
        }

        // POST: Repertoires/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StartDate,EndDate,Id")] Repertoire repertoire)
        {
            if (id != repertoire.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(repertoire);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RepertoireExists(repertoire.Id))
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
            return View(repertoire);
        }

        // GET: Repertoires/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var repertoire = await _context.Repertoires
                .SingleOrDefaultAsync(m => m.Id == id);
            if (repertoire == null)
            {
                return NotFound();
            }

            return View(repertoire);
        }

        // POST: Repertoires/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var repertoire = await _context.Repertoires.SingleOrDefaultAsync(m => m.Id == id);
            _context.Repertoires.Remove(repertoire);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RepertoireExists(int id)
        {
            return _context.Repertoires.Any(e => e.Id == id);
        }
    }
}
