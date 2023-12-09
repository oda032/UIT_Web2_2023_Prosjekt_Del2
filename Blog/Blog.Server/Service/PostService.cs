using System.Collections;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using Blazored.LocalStorage;
using Blog.Common.Constant;
using Blog.Common.Interface.IService;
using Blog.Common.Model.Dto;
using Newtonsoft.Json;


namespace Blog.Server.Service
{
    public class PostService : IPostService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorageService;

        public PostService(HttpClient httpClient, ILocalStorageService localStorageService)
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;
        }

        public async Task<IEnumerable<PostDto>> GetPosts(int blogId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"posts/all/{blogId}");

                var content = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<IEnumerable<PostDto>>(content);
            }

            catch (Exception ex)
            {
                return Enumerable.Empty<PostDto>();
            }
        }

        public async Task<int> CreatePost(PostDto postDto)
        {
            try
            {
                var token = await _localStorageService.GetItemAsStringAsync(Constant.Token);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var postDtoJson = JsonConvert.SerializeObject(postDto);
                var postDtoContent = new StringContent(postDtoJson, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("posts/create", postDtoContent);
                var content = await response.Content.ReadAsStringAsync();
                var postId = JsonConvert.DeserializeObject<int>(content);

                return postId;
            }

            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<PostDto> GetPost(int postId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"posts/single/{postId}");

                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<PostDto>(content);
            }

            catch (Exception ex)
            {
                return new PostDto();
            }
        }
    }
}
