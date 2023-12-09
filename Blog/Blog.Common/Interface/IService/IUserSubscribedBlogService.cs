using Blog.Common.Model.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Common.Model.Entity;

namespace Blog.Common.Interface.IService
{
    public interface IUserSubscribedBlogService
    {
        Task<IEnumerable<UserSubscribedBlogDto>> GetUserSubscribedBlogs(string userId);

        Task<int> CreateUserSubscribedBlog(UserSubscribedBlogDto userSubscribedBlogDto);
    }
}
