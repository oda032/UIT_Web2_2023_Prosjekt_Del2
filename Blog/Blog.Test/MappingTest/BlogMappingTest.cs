using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Blog.Api.Mapping;
using Blog.Common.Enum;
using Blog.Common.Model.Dto;
using Blog.DataAccess.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Blog.Test.MappingTest
{
    [TestClass]
    public class BlogMappingTest
    {
        private Common.Model.Entity.Blog fakeBlog1;
        private Common.Model.Entity.Blog fakeAddBlog;
        private List<Common.Model.Entity.Blog> fakeBlogs;
        private IdentityUser fakeIdentityUser;
        private BlogDto fakeBlogDto1;
        private IMapper mapper;

        [TestInitialize]
        public void Initialize()
        {
            fakeIdentityUser = new IdentityUser
            {
                Id = "user@test.com",
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

            fakeBlogDto1 = new BlogDto
            {
                BlogDetails = "details",
                BlogID = 1,
                BlogOwnerID = fakeIdentityUser.Id,
                BlogStatus = BlogStatus.Open,
                BlogTitle = "title",
                ObjectOwnerId = fakeIdentityUser.Id
            };

            fakeBlogs = new List<Common.Model.Entity.Blog> { fakeBlog1 };

            var configuration = new MapperConfiguration(cfg => { cfg.AddProfile<BlogMappingProfile>(); });
            mapper = configuration.CreateMapper();
        }

        [TestMethod]
        public void MappingFromBlogToBlogDto()
        {
            // Action
            var blogDto = mapper.Map<BlogDto>(fakeBlog1);

            // Assert
            Assert.AreEqual(fakeBlog1.BlogID, blogDto.BlogID);
            Assert.AreEqual(fakeBlog1.BlogDetails, blogDto.BlogDetails);
            Assert.AreEqual(fakeBlog1.BlogStatus, blogDto.BlogStatus);
            Assert.AreEqual(fakeBlog1.BlogTitle, blogDto.BlogTitle);
            Assert.AreEqual(fakeBlog1.BlogOwnerID, blogDto.BlogOwnerID);
            Assert.AreEqual(fakeBlog1.ObjectOwnerId, blogDto.ObjectOwnerId);
        }

        [TestMethod]
        public void MappingFromBlogDtoToBlog()
        {
            // Action
            var blog = mapper.Map<Common.Model.Entity.Blog>(fakeBlogDto1);

            // Assert
            Assert.AreEqual(fakeBlogDto1.BlogID, blog.BlogID);
            Assert.AreEqual(fakeBlogDto1.BlogDetails, blog.BlogDetails);
            Assert.AreEqual(fakeBlogDto1.BlogStatus, blog.BlogStatus);
            Assert.AreEqual(fakeBlogDto1.BlogTitle, blog.BlogTitle);
            Assert.AreEqual(fakeBlogDto1.BlogOwnerID, blog.BlogOwnerID);
            Assert.AreEqual(fakeBlogDto1.ObjectOwnerId, blog.ObjectOwnerId);
        }
    }
}
