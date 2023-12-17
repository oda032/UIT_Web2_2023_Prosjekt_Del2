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
    [Route("images")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageRepository _imageRepository;
        private readonly IMapper _mapper;
        public ImageController(IImageRepository imageRepository, IMapper mapper)
        {
            _imageRepository = imageRepository;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllImages()
        {
            var images = await _imageRepository.GetImages();
            var imagesDtos = _mapper.Map<IEnumerable<ImageDto>>(images);
            return Ok(imagesDtos);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateImage(ImageDto imageDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var image = _mapper.Map<Image>(imageDto);
            var imageId = await _imageRepository.Add(image, null);

            return Ok(imageId);
        }

        [AllowAnonymous]
        [HttpGet("single")]
        public async Task<IActionResult> GetImage(int imageId)
        {
            var image = await _imageRepository.GetOneImage(imageId);
            var imageDto = _mapper.Map<ImageDto>(image);
            return Ok(imageDto);
        }
    }
}
