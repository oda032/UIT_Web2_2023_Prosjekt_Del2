using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing.Text;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Blog.Common.Enum;
using Blog.Common.Interface.IRepository;
using Blog.DataAccess.Data;
using Blog.DataAccess.Repository;
using Blog.Test.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Blog.Test.RepositoryTest
{
    [TestClass]
    public class BlogRepositoryTest
    {
        private BlogRepository blogRepository;
        private Common.Model.Entity.Blog fakeBlog1;
        private Common.Model.Entity.Blog fakeBlog2;
        private Common.Model.Entity.Blog fakeAddBlog;
        private List<Common.Model.Entity.Blog> fakeBlogs;
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

            fakeAddBlog = new Common.Model.Entity.Blog
            {
                BlogTitle = "newTitle",
                BlogDetails = "newDetails"
            };

            fakeBlogs = new List<Common.Model.Entity.Blog> { fakeBlog1, fakeBlog2 };

            mockUserManager = UserManagerHelper.MockUserManager<IdentityUser>();

            mockUserManager.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(fakeIdentityUser);
        }

        [TestMethod]
        public async Task CanGetAllBlogs()
        {
            // Arrange
            var testContext = InMemoryHelper.GetTestDbContext(nameof(CanGetAllBlogs));
            testContext.AddRange(fakeBlogs);
            await testContext.SaveChangesAsync();

            blogRepository = new BlogRepository(testContext, mockUserManager.Object);

            // Action
            var blogs = await blogRepository.GetBlogs();


            // Assert
            Assert.IsNotNull(blogs);
            CollectionAssert.AllItemsAreInstancesOfType((ICollection)blogs, typeof(Common.Model.Entity.Blog));
            Assert.AreEqual(blogs.Count(), 2);
        }

        [TestMethod]
        public async Task CanGetOneBlog()
        {
            // Arrange
            var testContext = InMemoryHelper.GetTestDbContext(nameof(CanGetOneBlog));
            testContext.AddRange(fakeBlogs);
            await testContext.SaveChangesAsync();

            blogRepository = new BlogRepository(testContext, mockUserManager.Object);

            // Action
            var blog = await blogRepository.GetOneBlog(2);

            // Assert
            Assert.IsNotNull(blog);
            Assert.IsInstanceOfType(blog, typeof(Common.Model.Entity.Blog));
            Assert.AreSame(blog, fakeBlog2);
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public async Task EditBlogThrowExceptionBecauseNotImplemented()
        {
            try
            {
                // Arrange
                var testContext = InMemoryHelper.GetTestDbContext(nameof(EditBlogThrowExceptionBecauseNotImplemented));
                testContext.AddRange(fakeBlogs);
                await testContext.SaveChangesAsync();

                blogRepository = new BlogRepository(testContext, mockUserManager.Object);

                // Action
                await blogRepository.Edit(fakeBlog1);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public async Task DeleteBlogThrowExceptionBecauseNotImplemented()
        {
            try
            {
                // Arrange
                var testContext = InMemoryHelper.GetTestDbContext(nameof(DeleteBlogThrowExceptionBecauseNotImplemented));
                testContext.AddRange(fakeBlogs);
                await testContext.SaveChangesAsync();

                blogRepository = new BlogRepository(testContext, mockUserManager.Object);

                // Action
                await blogRepository.Delete(fakeBlog1);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [TestMethod]
        public async Task CanAddOneBlog()
        {
            // Arrange
            var testContext = InMemoryHelper.GetTestDbContext(nameof(CanAddOneBlog));
            testContext.AddRange(fakeBlogs);
            await testContext.SaveChangesAsync();

            blogRepository = new BlogRepository(testContext, mockUserManager.Object);

            // Action
            var blogId = await blogRepository.Add(fakeAddBlog, "astra1@email.com");

            // Assert
            Assert.IsNotNull(blogId);
            Assert.AreEqual(blogId, 3);
        }
    }
}
