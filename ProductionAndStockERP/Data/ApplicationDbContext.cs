using Microsoft.EntityFrameworkCore;
using ProductionAndStockERP.Models;

namespace ProductionAndStockERP.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
       : base(options)
        {
        }

        public DbSet<ActivityLogs> ActivityLogs { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<ProductionOrder> ProductionOrders { get; set; }
        public DbSet<StockTransaction> StockTransactions { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<Product> Products { get; set; }

        // Fluent API konfigurasyonları
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ActivityLogs>()
                .HasKey(a => a.LogId);

            modelBuilder.Entity<ActivityLogs>()
                .HasOne(a => a.User)
                .WithMany()
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<Order>()
                .HasKey(o => o.OrderId);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.CreatedBy) 
                .WithMany()
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<ProductionOrder>()
                .HasKey(p => p.ProductionId);
            modelBuilder.Entity<ProductionOrder>()
                .HasOne(p => p.Order)
                .WithMany()
                .HasForeignKey(p => p.OrderId)
                .OnDelete(DeleteBehavior.SetNull); 


            modelBuilder.Entity<ProductionOrder>()
                .HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<StockTransaction>()
                .HasKey(s => s.StockTxnId);

            modelBuilder.Entity<StockTransaction>()
                .HasOne(s => s.Order)
                .WithMany()
                .HasForeignKey(s => s.RelatedOrderId)
                .OnDelete(DeleteBehavior.SetNull); 

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(p => p.ProductId);
                entity.Property(p => p.Name).IsRequired().HasMaxLength(150);
                entity.Property(p => p.Description).HasMaxLength(500);
            });

            
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Product) 
                .WithMany()             
                .HasForeignKey(o => o.ProductId) 
                .OnDelete(DeleteBehavior.Restrict); 


            modelBuilder.Entity<ProductionOrder>()
                .HasOne(p => p.Product) 
                .WithMany()             
                .HasForeignKey(p => p.ProductId) 
                .OnDelete(DeleteBehavior.Restrict); 

           
            modelBuilder.Entity<StockTransaction>()
                .HasOne(s => s.Product)
                .WithMany()             
                .HasForeignKey(s => s.ProductId) 
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<Order>()
                .Property(o => o.Status)
                .HasConversion<string>();

            modelBuilder.Entity<ProductionOrder>()
                .Property(p => p.Status)
                .HasConversion<string>();

            modelBuilder.Entity<StockTransaction>()
                .Property(s => s.TransactionType)
                .HasConversion<string>();
        }
    }
}
