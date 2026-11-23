using KooliProjekt.Data;
using KooliProjekt.Models;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KooliProjekt.Controllers
{
    [Authorize]
    public class BatchesController : Controller
    {
        private readonly IBatchService _batchService;
        private readonly IBeerTypeService _beerTypeService;

        public BatchesController(IBatchService batchService, IBeerTypeService beerTypeService)
        {
            _batchService = batchService;
            _beerTypeService = beerTypeService;
        }

        // GET: Batches
        public async Task<IActionResult> Index(int page = 1, BatchesIndexModel? model = null)
        {
            model = model ?? new BatchesIndexModel();
            model.Data = await _batchService.List(page, 5, model.Search);
            await FillLookups(null);

            return View(model);
        }

        // GET: Batches/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var batch = await _batchService.Get(id.Value);
            if (batch == null)
            {
                return NotFound();
            }

            return View(batch);
        }

        // GET: Batches/Create
        public async Task<IActionResult> Create()
        {
            var batch = new Batch { BatchDate = DateTime.Today };
            await FillLookups(null);
            return View(batch);
        }

        // POST: Batches/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BeerTypeId,BatchDate,Code,Description,Summary")] Batch batch)
        {
            if (ModelState.IsValid)
            {
                await _batchService.Save(batch);
                return RedirectToAction(nameof(Index));
            }

            await FillLookups(batch.BeerTypeId);
            return View(batch);
        }

        // GET: Batches/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var batch = await _batchService.Get(id.Value);
            if (batch == null)
            {
                return NotFound();
            }

            await FillLookups(batch.BeerTypeId);
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
                await _batchService.Save(batch);
                return RedirectToAction(nameof(Index));
            }

            await FillLookups(batch.BeerTypeId);
            return View(batch);
        }

        // GET: Batches/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var batch = await _batchService.Get(id.Value);
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
            await _batchService.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task FillLookups(int? selectedId)
        {
            ViewData["BeerTypeId"] = new SelectList(await _beerTypeService.Lookup(), "Id", "Name", selectedId);
        }
    }
}
