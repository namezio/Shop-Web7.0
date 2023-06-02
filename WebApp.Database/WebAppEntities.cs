using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Database;

public partial class WebAppEntities : DbContext
{
    public WebAppEntities()
    {
    }

    public WebAppEntities(DbContextOptions<WebAppEntities> options)
        : base(options)
    {
    }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductCategory> ProductCategories { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserType> UserTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySQL("Host=127.0.0.1;Database=WebShopDB;Username=root;Password=Kobiet99");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Orders", "WebShopDb");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.Amount).HasPrecision(10);
            entity.Property(e => e.DoneDate).HasColumnType("datetime");
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.OrderDate)
                .ValueGeneratedOnAddOrUpdate()
                .HasColumnType("datetime");
            entity.Property(e => e.OrderName).HasMaxLength(255);
            entity.Property(e => e.PaymentDate).HasColumnType("datetime");
            entity.Property(e => e.Phone).HasMaxLength(20);
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("OrderDetails", "WebShopDb");

            entity.HasIndex(e => e.IdOrder, "idOrder");

            entity.HasIndex(e => e.IdProduct, "idProduct");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Amount).HasPrecision(10);
            entity.Property(e => e.IdOrder).HasColumnName("idOrder");
            entity.Property(e => e.IdProduct).HasColumnName("idProduct");
            entity.Property(e => e.Price).HasPrecision(10);
            entity.Property(e => e.SellingPrice).HasPrecision(10);

            entity.HasOne(d => d.IdOrderNavigation).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.IdOrder)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("orderdetails_ibfk_1");

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.IdProduct)
                .HasConstraintName("orderdetails_ibfk_2");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Products", "WebShopDb");

            entity.HasIndex(e => e.IdCategory, "IdCategory");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CorverImageUrl).HasMaxLength(255);
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Price).HasPrecision(10);
            entity.Property(e => e.SellingPrice).HasPrecision(10);

            entity.HasOne(d => d.IdCategoryNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.IdCategory)
                .HasConstraintName("products_ibfk_1");
        });

        modelBuilder.Entity<ProductCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("ProductCategories", "WebShopDb");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.ThumbnailUrl).HasMaxLength(255);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Users", "WebShopDb");

            entity.HasIndex(e => e.IdUserType, "IdUserType");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AvartarUrl).HasMaxLength(255);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.FullName).HasMaxLength(50);
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.ModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.Phone).HasMaxLength(15);
            entity.Property(e => e.UserName).HasMaxLength(50);

            entity.HasOne(d => d.IdUserTypeNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.IdUserType)
                .HasConstraintName("users_ibfk_1");
        });

        modelBuilder.Entity<UserType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("UserTypes", "WebShopDb");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
