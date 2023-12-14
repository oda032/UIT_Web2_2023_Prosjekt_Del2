using Blog.Common.Enum;
using Blog.Common.Model.Dto;
using Blog.Server.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Blog.Common.Constant;
using Blog.Common.Interface.IService;
using Moq.Protected;
using Newtonsoft.Json;
using System.Net;
using Blog.Common.Model.Entity;
using Microsoft.AspNetCore.Http;

namespace Blog.Test.ServiceTest
{
    [TestClass]
    public class CommentServiceTest
    {
        private Mock<HttpMessageHandler> mockHttpMessageHandler;
        private Mock<ILocalStorageService> mockLocalStorageService;
        private HttpClient httpClient;
        private CommentDto fakeCommentDto1;
        private CommentDto fakeCommentDto2;
        private CommentDto fakeCommentAddDto;
        private List<CommentDto> fakeCommentDtos;
        private IdentityUser fakeIdentityUser;
        private CommentService commentService;

        [TestInitialize]
        public void Initialize()
        {
            fakeIdentityUser = new IdentityUser
            {
                Id = "user@test.com",
            };

            fakeCommentDto1 = new CommentDto
            {
                CommentDetails = "details",
                BlogID = 1,
                CommentOwnerID = fakeIdentityUser.Id,
                CommentTitle = "title",
                ObjectOwnerId = fakeIdentityUser.Id,
                PostID = 1,
                CommentID = 1
            };

            fakeCommentDto2 = new CommentDto
            {
                CommentDetails = "details",
                BlogID = 1,
                CommentOwnerID = fakeIdentityUser.Id,
                CommentTitle = "title",
                ObjectOwnerId = fakeIdentityUser.Id,
                PostID = 1,
                CommentID = 2
            };

            fakeCommentAddDto = new CommentDto
            {
                CommentDetails = "details",
                BlogID = 1,
                CommentOwnerID = fakeIdentityUser.Id,
                CommentTitle = "title",
                ObjectOwnerId = fakeIdentityUser.Id,
                PostID = 1,
                CommentID = 3
            };

            fakeCommentDtos = new List<CommentDto> { fakeCommentDto1, fakeCommentDto2 };

            mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            httpClient = new HttpClient(mockHttpMessageHandler.Object);
            httpClient.BaseAddress = new Uri("https://example.com/");

            mockLocalStorageService = new Mock<ILocalStorageService>();
            mockLocalStorageService
                .Setup(ls => ls.GetItemAsStringAsync(It.IsAny<string>(), CancellationToken.None)).ReturnsAsync("abc");

            commentService = new CommentService(httpClient, mockLocalStorageService.Object);
        }

        [TestMethod]
        public async Task GetCommentsReturnsCommentDtos()
        {
            // Arrange
            mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(fakeCommentDtos), Encoding.UTF8),
                });

            // Action
            var result = await commentService.GetComments(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IEnumerable<CommentDto>));
            var resultAsList = result.ToList();
            Assert.AreEqual(fakeCommentDtos.Count, resultAsList.Count);
        }

        [TestMethod]
        public async Task GetCommentsReturnsEmptyListWhenException()
        {
            // Arrange
            mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("Error..."));

            // Action
            var result = await commentService.GetComments(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
            Assert.IsInstanceOfType(result, typeof(IEnumerable<CommentDto>));
        }

        [TestMethod]
        public async Task CreateCommentGetIdOfNewComment()
        {
            // Arrange
            mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("3"),
                });

            // Action
            var result = await commentService.CreateComment(fakeCommentAddDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(fakeCommentAddDto.CommentID, result);
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
            var result = await commentService.CreateComment(fakeCommentAddDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public async Task DeleteCommentCanDeleteComment()
        {
            // Arrange
            mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK
                });

            // Action
            await commentService.DeleteComment(fakeCommentDto1);

            // Assert
            mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(request =>
                    request.Method == HttpMethod.Post &&
                    request.RequestUri == new Uri("https://example.com/comments/delete")),
                ItExpr.IsAny<CancellationToken>());
        }

        [TestMethod]
        public async Task DeleteCommentThrowExceptionWhenException()
        {
            // Arrange
            mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("Error..."));

            // Action
            await Assert.ThrowsExceptionAsync<Exception>(async () => await commentService.DeleteComment(fakeCommentDto1));
        }
    }
}
