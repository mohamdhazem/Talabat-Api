using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services.Contract;

namespace Talabat.Service
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<AppUser> _userManager;

        public AuthService(IConfiguration configuration, UserManager<AppUser> userManager) 
        {
            _configuration = configuration;
            _userManager = userManager;
        }
        public async Task<string> CreateToken(AppUser user)
        {

            var _claims = new List<Claim>()
            {
                new Claim(ClaimTypes.GivenName,user.DisplayName),
                new Claim(ClaimTypes.Email ,user.Email),
                new Claim(ClaimTypes.Name, user.UserName)
            }; // custom claims

            var roles =await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
                _claims.Add(new Claim(ClaimTypes.Role, role));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));

            var token = new JwtSecurityToken(

                issuer: _configuration["Jwt:issuerIp"],
                audience: _configuration["Jwt:audianceIp"],
                claims: _claims,
                expires: DateTime.UtcNow.AddDays(double.Parse(_configuration["Jwt:Expireassion"])),
                signingCredentials: new SigningCredentials(key,SecurityAlgorithms.HmacSha256Signature)

                );//predefined Claims

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
