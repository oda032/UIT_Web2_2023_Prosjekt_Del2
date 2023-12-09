using Blog.Common.Model.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Common.Interface.IService
{
    public interface IPostService
    {
        Task<IEnumerable<PostDto>> GetPosts(int blogId);
        Task<int> CreatePost(PostDto postDto);
        Task<PostDto> GetPost(int postId);
    }
}
