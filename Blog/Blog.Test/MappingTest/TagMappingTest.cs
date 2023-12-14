using AutoMapper;
using Blog.Api.Mapping;
using Blog.Common.Interface.IRepository;
using Blog.Common.Model.Dto;
using Blog.Common.Model.Entity;
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
    public class TagMappingTest
    {
        private IMapper mapper;
        private Tag fakeTag1;
        private TagDto fakeTagDto1;

        [TestInitialize]
        public void Initialize()
        {
            fakeTag1 = new Tag
            {
                TagID = 1,
                TagName = "tag1"
            };

            fakeTagDto1 = new TagDto
            {
                TagID = 1,
                TagName = "newTag1"
            };

            var configuration = new MapperConfiguration(cfg => { cfg.AddProfile<TagMappingProfile>(); });
            mapper = configuration.CreateMapper();
        }

        [TestMethod]
        public void MappingFromTagToTagDto()
        {
            // Action
            var tagDto = mapper.Map<TagDto>(fakeTag1);

            // Assert
            Assert.AreEqual(fakeTag1.TagID, tagDto.TagID);
            Assert.AreEqual(fakeTag1.TagName, tagDto.TagName);
        }

        [TestMethod]
        public void MappingFromTagDtoToTag()
        {
            // Action
            var tag = mapper.Map<Tag>(fakeTagDto1);

            // Assert
            Assert.AreEqual(fakeTagDto1.TagID, tag.TagID);
            Assert.AreEqual(fakeTagDto1.TagName, tag.TagName);
        }
    }
}
