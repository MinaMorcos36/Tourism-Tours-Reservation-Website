using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TourismToursWebsite.Models;

public partial class TourismToursDbContext : DbContext
{
    public TourismToursDbContext()
    {
    }

    public TourismToursDbContext(DbContextOptions<TourismToursDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<Reservation> Reservations { get; set; }

    public virtual DbSet<Tour> Tours { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.;Database=TourismToursDB;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Cart__3214EC27AE7DEBD1");

            entity.ToTable("Cart");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AddedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Quantity).HasDefaultValue(1);
            entity.Property(e => e.TourId).HasColumnName("TourID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Tour).WithMany(p => p.Carts)
                .HasForeignKey(d => d.TourId)
                .HasConstraintName("FK__Cart__TourID__73BA3083");

            entity.HasOne(d => d.User).WithMany(p => p.Carts)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Cart__UserID__72C60C4A");
        });

        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Reservat__3214EC2761374F9D");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ReservationDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.TourId).HasColumnName("TourID");
            entity.Property(e => e.UserEmail).HasMaxLength(50);
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.UserName).HasMaxLength(50);
            entity.Property(e => e.UserPhone).HasMaxLength(50);

            entity.HasOne(d => d.Tour).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.TourId)
                .HasConstraintName("FK__Reservati__TourI__3C69FB99");

            entity.HasOne(d => d.User).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Reservati__UserI__3B75D760");
        });

        modelBuilder.Entity<Tour>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tours__3214EC278888D48A");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Location).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC27D911F7A3");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .HasDefaultValue("Customer");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
