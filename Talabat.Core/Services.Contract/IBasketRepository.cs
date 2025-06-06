﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Services.Contract
{
    public interface IBasketRepository
    {
        Task<CustomerBasket?> GetBasketAsync(string basketId);
        Task<bool> UpdateBasketAsync(CustomerBasket customerBasket);
        Task<bool> DeleteBasketAsync(string basketId);

    }
}
