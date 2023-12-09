using AutoMapper;
using Blog.Common.Model.Dto;
using Blog.Common.Model.Entity;

namespace Blog.Api.Mapping
{
    public class CommentMappingProfile : Profile
    {
        public CommentMappingProfile()
        {
            CreateMap<Comment, CommentDto>().ForMember(
                dest => dest.CommentOwnerName, opt => opt.MapFrom(src => src.CommentOwner.UserName));
            CreateMap<CommentDto, Comment>();
        }
    }
}
