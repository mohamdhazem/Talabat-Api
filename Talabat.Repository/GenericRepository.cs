using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Specifications;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEnitiy
    {
        private readonly StoreContext _storeContext;

        public GenericRepository(StoreContext storeContext)
        {
            _storeContext = storeContext;
        }
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            //if(typeof(T) == (typeof(Product)))
            //    return (IEnumerable<T>) await _storeContext.Set<Product>().Include(p => p.Brand).Include(p => p.Category).ToListAsync();
           return await _storeContext.Set<T>().ToListAsync();
        }
        public async Task<T?> GetAsync(int id)
        {
            //if(typeof(T) == (typeof(Product)))
            //    return (IEnumerable<T>) await _storeContext.Set<Product>().Where(p => p.Id == id).Include(p => p.Brand).Include(p => p.Category).ToListAsync();
            return await _storeContext.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllWithSpecAsync(ISpecifications<T> specifications)
        {
            //return await SpecificationsEvaluator<T>.GetQuery(_storeContext.Set<T>(), specifications).ToListAsync();
            return await ApplaySpecifications(specifications).ToListAsync();
        }


        public async Task<T?> GetWithSpecAsync(ISpecifications<T> specifications)
        {
            //return await SpecificationsEvaluator<T>.GetQuery(_storeContext.Set<T>(), specifications).FirstOrDefaultAsync();
            return await ApplaySpecifications(specifications).FirstOrDefaultAsync();
        }
        public async Task<int> GetCountWitSpecAsync(ISpecifications<T> specifications)
        {
            return await ApplaySpecifications(specifications).CountAsync();
        }

        private IQueryable<T> ApplaySpecifications(ISpecifications<T> specifications)
        {
            return SpecificationsEvaluator<T>.GetQuery(_storeContext.Set<T>(), specifications);
        }

    }
}
