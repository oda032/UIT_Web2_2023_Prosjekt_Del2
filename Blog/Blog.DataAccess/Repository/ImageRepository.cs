using Blog.Common.Interface.IRepository;
using Blog.Common.Model.Entity;
using Blog.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.DataAccess.Repository
{
    public class ImageRepository : IImageRepository
    {
        private ApplicationDbContext db;

        public ImageRepository(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task<int> Add(Image obj, string? objOwnerName = null)
        {
            await db.Images.AddAsync(obj);
            await db.SaveChangesAsync();
            return obj.ImageID;
        }

        public async Task Delete(Image obj)
        {
            throw new NotImplementedException();
        }

        public async Task Edit(Image obj)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Image>> GetImages()
        {
            var images = await db.Images
                .ToListAsync();
            return images;
        }

        public async Task<Image> GetOneImage(int? id)
        {
            var image = await db.Images.FirstOrDefaultAsync(image => image.ImageID.Equals(id));

            return image;
        }
    }
}
