using AutoMapper;
using Blog.Api.Controllers;
using Blog.Common.Interface.IRepository;
using Blog.Common.Model.Dto;
using Blog.Common.Model.Entity;
using Blog.DataAccess.Repository;
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
    public class PostTagApiControllerTest
    {
        private Mock<IPostTagRepository> mockPostTagRepository;
        private IMapper mapper;
        private Tag fakeTag1;
        private Post fakePost1;
        private PostTag fakePostTag1;
        private PostTag fakeAddPostTag;
        private PostTagDto fakePostTagDto1;
        private List<PostTag> fakePostTags;
        private List<PostTagDto> fakePostTagDtos;
        private IdentityUser fakeIdentityUser;


        [TestInitialize]
        public void Initialize()
        {
            fakeIdentityUser = new IdentityUser
            {
                Id = "astra1",
                UserName = "astra1@email.com",
                NormalizedUserName = "ASTRA1@EMAIL.COM",
                Email = "astra1@email.com",
                NormalizedEmail = "ASTRA1@EMAIL.COM",
                LockoutEnabled = false,
                EmailConfirmed = true,
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

            fakeAddPostTag = new PostTag
            {
                PostID = 1,
                Post = fakePost1,
                TagID = 1,
                Tag = fakeTag1
            };

            fakePostTags = new List<PostTag> { fakePostTag1 };
            fakePostTagDtos = new List<PostTagDto> { fakePostTagDto1 };

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PostTag, PostTagDto>();
                cfg.CreateMap<PostTagDto, PostTag>();
            });
            mapper = config.CreateMapper();

            mockPostTagRepository = new Mock<IPostTagRepository>();
            mockPostTagRepository.Setup(b => b.Add(It.IsAny<PostTag>(), null)).ReturnsAsync(1);
        }

        [TestMethod]
        public async Task PostReturnPostTagIds()
        {
            // Arrange
            var controller = new PostTagController(mockPostTagRepository.Object, mapper);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            // Act
            var result = await controller.CreatePostTags(fakePostTagDtos) as OkResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }
    }
}
