using AutoMapper;
using Blog.Common.Model.Dto;
using Blog.Common.Model.Entity;

namespace Blog.Api.Mapping
{
    public class PostMappingProfile : Profile
    {
        public PostMappingProfile()
        {
            CreateMap<Post, PostDto>().ForMember(dest => dest.PostTags, opt => opt.MapFrom(src => src.PostTags));
            //CreateMap<List<PostTag>, List<PostTagDto>>();
            //CreateMap<PostDto, Post>().ReverseMap();
           // CreateMap<PostDto, Post>().ForMember(dest => dest.PostTags, opt => opt.MapFrom(src => src.PostTags));

            CreateMap<PostTag, PostTagDto>();
        }
    }
}
