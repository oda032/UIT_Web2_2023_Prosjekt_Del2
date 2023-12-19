using Blazored.LocalStorage;
using Blog.Common.Interface.IService;
using Blog.Common.Model.Dto;
using Blog.Server.Service;
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
    public class ImageServiceTest
    {
        private Mock<HttpMessageHandler> mockHttpMessageHandler;
        private Mock<ILocalStorageService> mockLocalStorageService;
        private HttpClient httpClient;
        private ImageDto fakeImageDto1;
        private ImageDto fakeImageDto2;
        private ImageDto fakeImageAddDto;
        private List<ImageDto> fakeImageDtos;
        private ImageService imageService;

        [TestInitialize]
        public void Initialize()
        {
            fakeImageDto1 = new ImageDto
            {
                ImageID = 1,
                Data = new byte[10],
                Format = "png",
                Name = "pict1",
                Url = "pict1.png"
            };

            fakeImageDto2 = new ImageDto
            {
                ImageID = 2,
                Data = new byte[10],
                Format = "png",
                Name = "pict2",
                Url = "pict2.png"
            };

            fakeImageAddDto = new ImageDto
            {
                ImageID = 3,
                Data = new byte[10],
                Format = "png",
                Name = "pict3",
                Url = "pict3.png"
            };

            fakeImageDtos = new List<ImageDto> { fakeImageDto1, fakeImageDto2 };

            mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            httpClient = new HttpClient(mockHttpMessageHandler.Object);
            httpClient.BaseAddress = new Uri("https://example.com/");

            mockLocalStorageService = new Mock<ILocalStorageService>();
            mockLocalStorageService
                .Setup(ls => ls.GetItemAsStringAsync(It.IsAny<string>(), CancellationToken.None)).ReturnsAsync("abc");

            imageService = new ImageService(httpClient, mockLocalStorageService.Object);
        }

        [TestMethod]
        public async Task GetImagesReturnsImageDtos()
        {
            // Arrange
            mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(fakeImageDtos), Encoding.UTF8),
                });

            // Action
            var result = await imageService.GetImages();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IEnumerable<ImageDto>));
            var resultAsList = result.ToList();
            Assert.AreEqual(fakeImageDtos.Count, resultAsList.Count);
        }

        [TestMethod]
        public async Task GetImagesReturnsEmptyListWhenException()
        {
            // Arrange
            mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("Error..."));

            // Action
            var result = await imageService.GetImages();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
            Assert.IsInstanceOfType(result, typeof(IEnumerable<ImageDto>));
        }

        [TestMethod]
        public async Task CreateImageGetIdOfNewImage()
        {
            // Arrange
            mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("3"),
                });

            // Action
            var result = await imageService.CreateImage(fakeImageAddDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(fakeImageAddDto.ImageID, result);
        }

        [TestMethod]
        public async Task CreateImageGetZeroWhenException()
        {
            // Arrange
            mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.InternalServerError
                });

            // Action
            var result = await imageService.CreateImage(fakeImageAddDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public async Task GetImageReturnsImageDto()
        {
            // Arrange
            mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(fakeImageDto1), Encoding.UTF8),
                });

            // Action
            var result = await imageService.GetImage(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ImageDto));
            Assert.AreEqual(fakeImageDto1.Name, result.Name);
        }

        [TestMethod]
        public async Task GetImageReturnsEmptyImageDtoWhenException()
        {
            // Arrange
            mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("Error..."));

            // Action
            var result = await imageService.GetImage(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(null, result.Name);
            Assert.IsInstanceOfType(result, typeof(ImageDto));
        }
    }
}
