using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Common.Enum;
using Blog.Common.Interface.IRepository;
using Blog.Common.Model.Entity;
using Blog.DataAccess.Repository;
using Blog.Test.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Blog.Test.RepositoryTest
{
    [TestClass]
    public class PostTagRepositoryTest
    {
        private PostTagRepository postTagRepository;
        private Tag fakeTag1;
        private Tag fakeTag2;
        private Post fakePost1;
        private Post fakePost2;
        private PostTag fakePostTag1;
        private PostTag fakePostTag2;
        private PostTag fakeAddPostTag;
        private List<PostTag> fakePostTags;
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

            fakeTag2 = new Tag
            {
                TagID = 2,
                TagName = "tag2"
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

            fakePost2 = new Post
            {
                PostDetails = "details",
                BlogID = 2,
                PostOwner = fakeIdentityUser,
                PostOwnerID = fakeIdentityUser.Id,
                PostTitle = "title",
                ObjectOwnerId = fakeIdentityUser.Id,
                PostID = 2,
            };

            fakePostTag1 = new PostTag
            {
                PostTagID = 1,
                PostID = 1,
                Post = fakePost1,
                TagID = 1,
                Tag = fakeTag1
            };

            fakePostTag2 = new PostTag
            {
                PostTagID = 2,
                PostID = 2,
                Post = fakePost2,
                TagID = 2,
                Tag = fakeTag2
            };

            fakeAddPostTag = new PostTag
            {
                PostID = 1,
                Post = fakePost1,
                TagID = 2,
                Tag = fakeTag2
            };

            fakePostTags = new List<PostTag> { fakePostTag1, fakePostTag2 };
        }

        [TestMethod]
        public async Task CanGetAllPostTags()
        {
            // Arrange
            var testContext = InMemoryHelper.GetTestDbContext(nameof(CanGetAllPostTags));
            testContext.AddRange(fakePostTags);
            await testContext.SaveChangesAsync();

            postTagRepository = new PostTagRepository(testContext);

            // Action
            var postTags = await postTagRepository.GetPostTags();

            // Assert
            Assert.IsNotNull(postTags);
            CollectionAssert.AllItemsAreInstancesOfType((ICollection)postTags, typeof(PostTag));
            Assert.AreEqual(postTags.Count(), 2);
            Assert.AreEqual(postTags.ElementAt(0).PostTagID, 1);
        }

        [TestMethod]
        public async Task CanGetOnePostTag()
        {
            // Arrange
            var testContext = InMemoryHelper.GetTestDbContext(nameof(CanGetOnePostTag));
            testContext.AddRange(fakePostTags);
            await testContext.SaveChangesAsync();

            postTagRepository = new PostTagRepository(testContext);

            // Action
            var postTag = await postTagRepository.GetOnePostTag(2);

            // Assert
            Assert.IsNotNull(postTag);
            Assert.IsInstanceOfType(postTag, typeof(PostTag));
            Assert.AreSame(postTag, fakePostTag2);
        }

        [TestMethod]
        public async Task CanChangePostTag()
        {
            // Arrange
            var testContext = InMemoryHelper.GetTestDbContext(nameof(CanChangePostTag));
            testContext.AddRange(fakePostTags);
            await testContext.SaveChangesAsync();

            fakePostTags.ElementAt(0).Post.PostTitle = "Changed";

            postTagRepository = new PostTagRepository(testContext);

            // Action
            await postTagRepository.Edit(fakePostTags[0]);
            var postTags = await postTagRepository.GetPostTags();

            // Assert
            Assert.IsNotNull(postTags);
            Assert.IsInstanceOfType(postTags.ElementAt(0), typeof(PostTag));
            Assert.AreEqual(postTags.ElementAt(0).Post.PostTitle, "Changed");
        }

        [TestMethod]
        public async Task CanDeletePostTag()
        {
            // Arrange
            var testContext = InMemoryHelper.GetTestDbContext(nameof(CanDeletePostTag));
            testContext.AddRange(fakePostTags);
            await testContext.SaveChangesAsync();
            
            postTagRepository = new PostTagRepository(testContext);

            // Action
            await postTagRepository.Delete(fakePostTags[1]);
            var postTags = await postTagRepository.GetPostTags();

            // Assert
            Assert.IsNotNull(postTags);
            Assert.AreEqual(postTags.Count(), 1);
        }

        [TestMethod]
        public async Task CanAddOnePostTag()
        {
            // Arrange
            var testContext = InMemoryHelper.GetTestDbContext(nameof(CanAddOnePostTag));
            testContext.AddRange(fakePostTags);
            await testContext.SaveChangesAsync();

            postTagRepository = new PostTagRepository(testContext);

            // Action
            var postTagId = await postTagRepository.Add(fakeAddPostTag, null);
            var postTags = await postTagRepository.GetPostTags();

            // Assert
            Assert.IsNotNull(postTagId);
            Assert.AreEqual(postTagId, 3);
            Assert.AreEqual(postTags.Count(), 3);
        }
    }
}
