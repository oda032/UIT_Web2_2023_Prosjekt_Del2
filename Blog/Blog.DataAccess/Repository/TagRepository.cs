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
    public class TagRepository : ITagRepository
    {
        private ApplicationDbContext db;

        public TagRepository(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task<int> Add(Tag obj, string? tagOwnerName = null)
        {
            await db.Tags.AddAsync(obj);
            await db.SaveChangesAsync();
            return obj.TagID;
        }

        public async Task Edit(Tag obj)
        {
            db.Tags.Update(obj);
            await db.SaveChangesAsync();
        }

        public async Task Delete(Tag obj)
        {
            db.Tags.Remove(obj);
            await db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Tag>> GetTags()
        {
            var tags = await db.Tags
                .ToListAsync();
            return tags;
        }

        public async Task<Tag> GetOneTag(int? id)
        {
            var tag = await db.Tags
                .FirstOrDefaultAsync(tag => tag.TagID.Equals(id));

            return tag;
        }
    }
}
