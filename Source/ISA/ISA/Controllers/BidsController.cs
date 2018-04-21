using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ISA.DataAccess.Context;
using ISA.DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using ISA.Models;

namespace ISA.Controllers
{
    public class BidsController : Controller
    {
        private readonly ISAContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BidsController(ISAContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Bids
        public async Task<IActionResult> Index()
        {
            var appUser = await _userManager.GetUserAsync(HttpContext.User);
            var userId = _context.UserProfiles.SingleOrDefault(x => x.Id == appUser.UserProfileId).Id;
            var bids = _context.Bids.Include(b => b.ThematicProp).ToList();
            bids = bids.Where(b => b.User.Id == userId).ToList();
            return View(bids);
        }

        // GET: Bids/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bid = await _context.Bids
                .SingleOrDefaultAsync(m => m.Id == id);
            if (bid == null)
            {
                return NotFound();
            }

            return View(bid);
        }

        // GET: Bids/Create
        public async Task<IActionResult> Create(int propId)
        {
            var prop = _context.ThematicProps.SingleOrDefault(x => x.Id == propId);
            var bid = new Bid()
            {
                ThematicProp = prop,
                ThematicPropId = prop.Id
            };

            return View(bid);
        }

        // POST: Bids/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Price,Id")] Bid bid, int thematicPropId)
        {
            if (ModelState.IsValid)
            {
                var appUser = await _userManager.GetUserAsync(HttpContext.User);
                var prop = _context.ThematicProps.SingleOrDefault(x => x.Id == thematicPropId);
                bid.ThematicPropId = prop.Id;
                bid.User = _context.UserProfiles.SingleOrDefault(x => x.Id == appUser.UserProfileId);
                if (prop.Price < bid.Price)
                {
                    prop.Price = bid.Price;
                }

                _context.Add(bid);
                _context.Update(prop);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(bid);
        }

        // GET: Bids/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bid = await _context.Bids.SingleOrDefaultAsync(m => m.Id == id);
            if (bid == null)
            {
                return NotFound();
            }
            return View(bid);
        }

        // POST: Bids/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Price,Id")] Bid bid, int propId)
        {
            if (id != bid.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var prop = await _context.ThematicProps.SingleOrDefaultAsync(m => m.Id == propId);
                    bid.ThematicPropId = prop.Id;
                    _context.Entry(bid).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    UpdateThematicProp(prop);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BidExists(bid.Id))
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
            return View(bid);
        }

        // GET: Bids/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bid = await _context.Bids
                .SingleOrDefaultAsync(m => m.Id == id);
            if (bid == null)
            {
                return NotFound();
            }

            return View(bid);
        }

        // POST: Bids/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bid = await _context.Bids.SingleOrDefaultAsync(m => m.Id == id);
            var prop = await _context.ThematicProps.SingleOrDefaultAsync(m => m.Id == bid.ThematicPropId);
            _context.Bids.Remove(bid);
            UpdateThematicProp(prop);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BidExists(int id)
        {
            return _context.Bids.Any(e => e.Id == id);
        }

        private void UpdateThematicProp(ThematicProps prop)
        {
            var bids = _context.Bids.ToList();
            prop.Price = bids.Where(b => b.ThematicPropId == prop.Id).Max(b => b.Price);
            _context.Update(prop);
        }
    }
}
