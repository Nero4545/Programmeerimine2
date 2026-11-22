using System.Security.Claims;
using KooliProjekt.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Controllers
{
    [Authorize]
    public class TastingEntriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TastingEntriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TastingEntries
        public async Task<IActionResult> Index(int page = 1)
        {
            var data = await _context.TastingEntries
                .Include(x => x.Batch).Include(x => x.User)
                .OrderByDescending(x => x.Date)
                .GetPagedAsync(page, 5);

            return View(data);
        }

        // GET: TastingEntries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tastingEntry = await _context.TastingEntries
                .Include(x => x.Batch).Include(x => x.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tastingEntry == null)
            {
                return NotFound();
            }

            return View(tastingEntry);
        }

        // GET: TastingEntries/Create
        public IActionResult Create(int? batchId)
        {
            var tastingEntry = new TastingEntry { Date = DateTime.Today };
            if (batchId.HasValue)
            {
                tastingEntry.BatchId = batchId.Value;
            }

            FillBatches(tastingEntry.BatchId);
            return View(tastingEntry);
        }

        // POST: TastingEntries/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BatchId,Date,UserId,Rating,Comment")] TastingEntry tastingEntry)
        {
            tastingEntry.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (ModelState.IsValid)
            {
                _context.Add(tastingEntry);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            FillBatches(tastingEntry.BatchId);
            return View(tastingEntry);
        }

        // GET: TastingEntries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tastingEntry = await _context.TastingEntries.FindAsync(id);
            if (tastingEntry == null)
            {
                return NotFound();
            }
            FillBatches(tastingEntry.BatchId);
            return View(tastingEntry);
        }

        // POST: TastingEntries/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BatchId,Date,UserId,Rating,Comment")] TastingEntry tastingEntry)
        {
            if (id != tastingEntry.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(tastingEntry);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            FillBatches(tastingEntry.BatchId);
            return View(tastingEntry);
        }

        // GET: TastingEntries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tastingEntry = await _context.TastingEntries
                .Include(x => x.Batch).Include(x => x.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tastingEntry == null)
            {
                return NotFound();
            }

            return View(tastingEntry);
        }

        // POST: TastingEntries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tastingEntry = await _context.TastingEntries.FindAsync(id);
            if (tastingEntry != null)
            {
                _context.TastingEntries.Remove(tastingEntry);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private void FillBatches(int? selectedId)
        {
            ViewData["BatchId"] = new SelectList(_context.Batches.OrderBy(x => x.Code), "Id", "Code", selectedId);
        }
    }
}
