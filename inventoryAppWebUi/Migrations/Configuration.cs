using inventoryAppDomain.IdentityEntities;

namespace inventoryAppWebUi.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using inventoryAppDomain.Entities;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            SeedAdmin();
            SeedCategory();
            SeedRoles();
        }

        public static void SeedAdmin()
        {
            var userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>());
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>());
            var user = new IdentityUser()
            {
                UserName = "Admin@admin.com",
                Email = "Admin@admin.com",
                EmailConfirmed = true,
                // TwoFactorEnabled = true,
            };

            var result = userManager.Create(user, "Admin1234_");

            {
                if (result.Succeeded)
                {
                    var admin = new IdentityRole("Admin");
                    var roleResult = roleManager.Create(admin);

                    {
                        if (roleResult.Succeeded)
                        {
                            userManager.AddToRole(user.Id, "Admin");
                        }
                    }
                }
            }


        }
        public static void SeedRoles() 
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>());
            
            var cashier = new IdentityRole("Cashier");
            var storeManager = new IdentityRole("StoreManager");
            
            roleManager.Create(cashier);
            roleManager.Create(storeManager);
        }
        public static void SeedCategory()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var ProductCategory = new List<ProductCategory>
            {
                new ProductCategory() { CategoryName = "Perishable" },
                new ProductCategory() { CategoryName = "Kitchen" },
                new ProductCategory() { CategoryName = "Furniture" },
                new ProductCategory() { CategoryName = "Electronic" },
                new ProductCategory() { CategoryName = "Stationary" },
                new ProductCategory() { CategoryName = "Others" }
            };


            context.ProductCategories.AddRange(ProductCategory);
            context.SaveChanges();
        }
    }
}
