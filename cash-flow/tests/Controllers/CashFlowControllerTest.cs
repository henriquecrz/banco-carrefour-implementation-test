using api.Controllers;
using api.Data;
using api.Models;
using api.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace tests.Controllers
{
    [TestClass]
    public class CashFlowControllerTest
    {
        private CashFlowController _cashFlowController;
        private ApplicationDbContext _dbContext;

        [TestMethod]
        public async Task TestGetEntries()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
            _dbContext = new ApplicationDbContext(options);
            _dbContext.Database.EnsureCreated();

            var date = new DateTime(2023, 5, 13);
            var entriesMock = new List<Entry>
            {
                new Entry { Id = 1, Type = OperationType.Credit, Currency = Currency.USD, Value = 100, Description = "a", Date = date },
                new Entry { Id = 2, Type = OperationType.Debit, Currency = Currency.EUR, Value = 50, Description = "a", Date = date },
                new Entry { Id = 3, Type = OperationType.Credit, Currency = Currency.USD, Value = 200, Description = "a", Date = date },
                new Entry { Id = 4, Type = OperationType.Debit, Currency = Currency.EUR, Value = 75, Description = "a", Date = date },
                new Entry { Id = 5, Type = OperationType.Credit, Currency = Currency.USD, Value = 300, Description = "a", Date = date },
            };
            _dbContext.Entry.AddRange(entriesMock);
            _dbContext.SaveChanges();

            var loggerMock = new Mock<ILogger<CashFlowController>>();

            _cashFlowController = new CashFlowController(loggerMock.Object, _dbContext);

            var result = await _cashFlowController.GetEntries();

            var entries = result?.Value;

            Assert.IsNotNull(entries);
            Assert.AreEqual(5, entries.Count());

            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [TestMethod]
        public async Task TestPostEntry()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
            _dbContext = new ApplicationDbContext(options);
            _dbContext.Database.EnsureCreated();

            var date = new DateTime(2023, 5, 13);            

            var loggerMock = new Mock<ILogger<CashFlowController>>();

            _cashFlowController = new CashFlowController(loggerMock.Object, _dbContext);

            var entryMock = new Entry()
            {
                Id = 999,
                Currency = Currency.USD,
                Date = date,
                Description = "999",
                Type = OperationType.Debit,
                Value = 10
            };

            var result = await _cashFlowController.PostEntry(entryMock);

            var createdAtActionResult = result.Result as CreatedAtActionResult;
            var entry = createdAtActionResult?.Value as Entry;

            Assert.IsNotNull(entry);
            Assert.AreEqual(entryMock, entry);

            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }
    }
}
