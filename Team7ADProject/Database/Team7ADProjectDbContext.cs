using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Team7ADProject.Models;

namespace Team7ADProject.Database
{
    public class Team7ADProjectDbContext:DbContext
    {
        public Team7ADProjectDbContext() : base("Server=localhost; Database=Team7ADProject; Integrated Security=True")
        {
            System.Data.Entity.Database.SetInitializer(new Team7ADProjectDbInitializer());
            this.Configuration.LazyLoadingEnabled = true;
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DepartmentStaff>().ToTable("DepartmentStaffs");
        }
        public DbSet<User> User { get; set; }
    }
}