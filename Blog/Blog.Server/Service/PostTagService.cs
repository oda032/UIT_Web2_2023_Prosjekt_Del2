using Blazored.LocalStorage;
using Blog.Common.Interface.IService;
using Blog.Common.Model.Dto;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using Blog.Common.Constant;

namespace Blog.Server.Service
{
    public class PostTagService : IPostTagService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorageService;

        public PostTagService(HttpClient httpClient, ILocalStorageService localStorageService)
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;
        }
        public Task<IEnumerable<PostTagDto>> GetPostTags()
        {
            throw new NotImplementedException();
        }

        public Task<int> AddPostTag(PostTagDto postTagDto)
        {
            throw new NotImplementedException();
        }

        public async Task CreatePostTags(List<PostTagDto> postTagDtos)
        {
            try
            {
                var token = await _localStorageService.GetItemAsStringAsync(Constant.Token);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var postTagDtosJson = JsonConvert.SerializeObject(postTagDtos);
                var postTagDtosContent = new StringContent(postTagDtosJson, Encoding.UTF8, "application/json");
                await _httpClient.PostAsync("posttags/create/multiple", postTagDtosContent);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
