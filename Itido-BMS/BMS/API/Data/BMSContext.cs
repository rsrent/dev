using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using API.Models;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using API.Services;

namespace API.Data
{
    public class BMSContext : DbContext
    {
        private readonly SignedInUser _signedInUser;

        public BMSContext(DbContextOptions<BMSContext> options, SignedInUser signedInUser) : base(options)
        {
            this._signedInUser = signedInUser;
        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UnitUser> UnitUsers { get; set; }
        public DbSet<ProjectUser> ProjectUsers { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Folder> Folders { get; set; }
        public DbSet<FolderUser> FolderUsers { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>().HasBaseType<Unit>();
            modelBuilder.Entity<Location>().HasBaseType<Unit>();
            modelBuilder.Entity<Client>().HasBaseType<Unit>();

            modelBuilder.Entity<ProjectUser>().HasKey(pu => new { pu.ProjectId, pu.UserId });
            modelBuilder.Entity<ProjectUser>().HasOne(pu => pu.User).WithMany(u => u.ProjectUsers);


            modelBuilder.Entity<User>().HasQueryFilter(
                x => x.Organization.FirebaseOwnerId == _signedInUser.FirebaseId || x.Organization.Users.Any(ou => ou.FirebaseId == _signedInUser.FirebaseId));


        }
    }
}
