using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rent.Models;
using Rent.Models.Important;
using Rent.Models.General;
using Rent.Models.Projects;
using Rent.Models.TimePlanning;

namespace Rent.Data
{
    public class RentContext : DbContext
    {
        public RentContext(DbContextOptions<RentContext> options) : base(options)
        {

        }
        public DbSet<CleaningTask> CleaningTask { get; set; }
        public DbSet<CleaningPlan> CleaningPlan { get; set; }
        public DbSet<Floor> Floor { get; set; }
        public DbSet<Area> Area { get; set; }
        //public DbSet<CleaningDescription> CleaningDescription { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Login> Login { get; set; }
        public DbSet<Permission> Permission { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<PermissionsTemplate> PermissionsTemplate { get; set; }
        public DbSet<UserPermissions> UserPermissions { get; set; }

        //public DbSet<PermissionRole> PermissionRole { get; set; }
        public DbSet<QualityReport> QualityReport { get; set; }
        public DbSet<QualityReportItem> QualityReportItem { get; set; }
        public DbSet<Message> Message { get; set; }
        public DbSet<Conversation> Conversation { get; set; }
        public DbSet<ConversationUsers> ConversationUsers { get; set; }
        public DbSet<SpecialMessage> SpecialMessage { get; set; }
        //public DbSet<CustomerDocument> CustomerDocument { get; set; }
        public DbSet<CleaningTaskCompleted> CleaningTaskCompleted { get; set; }
        //public DbSet<UserToUser> UserToUser { get; set; }
        public DbSet<TranslationEntry> TranslationEntry { get; set; }
        public DbSet<Translation> Translation { get; set; }
        public DbSet<Customer> Customer { get; set; }

        //public DbSet<CustomerUser> CustomerUser { get; set; }
        //public DbSet<EmployeeUser> EmployeeUser { get; set; }

        //public DbSet<LocationCustomer> LocationCustomer { get; set; }
        //public DbSet<LocationEmployee> LocationEmployee { get; set; }
        public DbSet<LocationUser> LocationUser { get; set; }
        public DbSet<Location> Location { get; set; }

        public DbSet<DocumentFolder> DocumentFolder { get; set; }
        public DbSet<Document> Document { get; set; }
        //public DbSet<DocumentItem> DocumentItem { get; set; }

        public DbSet<FolderPermission> FolderPermission { get; set; }
        //public DbSet<FolderVisibleToRole> FolderVisibleToRole { get; set; }

        public DbSet<Rating> Rating { get; set; }
        public DbSet<RatingItem> RatingItem { get; set; }

        public DbSet<LocationLog> LocationLog { get; set; }
        public DbSet<MoreWork> MoreWork { get; set; }


        public DbSet<LocationEconomy> LocationEconomy { get; set; }
        public DbSet<LocationHour> LocationHour { get; set; }
        public DbSet<BlackListedToken> BlackListedToken { get; set; }
        //public DbSet<LocationCleaningPlanDocument> LocationCleaningPlanDocument { get; set; }
        //public DbSet<Work> Work { get; set; }

        //public DbSet<Term> Term { get; set; }
        //public DbSet<UserAcceptedTerm> UserAcceptedTerm { get; set; }

        public DbSet<News> News { get; set; }


        //public DbSet<Unit> Units { get; set; }
        //public DbSet<UnitUser> UnitUsers { get; set; }

        //public DbSet<Division> Divisions { get; set; }
        //public DbSet<DivisionUser> DivisionUsers { get; set; }


        public DbSet<Client> Client { get; set; }

        public DbSet<Agreement> Agreement { get; set; }
        public DbSet<Contract> Contract { get; set; }


        public DbSet<Absence> Absence { get; set; }

        public DbSet<AbsenceReason> AbsenceReason { get; set; }



        public DbSet<Noti> Notis { get; set; }

        public DbSet<Post> Posts { get; set; }

        /* */
        public DbSet<Work> Work { get; set; }
        public DbSet<WorkContract> WorkContract { get; set; }
        public DbSet<WorkReplacement> WorkReplacement { get; set; }
        public DbSet<WorkRegistration> WorkRegistration { get; set; }
        public DbSet<WorkDay> WorkDay { get; set; }
        public DbSet<WorkHoliday> WorkHoliday { get; set; }


        public DbSet<Holiday> Holiday { get; set; }

        public DbSet<AccidentReport> AccidentReports { get; set; }
        public DbSet<Request> Requests { get; set; }

        public DbSet<Project> Project { get; set; }
        public DbSet<ProjectItem> ProjectItem { get; set; }
        public DbSet<ProjectUser> ProjectUser { get; set; }
        public DbSet<FirestoreConversation> FirestoreConversation { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<WorkInvitation> WorkInvitation { get; set; }

        public DbSet<ProjectItemAccess> ProjectItemAccess { get; set; }
        public DbSet<ProjectItemAccessTemplate> ProjectItemAccessTemplate { get; set; }
        public DbSet<ProjectRole> ProjectRole { get; set; }

        public DbSet<ProjectItemUser> ProjectItemUser { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<LocationCleaningPlanDocument>()
            //    .HasKey(c => new { c.CleaningPlanID, c.DocumentFolderID, c.LocationID });

            modelBuilder.Entity<ConversationUsers>()
               .HasKey(c => new { c.ConversationID, c.UserID });

            modelBuilder.Entity<UserPermissions>()
                .HasKey(c => new { c.PermissionID, c.UserID });

            modelBuilder.Entity<PermissionsTemplate>()
                .HasKey(c => new { c.PermissionID, c.RoleID });

            modelBuilder.Entity<LocationUser>()
                .HasKey(c => new { c.LocationID, c.UserID });

            modelBuilder.Entity<Contract>()
                .HasKey(c => c.ID);

            modelBuilder.Entity<ProjectItemUser>()
                .HasKey(c => new { c.ProjectItemID, c.UserID });

            modelBuilder.Entity<ProjectUser>()
                .HasKey(c => new { c.ProjectID, c.UserID });

            modelBuilder.Entity<Project>().HasMany(p => p.Projects)
                .WithOne(p => p.Parent).HasForeignKey(p => p.ParentID);

            //modelBuilder.Entity<ProjectItem>().Property(p => p.Access).HasMaxLength(20);

            modelBuilder.Entity<ProjectItemAccess>()
               .HasKey(c => new { c.ProjectItemID, c.ProjectRoleID });

            modelBuilder.Entity<ProjectItemAccessTemplate>()
               .HasKey(c => new { c.ProjectItemType, c.ProjectRoleID });

            //modelBuilder.Entity<Work>().HasOne(wr => wr.WorkRegistration).WithOne(w => w.Work);
            //modelBuilder.Entity<WorkReplacement>().HasOne(wr => wr.Work).WithOne(w => w.WorkReplacement);
            //modelBuilder.Entity<WorkReplacement>().HasOne(wr => wr.Contract).WithMany(c => c.WorkReplacements).HasForeignKey(wr => wr.ContractID);

            modelBuilder.Entity<WorkHoliday>()
                .HasKey(wh => new { wh.HolidayName, wh.HolidayCountryCode, wh.WorkContractID });


            modelBuilder.Entity<WorkHoliday>()
                .HasOne(wh => wh.Holiday).WithMany(h => h.WorkHolidays).HasForeignKey(wh => new { wh.HolidayName, wh.HolidayCountryCode });

            modelBuilder.Entity<WorkDay>()
                .HasKey(wd => new { wd.WorkContractID, wd.DayOfWeek, wd.IsEvenWeek });

            modelBuilder.Entity<WorkDay>()
                .HasOne(wd => wd.WorkContract).WithMany(WorkContract => WorkContract.WorkDays).HasForeignKey(wd => wd.WorkContractID);

            modelBuilder.Entity<Holiday>()
                .HasKey(h => new { h.Name, h.CountryCode });

            modelBuilder.Entity<FolderPermission>()
                .HasKey(fp => new { fp.FolderID, fp.RoleID });

            modelBuilder.Entity<FolderPermission>()
                        .HasOne(fp => fp.Folder).WithMany(f => f.FolderPermissions).HasForeignKey(fp => fp.FolderID);

            modelBuilder.Entity<CleaningTaskCompleted>()
                        .HasOne(ctc => ctc.CleaningTask).WithMany(ct => ct.CompletedTasks).HasForeignKey(ctc => ctc.CleaningTaskID);

            modelBuilder.Entity<QualityReport>()
                        .HasOne(q => q.Location).WithMany(l => l.QualityReports).HasForeignKey(q => q.LocationID);

            modelBuilder.Entity<LocationUser>()
                        .HasOne(lu => lu.Location).WithMany(l => l.LocationUsers).HasForeignKey(lu => lu.LocationID);

            modelBuilder.Entity<LocationUser>()
                        .HasOne(lu => lu.User).WithMany(u => u.LocationUsers).HasForeignKey(lu => lu.UserID);

            modelBuilder.Entity<MoreWork>()
                        .HasOne(m => m.Location).WithMany(u => u.MoreWork).HasForeignKey(m => m.LocationID);

            modelBuilder.Entity<Contract>()
                        .HasOne(c => c.User).WithMany(u => u.Contracts).HasForeignKey(c => c.UserID);

            modelBuilder.Entity<Contract>()
                        .HasOne(c => c.Agreement).WithMany(a => a.Contracts).HasForeignKey(c => c.AgreementID);

            modelBuilder.Entity<Absence>()
                .HasOne(a => a.User).WithMany(u => u.Absences).HasForeignKey(a => a.UserID).OnDelete(DeleteBehavior.Restrict); ;


            modelBuilder.Entity<Absence>()
               .HasOne(a => a.Creator).WithMany(u => u.AbsencesCreated).HasForeignKey(a => a.CreatorID).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Absence>()
                .HasOne(a => a.AbsenceReason).WithMany(ar => ar.Absence).HasForeignKey(a => a.AbsenceReasonID);


            //.HasOne(m => m.Lo).WithMany(u => u.LocationUsers).HasForeignKey(lu => lu.UserID);
            //modelBuilder.Entity<Location>()
            //            .HasOne(h => h.LocationHour).WithOne(l => l.Location);
            //modelBuilder.Entity<LocationHour>().HasKey(lh => new { lh.LocationID });


            //modelBuilder.Entity<FolderVisibleToRole>()
            //    .HasKey(c => new { c.DocumentFolderID, c.RoleID});

            modelBuilder.Entity<User>().HasOne(u => u.Customer).WithMany(c => c.Users)
                .HasForeignKey(u => u.CustomerID);
            modelBuilder.Entity<Location>().HasOne(l => l.Customer).WithMany(c => c.Locations)
                .HasForeignKey(l => l.CustomerID);
            modelBuilder.Entity<CleaningTask>().HasOne(c => c.Location).WithMany(l => l.CleaningTasks)
                .HasForeignKey(c => c.LocationID);


            modelBuilder.Entity<RatingItem>().HasOne(i => i.Rating).WithMany(r => r.RatingItems).HasForeignKey(i => i.RatingID);


            /*
            modelBuilder.Entity<Unit>().Property(u => u.Access).HasMaxLength(10);
            modelBuilder.Entity<Unit>().Property(u => u.TaskAccess).HasMaxLength(10);
            modelBuilder.Entity<Unit>().Property(u => u.ReportAccess).HasMaxLength(10);
            modelBuilder.Entity<Unit>().Property(u => u.LogAccess).HasMaxLength(10);
            modelBuilder.Entity<Unit>().Property(u => u.DocumentAccess).HasMaxLength(10);
            modelBuilder.Entity<Unit>().Property(u => u.EconomyAccess).HasMaxLength(10);
            modelBuilder.Entity<Unit>().Property(u => u.HourAccess).HasMaxLength(10);
            modelBuilder.Entity<Unit>().Property(u => u.ShiftAccess).HasMaxLength(10);
            */




            //modelBuilder.Entity<DocumentFolder>().HasMany(d => d.VisibleToRoles).
            //modelBuilder.Entity<Location>().HasOne(l => l.CustomerConversation).WithOne(c => c.CustomerLocation).OnDelete(DeleteBehavior.);
            //modelBuilder.Entity<Location>().HasOne(l => l.TeamConversation).WithOne(c => c.TeamLocation).OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<Location>().ToTable("Area"); //add this line code
            //modelBuilder.Entity<SpecialMessage>().ToTable("SpecialMessage");

            //modelBuilder.Entity<FolderVisibleToRole>().HasOne(d => d.DocumentFolder).WithMany(f => f.VisibleToRoles).HasForeignKey(d => d.DocumentFolderID);

            modelBuilder.Entity<MessageComplaint>().ToTable("Complaint");
            modelBuilder.Entity<MessageMeeting>().ToTable("Meeting");
            modelBuilder.Entity<MessageMoreWork>().ToTable("MoreWork");
            modelBuilder.Entity<MessageImage>().ToTable("Image");
            modelBuilder.Entity<MessageVideo>().ToTable("Video");

            modelBuilder.Entity<CleaningTask>().ToTable("CleaningTask");
            modelBuilder.Entity<Location>().ToTable("Location");
            modelBuilder.Entity<LocationUser>().ToTable("LocationUser");
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Customer>().ToTable("Customer");
            modelBuilder.Entity<CleaningTaskCompleted>().ToTable("CleaningTaskCompleted");
            modelBuilder.Entity<Agreement>().ToTable("Agreement");
            modelBuilder.Entity<Contract>().ToTable("Contract");
            /*
            modelBuilder.Entity<Work>().ToTable("Work");
            modelBuilder.Entity<WorkContract>().ToTable("WorkContract");
            modelBuilder.Entity<WorkReplacement>().ToTable("WorkReplacement");
             

            modelBuilder.Entity<UnitUser>()
                .HasKey(c => new { c.UnitID, c.UserID });

            modelBuilder.Entity<DivisionUser>()
                .HasKey(c => new { c.DivisionID, c.UserID });

            modelBuilder.Entity<Unit>()
                .HasOne(u => u.Parent).WithMany(u => u.Children).HasForeignKey(u => u.ParentID);


            modelBuilder.Entity<Location>().HasOne(l => l.Unit);
            */


            modelBuilder.Entity<User>().Property(u => u.LanguageCode).HasMaxLength(2).HasDefaultValue("en");


            modelBuilder.Entity<DocumentFolder>().HasOne(df => df.RootDocumentFolder).WithMany(df => df.DecendantDocumentFolders).HasForeignKey(df => df.RootDocumentFolderID);


            /*
            modelBuilder.Ignore<CleaningTask.DTO>();
            modelBuilder.Ignore<Location.DTO>();
            modelBuilder.Ignore<LocationUser.DTO>();
            modelBuilder.Ignore<User.DTO>();
            modelBuilder.Ignore<Customer.DTO>();
            modelBuilder.Ignore<CleaningTaskCompleted.DTO>();
            */

            //modelBuilder.Entity<Location>().HasKey(l => new { }); //add this line code

            //modelBuilder.Entity<CustomerUser>().HasKey(c => new { c.CustomerID, c.UserID });
            /*
            modelBuilder.Entity<Area>().ToTable("Area");
            modelBuilder.Entity<Floor>().ToTable("Floor");
            modelBuilder.Entity<CleaningTask>().ToTable("CleaningTask");
            //modelBuilder.Entity<CleaningPlan>().ToTable("CleaningPlan");
            modelBuilder.Entity<CleaningDescription>().ToTable("CleaningDescription");
            modelBuilder.Entity<Login>().ToTable("Login");
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Permission>().ToTable("Permission");
            modelBuilder.Entity<Role>().ToTable("Role");
            //modelBuilder.Entity<PermissionRole>().ToTable("PermissionRole");
            modelBuilder.Entity<QualityReport>().ToTable("QualityReport");
            modelBuilder.Entity<QualityReportItem>().ToTable("QualityReportItem");
            modelBuilder.Entity<Message>().ToTable("Message");
            modelBuilder.Entity<Conversation>().ToTable("Conversation");
            //modelBuilder.Entity<ConversationUsers>().ToTable("ConversationUsers").HasKey(c => new { c.ConversationID, c.UserID });
            
            
            modelBuilder.Entity<CustomerDocument>().ToTable("CustomerDocument");
            modelBuilder.Entity<CleaningTaskCompleted>().ToTable("CleaningTaskCompleted");
            //modelBuilder.Entity<UserToUser>().ToTable("UserToUser");
            modelBuilder.Entity<TranslationEntry>().ToTable("TranslationEntry");
            modelBuilder.Entity<Translation>().ToTable("Translation");
            modelBuilder.Entity<Customer>().ToTable("Customer");*/
        }

        /*
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //optionsBuilder.UseSqlServer(@"Data Source=rent20170925043224dbserver.database.windows.net;Initial Catalog=Rent20170925043224_db;Integrated Security=False;User ID=sqlserver;Password=Kaffekopgulren123;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
                optionsBuilder.UseSqlServer(@"Server=tcp:rent.database.windows.net,1433;Initial Catalog=MainRentDB;Persist Security Info=False;User ID=rent;Password=lrR!EH61l@$@XD09EjV;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }
        */
    }
}

