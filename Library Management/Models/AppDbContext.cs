using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Library_Management.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<BookEdition> BookEditions { get; set; }

    public virtual DbSet<BookLibrary> BookLibraries { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Fine> Fines { get; set; }

    public virtual DbSet<Library> Libraries { get; set; }

    public virtual DbSet<Member> Members { get; set; }

    public virtual DbSet<Publisher> Publishers { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

     
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Author__3214EC07F5CE21C9");

            entity.ToTable("Author");

            entity.Property(e => e.BirthDate).HasColumnType("datetime");
            entity.Property(e => e.DeathDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Book__3214EC0796134583");

            entity.ToTable("Book");

            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.Author).WithMany(p => p.Books)
                .HasForeignKey(d => d.AuthorId)
                .HasConstraintName("FK__Book__AuthorId__2B3F6F97");

            entity.HasOne(d => d.Category).WithMany(p => p.Books)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__Book__CategoryId__2C3393D0");

            entity.HasOne(d => d.Publisher).WithMany(p => p.Books)
                .HasForeignKey(d => d.PublisherId)
                .HasConstraintName("FK__Book__PublisherI__2D27B809");
        });

        modelBuilder.Entity<BookEdition>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BookEdit__3214EC074914A7A4");

            entity.ToTable("BookEdition");

            entity.HasIndex(e => e.CopyNumber, "UQ__BookEdit__DC6AA2FFE30CE6D8").IsUnique();

            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.Book).WithMany(p => p.BookEditions)
                .HasForeignKey(d => d.BookId)
                .HasConstraintName("FK__BookEditi__BookI__30F848ED");
        });

        modelBuilder.Entity<BookLibrary>(entity =>
        {
            entity.HasKey(e => new { e.BookId, e.LibraryId }).HasName("PK__BookLibr__D7F3A67299345F5F");

            entity.ToTable("BookLibrary");

            entity.Property(e => e.AdditionalDetails).HasMaxLength(300);

            entity.HasOne(d => d.Book).WithMany(p => p.BookLibraries)
                .HasForeignKey(d => d.BookId)
                .HasConstraintName("FK__BookLibra__BookI__44FF419A");

            entity.HasOne(d => d.Library).WithMany(p => p.BookLibraries)
                .HasForeignKey(d => d.LibraryId)
                .HasConstraintName("FK__BookLibra__Libra__45F365D3");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Category__3214EC07D3A984D6");

            entity.ToTable("Category");

            entity.HasIndex(e => e.Name, "UQ__Category__737584F63281AE97").IsUnique();

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Fine>(entity =>
        {
            entity.HasKey(e => e.FineId).HasName("PK__Fine__9D4A9B2C53C32846");

            entity.ToTable("Fine");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Paid).HasDefaultValue(false);
            entity.Property(e => e.PaymentDate).HasColumnType("datetime");

            entity.HasOne(d => d.Transaction).WithMany(p => p.Fines)
                .HasForeignKey(d => d.TransactionId)
                .HasConstraintName("FK__Fine__Transactio__3B75D760");
        });

        modelBuilder.Entity<Library>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Library__3214EC07C0BE1BF1");

            entity.ToTable("Library");

            entity.Property(e => e.Location).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Member__3214EC07239A3027");

            entity.ToTable("Member");

            entity.HasIndex(e => e.Email, "UQ__Member__A9D105347E31A7F8").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.MembershipDate).HasColumnType("datetime");
            entity.Property(e => e.Phone).HasMaxLength(15);
        });

        modelBuilder.Entity<Publisher>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Publishe__3214EC07D21FE619");

            entity.ToTable("Publisher");

            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(15);
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("PK__Review__74BC79CE60C5571B");

            entity.ToTable("Review");

            entity.Property(e => e.Comment).HasMaxLength(500);
            entity.Property(e => e.ReviewDate).HasColumnType("datetime");

            entity.HasOne(d => d.Book).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.BookId)
                .HasConstraintName("FK__Review__BookId__3F466844");

            entity.HasOne(d => d.Member).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.MemberId)
                .HasConstraintName("FK__Review__MemberId__403A8C7D");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Transact__3214EC07527B858E");

            entity.Property(e => e.TransactionDate).HasColumnType("datetime");

            entity.HasOne(d => d.Edition).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.EditionId)
                .HasConstraintName("FK__Transacti__Editi__36B12243");

            entity.HasOne(d => d.Member).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.MemberId)
                .HasConstraintName("FK__Transacti__Membe__37A5467C");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
