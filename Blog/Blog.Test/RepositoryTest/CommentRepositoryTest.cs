using Blog.Common.Enum;
using Blog.Common.Interface.IRepository;
using Blog.Common.Model.Entity;
using Blog.DataAccess.Repository;
using Blog.Test.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Test.RepositoryTest
{
    [TestClass]
    public class CommentRepositoryTest
    {
        private CommentRepository commentRepository;
        private Common.Model.Entity.Blog fakeBlog1;
        private Common.Model.Entity.Blog fakeBlog2;
        private Post fakePost1;
        private Post fakePost2;
        private Comment fakeComment1;
        private Comment fakeComment2;
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
                BlogID = 2,
                PostOwner = fakeIdentityUser,
                PostOwnerID = fakeIdentityUser.Id,
                PostTitle = "title",
                ObjectOwnerId = fakeIdentityUser.Id,
                PostID = 2,
                Blog = fakeBlog2
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
                PostID = 2,
                BlogID = 2,
                Post = fakePost2
            };

            fakeAddComment = new Comment
            {
                CommentTitle = "newTitle",
                CommentDetails = "newDetails",
                BlogID = 1,
                PostID = 1
            };

            fakeComments = new List<Comment> { fakeComment1, fakeComment2 };

            mockUserManager = UserManagerHelper.MockUserManager<IdentityUser>();

            mockUserManager.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(fakeIdentityUser);
        }

        [TestMethod]
        public async Task CanGetAllComments()
        {
            // Arrange
            var testContext = InMemoryHelper.GetTestDbContext(nameof(CanGetAllComments));
            testContext.AddRange(fakeComments);
            await testContext.SaveChangesAsync();

            commentRepository = new CommentRepository(testContext, mockUserManager.Object);

            // Action
            var comments = await commentRepository.GetComments(1);


            // Assert
            Assert.IsNotNull(comments);
            CollectionAssert.AllItemsAreInstancesOfType((ICollection)comments, typeof(Comment));
            Assert.AreEqual(comments.Count(), 1);
            Assert.AreEqual(comments.ElementAt(0).CommentID, 1);
        }

        [TestMethod]
        public async Task CanGetOneComment()
        {
            // Arrange
            var testContext = InMemoryHelper.GetTestDbContext(nameof(CanGetOneComment));
            testContext.AddRange(fakeComments);
            await testContext.SaveChangesAsync();

            commentRepository = new CommentRepository(testContext, mockUserManager.Object);

            // Action
            var comment = await commentRepository.GetOneComment(2);

            // Assert
            Assert.IsNotNull(comment);
            Assert.IsInstanceOfType(comment, typeof(Comment));
            Assert.AreSame(comment, fakeComment2);
        }

        [TestMethod]
        public async Task CanChangeComment()
        {
            // Arrange
            var testContext = InMemoryHelper.GetTestDbContext(nameof(CanChangeComment));
            testContext.AddRange(fakeComments);
            await testContext.SaveChangesAsync();

            fakeComments.ElementAt(0).CommentTitle = "Changed";

            commentRepository = new CommentRepository(testContext, mockUserManager.Object);

            // Action
            await commentRepository.Edit(fakeComments[0]);
            var comments = await commentRepository.GetComments(1);

            // Assert
            Assert.IsNotNull(comments);
            Assert.IsInstanceOfType(comments.ElementAt(0), typeof(Comment));
            Assert.AreEqual(comments.ElementAt(0).CommentTitle, "Changed");
        }

        [TestMethod]
        public async Task CanDeleteComment()
        {
            // Arrange
            var testContext = InMemoryHelper.GetTestDbContext(nameof(CanDeleteComment));
            testContext.AddRange(fakeComments);
            await testContext.SaveChangesAsync();

            commentRepository = new CommentRepository(testContext, mockUserManager.Object);

            // Action
            await commentRepository.Delete(fakeComments[1]);
            var comments = await commentRepository.GetComments(2);

            // Assert
            Assert.IsNotNull(comments);
            Assert.AreEqual(comments.Count(), 0);
        }

        [TestMethod]
        public async Task CanAddOneComment()
        {
            // Arrange
            var testContext = InMemoryHelper.GetTestDbContext(nameof(CanAddOneComment));
            testContext.AddRange(fakeComments);
            await testContext.SaveChangesAsync();

            commentRepository = new CommentRepository(testContext, mockUserManager.Object);

            // Action
            var commentId = await commentRepository.Add(fakeAddComment, "astra1@email.com");
            var comments = await commentRepository.GetComments(1);

            // Assert
            Assert.IsNotNull(commentId);
            Assert.AreEqual(commentId, 3);
            Assert.AreEqual(comments.Count(), 2);
        }
    }
}
