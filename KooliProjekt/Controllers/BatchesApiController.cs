using KooliProjekt.Data;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Mvc;

namespace KooliProjekt.Controllers
{
    [Route("api/Batches")]
    [ApiController]
    public class BatchesApiController : ControllerBase
    {
        private readonly IBatchService _service;

        public BatchesApiController(IBatchService service)
        {
            _service = service;
        }

        // GET: api/Batches
        [HttpGet]
        public async Task<IEnumerable<Batch>> Get()
        {
            var result = await _service.List(1, 10000);
            return result.Results;
        }

        // GET api/Batches/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Batch>> Get(int id)
        {
            var item = await _service.Get(id);
            if (item == null)
            {
                return NotFound();
            }

            return item;
        }

        // POST api/Batches
        [HttpPost]
        public async Task<ActionResult<Batch>> Post([FromBody] Batch item)
        {
            item.Id = 0;
            await _service.Save(item);

            return Ok(item);
        }

        // PUT api/Batches/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Batch item)
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

            existing.BeerTypeId = item.BeerTypeId;
            existing.BatchDate = item.BatchDate;
            existing.Code = item.Code;
            existing.Description = item.Description;
            existing.Summary = item.Summary;
            await _service.Save(existing);

            return Ok();
        }

        // DELETE api/Batches/5
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
