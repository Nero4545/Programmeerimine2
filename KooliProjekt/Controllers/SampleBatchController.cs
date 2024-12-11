using KooliProjekt.Models;
using KooliProjekt.Search;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Mvc;

namespace KooliProjekt.Controllers
{
    public class SampleBatchController : Controller
    {
        private readonly ISampleBatchService _SampleBatchervice;

        public SampleBatchController(ISampleBatchService SampleBatchervice)
        {
            _SampleBatchervice = SampleBatchervice;
        }

        // GET: SampleBatch
        public async Task<IActionResult> Index(SampleBatchSearchParameters searchParameters)
        {
            var items = await _SampleBatchervice.ListAsync(searchParameters);
            var model = new SampleBatchIndexModel
            {
                SearchParameters = searchParameters,
                Items = items
            };
            return View(model);
        }
    }
}
