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
    public class PostTagRepository : IPostTagRepository
    {
        private ApplicationDbContext db;

        public PostTagRepository(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task<int> Add(PostTag obj, string? postTagOwnerName = null)
        {
            await db.PostTags.AddAsync(obj);
            await db.SaveChangesAsync();
            return obj.PostTagID;
        }

        public async Task Edit(PostTag obj)
        {
            db.PostTags.Update(obj);
            await db.SaveChangesAsync();
        }

        public async Task Delete(PostTag obj)
        {
            db.PostTags.Remove(obj);
            await db.SaveChangesAsync();
        }

        public async Task<IEnumerable<PostTag>> GetPostTags()
        {
            var postTags = await db.PostTags
                .Include(pot => pot.Tag)
                .Include(pot => pot.Post)
                .ToListAsync();
            return postTags;
        }

        public async Task<PostTag> GetOnePostTag(int? id)
        {
            var postTag = await db.PostTags
                .Include(pot => pot.Tag)
                .Include(pot => pot.Post)
                .FirstOrDefaultAsync(pot => pot.PostTagID.Equals(id));

            return postTag;
        }
    }
}
