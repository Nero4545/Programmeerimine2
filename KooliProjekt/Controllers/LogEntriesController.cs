using System.Security.Claims;
using KooliProjekt.Data;
using KooliProjekt.Models;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KooliProjekt.Controllers
{
    [Authorize]
    public class LogEntriesController : Controller
    {
        private readonly ILogEntryService _logEntryService;
        private readonly IBatchService _batchService;

        public LogEntriesController(ILogEntryService logEntryService, IBatchService batchService)
        {
            _logEntryService = logEntryService;
            _batchService = batchService;
        }

        // GET: LogEntries
        public async Task<IActionResult> Index(int page = 1, LogEntriesIndexModel? model = null)
        {
            model = model ?? new LogEntriesIndexModel();
            model.Data = await _logEntryService.List(page, 5, model.Search);
            await FillLookups(null);

            return View(model);
        }

        // GET: LogEntries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var logEntry = await _logEntryService.Get(id.Value);
            if (logEntry == null)
            {
                return NotFound();
            }

            return View(logEntry);
        }

        // GET: LogEntries/Create
        public async Task<IActionResult> Create(int? batchId)
        {
            var logEntry = new LogEntry { Date = DateTime.Today };
            if (batchId.HasValue)
            {
                logEntry.BatchId = batchId.Value;
            }

            await FillLookups(logEntry.BatchId);
            return View(logEntry);
        }

        // POST: LogEntries/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BatchId,Date,Description,UserId")] LogEntry logEntry)
        {
            logEntry.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (ModelState.IsValid)
            {
                await _logEntryService.Save(logEntry);
                return RedirectToAction(nameof(Index));
            }

            await FillLookups(logEntry.BatchId);
            return View(logEntry);
        }

        // GET: LogEntries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var logEntry = await _logEntryService.Get(id.Value);
            if (logEntry == null)
            {
                return NotFound();
            }

            await FillLookups(logEntry.BatchId);
            return View(logEntry);
        }

        // POST: LogEntries/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BatchId,Date,Description,UserId")] LogEntry logEntry)
        {
            if (id != logEntry.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _logEntryService.Save(logEntry);
                return RedirectToAction(nameof(Index));
            }

            await FillLookups(logEntry.BatchId);
            return View(logEntry);
        }

        // GET: LogEntries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var logEntry = await _logEntryService.Get(id.Value);
            if (logEntry == null)
            {
                return NotFound();
            }

            return View(logEntry);
        }

        // POST: LogEntries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _logEntryService.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task FillLookups(int? selectedId)
        {
            ViewData["BatchId"] = new SelectList(await _batchService.Lookup(), "Id", "Code", selectedId);
        }
    }
}
