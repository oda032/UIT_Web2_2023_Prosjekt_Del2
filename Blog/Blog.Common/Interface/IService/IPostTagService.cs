using Blog.Common.Model.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Common.Interface.IService
{
    public interface IPostTagService
    {
        Task<IEnumerable<PostTagDto>> GetPostTags();

        Task<int> AddPostTag(PostTagDto tagDto);

        Task CreatePostTags(List<PostTagDto> tags);
    }
}
