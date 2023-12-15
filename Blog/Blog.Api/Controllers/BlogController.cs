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
            var listOfValues = new List<(int, int)>();
            var blogs = await _blogRepository.GetBlogs();
            foreach (var blog in blogs)
            {
                var commentsSum = 0;
                var postsCount = blog.Posts.Count;
                foreach (var post in blog.Posts)
                {
                    commentsSum += post.Comments.Count;
                }
                listOfValues.Add((postsCount, commentsSum));
            }
            
            var blogDtos = _mapper.Map<IEnumerable<BlogDto>>(blogs);
            for(int i = 0; i < blogDtos.Count(); i++)
            {
                blogDtos.ElementAt(i).PostsCount = listOfValues[i].Item1;
                blogDtos.ElementAt(i).CommentsCount = listOfValues[i].Item2;
            }
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
