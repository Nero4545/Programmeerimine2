using KooliProjekt.Data;
using KooliProjekt.Models;
using KooliProjekt.Search;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Mvc;

namespace KooliProjekt.Controllers
{
    public class OilBatchesController : Controller
    {
        private readonly IOilBatchService _oilBatchService;

        public OilBatchesController(IOilBatchService oilBatchService)
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

    internal class OilBatchesIndexModel
    {
        public OilBatchSearchParameters SearchParameters { get; set; }
        public List<OilBatch> Items { get; set; }
    }
}
