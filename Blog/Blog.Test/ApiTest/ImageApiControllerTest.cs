using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Blog.Api.Controllers;
using Blog.Common.Interface.IRepository;
using Blog.Common.Model.Dto;
using Blog.Common.Model.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Blog.Test.ApiTest
{
    [TestClass]
    public class ImageApiControllerTest
    {
        private Mock<IImageRepository> mockImageRepository;
        private IMapper mapper;
        private Image fakeImage1;
        private ImageDto fakeImageDto1;
        private ImageDto fakeAddImageDto;
        private List<Image> fakeImages;
        private List<ImageDto> fakeImageDtos;

        [TestInitialize]
        public void Initialize()
        {
            fakeImage1 = new Image
            {
                ImageID = 1,
                Data = new byte[10],
                Format = "png",
                Name = "pict1",
                Url = "pict1.png"
            };

            fakeImageDto1 = new ImageDto
            {
                ImageID = 1,
                Data = new byte[10],
                Format = "png",
                Name = "pict1",
                Url = "pict1.png"
            };

            fakeImages = new List<Image> { fakeImage1 };
            fakeImageDtos = new List<ImageDto> { fakeImageDto1 };

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Image, ImageDto>();
                cfg.CreateMap<ImageDto, Image>();
            });
            mapper = config.CreateMapper();

            mockImageRepository = new Mock<IImageRepository>();
            mockImageRepository.Setup(i => i.GetImages()).ReturnsAsync(fakeImages);
            mockImageRepository.Setup(i => i.GetOneImage(It.IsAny<int>())).ReturnsAsync(fakeImage1);
            mockImageRepository.Setup(i => i.Add(It.IsAny<Image>(), null)).ReturnsAsync(1);
        }

        [TestMethod]
        public async Task GetReturnImages()
        {
            // Arrange
            var controller = new ImageController(mockImageRepository.Object, mapper);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            // Act
            var result = await controller.GetAllImages() as ObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            var images = result.Value as IEnumerable<ImageDto>;
            Assert.AreEqual(1, images.Count());
        }

        [TestMethod]
        public async Task GetReturnImage()
        {
            // Arrange
            var controller = new ImageController(mockImageRepository.Object, mapper);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            // Act
            var result = await controller.GetImage(1) as ObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            var image = result.Value as ImageDto;
            Assert.AreEqual(1, image.ImageID);
            Assert.AreEqual("pict1", image.Name);
        }

        [TestMethod]
        public async Task PostReturnImageId()
        {
            // Arrange
            var controller = new ImageController(mockImageRepository.Object, mapper);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            // Act
            var result = await controller.CreateImage(fakeImageDto1) as ObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            var imageId = result.Value as int?;
            Assert.AreEqual(1, imageId);
        }

        [TestMethod]
        public async Task PostReturnBadRequest()
        {
            // Arrange
            var controller = new ImageController(mockImageRepository.Object, mapper);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ModelState.AddModelError("Error", "Invalid...");

            // Act
            var result = await controller.CreateImage(fakeImageDto1) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
        }
    }
}
