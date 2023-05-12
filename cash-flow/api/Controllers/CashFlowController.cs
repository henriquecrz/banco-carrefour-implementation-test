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
        private readonly ApplicationDbContext _applicationDbContext;

        private readonly ILogger<CashFlowController> _logger;

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
            return await _applicationDbContext.Entry.ToListAsync();
        }

        [HttpPost(Name = "PostEntry")]
        public async Task<ActionResult<Entry>> PostEntry(Entry entry)
        {
            _applicationDbContext.Entry.Add(entry);

            await _applicationDbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEntries), new { id = entry.Id }, entry);
        }
    }
}
