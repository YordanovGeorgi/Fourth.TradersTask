using Fourth.TradersTask.Domain;
using Microsoft.EntityFrameworkCore;

namespace Fourth.TradersTask.Infrastructure.Data;

/// <summary>
/// DbContext for the Northwind database.
/// </summary>
public class NorthwindDbContext : DbContext
{
    public NorthwindDbContext(DbContextOptions<NorthwindDbContext> options) : base(options)
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }
    public virtual DbSet<Order> Orders { get; set; }
    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Customer configuration
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId)
                .HasName("PK_Customers");

            entity.Property(e => e.CustomerId)
                .HasColumnName("CustomerID")
                .HasMaxLength(5)
                .IsFixedLength();

            entity.Property(e => e.CompanyName)
                .HasMaxLength(40)
                .IsRequired();

            entity.Property(e => e.ContactName)
                .HasMaxLength(30);

            entity.Property(e => e.ContactTitle)
                .HasMaxLength(30);

            entity.Property(e => e.Address)
                .HasMaxLength(60);

            entity.Property(e => e.City)
                .HasMaxLength(15);

            entity.Property(e => e.Region)
                .HasMaxLength(15);

            entity.Property(e => e.PostalCode)
                .HasMaxLength(10);

            entity.Property(e => e.Country)
                .HasMaxLength(15);

            entity.Property(e => e.Phone)
                .HasMaxLength(24);

            entity.Property(e => e.Fax)
                .HasMaxLength(24);
        });

        // Order configuration
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId)
                .HasName("PK_Orders");

            entity.Property(e => e.OrderId)
                .HasColumnName("OrderID");

            entity.Property(e => e.CustomerId)
                .HasColumnName("CustomerID")
                .HasMaxLength(5)
                .IsFixedLength();

            entity.Property(e => e.Freight)
                .HasColumnType("money");

            entity.Property(e => e.ShipName)
                .HasMaxLength(40);

            entity.Property(e => e.ShipAddress)
                .HasMaxLength(60);

            entity.Property(e => e.ShipCity)
                .HasMaxLength(15);

            entity.Property(e => e.ShipRegion)
                .HasMaxLength(15);

            entity.Property(e => e.ShipPostalCode)
                .HasMaxLength(10);

            entity.Property(e => e.ShipCountry)
                .HasMaxLength(15);

            // Foreign key relationship
            entity.HasOne(d => d.Customer)
                .WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Orders_Customers");
        });

        // OrderDetail configuration
        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.ProductId })
                .HasName("PK_Order_Details");

            entity.ToTable("Order Details");

            entity.Property(e => e.OrderId)
                .HasColumnName("OrderID");

            entity.Property(e => e.ProductId)
                .HasColumnName("ProductID");

            entity.Property(e => e.UnitPrice)
                .HasColumnType("money");

            entity.HasOne(d => d.Order)
                .WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_Details_Orders");
        });
    }
}
