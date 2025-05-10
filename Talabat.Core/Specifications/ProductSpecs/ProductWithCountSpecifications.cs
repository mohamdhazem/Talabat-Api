using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications.ProductSpecs
{
    public class ProductWithCountSpecifications : BaseSpecifications<Product>
    {
        public ProductWithCountSpecifications(ProductSpecParams productSpecParams) : 
            base(  b =>
                        (!productSpecParams.brandId.HasValue || (b.BrandId == productSpecParams.brandId)) && // propbability of this side is (True(where brandId is null) or (b.BrandId == brandId) )
                        (!productSpecParams.categoryId.HasValue || (b.CategoryId == productSpecParams.categoryId))
            )
        {
            
        }
    }
}
