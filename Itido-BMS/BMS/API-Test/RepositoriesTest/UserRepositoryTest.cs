using System;
using Xunit;
using API.Repositories;
using API.Models;
using API.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace API_Test.RepositoriesTest
{
    public class UserRepositoryTest
    {
        /*
        public void RunTest(Func<BMSContext, Task> Test)
        {

            var options = new DbContextOptionsBuilder<BMSContext>()
            .UseInMemoryDatabase(databaseName: "BMSDatabase")
            .Options;

            // Insert seed data into the database using one instance of the context
            using (var context = new BMSContext(options))
            {

                context.Users.Add(new User
                {
                    Email = "todibbang@gmail.com",
                });
                context.Users.Add(new User
                {
                    Email = "updateTester@gmail.com",
                    Name = "upTester",
                });
                context.SaveChanges();
            }

            // Use a clean instance of the context to run the test
            using (var context = new BMSContext(options))
            {
                Test(context);
            }
        }

        public BMSContext CreateContext()
        {

            var options = new DbContextOptionsBuilder<BMSContext>()
            .UseInMemoryDatabase(databaseName: "BMSDatabase1")
            .Options;

            // Insert seed data into the database using one instance of the context
            using (var context = new BMSContext(options))
            {

                context.Users.Add(new User
                {
                    Id = 0,
                    Email = "todibbang@gmail.com",
                    Role = "Master",
                });
                context.Users.Add(new User
                {
                    Email = "updateTester@gmail.com",
                    Name = "upTester",
                });
                context.SaveChanges();
            }

            return new BMSContext(options);
        }
        [Fact]
        public void GetMany()
        {
            RunTest(async (context) =>
            {
                var userContext = new UserContext(context);
                var repo = new UserRepository(userContext);
                var results = await repo.GetMany();
                Assert.Equal(3, results.Count);
            });
        }

        [Fact]
        public async void Delete()
        {
            var context = CreateContext();
            var userContext = new UserContext(context);

            var repo = new UserRepository(userContext);

            //var moq = new Mock<UserContext>().Setup(x => x.GetRequirement)

            var users = await repo.GetMany();
            var newest = users.Last();

            await repo.Delete(newest.Id);
            var userOrNotUser = context.Users.Find(newest.Id);

            Assert.NotNull(userOrNotUser);

            
        }

        [Fact]
        public void Create()
        {
            RunTest(async (context) =>
            {
                var userContext = new UserContext(context);
                var repo = new UserRepository(userContext);
                var user = new User
                {
                    Email = "test@gmail.com",
                    Name = "createTester"
                };
                var result = await repo.Create(user);
                Assert.Equal(new User { Email = "test@gmail.com", Name = "createTester" }, result);
            });
        }

        [Fact]
        public void Update()
        {
            RunTest(async (context) =>
            {
                var userContext = new UserContext(context);
                var repo = new UserRepository(userContext);
                var user = new User
                {
                    Email = "updateTester@gmail.com",
                    Id = 1,
                    Name = "updateTester"
                };
                await repo.Update(0, user);
                Assert.Equal(new User { Email = "updateTester@gmail.com", Id = 1, Name = "updateTester" }, context.Users.Find(1));
            });

        }

        [Fact]
        public void Get()
        {
            RunTest(async (context) =>
            {
                var userContext = new UserContext(context);
                var repo = new UserRepository(userContext);
                var result = await repo.Get(0);
                Assert.Equal(new User
                {
                    Email = "todibbang@gmail.com",
                    Id = 0
                }, result);
            });

        }
         */
    }
}
