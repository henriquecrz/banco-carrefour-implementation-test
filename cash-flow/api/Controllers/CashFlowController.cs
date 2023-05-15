using api.Data;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CashFlowController : ControllerBase
    {
        private readonly ILogger<CashFlowController> _logger;
        private readonly ApplicationDbContext _applicationDbContext;

        public CashFlowController(
            ILogger<CashFlowController> logger,
            ApplicationDbContext applicationDbContext)
        {
            _logger = logger;
            _applicationDbContext = applicationDbContext;
        }

        [HttpGet(Name = "GetEntries")]
        public async Task<ActionResult<IEnumerable<Entry>>> GetEntries()
        {
            var entries = await _applicationDbContext.Entry.ToListAsync();

            _logger.Log(LogLevel.Information, $"Data: {entries}");

            return entries;
        }

        [HttpPost(Name = "PostEntry")]
        public async Task<ActionResult<Entry>> PostEntry(Entry entry)
        {
            _applicationDbContext.Entry.Add(entry);

            await _applicationDbContext.SaveChangesAsync();

            _logger.Log(LogLevel.Information, $"Data: {entry}");

            return CreatedAtAction(nameof(GetEntries), new { id = entry.Id }, entry);
        }
    }
}
