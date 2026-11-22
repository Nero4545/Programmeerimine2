using System.Security.Claims;
using KooliProjekt.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Controllers
{
    [Authorize]
    public class LogEntriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LogEntriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: LogEntries
        public async Task<IActionResult> Index(int page = 1)
        {
            var data = await _context.LogEntries
                .Include(x => x.Batch).Include(x => x.User)
                .OrderByDescending(x => x.Date)
                .GetPagedAsync(page, 5);

            return View(data);
        }

        // GET: LogEntries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var logEntry = await _context.LogEntries
                .Include(x => x.Batch).Include(x => x.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (logEntry == null)
            {
                return NotFound();
            }

            return View(logEntry);
        }

        // GET: LogEntries/Create
        public IActionResult Create(int? batchId)
        {
            var logEntry = new LogEntry { Date = DateTime.Today };
            if (batchId.HasValue)
            {
                logEntry.BatchId = batchId.Value;
            }

            FillBatches(logEntry.BatchId);
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
                _context.Add(logEntry);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            FillBatches(logEntry.BatchId);
            return View(logEntry);
        }

        // GET: LogEntries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var logEntry = await _context.LogEntries.FindAsync(id);
            if (logEntry == null)
            {
                return NotFound();
            }
            FillBatches(logEntry.BatchId);
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
                _context.Update(logEntry);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            FillBatches(logEntry.BatchId);
            return View(logEntry);
        }

        // GET: LogEntries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var logEntry = await _context.LogEntries
                .Include(x => x.Batch).Include(x => x.User)
                .FirstOrDefaultAsync(m => m.Id == id);
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
            var logEntry = await _context.LogEntries.FindAsync(id);
            if (logEntry != null)
            {
                _context.LogEntries.Remove(logEntry);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private void FillBatches(int? selectedId)
        {
            ViewData["BatchId"] = new SelectList(_context.Batches.OrderBy(x => x.Code), "Id", "Code", selectedId);
        }
    }
}
