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
    public class PostMappingTest
    {
        private IMapper mapper;
        private Common.Model.Entity.Blog fakeBlog1;
        private Post fakePost1;
        private PostDto fakePostDto;
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
                Blog = fakeBlog1,
                PostTags = new List<PostTag>
                {
                    new PostTag
                    {
                        PostTagID = 1,
                        TagID = 1,
                        PostID = 1
                    }
                }
            };

            fakePostDto = new PostDto
            {
                BlogID = 1,
                PostDetails = "newDetails",
                PostTitle = "newTitle",
                PostOwnerName = "astra1@email.com",
                PostTags = new List<PostTagDto> { new PostTagDto
                {
                    TagID = 1,
                    PostID = 1,
                    PostTagID = 1
                } }
            };

            var configuration = new MapperConfiguration(cfg => { cfg.AddProfile<PostMappingProfile>(); });
            mapper = configuration.CreateMapper();
        }

        [TestMethod]
        public void MappingFromPostToPostDto()
        {
            // Action
            var postDto = mapper.Map<PostDto>(fakePost1);

            // Assert
            Assert.AreEqual(fakePost1.PostID, postDto.PostID);
            Assert.AreEqual(fakePost1.PostDetails, postDto.PostDetails);
            Assert.AreEqual(fakePost1.PostTitle, postDto.PostTitle);
            Assert.AreEqual(fakePost1.PostOwnerID, postDto.PostOwnerID);
            Assert.AreEqual(fakePost1.ObjectOwnerId, postDto.ObjectOwnerId);
            Assert.AreEqual(fakePost1.BlogID, postDto.BlogID);
            Assert.AreEqual(fakePost1.PostID, postDto.PostID);
            Assert.AreEqual(fakePost1.PostTags[0].PostID, postDto.PostTags[0].PostID);
            Assert.AreEqual(fakePost1.PostTags[0].TagID, postDto.PostTags[0].TagID);
            Assert.AreEqual(fakePost1.PostTags[0].PostTagID, postDto.PostTags[0].PostTagID);
        }
    }
}
