using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using inventoryAppDomain.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace inventoryAppDomain.IdentityEntities
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }



    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Product> Products { get; set; } 
        public DbSet<Cashier> Cashiers { get; set; }
        public DbSet<StoreManager> StoreManagers { get; set; }

        public DbSet<Notification> Notifications { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductCartItem> ProductCartItems { get; set; }

        public DbSet<ProductCart> ProductCarts { get; set; }
        public DbSet<Order> Orders { get; set; }
        
        public DbSet<Report> Reports { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}