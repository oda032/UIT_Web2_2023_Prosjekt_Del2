using AutoMapper;
using Blog.Api.Controllers;
using Blog.Common.Enum;
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
    public class UserSubscribedBlogApiControllerTest
    {
        private Mock<IUserSubscribedBlogRepository> mockUserSubscribedBlogRepository;
        private IMapper mapper;
        private ApplicationUser fakeAppUser2;
        private Common.Model.Entity.Blog fakeBlog1;
        private Common.Model.Entity.Blog fakeBlog2;
        private UserSubscribedBlog fakeUserSubscribedBlog1;
        private UserSubscribedBlogDto fakeUserSubscribedBlogDto1;
        private List<UserSubscribedBlog> fakeUserSubscribedBlogs;
        private List<UserSubscribedBlogDto> fakeUserSubscribedBlogDtos;
        private IdentityUser fakeIdentityUser1;
        private IdentityUser fakeIdentityUser2;

        [TestInitialize]
        public void Initialize()
        {
            fakeIdentityUser1 = new IdentityUser
            {
                Id = "astra1",
                UserName = "astra1@email.com",
                NormalizedUserName = "ASTRA1@EMAIL.COM",
                Email = "astra1@email.com",
                NormalizedEmail = "ASTRA1@EMAIL.COM",
                LockoutEnabled = false,
                EmailConfirmed = true,
            };

            fakeIdentityUser2 = new IdentityUser
            {
                Id = "astra2",
                UserName = "astra2@email.com",
                NormalizedUserName = "ASTRA2@EMAIL.COM",
                Email = "astra2@email.com",
                NormalizedEmail = "ASTRA2@EMAIL.COM",
                LockoutEnabled = false,
                EmailConfirmed = true,
            };

            fakeBlog1 = new Common.Model.Entity.Blog
            {
                BlogDetails = "details",
                BlogID = 1,
                BlogOwner = fakeIdentityUser1,
                BlogOwnerID = fakeIdentityUser1.Id,
                BlogStatus = BlogStatus.Open,
                BlogTitle = "title",
                ObjectOwnerId = fakeIdentityUser1.Id
            };

            fakeBlog2 = new Common.Model.Entity.Blog
            {
                BlogDetails = "details",
                BlogID = 2,
                BlogOwner = fakeIdentityUser1,
                BlogOwnerID = fakeIdentityUser1.Id,
                BlogStatus = BlogStatus.Open,
                BlogTitle = "title",
                ObjectOwnerId = fakeIdentityUser1.Id
            };

            fakeAppUser2 = new ApplicationUser
            {
                Id = fakeIdentityUser2.Id,
                Email = fakeIdentityUser2.Email,
                UserName = fakeIdentityUser2.UserName,
                NormalizedEmail = fakeIdentityUser2.NormalizedEmail,
                NormalizedUserName = fakeIdentityUser2.NormalizedUserName,
                LockoutEnabled = fakeIdentityUser2.LockoutEnabled,
                EmailConfirmed = fakeIdentityUser2.EmailConfirmed
            };

            fakeUserSubscribedBlog1 = new UserSubscribedBlog
            {
                ApplicationUser = fakeAppUser2,
                ApplicationUserID = fakeAppUser2.Id,
                BlogID = 1,
                Blog = fakeBlog1,
                UserSubscribedBlogID = 1
            };

            fakeUserSubscribedBlogDto1 = new UserSubscribedBlogDto
            {
                ApplicationUserID = fakeAppUser2.Id,
                BlogID = 1,
                UserSubscribedBlogID = 1
            };

            fakeUserSubscribedBlogs = new List<UserSubscribedBlog> { fakeUserSubscribedBlog1 };
            fakeUserSubscribedBlogDtos = new List<UserSubscribedBlogDto> { fakeUserSubscribedBlogDto1 };

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserSubscribedBlog, UserSubscribedBlogDto>();
                cfg.CreateMap<UserSubscribedBlogDto, UserSubscribedBlog>();
            });
            mapper = config.CreateMapper();

            mockUserSubscribedBlogRepository = new Mock<IUserSubscribedBlogRepository>();
            mockUserSubscribedBlogRepository.Setup(b => b.GetUserSubscribedBlogs("astra2")).ReturnsAsync(fakeUserSubscribedBlogs);
            mockUserSubscribedBlogRepository.Setup(b => b.Add(It.IsAny<UserSubscribedBlog>(), null)).ReturnsAsync(1);
        }

        [TestMethod]
        public async Task GetReturnUserSubscribedBlogs()
        {
            // Arrange
            var controller = new UserSubscribedBlogController(mockUserSubscribedBlogRepository.Object, mapper);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            // Act
            var result = await controller.GetUserSubscribedBlogs("astra2") as ObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }

        [TestMethod]
        public async Task PostReturnUserSubscribedBlogId()
        {
            // Arrange
            var controller = new UserSubscribedBlogController(mockUserSubscribedBlogRepository.Object, mapper);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            // Act
            var result = await controller.CreateUserSubscribedBlog(fakeUserSubscribedBlogDto1) as ObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            var usbId = result.Value as int?;
            Assert.AreEqual(1, usbId);
        }

        [TestMethod]
        public async Task PostReturnBadRequest()
        {
            // Arrange
            var controller = new UserSubscribedBlogController(mockUserSubscribedBlogRepository.Object, mapper);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ModelState.AddModelError("Error", "Invalid...");

            // Act
            var result = await controller.CreateUserSubscribedBlog(fakeUserSubscribedBlogDto1) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
        }

        [TestMethod]
        public async Task PostReturnOkWhenUserSubscribedBlogIsDeleted()
        {
            // Arrange
            var controller = new UserSubscribedBlogController(mockUserSubscribedBlogRepository.Object, mapper);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            // Act
            var result = await controller.DeleteUserSubscribedBlog(fakeUserSubscribedBlogDto1) as OkResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public async Task PostReturnBadRequestWhenUserSubscribedBlogIsDeleted()
        {
            // Arrange
            var controller = new UserSubscribedBlogController(mockUserSubscribedBlogRepository.Object, mapper);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ModelState.AddModelError("Error", "Invalid...");

            // Act
            var result = await controller.DeleteUserSubscribedBlog(fakeUserSubscribedBlogDto1) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }
    }
}
