using Blog.Common.Interface.IService;
using Blog.Common.Model.Dto;
using Newtonsoft.Json;

namespace Blog.Server.Service
{
    public class ImageDownloadService : IImageDownloadService
    {
        private readonly HttpClient _httpClient;

        public ImageDownloadService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<(byte[], string)> GetImageData(string urlString)
        {
            try
            {
                var bytes = await _httpClient.GetByteArrayAsync(urlString);
                var format = await GetFormat(urlString);
                return (bytes, format);
            }

            catch (Exception ex)
            {
                return (null, null);
            }
        }

        private async Task<string> GetFormat(string Url)
        {
            string lowerCaseUrl = Url.ToLower();    

            if (lowerCaseUrl.Contains("jpg"))
            {
                return "jpg";
            }
            else if (lowerCaseUrl.Contains("jpeg"))
            {
                return "jpeg";
            }
            else if (lowerCaseUrl.Contains("bmp"))
            {
                return "bmp";
            }
            else if (lowerCaseUrl.Contains("png"))
            {
                return "png";
            }

            return null;
        }
    }
}
