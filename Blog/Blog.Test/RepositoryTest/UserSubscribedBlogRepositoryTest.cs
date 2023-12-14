using Blog.Common.Enum;
using Blog.Common.Interface.IRepository;
using Blog.Common.Model.Entity;
using Blog.DataAccess.Repository;
using Blog.Test.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Test.RepositoryTest
{
    [TestClass]
    public class UserSubscribedBlogRepositoryTest
    {
        private UserSubscribedBlogRepository userSubscribedBlogRepository;
        private ApplicationUser fakeAppUser2;
        private Common.Model.Entity.Blog fakeBlog1;
        private Common.Model.Entity.Blog fakeBlog2;
        private UserSubscribedBlog fakeUserSubscribedBlog1;
        private List<UserSubscribedBlog> fakeUserSubscribedBlogs;
        private IdentityUser fakeIdentityUser1;
        private IdentityUser fakeIdentityUser2;
        private UserSubscribedBlog fakeAddUserSubscribedBlog;

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

            fakeAddUserSubscribedBlog = new UserSubscribedBlog
            {
                ApplicationUser = fakeAppUser2,
                ApplicationUserID = fakeAppUser2.Id,
                BlogID = 2,
                Blog = fakeBlog2

            };

            fakeUserSubscribedBlogs = new List<UserSubscribedBlog> { fakeUserSubscribedBlog1 };
        }

        [TestMethod]
        public async Task CanGetAllUserSubscribedBlogs()
        {
            // Arrange
            var testContext = InMemoryHelper.GetTestDbContext(nameof(CanGetAllUserSubscribedBlogs));
            testContext.AddRange(fakeUserSubscribedBlogs);
            await testContext.SaveChangesAsync();

            userSubscribedBlogRepository = new UserSubscribedBlogRepository(testContext);

            // Action
            var userSubscribedBlogs = await userSubscribedBlogRepository.GetUserSubscribedBlogs(fakeAppUser2.Id);

            // Assert
            Assert.IsNotNull(userSubscribedBlogs);
            CollectionAssert.AllItemsAreInstancesOfType((ICollection)userSubscribedBlogs, typeof(UserSubscribedBlog));
            Assert.AreEqual(userSubscribedBlogs.Count(), 1);
            Assert.AreEqual(userSubscribedBlogs.ElementAt(0).UserSubscribedBlogID, 1);
        }

        [TestMethod]
        public async Task CanGetOneUserSubscribedBlog()
        {
            // Arrange
            var testContext = InMemoryHelper.GetTestDbContext(nameof(CanGetOneUserSubscribedBlog));
            testContext.AddRange(fakeUserSubscribedBlogs);
            await testContext.SaveChangesAsync();

            userSubscribedBlogRepository = new UserSubscribedBlogRepository(testContext);

            // Action
            var userSubscribedBlog = await userSubscribedBlogRepository.GetOneUserSubscribedBlog(1);

            // Assert
            Assert.IsNotNull(userSubscribedBlog);
            Assert.IsInstanceOfType(userSubscribedBlog, typeof(UserSubscribedBlog));
            Assert.AreSame(userSubscribedBlog, fakeUserSubscribedBlog1);
        }

        [TestMethod]
        public async Task CanChangeUserSubscribedBlog()
        {
            // Arrange
            var testContext = InMemoryHelper.GetTestDbContext(nameof(CanChangeUserSubscribedBlog));
            testContext.AddRange(fakeUserSubscribedBlogs);
            await testContext.SaveChangesAsync();

            fakeUserSubscribedBlogs.ElementAt(0).Blog.BlogTitle = "Changed";

            userSubscribedBlogRepository = new UserSubscribedBlogRepository(testContext);

            // Action
            await userSubscribedBlogRepository.Edit(fakeUserSubscribedBlogs[0]);
            var userSubscribedBlogs = await userSubscribedBlogRepository.GetUserSubscribedBlogs(fakeAppUser2.Id);

            // Assert
            Assert.IsNotNull(userSubscribedBlogs);
            Assert.IsInstanceOfType(userSubscribedBlogs.ElementAt(0), typeof(UserSubscribedBlog));
            Assert.AreEqual(userSubscribedBlogs.ElementAt(0).Blog.BlogTitle, "Changed");
        }

        [TestMethod]
        public async Task CanDeleteUserSubscribedBlog()
        {
            // Arrange
            var testContext = InMemoryHelper.GetTestDbContext(nameof(CanDeleteUserSubscribedBlog));
            testContext.AddRange(fakeUserSubscribedBlogs);
            await testContext.SaveChangesAsync();

            userSubscribedBlogRepository = new UserSubscribedBlogRepository(testContext);

            // Action
            await userSubscribedBlogRepository.Delete(fakeUserSubscribedBlogs[0]);
            var userSubscribedBlogs = await userSubscribedBlogRepository.GetUserSubscribedBlogs(fakeAppUser2.Id);

            // Assert
            Assert.IsNotNull(userSubscribedBlogs);
            Assert.AreEqual(userSubscribedBlogs.Count(), 0);
        }

        [TestMethod]
        public async Task CanAddOneUserSubscribedBlog()
        {
            // Arrange
            var testContext = InMemoryHelper.GetTestDbContext(nameof(CanAddOneUserSubscribedBlog));
            testContext.AddRange(fakeUserSubscribedBlogs);
            await testContext.SaveChangesAsync();

            userSubscribedBlogRepository = new UserSubscribedBlogRepository(testContext);

            // Action
            var userSubscribedBlogId = await userSubscribedBlogRepository.Add(fakeAddUserSubscribedBlog);
            var userSubscribedBlogs = await userSubscribedBlogRepository.GetUserSubscribedBlogs(fakeAppUser2.Id);

            // Assert
            Assert.IsNotNull(userSubscribedBlogId);
            Assert.AreEqual(userSubscribedBlogId, 2);
            Assert.AreEqual(userSubscribedBlogs.Count(), 2);
        }
    }
}
