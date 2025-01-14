﻿using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models;

public partial class ThaoDuocMarketContext : IdentityDbContext<ApplicationUser>

{
    public ThaoDuocMarketContext()
    {
    }

    public ThaoDuocMarketContext(DbContextOptions<ThaoDuocMarketContext> options)
        : base(options)
    {
    }

   

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<PostImage> PostImages { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductImage> ProductImages { get; set; }

  

    public virtual DbSet<Shipper> Shippers { get; set; }

    public virtual DbSet<TransactStatus> TransactStatuses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=LAPTOP-3H13Q862\\NGTRONGHIA;Initial Catalog=ThaoDuocMarket;Integrated Security=True;Encrypt=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IdentityUser>().HasKey(u => u.Id);
        modelBuilder.Entity<IdentityRole>().HasKey(r => r.Id);
        modelBuilder.Entity<IdentityUserLogin<string>>().HasKey(l => new { l.LoginProvider, l.ProviderKey });
        modelBuilder.Entity<IdentityUserRole<string>>().HasKey(p => new { p.UserId, p.RoleId });
        modelBuilder.Entity<IdentityUserToken<string>>().HasKey(p => new { p.UserId, p.LoginProvider, p.Name });
        modelBuilder.Entity<IdentityUserRole<string>>(entity =>
        {
            entity.HasKey(ur => new { ur.UserId, ur.RoleId });
            entity.ToTable("AspNetUserRoles"); // Tên bảng tùy chỉnh nếu cần
        });
        

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CatId);

            entity.Property(e => e.CatId).HasColumnName("CatID");
            entity.Property(e => e.CatName).HasMaxLength(50);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.BirthDay).HasColumnType("datetime");
            entity.Property(e => e.DateCreate).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.LastLogin).HasColumnType("datetime");
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.Phone)
                .HasMaxLength(12)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.OrderDate).HasColumnType("datetime");
            entity.Property(e => e.PaymentDate).HasColumnType("datetime");
            entity.Property(e => e.PaymentId).HasColumnName("PaymentID");
            entity.Property(e => e.ShipDate).HasColumnType("datetime");
            entity.Property(e => e.StransacStatusId).HasColumnName("StransacStatusID");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_Orders_Customers");

            entity.HasOne(d => d.StransacStatus).WithMany(p => p.Orders)
                .HasForeignKey(d => d.StransacStatusId)
                .HasConstraintName("FK_Orders_TransactStatus");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.ToTable("OrderDetail");

            entity.Property(e => e.OrderDetailId).HasColumnName("OrderDetailID");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.ShipDate).HasColumnType("datetime");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK_OrderDetail_Orders");
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.Property(e => e.PostId).HasColumnName("PostID");
            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<PostImage>(entity =>
        {
            entity.HasKey(e => e.ImageId);

            entity.ToTable("PostImage");

            entity.HasOne(d => d.Post).WithMany(p => p.PostImages)
                .HasForeignKey(d => d.PostId)
                .HasConstraintName("FK_PostImage_Posts");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.CatId).HasColumnName("CatID");
            entity.Property(e => e.DateCreate).HasColumnType("datetime");
            entity.Property(e => e.DateModified).HasColumnType("datetime");
            entity.Property(e => e.ShortDesc).HasMaxLength(255);
            entity.Property(e => e.Thumb).HasMaxLength(255);

            entity.HasOne(d => d.Cat).WithMany(p => p.Products)
                .HasForeignKey(d => d.CatId)
                .HasConstraintName("FK_Products_Categories");
        });

        modelBuilder.Entity<ProductImage>(entity =>
        {
            entity.HasKey(e => e.ImageId);

            entity.ToTable("ProductImage");

            entity.Property(e => e.ProductId).HasColumnName("ProductID");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductImages)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductImage_Products");
        });

        

        modelBuilder.Entity<Shipper>(entity =>
        {
            entity.Property(e => e.ShipperId).HasColumnName("ShipperID");
            entity.Property(e => e.Phone)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.ShipDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<TransactStatus>(entity =>
        {
            entity.ToTable("TransactStatus");

            entity.Property(e => e.TransactStatusId).HasColumnName("TransactStatusID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
