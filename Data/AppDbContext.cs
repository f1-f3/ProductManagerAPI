using Microsoft.EntityFrameworkCore;
using ProductManagerAPI.Models;


public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<ProductGroup> ProductGroups { get; set; }
    public DbSet<Store> Stores { get; set; }
    public DbSet<StoreProduct> StoreProducts { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //configuring Store Product M2M relationships
        modelBuilder.Entity<StoreProduct>()
            .HasKey(sp => new { sp.StoreID, sp.ProductID });

        modelBuilder.Entity<StoreProduct>()
            .HasOne(sp => sp.Store)
            .WithMany(s => s.StoreProducts)
            .HasForeignKey(sp => sp.StoreID);

        modelBuilder.Entity<StoreProduct>()
            .HasOne(sp => sp.Product)
            .WithMany(p => p.StoreProducts)
            .HasForeignKey(sp => sp.ProductID);


    }
}

