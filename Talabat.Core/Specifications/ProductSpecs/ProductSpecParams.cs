using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Specifications.ProductSpecs
{
    public class ProductSpecParams
    {
        public string? sort { get; set; }
        public int? brandId { get; set; }
        public int? categoryId { get; set; }
        
        // Pagination params

        private const int MaxPageSize = 10;

        private int _pageSize = 5;
        public int pageSize 
        {
            get { return _pageSize; }
            set { _pageSize = value > MaxPageSize ? MaxPageSize : value; }
        }
        public int pageIndex { get; set; } = 1;

    }
}
