using Blazored.LocalStorage;
using Blog.Common.Interface.IService;
using Blog.Common.Model.Dto;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Blog.Common.Constant;

namespace Blog.Server.Service
{
    public class TagService : ITagService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorageService;

        public TagService(HttpClient httpClient, ILocalStorageService localStorageService)
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;
        }

        public async Task<IEnumerable<TagDto>> GetTags()
        {
            try
            {
                var response = await _httpClient.GetAsync("tags/all");

                var content = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<IEnumerable<TagDto>>(content);
            }

            catch (Exception ex)
            {
                return Enumerable.Empty<TagDto>();
            }
        }

        public async Task<int> AddTag(TagDto tagDto)
        {
            throw new NotImplementedException();
        }

        public async Task<List<int>> CreateTags(List<TagDto> tagDtos)
        {
            try
            {
                var token = await _localStorageService.GetItemAsStringAsync(Constant.Token);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var tagDtosJson = JsonConvert.SerializeObject(tagDtos);
                var tagDtosContent = new StringContent(tagDtosJson, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("tags/create/multiple", tagDtosContent);
                var content = await response.Content.ReadAsStringAsync();
                var listTagIds = JsonConvert.DeserializeObject<List<int>>(content);

                return listTagIds;
            }

            catch (Exception ex)
            {
                return new List<int>();
            }
        }
    }
}
