using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public class BaseSpecifications<T> : ISpecifications<T> where T : BaseEnitiy
    {
        public Expression<Func<T, bool>> Criteria { get; set; } = null;
        public List<Expression<Func<T, object>>> Includes { get; set; } = new List<Expression<Func<T, object>>> ();

        public Expression<Func<T, object>> OrderBy { get; set; } = null;
        public Expression<Func<T, object>> OrderByDesc { get; set; } = null;

        public int Skip { get; set; }
        public int Take { get; set; }
        public bool IsPagination { get; set; } = false;


        public BaseSpecifications()
        {
            // to make criteria = null ==>> there is no where like Get All
        }

        public BaseSpecifications(Expression<Func<T, bool>> CriteriaExpression)
        {
            Criteria = CriteriaExpression; // p => p.brand == 10
        }

        protected void AddOrederBy(Expression<Func<T, object>> orderBy)
        {
            OrderBy = orderBy;
        }

        protected void AddOrederByDesc(Expression<Func<T, object>> orderByDesc)
        {
            OrderByDesc = orderByDesc;
        }

        protected void ApplyPagination(int skip,int take)
        {
            IsPagination = true;
            Skip = skip;
            Take = take;
        }
    }

}
