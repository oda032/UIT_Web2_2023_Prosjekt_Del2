using Blog.Common.Interface.IRepository;
using Blog.Common.Interface.IService;
using Blog.Common.Model.Dto;
using Newtonsoft.Json;
using System.Text;
using Blazored.LocalStorage;
using Blog.Common.Constant;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using Blog.Server.Helper;
using NuGet.Common;
using System.Security.Claims;

namespace Blog.Server.Service
{
    public class AccountService : IAccountService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorageService;

        public AccountService(HttpClient httpClient, ILocalStorageService localStorageService)
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;
        }

        public async Task<string> Login(UserDto userDto)
        {
            try
            {
                var userDtoJson = JsonConvert.SerializeObject(userDto);
                var userDtoContent = new StringContent(userDtoJson, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("account/verifyLogin", userDtoContent);
                var tokenString = await response.Content.ReadAsStringAsync();
                if( tokenString.Contains("Wrong credentials!!!"))
                {
                    throw new Exception();
                }

                await _localStorageService.SetItemAsStringAsync(Constant.Token, tokenString);

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", tokenString);

                return tokenString;
            }

            catch (Exception ex)
            {
                return String.Empty;
            }
        }

        public async Task<UserDto> Register(UserDto userDto)
        {
            var userDtoJson = JsonConvert.SerializeObject(userDto);
            var userDtoContent = new StringContent(userDtoJson, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("account/register", userDtoContent);
            var userString = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<UserDto>(userString);
            return user;
        }

        public async Task Logout()
        {
            try
            {
                var token = await _localStorageService.GetItemAsStringAsync(Constant.Token);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var respnse = await _httpClient.PostAsync("account/logout", null, CancellationToken.None);
                if (respnse.IsSuccessStatusCode)
                {
                    await _localStorageService.RemoveItemAsync(Constant.Token);
                    _httpClient.DefaultRequestHeaders.Authorization = null;
                }

                return;
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Error - {ex.Message}");
            }

        }

        public async Task<IEnumerable<UserDto>> GetUsers()
        {
            try
            {
                var response = await _httpClient.GetAsync("account/users");

                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<UserDto>>(content);
            }

            catch (Exception ex)
            {
                return Enumerable.Empty<UserDto>();
            }
        }
    }
}
