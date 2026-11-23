using System.Security.Claims;
using KooliProjekt.Data;
using KooliProjekt.Models;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KooliProjekt.Controllers
{
    [Authorize]
    public class TastingEntriesController : Controller
    {
        private readonly ITastingEntryService _tastingEntryService;
        private readonly IBatchService _batchService;

        public TastingEntriesController(ITastingEntryService tastingEntryService, IBatchService batchService)
        {
            _tastingEntryService = tastingEntryService;
            _batchService = batchService;
        }

        // GET: TastingEntries
        public async Task<IActionResult> Index(int page = 1, TastingEntriesIndexModel? model = null)
        {
            model = model ?? new TastingEntriesIndexModel();
            model.Data = await _tastingEntryService.List(page, 5, model.Search);
            await FillLookups(null);

            return View(model);
        }

        // GET: TastingEntries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tastingEntry = await _tastingEntryService.Get(id.Value);
            if (tastingEntry == null)
            {
                return NotFound();
            }

            return View(tastingEntry);
        }

        // GET: TastingEntries/Create
        public async Task<IActionResult> Create(int? batchId)
        {
            var tastingEntry = new TastingEntry { Date = DateTime.Today };
            if (batchId.HasValue)
            {
                tastingEntry.BatchId = batchId.Value;
            }

            await FillLookups(tastingEntry.BatchId);
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
                await _tastingEntryService.Save(tastingEntry);
                return RedirectToAction(nameof(Index));
            }

            await FillLookups(tastingEntry.BatchId);
            return View(tastingEntry);
        }

        // GET: TastingEntries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tastingEntry = await _tastingEntryService.Get(id.Value);
            if (tastingEntry == null)
            {
                return NotFound();
            }

            await FillLookups(tastingEntry.BatchId);
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
                await _tastingEntryService.Save(tastingEntry);
                return RedirectToAction(nameof(Index));
            }

            await FillLookups(tastingEntry.BatchId);
            return View(tastingEntry);
        }

        // GET: TastingEntries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tastingEntry = await _tastingEntryService.Get(id.Value);
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
            await _tastingEntryService.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task FillLookups(int? selectedId)
        {
            ViewData["BatchId"] = new SelectList(await _batchService.Lookup(), "Id", "Code", selectedId);
        }
    }
}
