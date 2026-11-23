using KooliProjekt.Data;
using KooliProjekt.Models;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KooliProjekt.Controllers
{
    [Authorize]
    public class IngredientsController : Controller
    {
        private readonly IIngredientService _ingredientService;
        private readonly IBatchService _batchService;

        public IngredientsController(IIngredientService ingredientService, IBatchService batchService)
        {
            _ingredientService = ingredientService;
            _batchService = batchService;
        }

        // GET: Ingredients
        public async Task<IActionResult> Index(int page = 1, IngredientsIndexModel? model = null)
        {
            model = model ?? new IngredientsIndexModel();
            model.Data = await _ingredientService.List(page, 5, model.Search);
            await FillLookups(null);

            return View(model);
        }

        // GET: Ingredients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ingredient = await _ingredientService.Get(id.Value);
            if (ingredient == null)
            {
                return NotFound();
            }

            return View(ingredient);
        }

        // GET: Ingredients/Create
        public async Task<IActionResult> Create(int? batchId)
        {
            var ingredient = new Ingredient();
            if (batchId.HasValue)
            {
                ingredient.BatchId = batchId.Value;
            }

            await FillLookups(ingredient.BatchId);
            return View(ingredient);
        }

        // POST: Ingredients/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BatchId,Name,Unit,UnitPrice,Quantity")] Ingredient ingredient)
        {
            if (ModelState.IsValid)
            {
                await _ingredientService.Save(ingredient);
                return RedirectToAction(nameof(Index));
            }

            await FillLookups(ingredient.BatchId);
            return View(ingredient);
        }

        // GET: Ingredients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ingredient = await _ingredientService.Get(id.Value);
            if (ingredient == null)
            {
                return NotFound();
            }

            await FillLookups(ingredient.BatchId);
            return View(ingredient);
        }

        // POST: Ingredients/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BatchId,Name,Unit,UnitPrice,Quantity")] Ingredient ingredient)
        {
            if (id != ingredient.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _ingredientService.Save(ingredient);
                return RedirectToAction(nameof(Index));
            }

            await FillLookups(ingredient.BatchId);
            return View(ingredient);
        }

        // GET: Ingredients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ingredient = await _ingredientService.Get(id.Value);
            if (ingredient == null)
            {
                return NotFound();
            }

            return View(ingredient);
        }

        // POST: Ingredients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _ingredientService.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task FillLookups(int? selectedId)
        {
            ViewData["BatchId"] = new SelectList(await _batchService.Lookup(), "Id", "Code", selectedId);
        }
    }
}
