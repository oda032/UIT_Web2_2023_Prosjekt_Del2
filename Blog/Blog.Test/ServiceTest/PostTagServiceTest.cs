using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Blazored.LocalStorage;
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

namespace Blog.Test.ServiceTest
{
    [TestClass]
    public class PostTagServiceTest
    {
        private Mock<HttpMessageHandler> mockHttpMessageHandler;
        private Mock<ILocalStorageService> mockLocalStorageService;
        private HttpClient httpClient;
        private Tag fakeTag1;
        private Post fakePost1;
        private PostTag fakePostTag1;
        private PostTagDto fakePostTagDto1;
        private List<PostTag> fakePostTags;
        private List<PostTagDto> fakePostTagDtos;
        private IdentityUser fakeIdentityUser;
        private PostTagService postTagService;

        [TestInitialize]
        public void Initialize()
        {
            fakeIdentityUser = new IdentityUser
            {
                Id = "user@test.com"
            };

            fakeTag1 = new Tag
            {
                TagID = 1,
                TagName = "tag1"
            };

            fakePost1 = new Post
            {
                PostDetails = "details",
                BlogID = 1,
                PostOwner = fakeIdentityUser,
                PostOwnerID = fakeIdentityUser.Id,
                PostTitle = "title",
                ObjectOwnerId = fakeIdentityUser.Id,
                PostID = 1,
            };

            fakePostTag1 = new PostTag
            {
                PostTagID = 1,
                PostID = 1,
                Post = fakePost1,
                TagID = 1,
                Tag = fakeTag1
            };

            fakePostTagDto1 = new PostTagDto
            {
                PostTagID = 1,
                PostID = 1,
                TagID = 1,
            };

            fakePostTags = new List<PostTag> { fakePostTag1 };
            fakePostTagDtos = new List<PostTagDto> { fakePostTagDto1 };

            mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            httpClient = new HttpClient(mockHttpMessageHandler.Object);
            httpClient.BaseAddress = new Uri("https://example.com/");

            mockLocalStorageService = new Mock<ILocalStorageService>();
            mockLocalStorageService
                .Setup(ls => ls.GetItemAsStringAsync(It.IsAny<string>(), CancellationToken.None)).ReturnsAsync("abc");

            postTagService = new PostTagService(httpClient, mockLocalStorageService.Object);
        }

        [TestMethod]
        public async Task GetPostTagsThrowExceptionBecauseNotImplemented()
        {
            await Assert.ThrowsExceptionAsync<NotImplementedException>(async () => await postTagService.GetPostTags());
        }

        [TestMethod]
        public async Task AddPostTagThrowExceptionBecauseNotImplemented()
        {
            await Assert.ThrowsExceptionAsync<NotImplementedException>(async () => await postTagService.AddPostTag(It.IsAny<PostTagDto>()));
        }

        [TestMethod]
        public async Task CreatePostTagsCallPostJustOnce()
        {
            // Arrange
            mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(new List<int> { 1 })),
                });

            // Action
            await postTagService.CreatePostTags(fakePostTagDtos);

            // Assert
            mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(request =>
                    request.Method == HttpMethod.Post &&
                    request.RequestUri == new Uri("https://example.com/posttags/create/multiple")),
                ItExpr.IsAny<CancellationToken>());
        }

        [TestMethod]
        public async Task CreatePostTagsThrowExceptionWhenException()
        {
            // Arrange
            mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("Error..."));

            // Action
            await Assert.ThrowsExceptionAsync<Exception>(async () => await postTagService.CreatePostTags(fakePostTagDtos));
        }
    }
}
