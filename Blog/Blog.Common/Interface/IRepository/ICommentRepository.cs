using Blog.Common.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Common.Interface.IRepository
{
    public interface ICommentRepository : IRepository<Comment>
    {
        public Task<IEnumerable<Comment>> GetComments(int postId);
        public Task<Comment> GetOneComment(int? id);
    }
}
