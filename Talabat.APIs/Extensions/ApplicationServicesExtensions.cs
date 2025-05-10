using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using Talabat.APIs.HandlingErrors;
using Talabat.APIs.Helpers;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;
using Talabat.Repository;
using Talabat.Service;

namespace Talabat.APIs.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddSingleton(typeof(ICacheService),typeof(CacheService));

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));

            services.AddScoped(typeof(IAuthService), typeof(AuthService));
            // add services for AutoMapper
            services.AddAutoMapper(typeof(MappingProfiles));
            // we can use that also
            //services.AddAutoMapper(M => M.AddProfile(MappingProfiles));

            // add services for Validation Error
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = ActionContext =>
                {
                    var errors = ActionContext.ModelState
                                              .Where(p => p.Value.Errors.Count() > 0)
                                              .SelectMany(p => p.Value.Errors) 
                                              .Select(e => e.ErrorMessage).ToArray();
                    var ValidationErrorResponse = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(ValidationErrorResponse);
                };
            });

            return services;
        }
    }
}
