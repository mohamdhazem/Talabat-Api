using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public interface ISpecifications<T> where T : BaseEnitiy
    {
        // make prob signiture for where and its lamda expression to get by id 
        // 2nd for include to get All 
        public Expression<Func<T,bool>> Criteria { get; set; } // this for where it is take parmeter and return bool

        public List<Expression<Func<T, object>>> Includes { get; set; } // this for includes it return list of expression 
                                                                        // it is take one parmeter and return object 

        public Expression<Func<T, object>> OrderBy { get; set; }
        public Expression<Func<T, object>> OrderByDesc { get; set; }

        public int Skip { get; set; }
        public int Take { get; set; }
        public bool IsPagination { get; set; }
    }
}
