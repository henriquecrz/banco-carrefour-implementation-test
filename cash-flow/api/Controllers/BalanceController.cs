using api.Data;
using api.Models;
using api.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers;

[ApiController]
[Route("[controller]")]
public class BalanceController(
    ILogger<BalanceController> logger,
    ApplicationDbContext applicationDbContext) : ControllerBase
{
    [HttpGet(Name = "GetConsolidatedDailyBalance")]
    public async Task<ActionResult<IEnumerable<Balance>>> GetConsolidatedDailyBalance(DateTime date)
    {
        var entries = await applicationDbContext.Entry.ToListAsync();

        var dailyEntries = entries.Where(entry =>
        {
            return entry.Date.Date.Equals(date.Date);
        });

        var groupedCurrency = dailyEntries.GroupBy(entry => entry.Currency);

        var consolidatedDailyBalance = groupedCurrency
            .Select(group =>
            {
                var value = group.Aggregate((decimal)0, (acc, entry) =>
                {
                    return entry.Type is OperationType.Credit ?
                        acc + entry.Value :
                        acc - entry.Value;
                });

                return new Balance()
                {
                    Currency = group.Key,
                    Value = value
                };
            });

        logger.LogInformation("Data: {ConsolidatedDailyBalance}", consolidatedDailyBalance);

        return Ok(consolidatedDailyBalance);
    }
}
