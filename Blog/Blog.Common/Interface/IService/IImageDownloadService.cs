using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Common.Interface.IService
{
    public interface IImageDownloadService
    {
        Task<(byte[], string)> GetImageData(string urlString);
    }
}
