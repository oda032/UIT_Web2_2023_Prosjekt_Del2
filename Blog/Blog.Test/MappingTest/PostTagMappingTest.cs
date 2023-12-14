using AutoMapper;
using Blog.Api.Mapping;
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
    public class PostTagMappingTest
    {
        private IMapper mapper;
        private Tag fakeTag1;
        private Post fakePost1;
        private PostTag fakePostTag1;
        private PostTagDto fakePostTagDto1;
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

            fakeTag1 = new Tag
            {
                TagID = 1,
                TagName = "tag1"
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

            fakePostTag1 = new PostTag
            {
                PostTagID = 1,
                PostID = 1,
                Post = fakePost1,
                TagID = 1,
                Tag = fakeTag1
            };

            fakePostTagDto1 = new PostTagDto
            {
                PostTagID = 1,
                PostID = 1,
                TagID = 1,
            };

            var configuration = new MapperConfiguration(cfg => { cfg.AddProfile<PostTagMappingProfile>(); });
            mapper = configuration.CreateMapper();
        }


        [TestMethod]
        public void MappingFromPostTagToPostTagDto()
        {
            // Action
            var postTagDto = mapper.Map<PostTagDto>(fakePostTag1);

            // Assert
            Assert.AreEqual(fakePostTag1.TagID, postTagDto.TagID);
            Assert.AreEqual(fakePostTag1.PostID, postTagDto.PostID);
            Assert.AreEqual(fakePostTag1.PostTagID, postTagDto.PostTagID);
        }

        [TestMethod]
        public void MappingFromPostTagDtoToPostTag()
        {
            // Action
            var postTag = mapper.Map<PostTag>(fakePostTagDto1);

            // Assert
            Assert.AreEqual(fakePostTagDto1.TagID, postTag.TagID);
            Assert.AreEqual(fakePostTagDto1.PostID, postTag.PostID);
            Assert.AreEqual(fakePostTagDto1.PostTagID, postTag.PostTagID);
        }
    }
}
