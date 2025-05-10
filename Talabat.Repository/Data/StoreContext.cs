using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Repository.Data.Config;

namespace Talabat.Repository.Data
{
    public class StoreContext : DbContext
    {
        // the first solution but not prefered for Dependancy Injection 
        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // can make this to applay configuration 
            //modelBuilder.ApplyConfiguration(new ProductConfigurations());
            //modelBuilder.ApplyConfiguration(new ProductCategoryConfigurations());
            //modelBuilder.ApplyConfiguration(new ProductBrandConfigurations());

            // can user reflection  (using system.reflection)
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }


        public DbSet<Product> Products { get; set; }

        public DbSet<ProductBrand> ProductBrands { get; set; }

        public DbSet<ProductCategory> ProductCategories { get; set; }
    }
}
