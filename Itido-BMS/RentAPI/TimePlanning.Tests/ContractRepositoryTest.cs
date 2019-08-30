using Microsoft.EntityFrameworkCore;
using Moq;
using Rent.Data;
using Rent.Models.TimePlanning;
using Rent.Models;
using Rent.Repositories.TimePlanning;
using Rent.DTOAssemblers;
using Rent.DTOs.TimePlanningDTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TimePlanning.Tests
{
    public class ContractRepositoryTest
    {

        [Fact(DisplayName = "Get All contracts of user")]
        public async Task GetAllContractsOfUser()
        {

            var options = new DbContextOptionsBuilder<RentContext>()
            .UseInMemoryDatabase(databaseName: "TimePlanningDatabase")
            .Options;

            var context = new RentContext(options);
            context.Database.EnsureCreated();

            var agreement = new Agreement
            {
                Name = "BestAgreement"
            };

            var agreement1 = new Agreement
            {
                Name = "SecondBestAgreement"
            };

            var user = new User
            {
                Disabled = false,
                LoginID = 0,
                Email = "123",
                Phone = "123",
                RoleID = 1,
                FirstName = "John",
                LastName = "Agreement",
                Comment = "Test",
                Title = "CEO",
                ImageLocation = "Here",

            };

            context.User.Add(user);
            await context.SaveChangesAsync();
            var userId = user.ID;


            context.Agreement.Add(agreement);
            await context.SaveChangesAsync();
            var agreementId = agreement.ID;

            context.Agreement.Add(agreement1);
            await context.SaveChangesAsync();
            var agreementId1 = agreement1.ID;


            var entity = new Contract
            {
                UserID = userId,
                AgreementID = agreementId,
                WeeklyHours = 7.5f,
                From = DateTime.Now,
                To = DateTime.Now,
                User = user,
                Agreement = agreement

            };

            var entity1 = new Contract
            {
                UserID = userId,
                AgreementID = agreementId1,
                WeeklyHours = 10,
                From = DateTime.Now,
                To = DateTime.Now,
                User = user,
                Agreement = agreement1

            };


            context.Contract.Add(entity);
            await context.SaveChangesAsync();

            context.Contract.Add(entity1);
            await context.SaveChangesAsync();

            var mock = new Mock<IAgreementRepository>();

            mock.Setup(ap => ap.Get(0, agreementId)).Returns(new Agreement
            {
                Name = "BestAgreement"
            });

            mock.Setup(ap => ap.Get(0, agreementId1)).Returns(new Agreement
            {
                Name = "SecondBestAgreement"
            });
            var contrackAssembler = new ContractAssembler(mock.Object);
            var mockRole = new Mock<IRoleAuthenticationRepository>();
            var repository = new ContractRepository(context, mockRole.Object);
            //var contracts = repository.GetAllContractsForUser(userId);
            //var contractDTOs = contrackAssembler.CreateContractDTOsFromListOfContracts(contracts);
           // ICollection<ContractDTO> contractsDTOList = contractDTOs as ICollection<ContractDTO>;

         //   Assert.Equal(2, contractsDTOList.Count);

        }


        [Fact(DisplayName = "Weekly Hours of agreement")]
        public async Task GetWeeklyHoursOfAgreement()
        {

            var options = new DbContextOptionsBuilder<RentContext>()
            .UseInMemoryDatabase(databaseName: "TimePlanningDatabase")
            .Options;

            var context = new RentContext(options);
            context.Database.EnsureCreated();

            var agreement = new Agreement
            {
                Name = "HK"
            };

            var user = new User
            {
                Disabled = false,
                LoginID = 0,
                Email = "123",
                Phone = "123",
                RoleID = 1,
                FirstName = "John",
                LastName = "Test",
                Comment = "Test",
                Title = "CEO",
                ImageLocation = "Here",

            };

            context.User.Add(user);
            await context.SaveChangesAsync();
            var userId = user.ID;


            context.Agreement.Add(agreement);
            await context.SaveChangesAsync();
            var agreementId = agreement.ID;


            var entity = new Contract
            {
                UserID = userId,
                AgreementID = agreementId,
                WeeklyHours = 7.5f,
                From = DateTime.Now,
                To = DateTime.Now,
                User = user,
                Agreement = agreement    

            };
           

            context.Contract.Add(entity);
            await context.SaveChangesAsync();

            var mock = new Mock<IAgreementRepository>();

            mock.Setup(ap => ap.Get(0, agreementId)).Returns(new Agreement
            {
                Name = "HK"
            });

          
            var contrackAssembler = new ContractAssembler(mock.Object);
            var mockRole = new Mock<IRoleAuthenticationRepository>();


            var repository = new ContractRepository(context, mockRole.Object);
           // var contract = repository.Get(0, agreementId, userId);

           // Assert.Equal(7.5, contract.WeeklyHours);

        }



        [Fact(DisplayName = "Get Contract DTO")]
        public async Task GetNameOfAgreementInContract()
        {

            var options = new DbContextOptionsBuilder<RentContext>()
            .UseInMemoryDatabase(databaseName: "TimePlanningDatabase")
            .Options;

            var context = new RentContext(options);
            context.Database.EnsureCreated();

            var agreement = new Agreement
            {
                Name = "HK"
            };

            var user = new User
            {
                Disabled = false,
                LoginID = 0,
                Email = "123",
                Phone = "123",
                RoleID = 1,
                FirstName = "John",
                LastName = "Test",
                Comment = "Test",
                Title = "CEO",
                ImageLocation = "Here",

            };

            context.User.Add(user);
            await context.SaveChangesAsync();
            var userId = user.ID;


            context.Agreement.Add(agreement);
            await context.SaveChangesAsync();
            var agreementId = agreement.ID;


            var entity = new Contract
            {
                UserID = userId,
                AgreementID = agreementId,
                WeeklyHours = 7.5f,
                From = DateTime.Now,
                To = DateTime.Now,
                User = user,
                Agreement = agreement

            };

            context.Contract.Add(entity);
            await context.SaveChangesAsync();

            var mock = new Mock<IAgreementRepository>();

            mock.Setup(ap => ap.Get(0, agreementId)).Returns(new Agreement
            {
                Name = "HK"
            });

            var contrackAssembler = new ContractAssembler(mock.Object);
            var mockRole = new Mock<IRoleAuthenticationRepository>();


            var repository = new ContractRepository(context, mockRole.Object);
           // var contract = repository.Get(0, agreementId, userId);
           // var contractDTO = contrackAssembler.CreateContractDTO(contract);
//Assert.Equal("HK", contractDTO.AgreementName);

        }
    }
}
