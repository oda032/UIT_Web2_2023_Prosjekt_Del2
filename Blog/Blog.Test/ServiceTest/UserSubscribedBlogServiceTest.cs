using AutoMapper;
using Blazored.LocalStorage;
using Blog.Common.Enum;
using Blog.Common.Interface.IRepository;
using Blog.Common.Interface.IService;
using Blog.Common.Model.Dto;
using Blog.Common.Model.Entity;
using Blog.Server.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Test.ServiceTest
{
    [TestClass]
    public class UserSubscribedBlogServiceTest
    {
        private Mock<HttpMessageHandler> mockHttpMessageHandler;
        private Mock<ILocalStorageService> mockLocalStorageService;
        private HttpClient httpClient;
        private ApplicationUser fakeAppUser2;
        private Common.Model.Entity.Blog fakeBlog1;
        private Common.Model.Entity.Blog fakeBlog2;
        private UserSubscribedBlog fakeUserSubscribedBlog1;
        private UserSubscribedBlogDto fakeUserSubscribedBlogDto1;
        private List<UserSubscribedBlog> fakeUserSubscribedBlogs;
        private List<UserSubscribedBlogDto> fakeUserSubscribedBlogDtos;
        private IdentityUser fakeIdentityUser1;
        private IdentityUser fakeIdentityUser2;
        private UserSubscribedBlogService userSubscribedBlogService;

        [TestInitialize]
        public void Initialize()
        {
            fakeIdentityUser1 = new IdentityUser
            {
                Id = "user1",
                UserName = "user1@test.com",
                Email = "user1@test.com",
            };

            fakeIdentityUser2 = new IdentityUser
            {
                Id = "user2",
                UserName = "user2@test.com",
                Email = "user2@test.com",
            };

            fakeBlog1 = new Common.Model.Entity.Blog
            {
                BlogDetails = "details",
                BlogID = 1,
                BlogOwner = fakeIdentityUser1,
                BlogOwnerID = fakeIdentityUser1.Id,
                BlogStatus = BlogStatus.Open,
                BlogTitle = "title",
                ObjectOwnerId = fakeIdentityUser1.Id
            };

            fakeBlog2 = new Common.Model.Entity.Blog
            {
                BlogDetails = "details",
                BlogID = 2,
                BlogOwner = fakeIdentityUser1,
                BlogOwnerID = fakeIdentityUser1.Id,
                BlogStatus = BlogStatus.Open,
                BlogTitle = "title",
                ObjectOwnerId = fakeIdentityUser1.Id
            };

            fakeAppUser2 = new ApplicationUser
            {
                Id = fakeIdentityUser2.Id,
                Email = fakeIdentityUser2.Email,
                UserName = fakeIdentityUser2.UserName,
            };

            fakeUserSubscribedBlog1 = new UserSubscribedBlog
            {
                ApplicationUser = fakeAppUser2,
                ApplicationUserID = fakeAppUser2.Id,
                BlogID = 1,
                Blog = fakeBlog1,
                UserSubscribedBlogID = 1
            };

            fakeUserSubscribedBlogDto1 = new UserSubscribedBlogDto
            {
                ApplicationUserID = fakeAppUser2.Id,
                BlogID = 1,
                UserSubscribedBlogID = 1
            };

            fakeUserSubscribedBlogs = new List<UserSubscribedBlog> { fakeUserSubscribedBlog1 };
            fakeUserSubscribedBlogDtos = new List<UserSubscribedBlogDto> { fakeUserSubscribedBlogDto1 };

            mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            httpClient = new HttpClient(mockHttpMessageHandler.Object);
            httpClient.BaseAddress = new Uri("https://example.com/");

            mockLocalStorageService = new Mock<ILocalStorageService>();
            mockLocalStorageService
                .Setup(ls => ls.GetItemAsStringAsync(It.IsAny<string>(), CancellationToken.None)).ReturnsAsync("abc");

            userSubscribedBlogService = new UserSubscribedBlogService(httpClient, mockLocalStorageService.Object);
        }

        [TestMethod]
        public async Task GetUserSubscribedBlogsReturnsUserSubscribedBlogDtos()
        {
            // Arrange
            mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(fakeUserSubscribedBlogDtos), Encoding.UTF8),
                });

            // Action
            var result = await userSubscribedBlogService.GetUserSubscribedBlogs("user1");

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IEnumerable<UserSubscribedBlogDto>));
            var resultAsList = result.ToList();
            Assert.AreEqual(fakeUserSubscribedBlogDtos.Count, resultAsList.Count);
        }

        [TestMethod]
        public async Task GetUserSubscribedBlogsReturnsEmptyListWhenException()
        {
            // Arrange
            mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("Error..."));

            // Action
            var result = await userSubscribedBlogService.GetUserSubscribedBlogs("user1");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
            Assert.IsInstanceOfType(result, typeof(IEnumerable<UserSubscribedBlogDto>));
        }

        [TestMethod]
        public async Task CreateUserSubscribeBlogGetIdOfNewUserSubscribedBlog()
        {
            // Arrange
            mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("3"),
                });

            // Action
            var result = await userSubscribedBlogService.CreateUserSubscribedBlog(new UserSubscribedBlogDto
            {
                UserSubscribedBlogID = 3,
                ApplicationUserID = fakeAppUser2.Id,
                BlogID = 2
            });

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result);
        }

        [TestMethod]
        public async Task CreateCommentGetZeroWhenException()
        {
            // Arrange
            mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.InternalServerError
                });

            // Action
            var result = await userSubscribedBlogService.CreateUserSubscribedBlog(new UserSubscribedBlogDto { });

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result);
        }

    }
}
