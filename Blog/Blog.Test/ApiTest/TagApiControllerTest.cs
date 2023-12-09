using AutoMapper;
using Blog.Api.Controllers;
using Blog.Common.Interface.IRepository;
using Blog.Common.Model.Dto;
using Blog.Common.Model.Entity;
using Blog.DataAccess.Repository;
using Blog.Test.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Test.ApiTest
{
    [TestClass]
    public class TagApiControllerTest
    {
        private Mock<ITagRepository> mockTagRepository;
        private IMapper mapper;
        private Tag fakeTag1;
        private TagDto fakeTagDto1;
        private Tag fakeAddTag;
        private List<Tag> fakeTags;
        private List<TagDto> fakeTagDtos;

        [TestInitialize]
        public void Initialize()
        {
            fakeTag1 = new Tag
            {
                TagID = 1,
                TagName = "tag1"
            };

            fakeAddTag = new Tag
            {
                TagName = "newTag",
            };

            fakeTagDto1 = new TagDto
            {
                TagName = "newTag1"
            };

            fakeTags = new List<Tag> { fakeTag1 };
            fakeTagDtos = new List<TagDto> {  fakeTagDto1 };

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Tag, TagDto>();
                cfg.CreateMap<TagDto, Tag>();
            });
            mapper = config.CreateMapper();

            mockTagRepository = new Mock<ITagRepository>();
            mockTagRepository.Setup(b => b.GetTags()).ReturnsAsync(fakeTags);
            mockTagRepository.Setup(b => b.Add(It.IsAny<Tag>(), null)).ReturnsAsync(1);
        }

        [TestMethod]
        public async Task GetReturnTags()
        {
            // Arrange
            var controller = new TagController(mockTagRepository.Object, mapper);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            // Act
            var result = await controller.GetAllTags() as ObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            var tags = result.Value as IEnumerable<TagDto>;
            Assert.AreEqual(1, tags.Count());
        }

        [TestMethod]
        public async Task PostReturnTagIds()
        {
            // Arrange
            var controller = new TagController(mockTagRepository.Object, mapper);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            // Act
            var result = await controller.CreateTags(fakeTagDtos) as ObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            var tagIds = result.Value as List<int>;
            Assert.AreEqual(1, tagIds.Count());
            Assert.AreEqual(1, tagIds[0]);
        }
    }
}
