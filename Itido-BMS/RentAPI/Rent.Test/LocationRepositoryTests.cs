using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using Rent.Data;
using Rent.Models;
using Rent.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Rent.Test
{
    public class LocationRepositoryTests
    {
        [Fact(DisplayName = "Get location name returns name")]
        public async Task GetLocationNameReturnsNameAsync()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var builder = new DbContextOptionsBuilder<RentContext>()
                              .UseSqlite(connection);

            var context = new RentContext(builder.Options);
            context.Database.EnsureCreated();

            var entity = new Location
            {
                Name = "middelfart",
                Address = "Middelfart",
                Comment = "Cool place"
            };

            context.Location.Add(entity);
            await context.SaveChangesAsync();
            var id = entity.ID;

            using (var repository = new LocationRepository(context, null, null))
            {
                var locationName = await repository.GetName(id);

                Assert.Equal("middelfart", locationName);
            }
        }
    }
}
