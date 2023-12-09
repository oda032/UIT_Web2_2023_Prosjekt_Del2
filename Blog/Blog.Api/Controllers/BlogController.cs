using AutoMapper;
using Blog.Common.Interface.IRepository;
using Blog.Common.Model.Dto;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("blogs")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IBlogRepository _blogRepository;
        private readonly IMapper _mapper;
        public BlogController(IBlogRepository blogRepository, IMapper mapper)
        {
            _blogRepository = blogRepository;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllBlogs()
        {
            var blogs = await _blogRepository.GetBlogs();
            var blogDtos = _mapper.Map<IEnumerable<BlogDto>>(blogs);
            return Ok(blogDtos);
        }

        [AllowAnonymous]
        [HttpGet("single/{blogId}")]
        public async Task<IActionResult> GetBlog(int blogId)
        {
            var blog = await _blogRepository.GetOneBlog(blogId);
            var blogDto = _mapper.Map<BlogDto>(blog);
            return Ok(blogDto);
        }
    }
}
