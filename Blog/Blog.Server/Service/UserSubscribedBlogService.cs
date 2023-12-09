using Blazored.LocalStorage;
using Blog.Common.Interface.IService;
using Blog.Common.Model.Dto;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using Blog.Common.Constant;

namespace Blog.Server.Service
{
    public class UserSubscribedBlogService : IUserSubscribedBlogService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorageService;

        public UserSubscribedBlogService(HttpClient httpClient, ILocalStorageService localStorageService)
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;
        }

        public async Task<IEnumerable<UserSubscribedBlogDto>> GetUserSubscribedBlogs(string userId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"usersubscribedblogs/all/{userId}");

                var content = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<IEnumerable<UserSubscribedBlogDto>>(content);
            }

            catch (Exception ex)
            {
                return Enumerable.Empty<UserSubscribedBlogDto>();
            }
        }

        public async Task<int> CreateUserSubscribedBlog(UserSubscribedBlogDto userSubscribedBlogDto)
        {
            try
            {
                var token = await _localStorageService.GetItemAsStringAsync(Constant.Token);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var usbDtoJson = JsonConvert.SerializeObject(userSubscribedBlogDto);
                var usbDtoContent = new StringContent(usbDtoJson, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("usersubscribedblogs/create", usbDtoContent);
                var content = await response.Content.ReadAsStringAsync();
                var usbId = JsonConvert.DeserializeObject<int>(content);

                return usbId;
            }

            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
