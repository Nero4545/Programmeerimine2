using KooliProjekt.Data;
using KooliProjekt.Models;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KooliProjekt.Controllers
{
    [Authorize]
    public class BatchPhotosController : Controller
    {
        private readonly IBatchPhotoService _batchPhotoService;
        private readonly IBatchService _batchService;

        public BatchPhotosController(IBatchPhotoService batchPhotoService, IBatchService batchService)
        {
            _batchPhotoService = batchPhotoService;
            _batchService = batchService;
        }

        // GET: BatchPhotos
        public async Task<IActionResult> Index(int page = 1, BatchPhotosIndexModel? model = null)
        {
            model = model ?? new BatchPhotosIndexModel();
            model.Data = await _batchPhotoService.List(page, 5, model.Search);
            await FillLookups(null);

            return View(model);
        }

        // GET: BatchPhotos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var batchPhoto = await _batchPhotoService.Get(id.Value);
            if (batchPhoto == null)
            {
                return NotFound();
            }

            return View(batchPhoto);
        }

        // GET: BatchPhotos/Create
        public async Task<IActionResult> Create(int? batchId)
        {
            var batchPhoto = new BatchPhoto();
            if (batchId.HasValue)
            {
                batchPhoto.BatchId = batchId.Value;
            }

            await FillLookups(batchPhoto.BatchId);
            return View(batchPhoto);
        }

        // POST: BatchPhotos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BatchId,Url,Description")] BatchPhoto batchPhoto)
        {
            if (ModelState.IsValid)
            {
                await _batchPhotoService.Save(batchPhoto);
                return RedirectToAction(nameof(Index));
            }

            await FillLookups(batchPhoto.BatchId);
            return View(batchPhoto);
        }

        // GET: BatchPhotos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var batchPhoto = await _batchPhotoService.Get(id.Value);
            if (batchPhoto == null)
            {
                return NotFound();
            }

            await FillLookups(batchPhoto.BatchId);
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
                await _batchPhotoService.Save(batchPhoto);
                return RedirectToAction(nameof(Index));
            }

            await FillLookups(batchPhoto.BatchId);
            return View(batchPhoto);
        }

        // GET: BatchPhotos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var batchPhoto = await _batchPhotoService.Get(id.Value);
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
            await _batchPhotoService.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task FillLookups(int? selectedId)
        {
            ViewData["BatchId"] = new SelectList(await _batchService.Lookup(), "Id", "Code", selectedId);
        }
    }
}
