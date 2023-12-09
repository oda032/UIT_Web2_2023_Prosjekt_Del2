using Blog.Common.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Common.Interface.IRepository
{
    public interface ITagRepository : IRepository<Tag>
    {
        public Task<IEnumerable<Tag>> GetTags();
        public Task<Tag> GetOneTag(int? id);
    }
}
