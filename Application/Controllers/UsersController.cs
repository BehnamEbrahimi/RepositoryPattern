using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Dtos;
using Core.Domain;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Application.Controllers
{

    public class UsersController : BaseController
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IJwtGenerator _jwtGenerator;
        private readonly IUserAccessor _userAccessor;

        public UsersController(UserManager<User> userManager, SignInManager<User> signInManager, IJwtGenerator jwtGenerator, IUserAccessor userAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtGenerator = jwtGenerator;
            _userAccessor = userAccessor;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(SaveUserDto saveUserDto)
        {
            if (await _userManager.Users.Where(u => u.Email == saveUserDto.Email).AnyAsync())
                return BadRequest("Email already exists");

            if (await _userManager.Users.Where(u => u.UserName == saveUserDto.Username).AnyAsync())
                return BadRequest("Username already exists");

            var user = new User
            {
                DisplayName = saveUserDto.DisplayName,
                Email = saveUserDto.Email,
                UserName = saveUserDto.Username
            };

            var result = await _userManager.CreateAsync(user, saveUserDto.Password);

            if (result.Succeeded)
            {
                return new UserDto
                {
                    DisplayName = user.DisplayName,
                    Token = _jwtGenerator.CreateToken(user),
                    Username = user.UserName,
                };
            }

            throw new Exception("Problem creating user");
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null)
                return Unauthorized("Invalid email or password");

            var result = await _signInManager
                .CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (result.Succeeded)
            {
                return new UserDto
                {
                    DisplayName = user.DisplayName,
                    Token = _jwtGenerator.CreateToken(user),
                    Username = user.UserName,
                };
            }

            return Unauthorized("Invalid email or password");
        }

        [HttpGet("me")]
        public async Task<ActionResult<UserDto>> CurrentUser()
        {
            var user = await _userManager.FindByNameAsync(_userAccessor.GetCurrentUsername());

            return new UserDto
            {
                DisplayName = user.DisplayName,
                Username = user.UserName,
                Token = _jwtGenerator.CreateToken(user),
            };
        }
    }
}