using AutoMapper;
using Blog.Api.Mapping;
using Blog.Common.Enum;
using Blog.Common.Interface.IRepository;
using Blog.Common.Model.Dto;
using Blog.Common.Model.Entity;
using Blog.Test.Helpers;
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
    public class CommentMappingTest
    {
        private IMapper mapper;
        private Common.Model.Entity.Blog fakeBlog1;
        private Post fakePost1;
        private Comment fakeComment1;
        private CommentDto fakeCommentDto;
        private List<Comment> fakeComments;
        private IdentityUser fakeIdentityUser;

        [TestInitialize]
        public void Initialize()
        {
            fakeIdentityUser = new IdentityUser
            {
                Id = "user@test.com",
                UserName = "user1@test.com",
                Email = "user1@test.com",
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

            fakeCommentDto = new CommentDto
            {
                CommentDetails = "details",
                CommentID = 1,
                CommentOwnerID = fakeIdentityUser.Id,
                CommentTitle = "title",
                ObjectOwnerId = fakeIdentityUser.Id,
                PostID = 1,
                BlogID = 1,
                CommentOwnerName = fakeIdentityUser.UserName
            };

            fakeComments = new List<Comment> { fakeComment1 };

            var configuration = new MapperConfiguration(cfg => { cfg.AddProfile<CommentMappingProfile>(); });
            mapper = configuration.CreateMapper();
        }

        [TestMethod]
        public void MappingFromCommentToCommentDto()
        {
            // Action
            var commentDto = mapper.Map<CommentDto>(fakeComment1);

            // Assert
            Assert.AreEqual(fakeComment1.CommentID, commentDto.CommentID);
            Assert.AreEqual(fakeComment1.CommentDetails, commentDto.CommentDetails);
            Assert.AreEqual(fakeComment1.CommentTitle, commentDto.CommentTitle);
            Assert.AreEqual(fakeComment1.CommentOwnerID, commentDto.CommentOwnerID);
            Assert.AreEqual(fakeComment1.ObjectOwnerId, commentDto.ObjectOwnerId);
            Assert.AreEqual(fakeComment1.BlogID, commentDto.BlogID);
            Assert.AreEqual(fakeComment1.PostID, commentDto.PostID);
            Assert.AreEqual(fakeComment1.CommentOwner.UserName, commentDto.CommentOwnerName);
        }

        [TestMethod]
        public void MappingFromCommentDtoToComment()
        {
            // Action
            var comment = mapper.Map<Comment>(fakeCommentDto);

            // Assert
            Assert.AreEqual(fakeCommentDto.CommentID, comment.CommentID);
            Assert.AreEqual(fakeCommentDto.CommentDetails, comment.CommentDetails);
            Assert.AreEqual(fakeCommentDto.CommentTitle, comment.CommentTitle);
            Assert.AreEqual(fakeCommentDto.CommentOwnerID, comment.CommentOwnerID);
            Assert.AreEqual(fakeCommentDto.ObjectOwnerId, comment.ObjectOwnerId);
            Assert.AreEqual(fakeCommentDto.BlogID, comment.BlogID);
            Assert.AreEqual(fakeCommentDto.PostID, comment.PostID);
        }
    }
}
