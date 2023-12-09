using AutoMapper;
using Blog.Common.Model.Dto;
using Blog.Common.Model.Entity;

namespace Blog.Api.Mapping
{
    public class PostTagMappingProfile : Profile
    {
        public PostTagMappingProfile()
        {
            CreateMap<PostTag, PostTagDto>();
            CreateMap<PostTagDto, PostTag>();
        }
    }
}
