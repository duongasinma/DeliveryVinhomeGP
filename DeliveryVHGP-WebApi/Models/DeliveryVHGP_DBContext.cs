﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DeliveryVHGP_WebApi.Models
{
    public partial class DeliveryVHGP_DBContext : DbContext
    {
        public DeliveryVHGP_DBContext()
        {
        }

        public DeliveryVHGP_DBContext(DbContextOptions<DeliveryVHGP_DBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; } = null!;
        public virtual DbSet<Address> Addresses { get; set; } = null!;
        public virtual DbSet<Area> Areas { get; set; } = null!;
        public virtual DbSet<Brand> Brands { get; set; } = null!;
        public virtual DbSet<Building> Buildings { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<CategoryInMenu> CategoryInMenus { get; set; } = null!;
        public virtual DbSet<Collection> Collections { get; set; } = null!;
        public virtual DbSet<Customer> Customers { get; set; } = null!;
        public virtual DbSet<DeliveryMode> DeliveryModes { get; set; } = null!;
        public virtual DbSet<DeliveryShiftOfShipper> DeliveryShiftOfShippers { get; set; } = null!;
        public virtual DbSet<FcmToken> FcmTokens { get; set; } = null!;
        public virtual DbSet<Hub> Hubs { get; set; } = null!;
        public virtual DbSet<Menu> Menus { get; set; } = null!;
        public virtual DbSet<Notification> Notifications { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderDetail> OrderDetails { get; set; } = null!;
        public virtual DbSet<OrderStatus> OrderStatuses { get; set; } = null!;
        public virtual DbSet<OrderTask> OrderTasks { get; set; } = null!;
        public virtual DbSet<Payment> Payments { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<ProductInCollection> ProductInCollections { get; set; } = null!;
        public virtual DbSet<ProductInMenu> ProductInMenus { get; set; } = null!;
        public virtual DbSet<ProductTag> ProductTags { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Schedule> Schedules { get; set; } = null!;
        public virtual DbSet<Shift> Shifts { get; set; } = null!;
        public virtual DbSet<Shipper> Shippers { get; set; } = null!;
        public virtual DbSet<Store> Stores { get; set; } = null!;
        public virtual DbSet<StoreCategory> StoreCategories { get; set; } = null!;
        public virtual DbSet<StoreInMenu> StoreInMenus { get; set; } = null!;
        public virtual DbSet<Tag> Tags { get; set; } = null!;
        public virtual DbSet<TimeOfOrderStage> TimeOfOrderStages { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=tcp:vinhome-fooddelivery.database.windows.net,1433;Initial Catalog=DeliveryVHGP_DB;Persist Security Info=False;User ID=vhgp;Password=Admin123@;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("Account");

                entity.Property(e => e.Id)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Name)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.RoleId)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Status)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_Account_Role");
            });

            modelBuilder.Entity<Address>(entity =>
            {
                entity.ToTable("Address");

                entity.Property(e => e.Id)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.BuildingId).HasMaxLength(50);

                entity.Property(e => e.CustomerId).HasMaxLength(50);

                entity.Property(e => e.Name)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Note)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Room)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Status)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.HasOne(d => d.Building)
                    .WithMany(p => p.Addresses)
                    .HasForeignKey(d => d.BuildingId)
                    .HasConstraintName("FK_CustomerBuilding_Building");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Addresses)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_CustomerBuilding_Customer");
            });

            modelBuilder.Entity<Area>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Area");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.Name)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.HasOne(d => d.IdNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.Id)
                    .HasConstraintName("FK_Area_Building");
            });

            modelBuilder.Entity<Brand>(entity =>
            {
                entity.ToTable("Brand");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.Image).HasMaxLength(100);

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<Building>(entity =>
            {
                entity.ToTable("Building");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.AreaId).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.Image).HasMaxLength(100);

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<CategoryInMenu>(entity =>
            {
                entity.ToTable("CategoryInMenu");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.CategoryId).HasMaxLength(50);

                entity.Property(e => e.MenuId).HasMaxLength(50);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.CategoryInMenus)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_CategoryInMenu_Category");

                entity.HasOne(d => d.Menu)
                    .WithMany(p => p.CategoryInMenus)
                    .HasForeignKey(d => d.MenuId)
                    .HasConstraintName("FK_CategoryInMenu_Menu");
            });

            modelBuilder.Entity<Collection>(entity =>
            {
                entity.ToTable("Collection");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.StoreId).HasMaxLength(50);

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.Collections)
                    .HasForeignKey(d => d.StoreId)
                    .HasConstraintName("FK_Collection_Store");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.FullName).HasMaxLength(50);

                entity.Property(e => e.Image).HasMaxLength(50);

                entity.Property(e => e.Phone).HasMaxLength(50);
            });

            modelBuilder.Entity<DeliveryMode>(entity =>
            {
                entity.ToTable("DeliveryMode");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<DeliveryShiftOfShipper>(entity =>
            {
                entity.ToTable("DeliveryShiftOfShipper");

                entity.Property(e => e.Id)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.HubId).HasMaxLength(50);

                entity.Property(e => e.ScheduleId)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.ShiftId)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.ShipperId).HasMaxLength(50);

                entity.Property(e => e.TaskType)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.HasOne(d => d.Hub)
                    .WithMany(p => p.DeliveryShiftOfShippers)
                    .HasForeignKey(d => d.HubId)
                    .HasConstraintName("FK_DeliveryShift_Hub");

                entity.HasOne(d => d.Schedule)
                    .WithMany(p => p.DeliveryShiftOfShippers)
                    .HasForeignKey(d => d.ScheduleId)
                    .HasConstraintName("FK_DeliveryShift_Schedule");

                entity.HasOne(d => d.Shift)
                    .WithMany(p => p.DeliveryShiftOfShippers)
                    .HasForeignKey(d => d.ShiftId)
                    .HasConstraintName("FK_DeliveryShift_Shift");

                entity.HasOne(d => d.Shipper)
                    .WithMany(p => p.DeliveryShiftOfShippers)
                    .HasForeignKey(d => d.ShipperId)
                    .HasConstraintName("FK_DeliveryShift_Shipper");
            });

            modelBuilder.Entity<FcmToken>(entity =>
            {
                entity.ToTable("FcmToken");

                entity.Property(e => e.Id)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.AccountId)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Token)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.FcmTokens)
                    .HasForeignKey(d => d.AccountId)
                    .HasConstraintName("FK_FcmToken_Account");
            });

            modelBuilder.Entity<Hub>(entity =>
            {
                entity.ToTable("Hub");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.BuildId).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.HasOne(d => d.Build)
                    .WithMany(p => p.Hubs)
                    .HasForeignKey(d => d.BuildId)
                    .HasConstraintName("FK_Hub_Building");
            });

            modelBuilder.Entity<Menu>(entity =>
            {
                entity.ToTable("Menu");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.DayFilter).HasMaxLength(50);

                entity.Property(e => e.EndDate).HasMaxLength(20);

                entity.Property(e => e.HourFilter).HasMaxLength(50);

                entity.Property(e => e.Image).HasMaxLength(150);

                entity.Property(e => e.ModeId).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(150);

                entity.Property(e => e.StartDate).HasMaxLength(20);

                entity.HasOne(d => d.Mode)
                    .WithMany(p => p.Menus)
                    .HasForeignKey(d => d.ModeId)
                    .HasConstraintName("FK_Menu_DeliveryMode");
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Notification");

                entity.Property(e => e.Date)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Id)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Image)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.NotiContent)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Time)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Title)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.UserId)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.HasOne(d => d.User)
                    .WithMany()
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Notification_Account");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.BuildingId).HasMaxLength(50);

                entity.Property(e => e.CustomerId).HasMaxLength(50);

                entity.Property(e => e.HubId).HasMaxLength(50);

                entity.Property(e => e.MenuId).HasMaxLength(50);

                entity.Property(e => e.StoreId).HasMaxLength(50);

                entity.Property(e => e.Total).HasMaxLength(50);

                entity.Property(e => e.Type).HasMaxLength(50);

                entity.HasOne(d => d.Building)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.BuildingId)
                    .HasConstraintName("FK_Order_Building");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_Order_Customer");

                entity.HasOne(d => d.Hub)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.HubId)
                    .HasConstraintName("FK_Order_Hub");

                entity.HasOne(d => d.Menu)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.MenuId)
                    .HasConstraintName("FK_Order_Menu");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.StoreId)
                    .HasConstraintName("FK_Order_Store");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.ToTable("OrderDetail");

                entity.Property(e => e.Id)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.OrderId).HasMaxLength(50);

                entity.Property(e => e.ProductInMenuId).HasMaxLength(50);

                entity.Property(e => e.Quantity)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_OrderDetail_Order");

                entity.HasOne(d => d.ProductInMenu)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.ProductInMenuId)
                    .HasConstraintName("FK_OrderDetail_ProductInMenu");
            });

            modelBuilder.Entity<OrderStatus>(entity =>
            {
                entity.ToTable("OrderStatus");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<OrderTask>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("OrderTask");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.OrderId).HasMaxLength(50);

                entity.Property(e => e.ShipperId).HasMaxLength(50);

                entity.Property(e => e.Status).HasMaxLength(50);

                entity.Property(e => e.Task).HasMaxLength(50);

                entity.HasOne(d => d.Order)
                    .WithMany()
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_OrderTask_Order");

                entity.HasOne(d => d.Shipper)
                    .WithMany()
                    .HasForeignKey(d => d.ShipperId)
                    .HasConstraintName("FK_OrderTask_Shipper");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("Payment");

                entity.Property(e => e.Id)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Amount)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.OrderId).HasMaxLength(50);

                entity.Property(e => e.Status)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Type)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_Payment_Order");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.CategoryId).HasMaxLength(50);

                entity.Property(e => e.Description).HasMaxLength(250);

                entity.Property(e => e.Image).HasMaxLength(250);

                entity.Property(e => e.LastUpdate).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.PackDescription).HasMaxLength(50);

                entity.Property(e => e.Status).HasMaxLength(50);

                entity.Property(e => e.StoreId).HasMaxLength(50);

                entity.Property(e => e.Unit).HasMaxLength(50);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_Product_ProductCategory");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.StoreId)
                    .HasConstraintName("FK_Product_ShopOwner");
            });

            modelBuilder.Entity<ProductInCollection>(entity =>
            {
                entity.ToTable("ProductInCollection");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.CollectionId).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.ProductId).HasMaxLength(50);

                entity.HasOne(d => d.Collection)
                    .WithMany(p => p.ProductInCollections)
                    .HasForeignKey(d => d.CollectionId)
                    .HasConstraintName("FK_ProductInCollection_Collection");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductInCollections)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_ProductInCollection_Product");
            });

            modelBuilder.Entity<ProductInMenu>(entity =>
            {
                entity.ToTable("ProductInMenu");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.MenuId).HasMaxLength(50);

                entity.Property(e => e.ProductId).HasMaxLength(50);

                entity.Property(e => e.Status).HasMaxLength(50);

                entity.HasOne(d => d.Menu)
                    .WithMany(p => p.ProductInMenus)
                    .HasForeignKey(d => d.MenuId)
                    .HasConstraintName("FK_ProductInMenu_Menu");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductInMenus)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_ProductInMenu_Product");
            });

            modelBuilder.Entity<ProductTag>(entity =>
            {
                entity.ToTable("ProductTag");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.ProductId).HasMaxLength(50);

                entity.Property(e => e.TagId).HasMaxLength(50);

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductTags)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_ProductTag_Product");

                entity.HasOne(d => d.Tag)
                    .WithMany(p => p.ProductTags)
                    .HasForeignKey(d => d.TagId)
                    .HasConstraintName("FK_ProductTag_Tag");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.Id)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Name)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Status)
                    .HasMaxLength(10)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.ToTable("Schedule");

                entity.Property(e => e.Id)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Day)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Month)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Year)
                    .HasMaxLength(10)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Shift>(entity =>
            {
                entity.ToTable("Shift");

                entity.Property(e => e.Id)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.EndTime)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Name)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.StartTime)
                    .HasMaxLength(10)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Shipper>(entity =>
            {
                entity.ToTable("Shipper");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.Address)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Age)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Email)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.FullName)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Image)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Phone)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Sex)
                    .HasMaxLength(10)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Store>(entity =>
            {
                entity.ToTable("Store");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.BrandId).HasMaxLength(50);

                entity.Property(e => e.BuildingId).HasMaxLength(50);

                entity.Property(e => e.CloseTime).HasMaxLength(50);

                entity.Property(e => e.Image).HasMaxLength(250);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.OpenTime).HasMaxLength(50);

                entity.Property(e => e.Phone).HasMaxLength(50);

                entity.Property(e => e.Rate)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Slogan).HasMaxLength(50);

                entity.Property(e => e.Status)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.StoreCategoryId).HasMaxLength(50);

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.Stores)
                    .HasForeignKey(d => d.BrandId)
                    .HasConstraintName("FK_ShopOwner_Brand");

                entity.HasOne(d => d.StoreCategory)
                    .WithMany(p => p.Stores)
                    .HasForeignKey(d => d.StoreCategoryId)
                    .HasConstraintName("FK_Store_StoreCategory");
            });

            modelBuilder.Entity<StoreCategory>(entity =>
            {
                entity.ToTable("StoreCategory");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Status)
                    .HasMaxLength(10)
                    .IsFixedLength();
            });

            modelBuilder.Entity<StoreInMenu>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("StoreInMenu");

                entity.Property(e => e.Id)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.MenuId).HasMaxLength(50);

                entity.Property(e => e.StoreId).HasMaxLength(50);

                entity.HasOne(d => d.Menu)
                    .WithMany()
                    .HasForeignKey(d => d.MenuId)
                    .HasConstraintName("FK_StoreInMenu_Menu");

                entity.HasOne(d => d.Store)
                    .WithMany()
                    .HasForeignKey(d => d.StoreId)
                    .HasConstraintName("FK_StoreInMenu_Store");
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.ToTable("Tag");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.Image).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<TimeOfOrderStage>(entity =>
            {
                entity.ToTable("TimeOfOrderStage");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.OrderId).HasMaxLength(50);

                entity.Property(e => e.StatusId).HasMaxLength(50);

                entity.Property(e => e.Time).HasMaxLength(50);

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.TimeOfOrderStages)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("FK_TimeOfOrderStage_OrderStatus");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
