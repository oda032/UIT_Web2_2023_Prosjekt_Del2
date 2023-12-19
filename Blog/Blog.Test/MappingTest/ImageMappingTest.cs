using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Blog.Api.Mapping;
using Blog.Common.Enum;
using Blog.Common.Model.Dto;
using Blog.Common.Model.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Blog.Test.MappingTest
{
    [TestClass]
    public class ImageMappingTest
    {
        private Image fakeImage1;
        private Image fakeAddImage;
        private List<Image> fakeImages;
        private ImageDto fakeImageDto1;
        private IMapper mapper;

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

            fakeAddImage = new Image
            {
                Data = new byte[10],
                Format = "png",
                Name = "pict3",
                Url = "pict3.png"
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

            var configuration = new MapperConfiguration(cfg => { cfg.AddProfile<ImageMappingProfile>(); });
            mapper = configuration.CreateMapper();
        }

        [TestMethod]
        public void MappingFromImageToImageDto()
        {
            // Action
            var imageDto = mapper.Map<ImageDto>(fakeImage1);

            // Assert
            Assert.AreEqual(fakeImage1.ImageID, imageDto.ImageID);
            Assert.AreEqual(fakeImage1.Name, imageDto.Name);
            Assert.AreEqual(fakeImage1.Data.Length, imageDto.Data.Length);
            Assert.AreEqual(fakeImage1.Format, imageDto.Format);
            Assert.AreEqual(fakeImage1.Url, imageDto.Url);
        }

        [TestMethod]
        public void MappingFromImageDtoToImage()
        {
            // Action
            var image = mapper.Map<Image>(fakeImageDto1);

            // Assert
            Assert.AreEqual(fakeImageDto1.ImageID, image.ImageID);
            Assert.AreEqual(fakeImageDto1.Name, image.Name);
            Assert.AreEqual(fakeImageDto1.Data.Length, image.Data.Length);
            Assert.AreEqual(fakeImageDto1.Format, image.Format);
            Assert.AreEqual(fakeImageDto1.Url, image.Url);
        }
    }
}
