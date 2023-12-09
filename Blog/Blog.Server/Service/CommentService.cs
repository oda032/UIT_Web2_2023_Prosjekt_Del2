using Blazored.LocalStorage;
using Blog.Common.Interface.IService;
using Blog.Common.Model.Dto;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using Blog.Common.Constant;

namespace Blog.Server.Service
{
    public class CommentService : ICommentService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorageService;

        public CommentService(HttpClient httpClient, ILocalStorageService localStorageService)
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;
        }
        public async Task<IEnumerable<CommentDto>> GetComments(int postId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"comments/all/{postId}");

                var content = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<IEnumerable<CommentDto>>(content);
            }

            catch (Exception ex)
            {
                return Enumerable.Empty<CommentDto>();
            }
        }

        public async Task<int> CreateComment(CommentDto commentDto)
        {
            try
            {
                var token = await _localStorageService.GetItemAsStringAsync(Constant.Token);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var commentDtoJson = JsonConvert.SerializeObject(commentDto);
                var commentDtoContent = new StringContent(commentDtoJson, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("comments/create", commentDtoContent);
                var content = await response.Content.ReadAsStringAsync();
                var commentId = JsonConvert.DeserializeObject<int>(content);

                return commentId;
            }

            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task DeleteComment(CommentDto commentDto)
        {
            try
            {
                var token = await _localStorageService.GetItemAsStringAsync(Constant.Token);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var commentDtoJson = JsonConvert.SerializeObject(commentDto);
                var commentDtoContent = new StringContent(commentDtoJson, Encoding.UTF8, "application/json");
                await _httpClient.PostAsync("comments/delete", commentDtoContent);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
