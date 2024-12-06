using KooliProjekt.Models;
using KooliProjekt.Search;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Mvc;

namespace KooliProjekt.Controllers
{
    public class OilBatchesController : Controller
    {
        private readonly IOilBatchService _oilTypeService;

        public OilBatchesController(IOilTypeService oilTypeService)
        {
            _oilBatchService = oilBatchService;
        }

        // GET: OilBatch
        public async Task<IActionResult> Index(OilBatchSearchParameters searchParameters)
        {
            var items = await _oilBatchService.ListAsync(searchParameters);
            var model = new OilBatchesIndexModel
            {
                SearchParameters = searchParameters,
                Items = items
            };
            return View(model);
        }
    }
}
