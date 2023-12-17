using Blog.Common.Model.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Common.Interface.IService
{
    public interface IImageService
    {
        Task<IEnumerable<ImageDto>> GetImages();
        Task<int> CreateImage(ImageDto imageDto);
        Task<ImageDto> GetImage(int imageId);
    }
}
