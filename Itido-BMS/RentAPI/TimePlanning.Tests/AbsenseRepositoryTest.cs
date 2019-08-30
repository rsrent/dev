using System;
using System.Collections.Generic;
using System.Text;
using Rent.Models.TimePlanning;
using Rent.Repositories.TimePlanning;
using Microsoft.EntityFrameworkCore;
using Rent.Data;
using Rent.Models; 
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace TimePlanning.Tests
{
    /*
    public class AbsenseRepositoryTest
    {
        [Fact(DisplayName = "Set State Of Absence Throws Exception")]
        public async Task SetStateThrowException()
        {
            var options = new DbContextOptionsBuilder<RentContext>()
            .UseInMemoryDatabase(databaseName: "TimePlanningDatabase")
            .Options;
            var context = new RentContext(options);
            context.Database.EnsureCreated();

            var requesterWhoIsUser = new User
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
                UserRole = "User"
            };
            context.User.Add(requesterWhoIsUser);
            await context.SaveChangesAsync();
            var userId = requesterWhoIsUser.ID;

            var absenceReason = new AbsenceReason
            {
                Description = "Sygdom",
                CanUserCreate = false,
                CanUserRequest = false,
                CanManagerCreate = false,
                CanManagerRequest = false,
            };

            context.AbsenceReason.Add(absenceReason);
            await context.SaveChangesAsync();
            var absenceReasonId = absenceReason.ID;

            var absence = new Absence
            {
                UserID = userId,
                ApprovalState = 0,
                CreatorID = userId,
                AbsenceReasonID = absenceReasonId,
                Comment = "Very sick",
                From = DateTime.Now,
                To = DateTime.Now
            };
            var roleRepoMock = new Mock<IRoleAuthenticationRepository>();
            var approvalMock = new Mock<IApprovalStateRepository>();
            var absenceRepository = new AbsenceRepository(context, roleRepoMock.Object, approvalMock.Object);

            await Assert.ThrowsAsync<UnauthorizedAccessException>(()  => absenceRepository.CreateAbsence(userId, absence, true));

        }

        [Fact(DisplayName = "Approved State For Manager Who Can Create")]
        public async Task SetStateApprovedForManagerWhoCanCreate()
        {
            var options = new DbContextOptionsBuilder<RentContext>()
            .UseInMemoryDatabase(databaseName: "TimePlanningDatabase")
            .Options;
            var context = new RentContext(options);
            context.Database.EnsureCreated();

            var requesterWhoIsManager = new User
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
                UserRole = "Manager"
            };
            context.User.Add(requesterWhoIsManager);
            await context.SaveChangesAsync();
            var userId = requesterWhoIsManager.ID;

            var absenceReason = new AbsenceReason
            {
                Description = "Sygdom",
                CanUserCreate = false,
                CanUserRequest = false,
                CanManagerCreate = true,
                CanManagerRequest = false,
            };

            context.AbsenceReason.Add(absenceReason);
            await context.SaveChangesAsync();
            var absenceReasonId = absenceReason.ID;

            var absence = new Absence
            {
                UserID = userId,
                ApprovalState = 0,
                CreatorID = userId,
                AbsenceReasonID = absenceReasonId,
                Comment = "Very sick",
                From = DateTime.Now,
                To = DateTime.Now
            };
            var roleRepoMock = new Mock<IRoleAuthenticationRepository>();
            var approvalMock = new Mock<IApprovalStateRepository>();


            var absenceRepository = new AbsenceRepository(context, roleRepoMock.Object, approvalMock.Object);

            var absenceFromDB = await absenceRepository.CreateAbsence(userId, absence, false);
            var absenceForAssert = context.Absence.Find(absenceFromDB);

            Assert.Equal(ApprovalState.Approved, absenceForAssert.ApprovalState);

        }



        [Fact(DisplayName = "Approved State For User Who Can Create")]
        public async Task SetStateApprovedForUserWhoCanCreate()
        {
            var options = new DbContextOptionsBuilder<RentContext>()
            .UseInMemoryDatabase(databaseName: "TimePlanningDatabase")
            .Options;
            var context = new RentContext(options);
            context.Database.EnsureCreated();

            var requesterWhoIsManager = new User
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
                UserRole = "User"
            };
            context.User.Add(requesterWhoIsManager);
            await context.SaveChangesAsync();
            var userId = requesterWhoIsManager.ID;

            var absenceReason = new AbsenceReason
            {
                Description = "Sygdom",
             
                CanUserCreate = true,
                CanUserRequest = false,
                CanManagerCreate = false,
                CanManagerRequest = false,
            };

            context.AbsenceReason.Add(absenceReason);
            await context.SaveChangesAsync();
            var absenceReasonId = absenceReason.ID;

            var absence = new Absence
            {
                UserID = userId,
                ApprovalState = 0,
                CreatorID = userId,
                AbsenceReasonID = absenceReasonId,
                Comment = "Very sick",
                From = DateTime.Now,
                To = DateTime.Now
            };
            var roleRepoMock = new Mock<IRoleAuthenticationRepository>();
            var approvalMock = new Mock<IApprovalStateRepository>();


            var absenceRepository = new AbsenceRepository(context, roleRepoMock.Object, approvalMock.Object);

            var absenceFromDB = await absenceRepository.CreateAbsence(userId, absence, false);
            var absenceForAssert = context.Absence.Find(absenceFromDB);

            Assert.Equal(ApprovalState.Approved, absenceForAssert.ApprovalState);

        }


        [Fact(DisplayName = "Pending State For User Who Can Request")]
        public async Task SetStatePendingForUserWhoCanRequest()
        {
            var options = new DbContextOptionsBuilder<RentContext>()
            .UseInMemoryDatabase(databaseName: "TimePlanningDatabase")
            .Options;
            var context = new RentContext(options);
            context.Database.EnsureCreated();

            var requesterWhoIsManager = new User
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
                UserRole = "User"
            };
            context.User.Add(requesterWhoIsManager);
            await context.SaveChangesAsync();
            var userId = requesterWhoIsManager.ID;

            var absenceReason = new AbsenceReason
            {
                Description = "Sygdom",
               
                CanUserCreate = false,
                CanUserRequest = true,
                CanManagerCreate = false,
                CanManagerRequest = false,
            };

            context.AbsenceReason.Add(absenceReason);
            await context.SaveChangesAsync();
            var absenceReasonId = absenceReason.ID;

            var absence = new Absence
            {
                UserID = userId,
                ApprovalState = 0,
                CreatorID = userId,
                AbsenceReasonID = absenceReasonId,
                Comment = "Very sick",
                From = DateTime.Now,
                To = DateTime.Now
            };
            var roleRepoMock = new Mock<IRoleAuthenticationRepository>();
            var approvalMock = new Mock<IApprovalStateRepository>();


            var absenceRepository = new AbsenceRepository(context, roleRepoMock.Object, approvalMock.Object);

            var absenceFromDB = await absenceRepository.CreateAbsence(userId, absence, true);
            var absenceForAssert = context.Absence.Find(absenceFromDB);

            Assert.Equal(ApprovalState.Pending, absenceForAssert.ApprovalState);

        }



        [Fact(DisplayName = "Pending State For Manager Who Can Request")]
        public async Task SetStatePendingForManagerWhoCanRequest()
        {
            var options = new DbContextOptionsBuilder<RentContext>()
            .UseInMemoryDatabase(databaseName: "TimePlanningDatabase")
            .Options;
            var context = new RentContext(options);
            context.Database.EnsureCreated();

            var requesterWhoIsManager = new User
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
                UserRole = "Manager"
            };
            context.User.Add(requesterWhoIsManager);
            await context.SaveChangesAsync();
            var userId = requesterWhoIsManager.ID;

            var absenceReason = new AbsenceReason
            {
                Description = "Sygdom",
              
                CanUserCreate = false,
                CanUserRequest = false,
                CanManagerCreate = false,
                CanManagerRequest = true,
            };

            context.AbsenceReason.Add(absenceReason);
            await context.SaveChangesAsync();
            var absenceReasonId = absenceReason.ID;

            var absence = new Absence
            {
                UserID = userId,
                ApprovalState = 0,
                CreatorID = userId,
                AbsenceReasonID = absenceReasonId,
                Comment = "Very sick",
                From = DateTime.Now,
                To = DateTime.Now
            };
            var roleRepoMock = new Mock<IRoleAuthenticationRepository>();
            var approvalMock = new Mock<IApprovalStateRepository>();


            var absenceRepository = new AbsenceRepository(context, roleRepoMock.Object, approvalMock.Object);

            var absenceFromDB = await absenceRepository.CreateAbsence(userId, absence, true);
            var absenceForAssert = context.Absence.Find(absenceFromDB);

            Assert.Equal(ApprovalState.Pending, absenceForAssert.ApprovalState);

        }


        [Fact(DisplayName = "Reply Accepted")]
        public async Task ReplyToAbsenceAsUser()
        {
            var options = new DbContextOptionsBuilder<RentContext>()
            .UseInMemoryDatabase(databaseName: "TimePlanningDatabase")
            .Options;
            var context = new RentContext(options);
            context.Database.EnsureCreated();
            var userWithAbsence = new User
            {
                Disabled = false,
                LoginID = 0,
                Email = "123",
                Phone = "123",
                RoleID = 1,
                FirstName = "John",
                LastName = "Absence",
                Comment = "Test",
                Title = "CEO",
                ImageLocation = "Here",
                UserRole = "User"
            };
            var userCreator = new User
            {
                Disabled = false,
                LoginID = 0,
                Email = "1234",
                Phone = "1234",
                RoleID = 1,
                FirstName = "John",
                LastName = "Manager",
                Comment = "Test",
                Title = "CEO",
                ImageLocation = "Here",
                UserRole = "Manager"
            };
            var absenceReason = new AbsenceReason
            {
                Description = "Sygdom",
            
                CanUserCreate = false,
                CanUserRequest = false,
                CanManagerCreate = false,
                CanManagerRequest = true,
            };
            context.User.Add(userWithAbsence);
            await context.SaveChangesAsync();
            var userWithAbsenceId = userWithAbsence.ID;

            context.User.Add(userCreator);
            await context.SaveChangesAsync();
            var userCreatorId = userCreator.ID;

            context.AbsenceReason.Add(absenceReason);
            await context.SaveChangesAsync();
            var absenceReasonId = absenceReason.ID;

            var absence = new Absence
            {
                UserID = userWithAbsenceId,
                ApprovalState = 0,
                CreatorID = userCreatorId,
                AbsenceReasonID = absenceReasonId,
                Comment = "Very sick",
                From = DateTime.Now,
                To = DateTime.Now
            };

            context.Absence.Add(absence);
            await context.SaveChangesAsync();
            var absenceId = absence.ID;
            var roleRepoMock = new Mock<IRoleAuthenticationRepository>();

            roleRepoMock.Setup(rr => rr.IsAdminOrManager(userCreatorId)).Returns(true);
            roleRepoMock.Setup(rr => rr.IsAdminOrManager(userWithAbsenceId)).Returns(false);

            roleRepoMock.Setup(rr => rr.IsUser(userWithAbsenceId)).Returns(true);
            var approvalMock = new Mock<IApprovalStateRepository>();

            approvalMock.Setup(ap => ap.CanRequesterReplyToApprovalState(userWithAbsenceId, absence.ApprovalState, absence.CreatorID, absence.UserID)).Returns(true);


            AbsenceRepository absenceRepo = new AbsenceRepository(context, roleRepoMock.Object, approvalMock.Object);

            await absenceRepo.ReplyToAbsence(userWithAbsenceId, absenceId, true);

            Assert.Equal(ApprovalState.Approved,absence.ApprovalState);


        }

        [Fact(DisplayName = "Reply Denied")]
        public async Task ReplyToAbsenceAsUserWithFalse()
        {
            var options = new DbContextOptionsBuilder<RentContext>()
            .UseInMemoryDatabase(databaseName: "TimePlanningDatabase")
            .Options;
            var context = new RentContext(options);
            context.Database.EnsureCreated();
            var userWithAbsence = new User
            {
                Disabled = false,
                LoginID = 0,
                Email = "123",
                Phone = "123",
                RoleID = 1,
                FirstName = "John",
                LastName = "Absence",
                Comment = "Test",
                Title = "CEO",
                ImageLocation = "Here",
                UserRole = "User"
            };
            var userCreator = new User
            {
                Disabled = false,
                LoginID = 0,
                Email = "1234",
                Phone = "1234",
                RoleID = 1,
                FirstName = "John",
                LastName = "Manager",
                Comment = "Test",
                Title = "CEO",
                ImageLocation = "Here",
                UserRole = "Manager"
            };
            var absenceReason = new AbsenceReason
            {
                Description = "Sygdom",
                
                CanUserCreate = false,
                CanUserRequest = false,
                CanManagerCreate = false,
                CanManagerRequest = true,
            };
            context.User.Add(userWithAbsence);
            await context.SaveChangesAsync();
            var userWithAbsenceId = userWithAbsence.ID;

            context.User.Add(userCreator);
            await context.SaveChangesAsync();
            var userCreatorId = userCreator.ID;

            context.AbsenceReason.Add(absenceReason);
            await context.SaveChangesAsync();
            var absenceReasonId = absenceReason.ID;

            var absence = new Absence
            {
                UserID = userWithAbsenceId,
                ApprovalState = 0,
                CreatorID = userCreatorId,
                AbsenceReasonID = absenceReasonId,
                Comment = "Very sick",
                From = DateTime.Now,
                To = DateTime.Now
            };

            context.Absence.Add(absence);
            await context.SaveChangesAsync();
            var absenceId = absence.ID;
            var roleRepoMock = new Mock<IRoleAuthenticationRepository>();

            roleRepoMock.Setup(rr => rr.IsAdminOrManager(userCreatorId)).Returns(true);
            roleRepoMock.Setup(rr => rr.IsAdminOrManager(userWithAbsenceId)).Returns(false);

            roleRepoMock.Setup(rr => rr.IsUser(userWithAbsenceId)).Returns(true);
            var approvalMock = new Mock<IApprovalStateRepository>();
            approvalMock.Setup(ap => ap.CanRequesterReplyToApprovalState(userWithAbsenceId, absence.ApprovalState, absence.CreatorID, absence.UserID)).Returns(true);


            AbsenceRepository absenceRepo = new AbsenceRepository(context, roleRepoMock.Object, approvalMock.Object);

            await absenceRepo.ReplyToAbsence(userWithAbsenceId, absenceId, false);

            Assert.Equal(ApprovalState.Denied, absence.ApprovalState);


        }

        [Fact(DisplayName = "Reply throws exception")]
        public async Task ReplyToAbsenceAsManagerForManager()
        {
            var options = new DbContextOptionsBuilder<RentContext>()
            .UseInMemoryDatabase(databaseName: "TimePlanningDatabase")
            .Options;
            var context = new RentContext(options);
            context.Database.EnsureCreated();
            var userWithAbsence = new User
            {
                Disabled = false,
                LoginID = 0,
                Email = "123",
                Phone = "123",
                RoleID = 1,
                FirstName = "John",
                LastName = "Absence",
                Comment = "Test",
                Title = "CEO",
                ImageLocation = "Here",
                UserRole = "Manager"
            };
            var userCreator = new User
            {
                Disabled = false,
                LoginID = 0,
                Email = "1234",
                Phone = "1234",
                RoleID = 1,
                FirstName = "John",
                LastName = "Manager",
                Comment = "Test",
                Title = "CEO",
                ImageLocation = "Here",
                UserRole = "Manager"
            };
            var absenceReason = new AbsenceReason
            {
                Description = "Sygdom",
              
                CanUserCreate = false,
                CanUserRequest = false,
                CanManagerCreate = false,
                CanManagerRequest = true,
            };
            context.User.Add(userWithAbsence);
            await context.SaveChangesAsync();
            var userWithAbsenceId = userWithAbsence.ID;

            context.User.Add(userCreator);
            await context.SaveChangesAsync();
            var userCreatorId = userCreator.ID;

            context.AbsenceReason.Add(absenceReason);
            await context.SaveChangesAsync();
            var absenceReasonId = absenceReason.ID;

            var absence = new Absence
            {
                UserID = userWithAbsenceId,
                ApprovalState = 0,
                CreatorID = userCreatorId,
                AbsenceReasonID = absenceReasonId,
                Comment = "Very sick",
                From = DateTime.Now,
                To = DateTime.Now
            };

            context.Absence.Add(absence);
            await context.SaveChangesAsync();
            var absenceId = absence.ID;
            var roleRepoMock = new Mock<IRoleAuthenticationRepository>();

            roleRepoMock.Setup(rr => rr.IsAdminOrManager(userCreatorId)).Returns(true);
            roleRepoMock.Setup(rr => rr.IsAdminOrManager(userWithAbsenceId)).Returns(true);

            roleRepoMock.Setup(rr => rr.IsUser(userWithAbsenceId)).Returns(false);
            var approvalMock = new Mock<IApprovalStateRepository>();
            approvalMock.Setup(ap => ap.CanRequesterReplyToApprovalState(userCreatorId, absence.ApprovalState, absence.CreatorID, absence.UserID)).Returns(false);


            AbsenceRepository absenceRepo = new AbsenceRepository(context, roleRepoMock.Object, approvalMock.Object);

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => absenceRepo.ReplyToAbsence(userCreatorId, absenceId, true));


        }



    }*/
}
