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
    public class CommentApiControllerTest
    {
        private Mock<ICommentRepository> mockCommentRepository;
        private IMapper mapper;
        private Common.Model.Entity.Blog fakeBlog1;
        private Post fakePost1;
        private Comment fakeComment1;
        private Comment fakeComment2;
        private CommentDto fakeCommentDto;
        private Comment fakeAddComment;
        private List<Comment> fakeComments;
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

            fakeComment1 = new Comment
            {
                CommentDetails = "details",
                CommentID = 1,
                CommentOwner = fakeIdentityUser,
                CommentOwnerID = fakeIdentityUser.Id,
                CommentTitle = "title",
                ObjectOwnerId = fakeIdentityUser.Id,
                PostID = 1,
                BlogID = 1,
                Post = fakePost1
            };

            fakeComment2 = new Comment
            {
                CommentDetails = "details",
                CommentID = 2,
                CommentOwner = fakeIdentityUser,
                CommentOwnerID = fakeIdentityUser.Id,
                CommentTitle = "title",
                ObjectOwnerId = fakeIdentityUser.Id,
                PostID = 1,
                BlogID = 1,
                Post = fakePost1
            };

            fakeAddComment = new Comment
            {
                CommentTitle = "newTitle",
                CommentDetails = "newDetails",
                BlogID = 1,
                PostID = 1
            };

            fakeCommentDto = new CommentDto
            {
                CommentTitle = "newTitle",
                CommentDetails = "newDetails",
                BlogID = 1,
                PostID = 1,
                CommentOwnerName = "astra1@email.com"
            };

            fakeComments = new List<Comment> { fakeComment1, fakeComment2 };

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Comment, CommentDto>();
                cfg.CreateMap<CommentDto, Comment>();
            });
            mapper = config.CreateMapper();

            mockUserManager = UserManagerHelper.MockUserManager<IdentityUser>();
            mockUserManager.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(fakeIdentityUser);

            mockCommentRepository = new Mock<ICommentRepository>();
            mockCommentRepository.Setup(b => b.GetComments(1)).ReturnsAsync(fakeComments);
            mockCommentRepository.Setup(b => b.GetOneComment(1)).ReturnsAsync(fakeComment1);
            mockCommentRepository.Setup(b => b.Add(It.IsAny<Comment>(), It.IsAny<string>())).ReturnsAsync(3);
        }

        [TestMethod]
        public async Task GetReturnComments()
        {
            // Arrange
            var controller = new CommentController(mockCommentRepository.Object, mapper);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            // Act
            var result = await controller.GetAllComments(1) as ObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            var comments = result.Value as IEnumerable<CommentDto>;
            Assert.AreEqual(2, comments.Count());
        }

        [TestMethod]
        public async Task PostReturnCommentId()
        {
            // Arrange
            var controller = new CommentController(mockCommentRepository.Object, mapper);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            // Act
            var result = await controller.CreateComment(fakeCommentDto) as ObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            var commentId = result.Value as int?;
            Assert.AreEqual(3, commentId);
        }

        [TestMethod]
        public async Task PostReturnBadRequest()
        {
            // Arrange
            var controller = new CommentController(mockCommentRepository.Object, mapper);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ModelState.AddModelError("Error", "Invalid...");

            // Act
            var result = await controller.CreateComment(fakeCommentDto) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
        }

        [TestMethod]
        public async Task PostReturnOkWhenCommentIsDeleted()
        {
            // Arrange
            var controller = new CommentController(mockCommentRepository.Object, mapper);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            // Act
            var result = await controller.DeleteComment(fakeCommentDto) as OkResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public async Task PostReturnBadRequestWhenCommentIsDeleted()
        {
            // Arrange
            var controller = new CommentController(mockCommentRepository.Object, mapper);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ModelState.AddModelError("Error", "Invalid...");

            // Act
            var result = await controller.DeleteComment(fakeCommentDto) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }
    }
}
