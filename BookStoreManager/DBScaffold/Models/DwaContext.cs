using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DBScaffold.Models;

public partial class DwaContext : DbContext
{
    public DwaContext()
    {
    }

    public DwaContext(DbContextOptions<DwaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Administrator> Administrators { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<BookLocationLink> BookLocationLinks { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<Login> Logins { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserBorrowingReservation> UserBorrowingReservations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Administrator>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("Administrator_PK");

            entity.ToTable("Administrator");

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnName("UserID");

            entity.HasOne(d => d.User).WithOne(p => p.Administrator)
                .HasForeignKey<Administrator>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Administrator_User_FK");
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Idbook).HasName("Book_PK");

            entity.ToTable("Book");

            entity.Property(e => e.Idbook).HasColumnName("IDBook");
            entity.Property(e => e.GenreId).HasColumnName("GenreID");
            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.Genre).WithMany(p => p.Books)
                .HasForeignKey(d => d.GenreId)
                .HasConstraintName("Book_Genre_FK");
        });

        modelBuilder.Entity<BookLocationLink>(entity =>
        {
            entity.HasKey(e => e.Idbllink).HasName("BookLocationLink_PK");

            entity.ToTable("BookLocationLink");

            entity.Property(e => e.Idbllink).HasColumnName("IDBLLink");
            entity.Property(e => e.BookId).HasColumnName("BookID");
            entity.Property(e => e.LocationId).HasColumnName("LocationID");

            entity.HasOne(d => d.Book).WithMany(p => p.BookLocationLinks)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("BookLocationLink_Book_FK");

            entity.HasOne(d => d.Location).WithMany(p => p.BookLocationLinks)
                .HasForeignKey(d => d.LocationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("BookLocationLink_Location_FK");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.Idgenre).HasName("Genre_PK");

            entity.ToTable("Genre");

            entity.Property(e => e.Idgenre).HasColumnName("IDGenre");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.Idlocation).HasName("Location_PK");

            entity.ToTable("Location");

            entity.Property(e => e.Idlocation).HasColumnName("IDLocation");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Login>(entity =>
        {
            entity.HasKey(e => e.Idlogin).HasName("Login_PK");

            entity.ToTable("Login");

            entity.Property(e => e.Idlogin).HasColumnName("IDLogin");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.PasswordHash).HasMaxLength(256);
            entity.Property(e => e.PasswordSalt).HasMaxLength(256);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Logins)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Login_User_FK");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Iduser).HasName("User_PK");

            entity.ToTable("User");

            entity.Property(e => e.Iduser).HasColumnName("IDUser");
            entity.Property(e => e.MiddleNames).HasColumnName("Middle_Names");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Surname).HasMaxLength(100);
        });

        modelBuilder.Entity<UserBorrowingReservation>(entity =>
        {
            entity.HasKey(e => e.Idreservation).HasName("UserBorrowingReservation_PK");

            entity.ToTable("UserBorrowingReservation");

            entity.Property(e => e.Idreservation).HasColumnName("IDReservation");
            entity.Property(e => e.BllinkId).HasColumnName("BLLinkID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Bllink).WithMany(p => p.UserBorrowingReservations)
                .HasForeignKey(d => d.BllinkId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("UserBorrowingReservation_BookLocationLink_FK");

            entity.HasOne(d => d.User).WithMany(p => p.UserBorrowingReservations)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("UserBorrowingReservation_User_FK");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
