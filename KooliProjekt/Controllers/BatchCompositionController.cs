using KooliProjekt.Models;
using KooliProjekt.Search;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Mvc;

namespace KooliProjekt.Controllers
{
    public class BatchCompositionController : Controller
    {
        private readonly IBatchCompositionService _BatchCompositionervice;

        public BatchCompositionController(IBatchCompositionService BatchCompositionervice)
        {
            _BatchCompositionervice = BatchCompositionervice;
        }

        // GET: BatchComposition
        public async Task<IActionResult> Index(BatchCompositionSearchParameters searchParameters)
        {
            var items = await _BatchCompositionervice.ListAsync(searchParameters);
            var model = new BatchCompositionIndexModel
            {
                SearchParameters = searchParameters,
                Items = items
            };
            return View(model);
        }
    }
}
