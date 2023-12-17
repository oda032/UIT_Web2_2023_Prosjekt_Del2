using AutoMapper;
using Blog.Common.Model.Dto;
using Blog.Common.Model.Entity;

namespace Blog.Api.Mapping
{
    public class ImageMappingProfile : Profile
    {
        public ImageMappingProfile()
        {
            CreateMap<Image, ImageDto>();
            CreateMap<ImageDto, Image>();
        }
    }
}
