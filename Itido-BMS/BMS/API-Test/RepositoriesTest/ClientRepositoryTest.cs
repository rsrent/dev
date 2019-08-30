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
    public class ClientRepositoryTest
    {
        /* 
        public void RunTest(Func<BMSContext, Task> Test)
        {

            var options = new DbContextOptionsBuilder<BMSContext>()
            .UseInMemoryDatabase(databaseName: "BMSDatabase")
            .Options;

            using (var context = new BMSContext(options))
            {
                context.Clients.Add(new Client { Name = "HM" });
                context.SaveChanges();
            }

            using (var context = new BMSContext(options))
            {
                Test(context);
            }
        }*/
    }
}
