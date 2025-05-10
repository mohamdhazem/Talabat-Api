using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities
{
    public class BasketItem
    {
        public int id { get; set; }
        public string productName { get; set; }
        public decimal price { get; set; }
        public string pictureUrl { get; set; }
        public string category { get; set; }
        public string brand { get; set; }

        public int quantity { get; set; }
    }
}
