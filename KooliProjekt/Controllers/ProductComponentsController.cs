using KooliProjekt.Models;
using KooliProjekt.Search;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Mvc;

namespace KooliProjekt.Controllers
{
    public class ProductComponentsController : Controller
    {
        private readonly IProductComponentService _ProductComponentservice;

        public ProductComponentsController(IProductComponentService ProductComponentservice)
        {
            _ProductComponentservice = ProductComponentservice;
        }

        
        public async Task<IActionResult> Index(ProductComponentSearchParameters searchParameters)
        {
            var items = await _ProductComponentservice.ListAsync(searchParameters);
            var model = new ProductComponentsIndexModel
            {
                SearchParameters = searchParameters,
                Items = items
            };
            return View(model);
        }
    }
}
