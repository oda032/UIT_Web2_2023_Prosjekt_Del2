using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Blog.Common.Interface.IRepository;
using Blog.DataAccess.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Blog.DataAccess.Repository
{
    public class BlogRepository : IBlogRepository
    {
        private ApplicationDbContext db;
        private UserManager<IdentityUser> userManager;

        public BlogRepository(ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            this.db = db;
            this.userManager = userManager;
        }

        public async Task<int> Add(Common.Model.Entity.Blog obj, string? blogOwnerName)
        {
            var user = await userManager.FindByNameAsync(blogOwnerName);
            obj.BlogOwner = user;
            obj.ObjectOwnerId = user.Id;
            obj.BlogOwnerID = user.Id;
            await db.Blogs.AddAsync(obj);
            await db.SaveChangesAsync();
            return obj.BlogID;
        }

        public async Task Edit(Common.Model.Entity.Blog obj)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(Common.Model.Entity.Blog obj)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Common.Model.Entity.Blog>> GetBlogs()
        {
            var result = await db.Blogs
                .Include(bl => bl.Posts)
                .Include(bl => bl.BlogOwner)
                .Include(bl => bl.UserSubscribedBlogs)
                .ToListAsync();
            return result;
        }

        public async Task<Common.Model.Entity.Blog> GetOneBlog(int? id)
        {
            var blog = await db.Blogs
                .Include(bl => bl.Posts)
                .Include(bl => bl.BlogOwner)
                .Include(bl => bl.UserSubscribedBlogs)
                .FirstOrDefaultAsync(bl => bl.BlogID.Equals(id));

            return blog;
        }
    }
}
