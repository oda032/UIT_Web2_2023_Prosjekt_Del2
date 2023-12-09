using AutoMapper;
using Blog.Common.Model.Dto;
using Blog.Common.Model.Entity;

namespace Blog.Api.Mapping
{
    public class UserSubscribedBlogMappingProfile : Profile
    {
        public UserSubscribedBlogMappingProfile()
        {
            CreateMap<UserSubscribedBlog, UserSubscribedBlogDto>();
            CreateMap<UserSubscribedBlogDto, UserSubscribedBlog>();
        }
    }
}
