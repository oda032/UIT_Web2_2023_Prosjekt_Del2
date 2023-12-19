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
    public class ImageRepositoryTest
    {
        private ImageRepository imageRepository;
        private Image fakeImage1;
        private Image fakeImage2;
        private Image fakeAddImage;
        private List<Image> fakeImages;

        [TestInitialize]
        public void Initialize()
        {
            fakeImage1 = new Image
            {
                ImageID = 1,
                Data = new byte[10],
                Format = "png",
                Name = "pict1",
                Url = "pict1.png"
            };

            fakeImage2 = new Image
            {
                ImageID = 2,
                Data = new byte[10],
                Format = "png",
                Name = "pict2",
                Url = "pict2.png"
            };

            fakeAddImage = new Image
            {
                Data = new byte[10],
                Format = "png",
                Name = "pict3",
                Url = "pict3.png"
            };

            fakeImages = new List<Image> { fakeImage1, fakeImage2 };
        }

        [TestMethod]
        public async Task CanGetAllImages()
        {
            // Arrange
            var testContext = InMemoryHelper.GetTestDbContext(nameof(CanGetAllImages));
            testContext.AddRange(fakeImages);
            await testContext.SaveChangesAsync();

            imageRepository = new ImageRepository(testContext);

            // Action
            var images = await imageRepository.GetImages();


            // Assert
            Assert.IsNotNull(images);
            CollectionAssert.AllItemsAreInstancesOfType((ICollection)images, typeof(Image));
            Assert.AreEqual(images.Count(), 2);
        }

        [TestMethod]
        public async Task CanGetOneImage()
        {
            // Arrange
            var testContext = InMemoryHelper.GetTestDbContext(nameof(CanGetOneImage));
            testContext.AddRange(fakeImages);
            await testContext.SaveChangesAsync();

            imageRepository = new ImageRepository(testContext);

            // Action
            var image = await imageRepository.GetOneImage(1);

            // Assert
            Assert.IsNotNull(image);
            Assert.IsInstanceOfType(image, typeof(Image));
            Assert.AreSame(image, fakeImage1);
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public async Task EditImageThrowExceptionBecauseNotImplemented()
        {
            try
            {
                // Arrange
                var testContext = InMemoryHelper.GetTestDbContext(nameof(EditImageThrowExceptionBecauseNotImplemented));
                testContext.AddRange(fakeImages);
                await testContext.SaveChangesAsync();

                imageRepository = new ImageRepository(testContext);

                // Action
                await imageRepository.Edit(fakeImage1);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public async Task DeleteImageThrowExceptionBecauseNotImplemented()
        {
            try
            {
                // Arrange
                var testContext = InMemoryHelper.GetTestDbContext(nameof(DeleteImageThrowExceptionBecauseNotImplemented));
                testContext.AddRange(fakeImages);
                await testContext.SaveChangesAsync();

                imageRepository = new ImageRepository(testContext);

                // Action
                await imageRepository.Delete(fakeImage1);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [TestMethod]
        public async Task CanAddOneImage()
        {
            // Arrange
            var testContext = InMemoryHelper.GetTestDbContext(nameof(CanAddOneImage));
            testContext.AddRange(fakeImages);
            await testContext.SaveChangesAsync();

            imageRepository = new ImageRepository(testContext);

            // Action
            var imageId = await imageRepository.Add(fakeAddImage);

            // Assert
            Assert.IsNotNull(imageId);
            Assert.AreEqual(imageId, 3);
        }
    }
}
