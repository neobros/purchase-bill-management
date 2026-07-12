using Microsoft.EntityFrameworkCore;
using PurchaseBill.Api.Entities;

namespace PurchaseBill.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<LocationDetail> LocationDetails => Set<LocationDetail>();
    public DbSet<PurchaseBillHeader> PurchaseBills => Set<PurchaseBillHeader>();
    public DbSet<PurchaseBillItem> PurchaseBillItems => Set<PurchaseBillItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LocationDetail>(entity =>
        {
            entity.ToTable("Location_Details");
            entity.HasIndex(l => l.LocationCode).IsUnique();
            entity.Property(l => l.LocationCode).HasMaxLength(50).IsRequired();
            entity.Property(l => l.LocationName).HasMaxLength(200).IsRequired();
        });

        modelBuilder.Entity<PurchaseBillHeader>(entity =>
        {
            entity.ToTable("Purchase_Bills");
        });

        modelBuilder.Entity<PurchaseBillItem>(entity =>
        {
            entity.ToTable("Purchase_Bill_Items");
            entity.Property(i => i.Item).HasMaxLength(100).IsRequired();
            entity.Property(i => i.Batch).HasMaxLength(200).IsRequired();
            entity.Property(i => i.StandardCost).HasColumnType("decimal(18,2)");
            entity.Property(i => i.StandardPrice).HasColumnType("decimal(18,2)");
            entity.Property(i => i.Margin).HasColumnType("decimal(18,2)");
            entity.Property(i => i.Discount).HasColumnType("decimal(5,2)");
            entity.Property(i => i.TotalCost).HasColumnType("decimal(18,2)");
            entity.Property(i => i.TotalSelling).HasColumnType("decimal(18,2)");

            entity.HasOne(i => i.Bill)
                  .WithMany(b => b.Items)
                  .HasForeignKey(i => i.PurchaseBillId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
