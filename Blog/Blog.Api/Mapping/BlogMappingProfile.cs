using AutoMapper;
using Blog.Common.Model.Dto;

namespace Blog.Api.Mapping
{
    public class BlogMappingProfile : Profile
    {
        public BlogMappingProfile()
        {
            CreateMap<Common.Model.Entity.Blog, BlogDto>();
            CreateMap<BlogDto, Common.Model.Entity.Blog>();
        }
    }
}
