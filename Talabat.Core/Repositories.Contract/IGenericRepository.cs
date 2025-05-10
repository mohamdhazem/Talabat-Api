using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Core.Repositories.Contract
{
    public interface IGenericRepository<T> where T : BaseEnitiy
    {
        Task<T?> GetAsync(int id);
        Task<IReadOnlyList<T>> GetAllAsync();

        Task<IEnumerable<T>> GetAllWithSpecAsync(ISpecifications<T> specifications); 

        Task<T?> GetWithSpecAsync(ISpecifications<T> specifications);

        Task<int> GetCountWitSpecAsync(ISpecifications<T> specifications);

    }
}
