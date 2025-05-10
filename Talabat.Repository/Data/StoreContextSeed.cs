using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Repository.Data
{
    public static class StoreContextSeed
    {

        public static async Task SeedAsync(StoreContext context)
        {
            if (context.ProductBrands.Count() == 0)
            {
                var BrandsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/brands.json");
                var Brands = JsonSerializer.Deserialize<List<ProductBrand>>(BrandsData);

                if (Brands?.Count() > 0)
                {
                    // to take only the name of brand 
                    //Brands = Brands.Select(b => new ProductBrand
                    //{
                    //    Name = b.Name
                    //}).ToList();

                    foreach (var brand in Brands)
                    {
                        context.Set<ProductBrand>().Add(brand);
                    }
                    await context.SaveChangesAsync();
                } 
            }

            if (context.ProductCategories.Count() == 0)
            {
                var CategoriesData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/categories.json");
                var Categories = JsonSerializer.Deserialize<List<ProductCategory>>(CategoriesData);

                if (Categories?.Count() > 0)
                {

                    foreach (var category in Categories)
                    {
                        context.Set<ProductCategory>().Add(category);
                    }
                    await context.SaveChangesAsync();
                }
            }

            if (context.Products.Count() == 0)
            {
                var ProductsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/products.json");
                var Products = JsonSerializer.Deserialize<List<Product>>(ProductsData);

                if (Products?.Count() > 0)
                {
                    foreach (var product in Products)
                    {
                        context.Set<Product>().Add(product);
                    }
                    await context.SaveChangesAsync();
                }
            }

        }

    }
}
