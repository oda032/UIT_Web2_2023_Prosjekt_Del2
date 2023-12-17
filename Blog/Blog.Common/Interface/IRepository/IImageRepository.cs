using Blog.Common.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Common.Interface.IRepository
{
    public interface IImageRepository : IRepository<Image>
    {
        public Task<IEnumerable<Image>> GetImages();
        public Task<Image> GetOneImage(int? id);
    }
}
