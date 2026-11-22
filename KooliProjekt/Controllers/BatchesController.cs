using KooliProjekt.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Controllers
{
    [Authorize]
    public class BatchesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BatchesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Batches
        public async Task<IActionResult> Index(int page = 1)
        {
            var data = await _context.Batches
                .Include(b => b.BeerType)
                .OrderByDescending(b => b.BatchDate)
                .GetPagedAsync(page, 5);

            return View(data);
        }

        // GET: Batches/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var batch = await _context.Batches
                .Include(b => b.BeerType)
                .Include(b => b.Ingredients)
                .Include(b => b.LogEntries).ThenInclude(l => l.User)
                .Include(b => b.TastingEntries).ThenInclude(t => t.User)
                .Include(b => b.Photos)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (batch == null)
            {
                return NotFound();
            }

            return View(batch);
        }

        // GET: Batches/Create
        public IActionResult Create()
        {
            FillBeerTypes(null);
            return View(new Batch { BatchDate = DateTime.Today });
        }

        // POST: Batches/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BeerTypeId,BatchDate,Code,Description,Summary")] Batch batch)
        {
            if (ModelState.IsValid)
            {
                _context.Add(batch);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            FillBeerTypes(batch.BeerTypeId);
            return View(batch);
        }

        // GET: Batches/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var batch = await _context.Batches.FindAsync(id);
            if (batch == null)
            {
                return NotFound();
            }
            FillBeerTypes(batch.BeerTypeId);
            return View(batch);
        }

        // POST: Batches/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BeerTypeId,BatchDate,Code,Description,Summary")] Batch batch)
        {
            if (id != batch.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(batch);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            FillBeerTypes(batch.BeerTypeId);
            return View(batch);
        }

        // GET: Batches/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var batch = await _context.Batches
                .Include(b => b.BeerType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (batch == null)
            {
                return NotFound();
            }

            return View(batch);
        }

        // POST: Batches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var batch = await _context.Batches.FindAsync(id);
            if (batch != null)
            {
                _context.Batches.Remove(batch);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private void FillBeerTypes(int? selectedId)
        {
            ViewData["BeerTypeId"] = new SelectList(_context.BeerTypes.OrderBy(x => x.Name), "Id", "Name", selectedId);
        }
    }
}
