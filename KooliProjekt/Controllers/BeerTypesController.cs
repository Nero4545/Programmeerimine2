using KooliProjekt.Data;
using KooliProjekt.Models;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KooliProjekt.Controllers
{
    [Authorize]
    public class BeerTypesController : Controller
    {
        private readonly IBeerTypeService _beerTypeService;

        public BeerTypesController(IBeerTypeService beerTypeService)
        {
            _beerTypeService = beerTypeService;
        }

        // GET: BeerTypes
        public async Task<IActionResult> Index(int page = 1, BeerTypesIndexModel? model = null)
        {
            model = model ?? new BeerTypesIndexModel();
            model.Data = await _beerTypeService.List(page, 5, model.Search);

            return View(model);
        }

        // GET: BeerTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var beerType = await _beerTypeService.Get(id.Value);
            if (beerType == null)
            {
                return NotFound();
            }

            return View(beerType);
        }

        // GET: BeerTypes/Create
        public IActionResult Create()
        {
            return View(new BeerType());
        }

        // POST: BeerTypes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] BeerType beerType)
        {
            if (ModelState.IsValid)
            {
                await _beerTypeService.Save(beerType);
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

            var beerType = await _beerTypeService.Get(id.Value);
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
                await _beerTypeService.Save(beerType);
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

            var beerType = await _beerTypeService.Get(id.Value);
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
            await _beerTypeService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
