using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Blog.Common.Interface.IRepository;
using Blog.Common.Model.Entity;
using Blog.DataAccess.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Blog.DataAccess.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private ApplicationDbContext db;
        private UserManager<IdentityUser> userManager;

        public CommentRepository(ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            this.db = db;
            this.userManager = userManager;
        }

        public async Task<int> Add(Comment obj, string? commentOwnerName)
        {
            var user = await userManager.FindByNameAsync(commentOwnerName);
            obj.CommentOwner = user;
            obj.ObjectOwnerId = user.Id;
            obj.CommentOwnerID = user.Id;
            await db.Comments.AddAsync(obj);
            await db.SaveChangesAsync();
            return obj.CommentID;
        }

        public async Task Edit(Comment obj)
        {
            db.Comments.Update(obj);
            await db.SaveChangesAsync();
        }

        public async Task Delete(Comment obj)
        {
            db.Comments.Remove(obj);
            await db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Comment>> GetComments(int postId)
        {
            var comments = await db.Comments
                .Include(co => co.CommentOwner)
                .Include(co => co.Post)
                .Where(co => co.PostID.Equals(postId))
                .ToListAsync();
            return comments;
        }

        public async Task<Comment> GetOneComment(int? id)
        {
            var comment = await db.Comments
                .Include(co => co.Post)
                .Include(co => co.CommentOwner)
                .FirstOrDefaultAsync(co => co.CommentID.Equals(id));

            return comment;
        }
    }
}
