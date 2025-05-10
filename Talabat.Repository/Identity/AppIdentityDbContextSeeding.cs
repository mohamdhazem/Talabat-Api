using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Talabat.Core.Entities.Identity;

namespace Talabat.Repository.Identity
{
    public static class AppIdentityDbContextSeeding
    {
        public static async Task UsersSeeding(UserManager<AppUser> userManager)
        {
            if(userManager.Users.Count() == 0) {
            
                var user = new AppUser
                {
                    DisplayName = "Mohamed hazem",
                    Email = "Mohamed@gmail.com",
                    UserName = "Mohamed.hazem",
                    PhoneNumber = "01122355564",
                };

                var result = await userManager.CreateAsync(user, "Pa$$w0rd");
                
                
            }
        }
    }
}
