using WebXeDapAPI.Models;
using Microsoft.EntityFrameworkCore;
using Type = WebXeDapAPI.Models.Type;

namespace WebXeDapAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        { }
        public DbSet<User> Users { get; set; }
        public DbSet<Type> Types { get; set; }
        public DbSet<Slide> Slides { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Product_Details> Product_Details { get; set; }
        public DbSet<Order_Details> Order_Details { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<AccessToken> AccessTokens { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<InputStock> InputStocks { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Ads> Ads { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                       new User
                       {
                           Id = -1,
                           Name = "Not logged in",
                           Email = "",
                           Password = "",
                           Address = "",
                           City = "",
                           Phone = "",
                           Gender = "",
                           Image = "",
                           DateOfBirth = "",
                           roles = Models.Enum.Roles.User,  // Assuming '1' corresponds to Roles.Customer
                           Create = DateTime.UtcNow
                       }
                   );

            modelBuilder.Entity<Delivery>()
                .Property(d => d.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Delivery>()
                .Property(d => d.CreatedTime)
                .HasColumnType("datetime")
                .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<Delivery>()
                .Property(d => d.UpdatedTime)
                .HasColumnType("datetime")
                .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<Delivery>()
                .HasOne(d => d.Payment)
                .WithOne()
                .HasForeignKey<Delivery>("PaymentId");

            modelBuilder.Entity<Slide>()
                .Property(x => x.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Order>()
                .Property(x => x.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Payment>()
                .Property(x => x.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Payment>()
                .Property(x => x.TotalPrice)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.User)
                .WithMany()
                .HasForeignKey("UserId");

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Order)
                .WithMany()
                .HasForeignKey("OrderId");

            modelBuilder.Entity<Payment>()
                .Property(p => p.CreatedTime)
                .HasColumnType("datetime")
                .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<Payment>()
                .Property(p => p.UpdatedTime)
                .HasColumnType("datetime")
                .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<Payment>()
                .Property(p => p.ExtraFee)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Payment>()
                .Property(p => p.Method)
                .HasConversion<string>();

            modelBuilder.Entity<Brand>()
                .Property(x => x.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Cart>()
                .Property(x => x.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Products>()
                .Property(x => x.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Products>()
                .Property(x => x.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Products>()
                .Property(x => x.PriceHasDecreased)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Order_Details>()
                .Property(x => x.TotalPrice)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Order_Details>()
                .Property(x => x.PriceProduc)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Cart>()
                .Property(x => x.TotalPrice)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Cart>()
                .Property(x => x.PriceProduct)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Product_Details>()
                .Property(x => x.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Product_Details>()
                .Property(x => x.PriceHasDecreased)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Slide>()
                .Property(x => x.PriceHasDecreased)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Comment>()
                .Property(x => x.Description)
                .HasColumnType("text");

            // Configure Product relationship in InputStock
            modelBuilder.Entity<InputStock>()
                .HasOne(i => i.Product)
                .WithMany() // No navigation property defined in Products, so use WithMany() without parameters
                .HasForeignKey("ProductId") // Assuming a foreign key "ProductId" is needed
                .OnDelete(DeleteBehavior.Restrict); // Use Restrict to prevent cascading delete

            // Configure Product relationship in Stock
            modelBuilder.Entity<Stock>()
                .HasOne(s => s.Product)
                .WithMany()
                .HasForeignKey("ProductId")
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
