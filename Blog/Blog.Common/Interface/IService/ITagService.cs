using Blog.Common.Model.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Common.Interface.IService
{
    public interface ITagService
    {
        Task<IEnumerable<TagDto>> GetTags();

        Task<int> AddTag(TagDto tagDto);

        Task<List<int>> CreateTags(List<TagDto> tags);
    }
}
