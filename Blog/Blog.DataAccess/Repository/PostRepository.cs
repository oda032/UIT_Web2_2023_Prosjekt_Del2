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
    public class PostRepository : IPostRepository
    {
        private ApplicationDbContext db;
        private UserManager<IdentityUser> userManager;

        public PostRepository(ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            this.db = db;
            this.userManager = userManager;
        }

        public async Task<int> Add(Post obj, string? postOwnerName)
        {
            var user = await userManager.FindByNameAsync(postOwnerName);
            obj.PostOwner = user;
            obj.ObjectOwnerId = user.Id;
            obj.PostOwnerID = user.Id;
            await db.Posts.AddAsync(obj);
            await db.SaveChangesAsync();
            return obj.PostID;
        }

        public async Task Edit(Post obj)
        {
            db.Posts.Update(obj);
            await db.SaveChangesAsync();
        }

        public async Task Delete(Post obj)
        {
            db.Posts.Remove(obj);
            await db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Post>> GetPosts(int blogId)
        {
            var posts = await db.Posts
                .Include(po => po.Comments)
                .Include(po => po.PostOwner)
                .Include(po => po.Blog)
                .Include(po => po.PostTags)
                .Where(po => po.BlogID.Equals(blogId))
                .ToListAsync();
            return posts;
        }

        public async Task<Post> GetOnePost(int? id)
        {
            var post = await db.Posts
                .Include(po => po.Blog)
                .Include(po => po.PostOwner)
                .Include(po => po.Comments)
                .Include(po => po.PostTags)
                .FirstOrDefaultAsync(po => po.PostID.Equals(id));

            return post;
        }
    }
}
