using api.Data;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers;

[ApiController]
[Route("[controller]")]
public class CashFlowController(
    ILogger<CashFlowController> logger,
    ApplicationDbContext applicationDbContext) : ControllerBase
{
    [HttpGet(Name = "GetEntries")]
    public async Task<ActionResult<IEnumerable<Entry>>> GetEntries()
    {
        var entries = await applicationDbContext.Entry.ToListAsync();

        logger.LogInformation("Data: {Entries}", entries);

        return entries;
    }

    [HttpPost(Name = "PostEntry")]
    public async Task<ActionResult<Entry>> PostEntry(Entry entry)
    {
        applicationDbContext.Entry.Add(entry);

        await applicationDbContext.SaveChangesAsync();

        logger.LogInformation("Data: {Entry}", entry);

        return CreatedAtAction(nameof(GetEntries), new { id = entry.Id }, entry);
    }
}
