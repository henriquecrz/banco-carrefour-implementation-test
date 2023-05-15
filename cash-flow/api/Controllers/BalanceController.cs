using api.Data;
using api.Models;
using api.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BalanceController : ControllerBase
    {
        private readonly ILogger<BalanceController> _logger;
        private readonly ApplicationDbContext _applicationDbContext;

        public BalanceController(
            ILogger<BalanceController> logger,
            ApplicationDbContext applicationDbContext)
        {
            _logger = logger;
            _applicationDbContext = applicationDbContext;
        }

        [HttpGet(Name = "GetConsolidatedDailyBalance")]
        public async Task<ActionResult<IEnumerable<Balance>>> GetConsolidatedDailyBalance(DateTime date)
        {
            var entries = await _applicationDbContext.Entry.ToListAsync();

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

            _logger.Log(LogLevel.Information, $"Data: {consolidatedDailyBalance}");

            return Ok(consolidatedDailyBalance);
        }
    }
}
