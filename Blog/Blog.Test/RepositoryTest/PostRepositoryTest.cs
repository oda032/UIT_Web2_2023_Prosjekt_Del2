using Blog.Common.Enum;
using Blog.DataAccess.Repository;
using Blog.Test.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Common.Model.Entity;
using Blog.Common.Interface.IRepository;
using System.Collections;
using Microsoft.Extensions.Hosting;

namespace Blog.Test.RepositoryTest
{
    [TestClass]
    public class PostRepositoryTest
    {
        private PostRepository postRepository;
        private Common.Model.Entity.Blog fakeBlog1;
        private Common.Model.Entity.Blog fakeBlog2;
        private Post fakePost1;
        private Post fakePost2;
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

            fakeAddPost = new Post
            {
                PostTitle = "newTitle",
                PostDetails = "newDetails",
                BlogID = 1
            };

            fakePosts = new List<Post> { fakePost1, fakePost2 };

            mockUserManager = UserManagerHelper.MockUserManager<IdentityUser>();

            mockUserManager.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(fakeIdentityUser);
        }

        [TestMethod]
        public async Task CanGetAllPosts()
        {
            // Arrange
            var testContext = InMemoryHelper.GetTestDbContext(nameof(CanGetAllPosts));
            testContext.AddRange(fakePosts);
            await testContext.SaveChangesAsync();

            postRepository = new PostRepository(testContext, mockUserManager.Object);

            // Action
            var posts = await postRepository.GetPosts(1);


            // Assert
            Assert.IsNotNull(posts);
            CollectionAssert.AllItemsAreInstancesOfType((ICollection)posts, typeof(Post));
            Assert.AreEqual(posts.Count(), 1);
            Assert.AreEqual(posts.ElementAt(0).PostID, 1);
        }

        [TestMethod]
        public async Task CanGetOnePost()
        {
            // Arrange
            var testContext = InMemoryHelper.GetTestDbContext(nameof(CanGetOnePost));
            testContext.AddRange(fakePosts);
            await testContext.SaveChangesAsync();

            postRepository = new PostRepository(testContext, mockUserManager.Object);

            // Action
            var post = await postRepository.GetOnePost(2);

            // Assert
            Assert.IsNotNull(post);
            Assert.IsInstanceOfType(post, typeof(Post));
            Assert.AreSame(post, fakePost2);
        }

        [TestMethod]
        public async Task CanChangePost()
        {
            // Arrange
            var testContext = InMemoryHelper.GetTestDbContext(nameof(CanChangePost));
            testContext.AddRange(fakePosts);
            await testContext.SaveChangesAsync();

            fakePosts.ElementAt(0).PostTitle = "Changed";

            postRepository = new PostRepository(testContext, mockUserManager.Object);

            // Action
            await postRepository.Edit(fakePosts[0]);
            var posts = await postRepository.GetPosts(1);

            // Assert
            Assert.IsNotNull(posts);
            Assert.IsInstanceOfType(posts.ElementAt(0), typeof(Post));
            Assert.AreEqual(posts.ElementAt(0).PostTitle, "Changed");
        }

        [TestMethod]
        public async Task CanDeletePost()
        {
            // Arrange
            var testContext = InMemoryHelper.GetTestDbContext(nameof(CanDeletePost));
            testContext.AddRange(fakePosts);
            await testContext.SaveChangesAsync();

            postRepository = new PostRepository(testContext, mockUserManager.Object);

            // Action
            await postRepository.Delete(fakePosts[1]);
            var posts = await postRepository.GetPosts(2);

            // Assert
            Assert.IsNotNull(posts);
            Assert.AreEqual(posts.Count(), 0);
        }

        [TestMethod]
        public async Task CanAddOnePost()
        {
            // Arrange
            var testContext = InMemoryHelper.GetTestDbContext(nameof(CanAddOnePost));
            testContext.AddRange(fakePosts);
            await testContext.SaveChangesAsync();

            postRepository = new PostRepository(testContext, mockUserManager.Object);

            // Action
            var postId = await postRepository.Add(fakeAddPost, "astra1@email.com");
            var posts = await postRepository.GetPosts(1);

            // Assert
            Assert.IsNotNull(postId);
            Assert.AreEqual(postId, 3);
            Assert.AreEqual(posts.Count(), 2);
        }
    }
}
