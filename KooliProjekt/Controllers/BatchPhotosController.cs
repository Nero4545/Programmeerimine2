using KooliProjekt.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Controllers
{
    [Authorize]
    public class BatchPhotosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BatchPhotosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BatchPhotos
        public async Task<IActionResult> Index(int page = 1)
        {
            var data = await _context.BatchPhotos
                .Include(x => x.Batch)
                .OrderBy(x => x.Batch!.Code)
                .GetPagedAsync(page, 5);

            return View(data);
        }

        // GET: BatchPhotos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var batchPhoto = await _context.BatchPhotos
                .Include(x => x.Batch)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (batchPhoto == null)
            {
                return NotFound();
            }

            return View(batchPhoto);
        }

        // GET: BatchPhotos/Create
        public IActionResult Create(int? batchId)
        {
            var batchPhoto = new BatchPhoto();
            if (batchId.HasValue)
            {
                batchPhoto.BatchId = batchId.Value;
            }

            FillBatches(batchPhoto.BatchId);
            return View(batchPhoto);
        }

        // POST: BatchPhotos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BatchId,Url,Description")] BatchPhoto batchPhoto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(batchPhoto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            FillBatches(batchPhoto.BatchId);
            return View(batchPhoto);
        }

        // GET: BatchPhotos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var batchPhoto = await _context.BatchPhotos.FindAsync(id);
            if (batchPhoto == null)
            {
                return NotFound();
            }
            FillBatches(batchPhoto.BatchId);
            return View(batchPhoto);
        }

        // POST: BatchPhotos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BatchId,Url,Description")] BatchPhoto batchPhoto)
        {
            if (id != batchPhoto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(batchPhoto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            FillBatches(batchPhoto.BatchId);
            return View(batchPhoto);
        }

        // GET: BatchPhotos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var batchPhoto = await _context.BatchPhotos
                .Include(x => x.Batch)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (batchPhoto == null)
            {
                return NotFound();
            }

            return View(batchPhoto);
        }

        // POST: BatchPhotos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var batchPhoto = await _context.BatchPhotos.FindAsync(id);
            if (batchPhoto != null)
            {
                _context.BatchPhotos.Remove(batchPhoto);
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
