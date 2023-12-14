using AutoMapper;
using Blog.Common.Enum;
using Blog.Common.Interface.IRepository;
using Blog.Common.Model.Dto;
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
    public class BlogServiceTest
    {
        private Mock<HttpMessageHandler> mockHttpMessageHandler;
        private HttpClient httpClient;
        private BlogDto fakeBlogDto1;
        private BlogDto fakeBlogDto2;
        private List<BlogDto> fakeBlogDtos;
        private IdentityUser fakeIdentityUser;
        private BlogService blogService;

        [TestInitialize]
        public void Initialize()
        {
            fakeIdentityUser = new IdentityUser
            {
                Id = "user@test.com",
            };

            fakeBlogDto1 = new BlogDto
            {
                BlogDetails = "details",
                BlogID = 1,
                BlogOwnerID = fakeIdentityUser.Id,
                BlogStatus = BlogStatus.Open,
                BlogTitle = "title",
                ObjectOwnerId = fakeIdentityUser.Id
            };

            fakeBlogDto2 = new BlogDto
            {
                BlogDetails = "details",
                BlogID = 2,
                BlogOwnerID = fakeIdentityUser.Id,
                BlogStatus = BlogStatus.Open,
                BlogTitle = "title",
                ObjectOwnerId = fakeIdentityUser.Id
            };

            fakeBlogDtos = new List<BlogDto> { fakeBlogDto1, fakeBlogDto2 };

            mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            httpClient = new HttpClient(mockHttpMessageHandler.Object);
            httpClient.BaseAddress = new Uri("https://example.com/");

            blogService = new BlogService(httpClient);
        }

        [TestMethod]
        public async Task GetBlogsReturnsBlogDtos()
        {
            // Arrange
            mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(fakeBlogDtos), Encoding.UTF8),
            });

            // Action
            var result = await blogService.GetBlogs();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IEnumerable<BlogDto>));
            var resultAsList = result.ToList();
            Assert.AreEqual(fakeBlogDtos.Count, resultAsList.Count);

        }

        [TestMethod]
        public async Task GetBlogsReturnsEmptyListWhenException()
        {
            // Arrange
            mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("Error..."));

            // Action
            var result = await blogService.GetBlogs();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
            Assert.IsInstanceOfType(result, typeof(IEnumerable<BlogDto>));
        }

        [TestMethod]
        public async Task GetBlogReturnsBlogDto()
        {
            // Arrange
            mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(fakeBlogDto1), Encoding.UTF8),
                });

            // Action
            var result = await blogService.GetBlog(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BlogDto));
            Assert.AreEqual(fakeBlogDto1.BlogTitle, result.BlogTitle);
            Assert.AreEqual(fakeBlogDto1.BlogDetails, result.BlogDetails);
            Assert.AreEqual(fakeBlogDto1.BlogOwnerID, result.BlogOwnerID);

        }

        [TestMethod]
        public async Task GetBlogReturnsEmptyBlogDtoWhenException()
        {
            // Arrange
            mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("Error..."));

            // Action
            var result = await blogService.GetBlog(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(null, result.BlogDetails);
            Assert.AreEqual(null, result.BlogTitle);
            Assert.IsInstanceOfType(result, typeof(BlogDto));
        }
    }
}
