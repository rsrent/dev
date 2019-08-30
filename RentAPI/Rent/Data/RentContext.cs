using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rent.Models;
using Rent.Models.Important;

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<LocationCleaningPlanDocument>()
            //    .HasKey(c => new { c.CleaningPlanID, c.DocumentFolderID, c.LocationID });

            modelBuilder.Entity<ConversationUsers>()
               .HasKey(c => new { c.ConversationID, c.UserID});

            modelBuilder.Entity<UserPermissions>()
                .HasKey(c => new { c.PermissionID, c.UserID });

            modelBuilder.Entity<PermissionsTemplate>()
                .HasKey(c => new { c.PermissionID, c.RoleID });

            modelBuilder.Entity<LocationUser>()
                .HasKey(c => new { c.LocationID, c.UserID });

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
