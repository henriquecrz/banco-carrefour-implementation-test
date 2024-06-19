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
public class CashFlowControllerTest
{
    [TestMethod]
    public async Task TestGetEntries()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;
        var dbContext = new ApplicationDbContext(options);
        dbContext.Database.EnsureCreated();

        var date = new DateTime(2023, 5, 13);
        var entriesMock = new List<Entry>
        {
            new() { Id = 1, Type = OperationType.Credit, Currency = Currency.USD, Value = 100, Description = "a", Date = date },
            new() { Id = 2, Type = OperationType.Debit, Currency = Currency.EUR, Value = 50, Description = "a", Date = date },
            new() { Id = 3, Type = OperationType.Credit, Currency = Currency.USD, Value = 200, Description = "a", Date = date },
            new() { Id = 4, Type = OperationType.Debit, Currency = Currency.EUR, Value = 75, Description = "a", Date = date },
            new() { Id = 5, Type = OperationType.Credit, Currency = Currency.USD, Value = 300, Description = "a", Date = date },
        };

        dbContext.Entry.AddRange(entriesMock);
        dbContext.SaveChanges();

        var loggerMock = new Mock<ILogger<CashFlowController>>();
        var cashFlowController = new CashFlowController(loggerMock.Object, dbContext);

        var result = await cashFlowController.GetEntries();

        var entries = result?.Value;

        Assert.IsNotNull(entries);
        Assert.AreEqual(5, entries.Count());

        dbContext.Database.EnsureDeleted();
        dbContext.Dispose();
    }

    [TestMethod]
    public async Task TestPostEntry()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;
        var dbContext = new ApplicationDbContext(options);
        dbContext.Database.EnsureCreated();

        var loggerMock = new Mock<ILogger<CashFlowController>>();
        var cashFlowController = new CashFlowController(loggerMock.Object, dbContext);

        var date = new DateTime(2023, 5, 13);
        var entryMock = new Entry()
        {
            Id = 999,
            Currency = Currency.USD,
            Date = date,
            Description = "999",
            Type = OperationType.Debit,
            Value = 10
        };

        var result = await cashFlowController.PostEntry(entryMock);

        var createdAtActionResult = result.Result as CreatedAtActionResult;
        var entry = createdAtActionResult?.Value as Entry;

        Assert.IsNotNull(entry);
        Assert.AreEqual(entryMock, entry);

        dbContext.Database.EnsureDeleted();
        dbContext.Dispose();
    }
}
