using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Common.Interface.IRepository
{
    public interface IBlogRepository : IRepository<Model.Entity.Blog>
    {
        public Task<IEnumerable<Model.Entity.Blog>> GetBlogs();
        public Task<Model.Entity.Blog> GetOneBlog(int? id);
    }
}
