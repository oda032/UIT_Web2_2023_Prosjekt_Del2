using Blog.Common.Interface.IRepository;
using Blog.Common.Interface.IService;
using Blog.Common.Model.Dto;
using Newtonsoft.Json;

namespace Blog.Server.Service
{
    public class BlogService : IBlogService
    {
        private readonly HttpClient _httpClient;

        public BlogService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<BlogDto>> GetBlogs()
        {
            try
            {
                var response = await _httpClient.GetAsync("blogs/all");

                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<BlogDto>>(content);
            }

            catch (Exception ex)
            {
                return Enumerable.Empty<BlogDto>();
            }
        }

        public async Task<BlogDto> GetBlog(int blogId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"blogs/single/{blogId}");

                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<BlogDto>(content);
            }

            catch (Exception ex)
            {
                return new BlogDto();
            }
        }
    }
}
