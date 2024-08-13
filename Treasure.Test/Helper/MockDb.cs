using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using Treasure.Data.Entities;

namespace Treasure.Test.Helper;

public class MockDbContext
{
    public static TreasureContext CreateInMemoryDbContext(string dbName = "")
    {
        var options = new DbContextOptionsBuilder<TreasureContext>()
                        .UseInMemoryDatabase(databaseName: dbName == "" ? Guid.NewGuid().ToString() : dbName)
                        .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                        .EnableSensitiveDataLogging()
                        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                        .Options;
        return new TreasureContext(options);
    }
}