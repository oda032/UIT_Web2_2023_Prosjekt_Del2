using System.Security.Principal;
using System.Text;
using AutoMapper;
using Blog.Common.Interface.IRepository;
using Blog.Common.Model.Dto;
using Blog.Common.Model.Entity;
using Blog.DataAccess.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Blog.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("posts")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;
        public PostController(IPostRepository postRepository, IMapper mapper)
        {
            _postRepository = postRepository;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet("all/{blogId}")]
        public async Task<IActionResult> GetAllPosts(int blogId)
        {
            var posts = await _postRepository.GetPosts(blogId);
            var postDtos = new List<PostDto>();
            foreach (var post in posts)
            {
                var postDto = new PostDto
                {
                    BlogID = blogId,
                    ObjectOwnerId = post.ObjectOwnerId,
                    PostDetails = post.PostDetails,
                    PostID = post.PostID,
                    PostOwnerID = post.PostOwnerID,
                    PostTitle = post.PostTitle,
                    PostOwnerName = post.PostOwner.UserName,
                    PostTags = _mapper.Map<List<PostTagDto>?>(post.PostTags)
                };

                postDtos.Add(postDto);
            }
            return Ok(postDtos);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreatePost(PostDto postDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var post = new Post
            {
                PostTitle = postDto.PostTitle,
                PostDetails = postDto.PostDetails,
                BlogID = postDto.BlogID
            };
            var postId = await _postRepository.Add(post, postDto.PostOwnerName);



            return Ok(postId);
        }

        [AllowAnonymous]
        [HttpGet("single/{postId}")]
        public async Task<IActionResult> GetPost(int postId)
        {
            var post = await _postRepository.GetOnePost(postId);
            var postDto = new PostDto
            {
                PostID = postId,
                ObjectOwnerId = post.ObjectOwnerId,
                PostDetails = post.PostDetails,
                PostOwnerID = post.PostOwnerID,
                PostTitle = post.PostTitle,
                PostOwnerName = post.PostOwner.UserName,
                PostTags = _mapper.Map<List<PostTagDto>?>(post.PostTags)
            };
            return Ok(postDto);
        }
    }
}
