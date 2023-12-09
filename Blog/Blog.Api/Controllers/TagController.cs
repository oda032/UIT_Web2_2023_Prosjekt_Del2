using AutoMapper;
using Blog.Common.Interface.IRepository;
using Blog.Common.Model.Dto;
using Blog.Common.Model.Entity;
using Blog.DataAccess.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace Blog.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("tags")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly ITagRepository _tagRepository;
        private readonly IMapper _mapper;
        public TagController(ITagRepository tagRepository, IMapper mapper)
        {
            _tagRepository = tagRepository;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllTags()
        {
            var tags = await _tagRepository.GetTags();
            var tagDtos = _mapper.Map<IEnumerable<TagDto>>(tags);
            return Ok(tagDtos);
        }

        [HttpPost("create/multiple")]
        public async Task<IActionResult> CreateTags(List<TagDto> tagDtos)
        {
            var tags = _mapper.Map<IEnumerable<Tag>>(tagDtos);
            var tagIds = new List<int>();
            foreach (var tag in tags)
            {
                var tagId = await _tagRepository.Add(tag, null);
                tagIds.Add(tagId);
            }
            
            return Ok(tagIds);
        }

    }
}
