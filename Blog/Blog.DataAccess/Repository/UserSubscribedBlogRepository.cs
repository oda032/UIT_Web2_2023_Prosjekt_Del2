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
    public class UserSubscribedBlogRepository : IUserSubscribedBlogRepository
    {
        private ApplicationDbContext db;

        public UserSubscribedBlogRepository(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task<int> Add(UserSubscribedBlog obj, string? usbOwnerName = null)
        {
            await db.UserSubscribedBlogs.AddAsync(obj);
            await db.SaveChangesAsync();
            return obj.UserSubscribedBlogID;
        }

        public async Task Edit(UserSubscribedBlog obj)
        {
            db.UserSubscribedBlogs.Update(obj);
            await db.SaveChangesAsync();
        }

        public async Task Delete(UserSubscribedBlog obj)
        {
            db.UserSubscribedBlogs.Remove(obj);
            await db.SaveChangesAsync();
        }

        public async Task<IEnumerable<UserSubscribedBlog>> GetUserSubscribedBlogs(string userId)
        {
            var userSubscribedBlogs = await db.UserSubscribedBlogs
                //.Include(usb => usb.Blog)
                //.Include(usb => usb.ApplicationUser).ToListAsync();
                .Where(usb => usb.ApplicationUserID.Equals(userId)).ToListAsync();
            return userSubscribedBlogs;
        }

        public async Task<UserSubscribedBlog> GetOneUserSubscribedBlog(int? id)
        {
            var userSubscribedBlog = await db.UserSubscribedBlogs
                .Include(usb => usb.Blog)
                .Include(usb => usb.ApplicationUser)
                .FirstOrDefaultAsync(usb => usb.UserSubscribedBlogID.Equals(id));

            return userSubscribedBlog;
        }
    }
}
