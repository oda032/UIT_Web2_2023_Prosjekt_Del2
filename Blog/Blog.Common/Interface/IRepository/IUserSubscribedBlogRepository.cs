using Blog.Common.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Common.Interface.IRepository
{
    public interface IUserSubscribedBlogRepository : IRepository<UserSubscribedBlog>
    {
        public Task<IEnumerable<UserSubscribedBlog>> GetUserSubscribedBlogs(string userId);
        public Task<UserSubscribedBlog> GetOneUserSubscribedBlog(int? id);
    }
}
