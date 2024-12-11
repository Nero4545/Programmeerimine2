using KooliProjekt.Models;
using KooliProjekt.Search;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Mvc;

namespace KooliProjekt.Controllers
{
    public class ManufacturingLogController : Controller
    {
        private readonly IManufacturingLogService _ManufacturingLogervice;

        public ManufacturingLogController(IManufacturingLogService ManufacturingLogervice)
        {
            _ManufacturingLogervice = ManufacturingLogervice;
        }

        // GET: ManufacturingLog
        public async Task<IActionResult> Index(ManufacturingLogSearchParameters searchParameters)
        {
            var items = await _ManufacturingLogervice.ListAsync(searchParameters);
            var model = new ManufacturingLogIndexModel
            {
                SearchParameters = searchParameters,
                Items = items
            };
            return View(model);
        }
    }
}
