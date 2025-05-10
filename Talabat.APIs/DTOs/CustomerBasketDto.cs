using Talabat.Core.Entities;

namespace Talabat.APIs.DTOs
{
    public class CustomerBasketDto
    {
        public string id { get; set; }

        public List<BasketItemDto> items { get; set; }
    }
}
