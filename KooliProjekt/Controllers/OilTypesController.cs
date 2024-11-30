using KooliProjekt.Models;
using KooliProjekt.Search;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Mvc;

namespace KooliProjekt.Controllers
{
    public class OilTypesController : Controller
    {
        private readonly IOilTypeService _oilTypeService;

        public OilTypesController(IOilTypeService oilTypeService)
        {
            _oilTypeService = oilTypeService;
        }

        // GET: OilTypes
        public async Task<IActionResult> Index(OilTypeSearchParameters searchParameters)
        {
            var items = await _oilTypeService.ListAsync(searchParameters);
            var model = new OilTypesIndexModel
            {
                SearchParameters = searchParameters,
                Items = items
            };
            return View(model);
        }
    }
}
