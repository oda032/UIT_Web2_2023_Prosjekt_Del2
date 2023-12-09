using AutoMapper;
using Blog.Api.Controllers;
using Blog.Common.Enum;
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
    public class PostApiControllerTest
    {
        private Mock<IPostRepository> mockPostRepository;
        private IMapper mapper;
        private Common.Model.Entity.Blog fakeBlog1;
        private Post fakePost1;
        private Post fakePost2;
        private PostDto fakePostDto;
        private Post fakeAddPost;
        private List<Post> fakePosts;
        private IdentityUser fakeIdentityUser;
        private Mock<UserManager<IdentityUser>> mockUserManager;

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

            fakePost1 = new Post
            {
                PostDetails = "details",
                BlogID = 1,
                PostOwner = fakeIdentityUser,
                PostOwnerID = fakeIdentityUser.Id,
                PostTitle = "title",
                ObjectOwnerId = fakeIdentityUser.Id,
                PostID = 1,
                Blog = fakeBlog1
            };

            fakePost2 = new Post
            {
                PostDetails = "details",
                BlogID = 1,
                PostOwner = fakeIdentityUser,
                PostOwnerID = fakeIdentityUser.Id,
                PostTitle = "title",
                ObjectOwnerId = fakeIdentityUser.Id,
                PostID = 2,
                Blog = fakeBlog1
            };

            fakePostDto = new PostDto
            {
                BlogID = 1,
                PostDetails = "newDetails",
                PostTitle = "newTitle",
                PostOwnerName = "astra1@email.com"
            };

            fakeAddPost = new Post
            {
                PostTitle = "newTitle",
                PostDetails = "newDetails",
                BlogID = 1
            };

            fakePosts = new List<Post> { fakePost1, fakePost2};

            var config = new MapperConfiguration(cfg =>
            {
                //
            });
            mapper = config.CreateMapper();

            mockUserManager = UserManagerHelper.MockUserManager<IdentityUser>();
            mockUserManager.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(fakeIdentityUser);

            mockPostRepository = new Mock<IPostRepository>();
            mockPostRepository.Setup(b => b.GetPosts(1)).ReturnsAsync(fakePosts);
            mockPostRepository.Setup(b => b.GetOnePost(1)).ReturnsAsync(fakePost1);
            mockPostRepository.Setup(b => b.Add(It.IsAny<Post>(), It.IsAny<string>())).ReturnsAsync(3);
        }

        [TestMethod]
        public async Task GetReturnPosts()
        {
            // Arrange
            var controller = new PostController(mockPostRepository.Object, mapper);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            // Act
            var result = await controller.GetAllPosts(1) as ObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            var posts = result.Value as IEnumerable<PostDto>;
            Assert.AreEqual(2, posts.Count());
        }

        [TestMethod]
        public async Task GetReturnPost()
        {
            // Arrange
            var controller = new PostController(mockPostRepository.Object, mapper);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            // Act
            var result = await controller.GetPost(1) as ObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            var post = result.Value as PostDto;
            Assert.AreEqual(1, post.PostID);
            Assert.AreEqual("details", post.PostDetails);
        }

        [TestMethod]
        public async Task PostReturnPostId()
        {
            // Arrange
            var controller = new PostController(mockPostRepository.Object, mapper);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            // Act
            var result = await controller.CreatePost(fakePostDto) as ObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            var postId = result.Value as int?;
            Assert.AreEqual(3, postId);
        }

        [TestMethod]
        public async Task PostReturnBadRequest()
        {
            // Arrange
            var controller = new PostController(mockPostRepository.Object, mapper);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ModelState.AddModelError("Error", "Invalid...");

            // Act
            var result = await controller.CreatePost(fakePostDto) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
        }
    }
}
