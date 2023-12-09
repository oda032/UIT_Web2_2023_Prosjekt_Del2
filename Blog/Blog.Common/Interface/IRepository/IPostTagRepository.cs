using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Common.Model.Entity;

namespace Blog.Common.Interface.IRepository
{
    public interface IPostTagRepository : IRepository<PostTag>
    {
        public Task<IEnumerable<PostTag>> GetPostTags();
        public Task<PostTag> GetOnePostTag(int? id);
    }
}
