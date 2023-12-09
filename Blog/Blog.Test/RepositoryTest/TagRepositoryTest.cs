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
    public class TagRepositoryTest
    {
        private TagRepository tagRepository;
        private Tag fakeTag1;
        private Tag fakeTag2;
        private Tag fakeAddTag;
        private List<Tag> fakeTags;

        [TestInitialize]
        public void Initialize()
        {
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

            fakeAddTag = new Tag
            {
                TagName = "newTag",
            };

            fakeTags = new List<Tag> { fakeTag1, fakeTag2 };
        }

        [TestMethod]
        public async Task CanGetAllTags()
        {
            // Arrange
            var testContext = InMemoryHelper.GetTestDbContext(nameof(CanGetAllTags));
            testContext.AddRange(fakeTags);
            await testContext.SaveChangesAsync();

            tagRepository = new TagRepository(testContext);

            // Action
            var tags = await tagRepository.GetTags();


            // Assert
            Assert.IsNotNull(tags);
            CollectionAssert.AllItemsAreInstancesOfType((ICollection)tags, typeof(Tag));
            Assert.AreEqual(tags.Count(), 2);
            Assert.AreEqual(tags.ElementAt(0).TagID, 1);
        }

        [TestMethod]
        public async Task CanGetOneTag()
        {
            // Arrange
            var testContext = InMemoryHelper.GetTestDbContext(nameof(CanGetOneTag));
            testContext.AddRange(fakeTags);
            await testContext.SaveChangesAsync();

            tagRepository = new TagRepository(testContext);

            // Action
            var tag = await tagRepository.GetOneTag(2);

            // Assert
            Assert.IsNotNull(tag);
            Assert.IsInstanceOfType(tag, typeof(Tag));
            Assert.AreSame(tag, fakeTag2);
        }

        [TestMethod]
        public async Task CanChangeTag()
        {
            // Arrange
            var testContext = InMemoryHelper.GetTestDbContext(nameof(CanChangeTag));
            testContext.AddRange(fakeTags);
            await testContext.SaveChangesAsync();

            fakeTags.ElementAt(0).TagName = "Changed";

            tagRepository = new TagRepository(testContext);

            // Action
            await tagRepository.Edit(fakeTags[0]);
            var tags = await tagRepository.GetTags();

            // Assert
            Assert.IsNotNull(tags);
            Assert.IsInstanceOfType(tags.ElementAt(0), typeof(Tag));
            Assert.AreEqual(tags.ElementAt(0).TagName, "Changed");
        }

        [TestMethod]
        public async Task CanDeleteTag()
        {
            // Arrange
            var testContext = InMemoryHelper.GetTestDbContext(nameof(CanDeleteTag));
            testContext.AddRange(fakeTags);
            await testContext.SaveChangesAsync();

            tagRepository = new TagRepository(testContext);

            // Action
            await tagRepository.Delete(fakeTags[1]);
            var tags = await tagRepository.GetTags();

            // Assert
            Assert.IsNotNull(tags);
            Assert.AreEqual(tags.Count(), 1);
        }

        [TestMethod]
        public async Task CanAddOneTag()
        {
            // Arrange
            var testContext = InMemoryHelper.GetTestDbContext(nameof(CanAddOneTag));
            testContext.AddRange(fakeTags);
            await testContext.SaveChangesAsync();

            tagRepository = new TagRepository(testContext);

            // Action
            var tagId = await tagRepository.Add(fakeAddTag, null);
            var tags = await tagRepository.GetTags();

            // Assert
            Assert.IsNotNull(tagId);
            Assert.AreEqual(tagId, 3);
            Assert.AreEqual(tags.Count(), 3);
        }
    }
}
