using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using StackExchange.Redis;
using Talabat.Core.Entities;
using Talabat.Core.Services.Contract;

namespace Talabat.Repository
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _dataBase;

        public BasketRepository(IConnectionMultiplexer redis)
        {
            _dataBase = redis.GetDatabase();
        }
        public async Task<bool> DeleteBasketAsync(string basketId)
        {
            var result =await _dataBase.KeyDeleteAsync(basketId);
            return result;
        }

        public async Task<CustomerBasket?> GetBasketAsync(string basketId)
        {
            var result = await _dataBase.StringGetAsync(basketId);
            return result.IsNullOrEmpty? null : JsonSerializer.Deserialize<CustomerBasket>(result);
        }

        public async Task<bool> UpdateBasketAsync(CustomerBasket customerBasket)
        {
            var customerBasketSerialized = JsonSerializer.Serialize(customerBasket);
            var result = await _dataBase.StringSetAsync(customerBasket.id, customerBasketSerialized, TimeSpan.FromDays(10));

            return result;
        }
    }
}
