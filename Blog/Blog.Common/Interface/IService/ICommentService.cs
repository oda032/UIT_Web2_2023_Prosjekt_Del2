using Blog.Common.Model.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Common.Interface.IService
{
    public interface ICommentService
    {
        Task<IEnumerable<CommentDto>> GetComments(int postId);
        Task<int> CreateComment(CommentDto commentDto);
        Task DeleteComment(CommentDto commentDto);
    }
}
