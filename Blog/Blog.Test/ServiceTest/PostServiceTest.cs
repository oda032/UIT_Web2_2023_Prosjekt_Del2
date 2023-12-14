using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Blog.Common.Interface.IService;
using Blog.Common.Model.Dto;
using Blog.Server.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;

namespace Blog.Test.ServiceTest
{
    [TestClass]
    public class PostServiceTest
    {
        private Mock<HttpMessageHandler> mockHttpMessageHandler;
        private Mock<ILocalStorageService> mockLocalStorageService;
        private HttpClient httpClient;
        private PostDto fakePostDto1;
        private PostDto fakePostDto2;
        private PostDto fakePostAddDto;
        private List<PostDto> fakePostDtos;
        private IdentityUser fakeIdentityUser;
        private PostService postService;

        [TestInitialize]
        public void Initialize()
        {
            fakeIdentityUser = new IdentityUser
            {
                Id = "user@test.com",
            };

            fakePostDto1 = new PostDto
            {
                PostDetails = "details",
                BlogID = 1,
                PostOwnerID = fakeIdentityUser.Id,
                PostTitle = "title",
                ObjectOwnerId = fakeIdentityUser.Id,
                PostID = 1
            };

            fakePostDto2 = new PostDto
            {
                PostDetails = "details",
                BlogID = 1,
                PostOwnerID = fakeIdentityUser.Id,
                PostTitle = "title",
                ObjectOwnerId = fakeIdentityUser.Id,
                PostID = 2
            };

            fakePostAddDto = new PostDto
            {
                PostDetails = "details",
                BlogID = 1,
                PostOwnerID = fakeIdentityUser.Id,
                PostTitle = "title",
                ObjectOwnerId = fakeIdentityUser.Id,
                PostID = 3
            };

            fakePostDtos = new List<PostDto> { fakePostDto1, fakePostDto2 };

            mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            httpClient = new HttpClient(mockHttpMessageHandler.Object);
            httpClient.BaseAddress = new Uri("https://example.com/");

            mockLocalStorageService = new Mock<ILocalStorageService>();
            mockLocalStorageService
                .Setup(ls => ls.GetItemAsStringAsync(It.IsAny<string>(), CancellationToken.None)).ReturnsAsync("abc");

            postService = new PostService(httpClient, mockLocalStorageService.Object);
        }

        [TestMethod]
        public async Task GetPostsReturnsPostDtos()
        {
            // Arrange
            mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(fakePostDtos), Encoding.UTF8),
                });

            // Action
            var result = await postService.GetPosts(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IEnumerable<PostDto>));
            var resultAsList = result.ToList();
            Assert.AreEqual(fakePostDtos.Count, resultAsList.Count);
        }

        [TestMethod]
        public async Task GetPostsReturnsEmptyListWhenException()
        {
            // Arrange
            mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("Error..."));

            // Action
            var result = await postService.GetPosts(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
            Assert.IsInstanceOfType(result, typeof(IEnumerable<PostDto>));
        }

        [TestMethod]
        public async Task CreateGetIdOfNewPost()
        {
            // Arrange
            mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("3"),
                });

            // Action
            var result = await postService.CreatePost(fakePostAddDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(fakePostAddDto.PostID, result);
        }

        [TestMethod]
        public async Task CreatePostGetZeroWhenException()
        {
            // Arrange
            mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.InternalServerError
                });

            // Action
            var result = await postService.CreatePost(fakePostAddDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public async Task GetPostReturnsPostDto()
        {
            // Arrange
            mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(fakePostDto1), Encoding.UTF8),
                });

            // Action
            var result = await postService.GetPost(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(PostDto));
            Assert.AreEqual(fakePostDto1.PostTitle, result.PostTitle);
            Assert.AreEqual(fakePostDto1.PostDetails, result.PostDetails);
            Assert.AreEqual(fakePostDto1.PostOwnerID, result.PostOwnerID);

        }

        [TestMethod]
        public async Task GetPostReturnsEmptyPostDtoWhenException()
        {
            // Arrange
            mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("Error..."));

            // Action
            var result = await postService.GetPost(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(null, result.PostDetails);
            Assert.AreEqual(null, result.PostTitle);
            Assert.IsInstanceOfType(result, typeof(PostDto));
        }
    }
}
