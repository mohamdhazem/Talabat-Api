using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using Talabat.APIs.Extensions;
using Talabat.APIs.HandlingErrors;
using Talabat.APIs.Helpers;
using Talabat.APIs.Middlewares;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Repositories.Contract;
using Talabat.Repository;
using Talabat.Repository.Data;
using Talabat.Repository.Identity;

namespace Talabat.APIs.Solution
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // to make update database auto during run the applicaiton 

            var builder = WebApplication.CreateBuilder(args);


            #region Configure Service
            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddSwaggerServices(); // The implementation of services in Extension Folder

            builder.Services.AddDbContext<StoreContext>(Options =>
            {
                Options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            // Identity & Token Services
            builder.Services.Add_Identity_Token_Extensions(builder.Configuration);

            // Redis Services
            builder.Services.AddSingleton<IConnectionMultiplexer>((provider) =>
            {
                var connectionString = builder.Configuration.GetConnectionString("Redis");
                return ConnectionMultiplexer.Connect(connectionString);
            });
            #endregion

            var app = builder.Build();

            #region Update-data base automatic

            // to make update database auto during run the applicaiton 
            //StoreContext storeContext = new StoreContext(); /* it is not correct as the constructor contain parameter not parameter less */
            //await storeContext.Database.MigrateAsync();

            // the correct to ask CLR to creat object from DbContext
            // we need to define scope and get services
            using var scope = app.Services.CreateScope();
            //try
            //{
                var services = scope.ServiceProvider; 
                var _dbContext = services.GetRequiredService<StoreContext>(); // Ask CLR to create object from DbContext Explicitly
                var _identityDbContext = services.GetRequiredService<AppIdentityDbContext>(); // Ask CLR to create object from DbContext Explicitly

            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            try
            {
                await _dbContext.Database.MigrateAsync(); // update-database
                await StoreContextSeed.SeedAsync(_dbContext);// Data Seeding

                await _identityDbContext.Database.MigrateAsync(); // update-database

                var userManager = services.GetRequiredService<UserManager<AppUser>>();// expilicilty
                await AppIdentityDbContextSeeding.UsersSeeding(userManager); // Data Seeding for Users

            }
            catch (Exception Ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(Ex, "An error has occured during apply the migration"); 
                
            }
            //}
            //finally { scope.Dispose(); } // to end scope after finishing 
 
            #endregion

            #region Configure kestrel Middlewares

            //Middlewares of Exception Handling 
            app.UseMiddleware<ExceptionMiddleware>();

            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerMiddleware(); // The implementation of services in Extension Folder
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();
            #endregion

            app.Run();
        }
    }
}
