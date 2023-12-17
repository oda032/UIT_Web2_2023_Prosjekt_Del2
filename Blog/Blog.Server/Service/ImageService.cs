using Blazored.LocalStorage;
using Blog.Common.Constant;
using Blog.Common.Interface.IService;
using Blog.Common.Model.Dto;
using Blog.Common.Model.Entity;
using Blog.Server.Pages.Authentication;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Blog.Server.Service
{
    public class ImageService : IImageService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorageService;

        public ImageService(HttpClient httpClient, ILocalStorageService localStorageService)
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;
        }

        public async Task<int> CreateImage(ImageDto imageDto)
        {
            try
            {
                var token = await _localStorageService.GetItemAsStringAsync(Constant.Token);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var imageDtoJson = JsonConvert.SerializeObject(imageDto);
                var imageDtoContent = new StringContent(imageDtoJson, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("images/create", imageDtoContent);
                var content = await response.Content.ReadAsStringAsync();
                var imageId = JsonConvert.DeserializeObject<int>(content);

                return imageId;
            }

            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<ImageDto> GetImage(int imageId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"images/single");

                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ImageDto>(content);
            }

            catch (Exception ex)
            {
                return new ImageDto();
            }
        }

        public async Task<IEnumerable<ImageDto>> GetImages()
        {
            try
            {
                var response = await _httpClient.GetAsync($"images/all");

                var content = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<IEnumerable<ImageDto>>(content);
            }

            catch (Exception ex)
            {
                return Enumerable.Empty<ImageDto>();
            }
        }
    }
}
