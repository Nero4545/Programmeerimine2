using KooliProjekt.Data;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Mvc;

namespace KooliProjekt.Controllers
{
    [Route("api/BeerTypes")]
    [ApiController]
    public class BeerTypesApiController : ControllerBase
    {
        private readonly IBeerTypeService _service;

        public BeerTypesApiController(IBeerTypeService service)
        {
            _service = service;
        }

        // GET: api/BeerTypes
        [HttpGet]
        public async Task<IEnumerable<BeerType>> Get()
        {
            var result = await _service.List(1, 10000);
            return result.Results;
        }

        // GET api/BeerTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BeerType>> Get(int id)
        {
            var item = await _service.Get(id);
            if (item == null)
            {
                return NotFound();
            }

            return item;
        }

        // POST api/BeerTypes
        [HttpPost]
        public async Task<ActionResult<BeerType>> Post([FromBody] BeerType item)
        {
            item.Id = 0;
            await _service.Save(item);

            return Ok(item);
        }

        // PUT api/BeerTypes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] BeerType item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }

            var existing = await _service.Get(id);
            if (existing == null)
            {
                return NotFound();
            }

            existing.Name = item.Name;
            existing.Description = item.Description;
            await _service.Save(existing);

            return Ok();
        }

        // DELETE api/BeerTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _service.Get(id);
            if (existing == null)
            {
                return NotFound();
            }

            await _service.Delete(id);

            return Ok();
        }
    }
}
