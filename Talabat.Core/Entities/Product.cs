using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities
{
    public class Product : BaseEnitiy
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }
        public string PictureUrl { get; set; }

        // Product <=> Category ( M ==>> 1 )
        public int CategoryId { get; set; } // Foregin  Key Colum ==>> ProductCategory
        public ProductCategory Category { get; set; } // navigational Property [ONE]

        // Product <=> Brand ( M ==>> 1 )
        //[ForeignKey(nameof(Product.Brand))] 
        public int BrandId { get; set; } // Foregin  Key Colum ==>> ProductBrand
        public ProductBrand Brand { get; set; } // navigational Property [ONE]

    }
}
