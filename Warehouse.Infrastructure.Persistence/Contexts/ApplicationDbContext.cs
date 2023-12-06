using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using Warehouse.Application.Interfaces;
using Warehouse.Domain.Common;
using Warehouse.Domain.Entities;
using Warehouse.Infrastructure.Shared.Services;

namespace Warehouse.Infrastructure.Persistence.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IDateTimeService _dateTime;
        private readonly ILoggerFactory _loggerFactory;


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
            IDateTimeService dateTime,
            ILoggerFactory loggerFactory
            ) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            _dateTime = dateTime;
            _loggerFactory = loggerFactory;
        }

        public DbSet<Position> Positions => Set<Position>();
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Address> Addresses => Set<Address>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();

        public DbSet<Product> Products => Set<Product>();
        public DbSet<ProductCategory> ProductCategories => Set<ProductCategory>();
        public DbSet<ProductProductCategory> ProductProductCategories => Set<ProductProductCategory>();

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableBaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.Created = _dateTime.NowUtc;
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModified = _dateTime.NowUtc;
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            var _mockData = this.Database.GetService<IMockService>();
            var seedPositions = _mockData.SeedPositions(1000);

            modelBuilder.Entity<Position>().HasData(seedPositions);


            // Configure the tables
            modelBuilder.ApplyConfiguration(new OrderItemConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new ProductProductCategoryConfiguration());
            modelBuilder.ApplyConfiguration(new ProductCategoryConfiguration());

            // Generate seed data with Bogus
            var databaseSeeder = new DatabaseSeeder(10000);

            // Apply the seed data on the tables
            modelBuilder.Entity<Address>().HasData(databaseSeeder.Addresses);
            modelBuilder.Entity<Customer>().HasData(databaseSeeder.Customers);

            modelBuilder.Entity<Order>().HasData(databaseSeeder.Orders);
            modelBuilder.Entity<OrderItem>().HasData(databaseSeeder.OrderItems);


            modelBuilder.Entity<Product>().HasData(databaseSeeder.Products);
            modelBuilder.Entity<ProductCategory>().HasData(databaseSeeder.ProductCategories);
            modelBuilder.Entity<ProductProductCategory>().HasData(databaseSeeder.ProductProductCategories);


            base.OnModelCreating(modelBuilder);


        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(_loggerFactory);
        }
    }

    internal class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.CreationDate).IsRequired();
            builder.Property(x => x.Description).IsRequired();
        }
    }

    internal class ProductCategoryConfiguration : IEntityTypeConfiguration<ProductCategory>
    {
        public void Configure(EntityTypeBuilder<ProductCategory> builder)
        {
            builder.ToTable("ProductCategories");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired();
        }
    }

    internal class ProductProductCategoryConfiguration : IEntityTypeConfiguration<ProductProductCategory>
    {
        public void Configure(EntityTypeBuilder<ProductProductCategory> builder)
        {
            builder.ToTable("ProductProductCategories");

            builder.HasKey(x => new { x.ProductId, x.CategoryId });

            builder.HasOne(x => x.Product)
                .WithMany(x => x.ProductProductCategories)
                .HasForeignKey(x => x.ProductId);

            builder.HasOne(b => b.Category)
                .WithMany()
                .HasForeignKey(x => x.CategoryId);
        }
    }

    internal class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("OrderItems");
            builder.HasKey(x => x.Id);
        }
    }

}