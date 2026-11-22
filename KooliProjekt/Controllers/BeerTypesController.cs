using KooliProjekt.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Controllers
{
    [Authorize]
    public class BeerTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BeerTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BeerTypes
        public async Task<IActionResult> Index(int page = 1)
        {
            var data = await _context.BeerTypes
                .OrderBy(x => x.Name)
                .GetPagedAsync(page, 5);

            return View(data);
        }

        // GET: BeerTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var beerType = await _context.BeerTypes
                .Include(x => x.Batches)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (beerType == null)
            {
                return NotFound();
            }

            return View(beerType);
        }

        // GET: BeerTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BeerTypes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] BeerType beerType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(beerType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(beerType);
        }

        // GET: BeerTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var beerType = await _context.BeerTypes.FindAsync(id);
            if (beerType == null)
            {
                return NotFound();
            }
            return View(beerType);
        }

        // POST: BeerTypes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] BeerType beerType)
        {
            if (id != beerType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(beerType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(beerType);
        }

        // GET: BeerTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var beerType = await _context.BeerTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (beerType == null)
            {
                return NotFound();
            }

            return View(beerType);
        }

        // POST: BeerTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var beerType = await _context.BeerTypes.FindAsync(id);
            if (beerType != null)
            {
                _context.BeerTypes.Remove(beerType);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
