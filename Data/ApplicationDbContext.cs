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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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

        }
    }
}
