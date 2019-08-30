using Microsoft.EntityFrameworkCore;
using Rent.Data;
using Rent.Models.TimePlanning;
using Rent.Repositories.TimePlanning;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Moq;

namespace Rent.Test
{
    public class AgreementRepositoryTest
    {
        [Fact(DisplayName = "Name of agreement")]
        public async Task GetNameOfAgreement()
        {

            var options = new DbContextOptionsBuilder<RentContext>()
            .UseInMemoryDatabase(databaseName: "TimePlanningDatabase")
            .Options;

            var context = new RentContext(options);
            context.Database.EnsureCreated();

            var entity = new Agreement
            {
                Name = "HK",
            };

            context.Agreement.Add(entity);
            await context.SaveChangesAsync();
            var id = entity.ID;
            var mockRole = new Mock<IRoleAuthenticationRepository>();


            var repository = new AgreementRepository(context, mockRole.Object);
            var agreement = repository.Get(0, id);

           // Assert.Equal("HK", agreement.Name);

        }

        [Fact(DisplayName = "Wrong Name Of Agreement")]
        public async Task GetWrongNameOfAgreement()
        {

            var options = new DbContextOptionsBuilder<RentContext>()
            .UseInMemoryDatabase(databaseName: "TimePlanningDatabase")
            .Options;

            var context = new RentContext(options);
            context.Database.EnsureCreated();

            var entity = new Agreement
            {
                Name = "HK",
            };

            context.Agreement.Add(entity);
            await context.SaveChangesAsync();
            var id = entity.ID;
            var mockRole = new Mock<IRoleAuthenticationRepository>();

            var repository = new AgreementRepository(context, mockRole.Object);
            var agreement = repository.Get(0, id);

           // Assert.NotEqual("Hej", agreement.Name);

        }

        [Fact(DisplayName = "Get All Agreements")]
        public async Task GetAllAgreements()
        {

            var options = new DbContextOptionsBuilder<RentContext>()
            .UseInMemoryDatabase(databaseName: "TimePlanningDatabase")
            .Options;

            var context = new RentContext(options);
            context.Database.EnsureCreated();

            var entity = new Agreement
            {
                Name = "HK",
            };
            var entity1 = new Agreement
            {
                Name = "HK1",
            };
            var entity2 = new Agreement
            {
                Name = "HK2",
            };

            context.Agreement.Add(entity);
            await context.SaveChangesAsync();
            context.Agreement.Add(entity1);
            await context.SaveChangesAsync();
            context.Agreement.Add(entity2);
            await context.SaveChangesAsync();
            var mockRole = new Mock<IRoleAuthenticationRepository>();


            var repository = new AgreementRepository(context, mockRole.Object);
            var agreement = repository.GetAll(0);
            ICollection<Agreement> collections = agreement as ICollection<Agreement>;

            Assert.Equal(3, collections.Count);

        }

        [Fact(DisplayName = "Update Agreement")]
        public async Task UpdateAgreement()
        {

            var options = new DbContextOptionsBuilder<RentContext>()
            .UseInMemoryDatabase(databaseName: "TimePlanningDatabase")
            .Options;

            var context = new RentContext(options);
            context.Database.EnsureCreated();

            var entity = new Agreement
            {
                Name = "HK",
            };
           

            context.Agreement.Add(entity);
            await context.SaveChangesAsync();
            var mockRole = new Mock<IRoleAuthenticationRepository>();

            var repository = new AgreementRepository(context, mockRole.Object);
            var agreementToUpdate = repository.Get(0,entity.ID);
            //agreementToUpdate.Name = "Not HK";
           // await repository.UpdateAgreement(0, agreementToUpdate);
            var updatedAgreement = context.Agreement.Find(entity.ID);

            Assert.Equal("Not HK", updatedAgreement.Name);

        }

    }
}
