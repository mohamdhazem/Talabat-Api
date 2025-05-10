using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Repository
{
    internal static class SpecificationsEvaluator<TEntity> where TEntity : BaseEnitiy 
    { 
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> InputQuery ,ISpecifications<TEntity> spec) /* it will return IQueryable of T + Get Query take two parameter (dbset<> , object from class Ispecification)*/
        {

            /************************************* Steps ************************************
             * Get All ==>> query =  _storeContext.Set<Product>().Include(p => p.Brand).Include(p => p.Category).ToListAsync();
             * Get     ==>> query =  _storeContext.Set<Product>().where(p => p.Id == id).Skip(10).Take(5).Include(p => p.Brand).Include(p => p.Category).firstOrDefault();
             * 
             * input Query ==>> _storeContext.Set<Product>()   --->> Query = InputQuery
             * Criteria    ==>> p => p.Id == id                --->> so i want to check here that criteria not equal null
             * where       ==>> query = query.where            --->> query = query.where(spec.Criteria)
             * include     ==>> Aggregation                    --->> 
             */

            var query = InputQuery; // initalize ==>> query = _storeContext.Set<T>

            // now i will check if creatria = null or not ====>> p => p.id = 10
            if(spec.Criteria is not null)
            {
                query = query.Where(spec.Criteria);
            }

            // orderBy query 
            if(spec.OrderBy is not null)
                query = query.OrderBy(spec.OrderBy);
                                      // OrderBy Exp that be in the specs object
            // orderByDesc query 
            if (spec.OrderByDesc is not null)
                query = query.OrderByDescending(spec.OrderByDesc);
                                               // OrderByDesc Exp that be in the specs object
            
            // query = _storeContext.Set<Product>().where(p => p.Id == id)

            // ApplY Pagination
            if (spec.IsPagination is true)
                query = query.Skip(spec.Skip).Take(spec.Take);

            // query = _storeContext.Set<Product>().where(p => p.Id == id).Skip(10).Take(5)

            // now we will to increase includes by aggregate
            /*
             * 1. p => p.Brand
             * 2. p => p.Category
             */

            query = spec.Includes.Aggregate(query, (currentQuery, includeExpression) => currentQuery.Include(includeExpression));

            // _storeContext.Set<Product>().where(p => p.Id == id).Include(p => p.Brand) ==>>  then store in currentQuery
            // _storeContext.Set<Product>().where(p => p.Id == id).Include(p => p.Brand).Include(p => p.Category)



            /*
             Ex for Aggregation 
                string[] names = {"Mohammed" , "Hassan" , "Ahmed"};
                string Message = "Hello" ;
                Message = names.Aggregate ==>> takes two parameter (1- value , parameter **** 2- take parameter)
                1- Message = names.Aggregate((str01 , str02) => $"{str01} {str02}" ==>> str01 = Mohammed ,
                    str02 = Hassan then ==>> str01 = Mohammed Hassan , str02 = Ahmed  so message will be Mohammed Hassan Ahmed

                2- Message = names.Aggregate(message , (str01 , str02) => $"{str01} {str02}" ==>> message will equal --> Hello Mohammed Hassan Ahmed
             */


            return query;
        }
    }
}
