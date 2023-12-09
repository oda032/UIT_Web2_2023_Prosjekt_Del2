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
    [Route("comments")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;
        public CommentController(ICommentRepository commentRepository, IMapper mapper)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet("all/{postId}")]
        public async Task<IActionResult> GetAllComments(int postId)
        {
            var comments = await _commentRepository.GetComments(postId);
            var commentDtos = _mapper.Map<IEnumerable<CommentDto>>(comments);
            return Ok(commentDtos);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateComment(CommentDto commentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var comment = new Comment
            {
                CommentTitle = commentDto.CommentTitle,
                CommentDetails = commentDto.CommentDetails,
                BlogID = commentDto.BlogID,
                PostID = commentDto.PostID,
            };
            var commentId = await _commentRepository.Add(comment, commentDto.CommentOwnerName);

            return Ok(commentId);
        }
    }
}
