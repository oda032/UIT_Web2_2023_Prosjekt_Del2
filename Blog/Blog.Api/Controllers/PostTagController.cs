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
    [Route("posttags")]
    [ApiController]
    public class PostTagController : ControllerBase
    {
        private readonly IPostTagRepository _postTagRepository;
        private readonly IMapper _mapper;
        public PostTagController(IPostTagRepository postTagRepository, IMapper mapper)
        {
            _postTagRepository = postTagRepository;
            _mapper = mapper;
        }

        [HttpPost("create/multiple")]
        public async Task<IActionResult> CreatePostTags(List<PostTagDto> postTagDtos)
        {
            var postTags = _mapper.Map<IEnumerable<PostTag>>(postTagDtos);
            foreach (var postTag in postTags)
            {
                await _postTagRepository.Add(postTag, null);
            }

            return Ok();
        }
    }
}
