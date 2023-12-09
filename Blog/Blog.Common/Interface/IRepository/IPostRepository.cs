using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Common.Model.Entity;

namespace Blog.Common.Interface.IRepository
{
    public interface IPostRepository : IRepository<Post>
    {
        public Task<IEnumerable<Post>> GetPosts(int blogId);
        public Task<Post> GetOnePost(int? postId);
    }
}
