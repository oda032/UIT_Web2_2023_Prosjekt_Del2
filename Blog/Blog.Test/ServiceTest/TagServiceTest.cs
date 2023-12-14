using Blazored.LocalStorage;
using Blog.Common.Interface.IService;
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
using Blog.DataAccess.Repository;
using Blog.Test.Helpers;

namespace Blog.Test.ServiceTest
{
    [TestClass]
    public class TagServiceTest
    {
        private Mock<HttpMessageHandler> mockHttpMessageHandler;
        private Mock<ILocalStorageService> mockLocalStorageService;
        private HttpClient httpClient;
        private TagDto fakeTagDto1;
        private TagDto fakeTagDto2;
        private TagDto fakeTagAddDto;
        private List<TagDto> fakeTagDtos;
        private TagService tagService;

        [TestInitialize]
        public void Initialize()
        {
            fakeTagDto1 = new TagDto
            {
                TagID = 1,
                TagName = "1"
            };

            fakeTagDto2 = new TagDto
            {
                TagID = 2,
                TagName = "2"
            };

            fakeTagAddDto = new TagDto
            {
                TagID = 3,
                TagName = "3"
            };

            fakeTagDtos = new List<TagDto> { fakeTagDto1, fakeTagDto2 };

            mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            httpClient = new HttpClient(mockHttpMessageHandler.Object);
            httpClient.BaseAddress = new Uri("https://example.com/");

            mockLocalStorageService = new Mock<ILocalStorageService>();
            mockLocalStorageService
                .Setup(ls => ls.GetItemAsStringAsync(It.IsAny<string>(), CancellationToken.None)).ReturnsAsync("abc");

            tagService = new TagService(httpClient, mockLocalStorageService.Object);
        }

        [TestMethod]
        public async Task GetTagsReturnsTagDtos()
        {
            // Arrange
            mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(fakeTagDtos), Encoding.UTF8),
                });

            // Action
            var result = await tagService.GetTags();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IEnumerable<TagDto>));
            var resultAsList = result.ToList();
            Assert.AreEqual(fakeTagDtos.Count, resultAsList.Count);
        }

        [TestMethod]
        public async Task GetTagsReturnsEmptyListWhenException()
        {
            // Arrange
            mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("Error..."));

            // Action
            var result = await tagService.GetTags();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
            Assert.IsInstanceOfType(result, typeof(IEnumerable<TagDto>));
        }


        [TestMethod]
        public async Task AddTagThrowExceptionBecauseNotImplemented()
        {
            await Assert.ThrowsExceptionAsync<NotImplementedException>(async () => await tagService.AddTag(fakeTagDto1));
        }

        [TestMethod]
        public async Task CreateTagsGetListOfTagIds()
        {
            // Arrange
            mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(new List<int>{1, 2})),
                });

            // Action
            var result = await tagService.CreateTags(fakeTagDtos);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(fakeTagDtos.Count, result.Count);
            Assert.AreEqual(fakeTagDtos[0].TagID, result[0]);
            Assert.AreEqual(fakeTagDtos[1].TagID, result[1]);
        }

        [TestMethod]
        public async Task CreateTagsGetEmptyListWhenException()
        {
            // Arrange
            mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("Error..."));

            // Action
            var result = await tagService.CreateTags(fakeTagDtos);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }
    }
}
