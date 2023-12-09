using Blog.Common.Interface.IRepository;
using Blog.Common.Model.Dto;
using Blog.DataAccess.Data;
using Blog.DataAccess.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IAccountRepository _accountRepository;

        public AccountController(ApplicationDbContext db, UserManager<IdentityUser> userManager, IAccountRepository accountRepository)
        {
            _db = db;
            _userManager = userManager;
            _accountRepository = accountRepository;
        }

        [AllowAnonymous]
        [HttpPost("verifyLogin")]
        public async Task<IActionResult> VerifyLogin(UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            UserDto resultat = await _accountRepository.VerifyCredentials(userDto);

            if (resultat == null)
            {
                return Ok(new { res = "Wrong credentials!!! " });
            }

            return new ObjectResult(_accountRepository.GenerateJwtToken(resultat));
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _accountRepository.LogoutUser();

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserDto userDto)
        {
            var user = await _accountRepository.CreateUser(userDto);

            return Ok(user);
        }

        [AllowAnonymous]
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var userDtos = new List<UserDto>();
            var users = await _userManager.Users.ToListAsync();
            foreach (var user in users)
            {
                var userDto = new UserDto
                {
                    Id = user.Id,
                    Email = user.UserName
                };
                userDtos.Add(userDto);
            }

            return Ok(userDtos);
        }
    }
}
