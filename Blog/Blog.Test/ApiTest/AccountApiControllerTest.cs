using AutoMapper;
using Blog.Api.Controllers;
using Blog.Common.Enum;
using Blog.Common.Interface.IRepository;
using Blog.Common.Model.Dto;
using Blog.Common.Model.Entity;
using Blog.Test.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Blog.Test.ApiTest
{
    [TestClass]
    public class AccountApiControllerTest
    {
        private Mock<IAccountRepository> mockAccountRepository;
        private UserDto fakeUserDto1;
        private UserDto fakeUserDto2;
        private List<UserDto> fakeUserDtos;
        private IdentityUser fakeIdentityUser1;
        private IdentityUser fakeIdentityUser2;
        private List<IdentityUser> fakeIdentityUsers;
        private Mock<UserManager<IdentityUser>> mockUserManager;

        [TestInitialize]
        public void Initialize()
        {
            fakeUserDto1 = new UserDto
            {
                Id = "abc",
                Email = "user1@test.com",
                Passwd = "aaBB11@!"
            };

            fakeUserDto2 = new UserDto
            {
                Id = "qrs",
                Email = "user2@test.com",
                Passwd = "ccGG@!!216"
            };

            fakeIdentityUser1 = new IdentityUser
            {
                Id = fakeUserDto1.Id,
                UserName = fakeUserDto1.Email,
                NormalizedUserName = fakeUserDto1.Email.ToUpper(),
                Email = fakeUserDto1.Email,
                NormalizedEmail = fakeUserDto1.Email.ToUpper(),
                LockoutEnabled = false,
                EmailConfirmed = true,
            };

            fakeIdentityUser2 = new IdentityUser
            {
                Id = fakeUserDto2.Id,
                UserName = fakeUserDto2.Email,
                NormalizedUserName = fakeUserDto2.Email.ToUpper(),
                Email = fakeUserDto2.Email,
                NormalizedEmail = fakeUserDto2.Email.ToUpper(),
                LockoutEnabled = false,
                EmailConfirmed = true,
            };

            fakeUserDtos = new List<UserDto> { fakeUserDto1, fakeUserDto2 };
            fakeIdentityUsers = new List<IdentityUser> { fakeIdentityUser1, fakeIdentityUser2 };

            mockAccountRepository = new Mock<IAccountRepository>();
            mockAccountRepository.Setup(ac => ac.GenerateJwtToken(It.IsAny<UserDto>())).Returns("token");

            mockUserManager = UserManagerHelper.MockUserManager<IdentityUser>();
        }

        [TestMethod]
        public async Task VerifyLoginReturnsBadRequest()
        {
            // Arrange
            var controller = new AccountController(mockUserManager.Object, mockAccountRepository.Object);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ModelState.AddModelError("Error", "Invalid...");

            // Act
            var result = await controller.VerifyLogin(fakeUserDto1) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
        }

        [TestMethod]
        public async Task VerifyLoginReturnsNullWhenUserIsNotVerified()
        {
            // Arrange
            var controller = new AccountController(mockUserManager.Object, mockAccountRepository.Object);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            mockAccountRepository.Setup(ac => ac.VerifyCredentials(It.IsAny<UserDto>())).ReturnsAsync((UserDto)null);

            // Act
            var result = await controller.VerifyLogin(fakeUserDto1) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }

        [TestMethod]
        public async Task VerifyLoginReturnsObjectResultWhenUserIsVerified()
        {
            // Arrange
            var controller = new AccountController(mockUserManager.Object, mockAccountRepository.Object);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            mockAccountRepository.Setup(ac => ac.VerifyCredentials(It.IsAny<UserDto>())).ReturnsAsync(fakeUserDto1);

            // Act
            var result = await controller.VerifyLogin(fakeUserDto1) as ObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ObjectResult));
            Assert.AreEqual(result.Value, "token");
        }

        [TestMethod]
        public async Task LogoutReturnsOkResult()
        {
            // Arrange
            var controller = new AccountController(mockUserManager.Object, mockAccountRepository.Object);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            mockAccountRepository.Setup(ac => ac.LogoutUser()).Returns(Task.CompletedTask);

            // Act
            var result = await controller.Logout() as OkResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkResult));
            Assert.AreEqual(200, result.StatusCode);
        }

        [TestMethod]
        public async Task RegisterReturnsOkObjectResult()
        {
            // Arrange
            var controller = new AccountController(mockUserManager.Object, mockAccountRepository.Object);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            mockAccountRepository.Setup(ac => ac.CreateUser(It.IsAny<UserDto>())).ReturnsAsync(fakeUserDto1);

            // Act
            var result = await controller.Register(fakeUserDto1) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(fakeUserDto1, result.Value);
        }

    }
}
