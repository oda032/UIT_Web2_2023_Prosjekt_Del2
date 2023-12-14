using AutoMapper;
using Blog.Api.Mapping;
using Blog.Common.Enum;
using Blog.Common.Interface.IRepository;
using Blog.Common.Model.Dto;
using Blog.Common.Model.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Test.MappingTest
{
    [TestClass]
    public class UserSubscribedBlogMappingTest
    {
        private IMapper mapper;
        private ApplicationUser fakeAppUser2;
        private Common.Model.Entity.Blog fakeBlog1;
        private UserSubscribedBlog fakeUserSubscribedBlog1;
        private UserSubscribedBlogDto fakeUserSubscribedBlogDto1;
        private List<UserSubscribedBlog> fakeUserSubscribedBlogs;
        private List<UserSubscribedBlogDto> fakeUserSubscribedBlogDtos;
        private IdentityUser fakeIdentityUser1;
        private IdentityUser fakeIdentityUser2;

        [TestInitialize]
        public void Initialize()
        {
            fakeIdentityUser1 = new IdentityUser
            {
                Id = "user@test.com",
                UserName = "user@test.com",
                Email = "user@test.com",
            };

            fakeIdentityUser2 = new IdentityUser
            {
                Id = "user2@test.com",
                UserName = "user2@test.com",
                Email = "user2@test.com",
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

            fakeAppUser2 = new ApplicationUser
            {
                Id = fakeIdentityUser2.Id,
                Email = fakeIdentityUser2.Email,
                UserName = fakeIdentityUser2.UserName
            };

            fakeUserSubscribedBlog1 = new UserSubscribedBlog
            {
                ApplicationUser = fakeAppUser2,
                ApplicationUserID = fakeAppUser2.Id,
                BlogID = 1,
                Blog = fakeBlog1,
                UserSubscribedBlogID = 1
            };

            fakeUserSubscribedBlogDto1 = new UserSubscribedBlogDto
            {
                ApplicationUserID = fakeAppUser2.Id,
                BlogID = 1,
                UserSubscribedBlogID = 1
            };

            var configuration = new MapperConfiguration(cfg => { cfg.AddProfile<UserSubscribedBlogMappingProfile>(); });
            mapper = configuration.CreateMapper();
        }

        [TestMethod]
        public void MappingFromUserSubscribedBlogToDto()
        {
            // Action
            var usbDto = mapper.Map<UserSubscribedBlogDto>(fakeUserSubscribedBlog1);

            // Assert
            Assert.AreEqual(fakeUserSubscribedBlog1.ApplicationUserID, usbDto.ApplicationUserID);
            Assert.AreEqual(fakeUserSubscribedBlog1.BlogID, usbDto.BlogID);
            Assert.AreEqual(fakeUserSubscribedBlog1.UserSubscribedBlogID, usbDto.UserSubscribedBlogID);
        }

        [TestMethod]
        public void MappingFromDtoToUserSubscribedBlog()
        {
            // Action
            var usb = mapper.Map<UserSubscribedBlog>(fakeUserSubscribedBlogDto1);

            // Assert
            Assert.AreEqual(fakeUserSubscribedBlogDto1.ApplicationUserID, usb.ApplicationUserID);
            Assert.AreEqual(fakeUserSubscribedBlogDto1.BlogID, usb.BlogID);
            Assert.AreEqual(fakeUserSubscribedBlogDto1.UserSubscribedBlogID, usb.UserSubscribedBlogID);
        }
    }
}
