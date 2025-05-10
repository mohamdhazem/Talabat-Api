using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.DTOs;
using Talabat.APIs.HandlingErrors;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services.Contract;

namespace Talabat.APIs.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IAuthService _authService;

        public AccountController(UserManager<AppUser> userManager 
                                , SignInManager<AppUser> signInManager
                                , IAuthService authService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authService = authService;
        }

        [HttpPost("Login")] // Api/Account/Login
        public async Task<ActionResult<UserDto>> Login(LogInDto logInDto)
        {
            var user = await _userManager.FindByEmailAsync(logInDto.Email);
            if(user == null)
                return Unauthorized(new ApiResponse(StatusCodes.Status401Unauthorized));

            var result = await _signInManager.CheckPasswordSignInAsync(user , logInDto.Password, false);
            if(result.Succeeded is false)
                return Unauthorized(new ApiResponse(StatusCodes.Status401Unauthorized));

            return Ok(new UserDto
            {
                Mail = user.Email,
                UserName = user.UserName,
                Token = "Will be generated"
            });
        }

        [HttpPost("Register")] // Api/Account/Register
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {
            AppUser user = new AppUser
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                UserName = model.Email.Split('@')[0],
                PhoneNumber = model.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user);
            if (result.Succeeded is false)
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest));

            return Ok(new UserDto
            {
                Mail = user.Email,
                UserName = user.UserName,
                Token = await _authService.CreateToken(user)
            });
        }
    }
}
