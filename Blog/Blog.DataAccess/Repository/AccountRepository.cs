using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Blog.Common.Interface.IRepository;
using Blog.Common.Model.Dto;
using Blog.Common.Model.Entity;
using Blog.DataAccess.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Blog.DataAccess.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _conf;

        public AccountRepository(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, ApplicationDbContext db, IConfiguration conf)
        {
            _db = db;
            _conf = conf;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<UserDto> VerifyCredentials(UserDto userDto)
        {
            if (userDto.Email == null || userDto.Passwd == null || userDto.Email.Length == 0 || userDto.Passwd.Length == 0)
            {
                return null;
            }

            var user = await _userManager.FindByNameAsync(userDto.Email);
            if (user == null)
                return (null);

            var result = await _signInManager.PasswordSignInAsync(userDto.Email, userDto.Passwd, false, lockoutOnFailure: true);
            if (!result.Succeeded)
            {
                return null;
            }

            return new UserDto() { Id = user.Id, Email = user.UserName };
        }

        public string GenerateJwtToken(UserDto userDto)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var confKey = _conf.GetSection("TokenSettings")["SecretKey"];
            var key = Encoding.ASCII.GetBytes(confKey);
            var cIdentity = new ClaimsIdentity(new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.NameIdentifier, userDto.Id),
                    new Claim(ClaimTypes.Name, userDto.Email)
                });

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = cIdentity,
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }

        public async Task LogoutUser()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<UserDto> CreateUser(UserDto userDto)
        {
            var user = await _userManager.FindByNameAsync(userDto.Email);
            if (user == null)
            {
                var newUser = new IdentityUser
                {
                    UserName = userDto.Email,
                    Email = userDto.Email,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(newUser, userDto.Passwd);
                user = await _userManager.FindByNameAsync(userDto.Email);

                if (result.Succeeded)
                {
                    return new UserDto
                    {
                        Id = user.Id,
                        Email = userDto.Email,
                        Passwd = userDto.Passwd
                    };
                }
            }

            else
            {
                throw new Exception("User is already registered");
            }

            return null;

        }
    }
}
