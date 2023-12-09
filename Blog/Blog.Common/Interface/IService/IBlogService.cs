using Blog.Common.Model.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Common.Interface.IService
{
    public interface IBlogService
    {
        Task<IEnumerable<BlogDto>> GetBlogs();
        Task<BlogDto> GetBlog(int blogId);
    }
}
