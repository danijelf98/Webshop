using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Webshop.Models.Dbo.Common;
using Webshop.Models.Dbo.CompanyModels;
using Webshop.Models.Dbo.OrderModels;
using Webshop.Models.Dbo.ProductModels;
using Webshop.Models.Dbo.UserModel;
using Webshop.Shared.Interfaces;

namespace Webshop.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public ApplicationDbContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<QuantityType>().HasData(
                new QuantityType { Id = 1, Name = "Day", Created = new DateTime(2024, 5, 22), Valid = true },
                new QuantityType { Id = 2, Name = "Month", Created = new DateTime(2024, 5, 22), Valid = true },
                new QuantityType { Id = 3, Name = "Year", Created = new DateTime(2024, 5, 22), Valid = true },
                new QuantityType { Id = 4, Name = "Kg", Created = new DateTime(2024, 5, 22), Valid = true },
                new QuantityType { Id = 5, Name = "Liter", Created = new DateTime(2024, 5, 22), Valid = true },
                new QuantityType { Id = 6, Name = "Dag", Created = new DateTime(2024, 5, 22), Valid = true },
                new QuantityType { Id = 7, Name = "PCS", Created = new DateTime(2024, 5, 22), Valid = true });

            modelBuilder.Entity<Address>().HasData(
                new Address
                {
                    Id = 1,
                    City = "Zagreb",
                    Created = new DateTime(2024, 5, 22),
                    Country = "Craotia",
                    Street = "Maksimirska",
                    Number = "101",
                    Valid = true
                });

            modelBuilder.Entity<Company>().HasData(
                new Company
                {
                    Id = 1,
                    Created = new DateTime(2024, 5, 22),
                    AddressId = 1,
                    FullName = "Company d.o.o",
                    ShortName = "Company",
                    VAT = "789456123",
                    Valid = true
                });  

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {

            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is IBaseTableAtributes && (
                e.State == EntityState.Added || e.State == EntityState.Modified));


            foreach (var entityEntry in entries)
            {
                switch (entityEntry.State)
                {
                    case EntityState.Modified:
                        ((IBaseTableAtributes)entityEntry.Entity).Updated = DateTime.Now;
                        break;
                    case EntityState.Added:
                        ((IBaseTableAtributes)entityEntry.Entity).Valid = true;
                        ((IBaseTableAtributes)entityEntry.Entity).Created = DateTime.Now;
                        break;
                    default:
                        break;
                }

            }
            return base.SaveChanges();
        }
        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {

            var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is IBaseTableAtributes && (
              e.State == EntityState.Added
              || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                switch (entityEntry.State)
                {
                    case EntityState.Deleted:
                        entityEntry.State = EntityState.Modified;
                        ((IBaseTableAtributes)entityEntry.Entity).Valid = false;
                        break;
                    case EntityState.Modified:
                        ((IBaseTableAtributes)entityEntry.Entity).Updated = DateTime.Now;
                        break;
                    case EntityState.Added:
                        ((IBaseTableAtributes)entityEntry.Entity).Valid = true;
                        ((IBaseTableAtributes)entityEntry.Entity).Created = DateTime.Now;
                        break;
                    default:
                        break;
                }

            }

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        #region Common

        public DbSet<Address> Addresses { get; set; }
        public DbSet<SessionItem> SessionItems { get; set; }
        

        #endregion

        #region Product Models

        public DbSet<ProductItem> ProductItems { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<QuantityType> QuantityTypes { get; set; }

        #endregion

        #region Company Models

        public DbSet<Company> Companies { get; set; }

        #endregion

        #region Order Models

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<BuyerFeedback> BuyerFeedbacks { get; set; }

        #endregion

    }
}
