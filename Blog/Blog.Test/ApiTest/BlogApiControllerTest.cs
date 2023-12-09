using AutoMapper;
using Blog.Api.Controllers;
using Blog.Common.Enum;
using Blog.Common.Interface.IRepository;
using Blog.Common.Model.Dto;
using Blog.DataAccess.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
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
    public class BlogApiControllerTest
    {
        private Mock<IBlogRepository> mockBlogRepository;
        private IMapper mapper;
        private Common.Model.Entity.Blog fakeBlog1;
        private Common.Model.Entity.Blog fakeBlog2;
        private List<Common.Model.Entity.Blog> fakeBlogs;
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

            fakeBlog1 = new Common.Model.Entity.Blog
            {
                BlogDetails = "details",
                BlogID = 1,
                BlogOwner = fakeIdentityUser,
                BlogOwnerID = fakeIdentityUser.Id,
                BlogStatus = BlogStatus.Open,
                BlogTitle = "title",
                ObjectOwnerId = fakeIdentityUser.Id
            };

            fakeBlog2 = new Common.Model.Entity.Blog
            {
                BlogDetails = "details",
                BlogID = 2,
                BlogOwner = fakeIdentityUser,
                BlogOwnerID = fakeIdentityUser.Id,
                BlogStatus = BlogStatus.Open,
                BlogTitle = "title",
                ObjectOwnerId = fakeIdentityUser.Id
            };

            fakeBlogs = new List<Common.Model.Entity.Blog> { fakeBlog1, fakeBlog2};

            mockBlogRepository = new Mock<IBlogRepository>();
            mockBlogRepository.Setup(b=>b.GetBlogs()).ReturnsAsync(fakeBlogs);
            mockBlogRepository.Setup(b => b.GetOneBlog(1)).ReturnsAsync(fakeBlog1);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Common.Model.Entity.Blog, BlogDto>();
            });
            mapper = config.CreateMapper();
        }

        [TestMethod]
        public async Task GetReturnBlogs()
        {
            // Arrange
            var controller = new BlogController(mockBlogRepository.Object, mapper);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            // Act
            var result = await controller.GetAllBlogs() as ObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            var blogs = result.Value as IEnumerable<BlogDto>;
            Assert.AreEqual(2, blogs.Count());
        }

        [TestMethod]
        public async Task GetReturnBlog()
        {
            // Arrange
            var controller = new BlogController(mockBlogRepository.Object, mapper);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            // Act
            var result = await controller.GetBlog(1) as ObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            var blog = result.Value as BlogDto;
            Assert.AreEqual(1, blog.BlogID);
            Assert.AreEqual("details", blog.BlogDetails);
        }
    }
}
