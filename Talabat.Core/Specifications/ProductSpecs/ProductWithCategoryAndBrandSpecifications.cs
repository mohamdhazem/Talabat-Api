using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications.ProductSpecs
{
    public class ProductWithCategoryAndBrandSpecifications : BaseSpecifications<Product>
    {
        /*This constructor will be used to create an Object, That will be used to Get All*/
        public ProductWithCategoryAndBrandSpecifications(ProductSpecParams productSpecParams) : // if (brandId or categoryId) is null this will make s.c
            base(b =>
                        (!productSpecParams.brandId.HasValue || (b.BrandId == productSpecParams.brandId)) && // propbability of this side is (True(where brandId is null) or (b.BrandId == brandId) )
                        (!productSpecParams.categoryId.HasValue || (b.CategoryId == productSpecParams.categoryId))
            )
        {
            AddIncludes();

            if (!string.IsNullOrEmpty(productSpecParams.sort))
            {
                switch (productSpecParams.sort) 
                {
                    case "Price":
                        AddOrederBy(p => p.Price);
                        break;

                    case "PriceDesc":
                        AddOrederByDesc(p => p.Price);
                        break;

                    default:
                        AddOrederBy(p => p.Name);
                        break;

                }
            }
            else
            {
                AddOrederBy(p => p.Name);
            }

            //pageIndex = 2
            //pageSize = 10

            ApplyPagination((productSpecParams.pageIndex - 1) * productSpecParams.pageSize, productSpecParams.pageSize);
        }

        /*This constructor will be used to create an Object, That will be used to Get specific product with id */
        public ProductWithCategoryAndBrandSpecifications(int id) : base(p => p.Id == id)
        {
            AddIncludes();
        }

        private void AddIncludes()
        {
            Includes.Add(p => p.Brand);
            Includes.Add(p => p.Category);
        }


    }
}
