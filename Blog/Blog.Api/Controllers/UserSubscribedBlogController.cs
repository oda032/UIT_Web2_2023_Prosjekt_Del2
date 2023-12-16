using AutoMapper;
using Blog.Common.Interface.IRepository;
using Blog.Common.Model.Dto;
using Blog.Common.Model.Entity;
using Blog.DataAccess.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("usersubscribedblogs")]
    [ApiController]
    public class UserSubscribedBlogController : ControllerBase
    {
        private readonly IUserSubscribedBlogRepository _userSubscribedBlogRepository;
        private readonly IMapper _mapper;
        public UserSubscribedBlogController(IUserSubscribedBlogRepository userSubscribedBlogRepository, IMapper mapper)
        {
            _userSubscribedBlogRepository = userSubscribedBlogRepository;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet("all/{userId}")]
        public async Task<IActionResult> GetUserSubscribedBlogs(string userId)
        {
            var usb = await _userSubscribedBlogRepository.GetUserSubscribedBlogs(userId);
            var usbDtos = _mapper.Map<IEnumerable<UserSubscribedBlogDto>>(usb);
            return Ok(usbDtos);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateUserSubscribedBlog(UserSubscribedBlogDto usbDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var usb = _mapper.Map<UserSubscribedBlog>(usbDto);
            var usbId = await _userSubscribedBlogRepository.Add(usb, null);

            return Ok(usbId);
        }

        [HttpPost("delete")]
        public async Task<IActionResult> DeleteUserSubscribedBlog(UserSubscribedBlogDto userSubscribedBlogDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var usb = _mapper.Map<UserSubscribedBlog>(userSubscribedBlogDto);

            await _userSubscribedBlogRepository.Delete(usb);

            return Ok();
        }
    }
}
