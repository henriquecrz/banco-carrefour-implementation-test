using api.Controllers;
using api.Data;
using api.Models;
using api.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace tests.Controllers;

[TestClass]
public class BalanceControllerTest
{
    [TestMethod]
    public async Task TestGetConsolidatedDailyBalance()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;
        var dbContext = new ApplicationDbContext(options);
        dbContext.Database.EnsureCreated();

        var date = new DateTime(2023, 5, 13);
        var entries = new List<Entry>
        {
            new() { Id = 1, Type = OperationType.Credit, Currency = Currency.USD, Value = 100, Description = "a", Date = date },
            new() { Id = 2, Type = OperationType.Debit, Currency = Currency.EUR, Value = 50, Description = "a", Date = date },
            new() { Id = 3, Type = OperationType.Credit, Currency = Currency.USD, Value = 200, Description = "a", Date = date },
            new() { Id = 4, Type = OperationType.Debit, Currency = Currency.EUR, Value = 75, Description = "a", Date = date },
            new() { Id = 5, Type = OperationType.Credit, Currency = Currency.USD, Value = 300, Description = "a", Date = date },
        };
        dbContext.Entry.AddRange(entries);
        dbContext.SaveChanges();

        var loggerMock = new Mock<ILogger<BalanceController>>();

        var balanceController = new BalanceController(loggerMock.Object, dbContext);

        var result = await balanceController.GetConsolidatedDailyBalance(date);

        var okResult = result.Result as OkObjectResult;
        var consolidatedBalance = okResult?.Value as IEnumerable<Balance>;

        Assert.IsNotNull(consolidatedBalance);
        Assert.AreEqual(2, consolidatedBalance.Count());
        Assert.AreEqual(Currency.USD, consolidatedBalance.First().Currency);
        Assert.AreEqual(600, consolidatedBalance.First().Value);
        Assert.AreEqual(Currency.EUR, consolidatedBalance.Last().Currency);
        Assert.AreEqual(-125, consolidatedBalance.Last().Value);

        dbContext.Database.EnsureDeleted();
        dbContext.Dispose();
    }
}
