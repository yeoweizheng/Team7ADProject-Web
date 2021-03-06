﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Team7ADProject.Models;

namespace Team7ADProject.Database
{
    public class Team7ADProjectDbContext : DbContext
    {
        public Team7ADProjectDbContext() : base("Server=localhost; Database=Team7ADProject; Integrated Security=True")
        {
            System.Data.Entity.Database.SetInitializer(new Team7ADProjectDbInitializer());
            this.Configuration.LazyLoadingEnabled = true;
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DepartmentStaff>().ToTable("DepartmentStaffs");
            modelBuilder.Entity<DepartmentHead>().ToTable("DepartmentHeads");
            modelBuilder.Entity<StoreSupervisor>().ToTable("StoreSupervisors");
            modelBuilder.Entity<StoreClerk>().ToTable("StoreClerks");
        }
        public DbSet<User> User { get; set; }
        public DbSet<Session> Session { get; set; }
        public DbSet<Stationery> Stationery { get; set; }
        public DbSet<StationeryRequest> StationeryRequest { get; set; }
        public DbSet<StationeryQuantity> StationeryQuantity { get; set; }
        public DbSet<AdjustmentVoucher> AdjustmentVoucher { get; set; }
        public DbSet<Notification> Notification { get; set; }
        public DbSet<NotificationStatus> NotificationStatus { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<DepartmentRequest> DepartmentRequest { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<RetrievalList> RetrievalList { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<UnitOfMeasure> UnitOfMeasure { get; set; }
        public DbSet<DisbursementList> DisbursementList { get; set; }
        public DbSet<Supplier> Supplier { get; set; }
        public DbSet<StationerySupplierPrice> StationerySupplierPrice { get; set; }
        public DbSet<AuthorizeForm> AuthorizeForm { get; set; }
        public DbSet<CollectionPoint> CollectionPoint { get; set; }
        public DbSet<ScheduledJob> ScheduledJob { get; set; }
    }
}