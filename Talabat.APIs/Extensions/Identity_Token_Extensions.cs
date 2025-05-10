using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Talabat.Core.Entities.Identity;
using Talabat.Repository.Identity;

namespace Talabat.APIs.Extensions
{
    public static class Identity_Token_Extensions
    {
        public static IServiceCollection Add_Identity_Token_Extensions(this IServiceCollection service,IConfiguration configuration)
        {

            // Identity Services
            service.AddDbContext<AppIdentityDbContext>(OptionsBuilder =>
            {
                OptionsBuilder.UseSqlServer(configuration.GetConnectionString("IdentityConnection"));
            });

            service.AddIdentity<AppUser, IdentityRole>(options =>
            {

            })
            .AddEntityFrameworkStores<AppIdentityDbContext>(); // allow DI for the store layer (Repository Layer) to use AppIdentityDbContext

            service.AddApplicationServices(); // The implementation of services in Extension Folder

            service.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            })
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters()
                        {
                            ValidateIssuer = true,
                            ValidIssuer = configuration["Jwt:issuerIp"],
                            ValidateAudience = true,
                            ValidAudience = configuration["Jwt:audianceIp"],
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"])),
                            ClockSkew = TimeSpan.FromDays(double.Parse(configuration["Jwt:Expireassion"]))
                        };
                    });

            return service;
        }
    }
}
