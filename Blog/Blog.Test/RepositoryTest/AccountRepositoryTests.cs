using Blog.DataAccess.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Blog.Common.Model.Entity;
using Blog.DataAccess.Repository;
using Blog.Test.Helpers;
using System.Collections;
using Blog.Common.Model.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Blog.Test.RepositoryTest
{
    [TestClass]
    public class AccountRepositoryTests
    {
        private AccountRepository accountRepository;
        private Mock<SignInManager<IdentityUser>> mockSignInManager;
        private Mock<UserManager<IdentityUser>> mockUserManager;
        private Mock<IConfiguration> mockConfig;
        private UserDto fakeUserDto;

        [TestInitialize]
        public void Initialize()
        {
            mockConfig = new Mock<IConfiguration>();
            mockUserManager = UserManagerHelper.MockUserManager<IdentityUser>();
            mockSignInManager = UserManagerHelper.MockSignInManager<IdentityUser>(mockUserManager);
            mockSignInManager.Setup(m => m.SignOutAsync()).Returns(Task.CompletedTask);

            fakeUserDto = new UserDto
            {
                Email = "user@test.com",
                Id = "abc",
                Passwd = "abc!@123ABC"
            };
        }

        [TestMethod]
        public async Task CanLogout()
        {
            // Arrange
            var testContext = InMemoryHelper.GetTestDbContext(nameof(CanLogout));
            await testContext.SaveChangesAsync();

            accountRepository = new AccountRepository(mockSignInManager.Object, mockUserManager.Object, testContext, mockConfig.Object );

            // Action
            await accountRepository.LogoutUser();


            // Assert
            mockSignInManager.Verify(m => m.SignOutAsync(), Times.Once);
        }

        [TestMethod]
        public async Task VerifyCredentialsReturnNullWhenEmailIsNull()
        {
            // Arrange
            var testContext = InMemoryHelper.GetTestDbContext(nameof(VerifyCredentialsReturnNullWhenEmailIsNull));
            await testContext.SaveChangesAsync();

            fakeUserDto.Email = null;

            accountRepository = new AccountRepository(mockSignInManager.Object, mockUserManager.Object, testContext, mockConfig.Object);

            // Action
            var result = await accountRepository.VerifyCredentials(fakeUserDto);


            // Assert
            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public async Task VerifyCredentialsReturnNullWhenPasswordIsNull()
        {
            // Arrange
            var testContext = InMemoryHelper.GetTestDbContext(nameof(VerifyCredentialsReturnNullWhenPasswordIsNull));
            await testContext.SaveChangesAsync();

            fakeUserDto.Passwd = null;

            accountRepository = new AccountRepository(mockSignInManager.Object, mockUserManager.Object, testContext, mockConfig.Object);

            // Action
            var result = await accountRepository.VerifyCredentials(fakeUserDto);


            // Assert
            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public async Task VerifyCredentialsReturnNullWhenUserIsNotFound()
        {
            // Arrange
            var testContext = InMemoryHelper.GetTestDbContext(nameof(VerifyCredentialsReturnNullWhenUserIsNotFound));
            await testContext.SaveChangesAsync();
            mockUserManager.Setup(mgr => mgr.FindByNameAsync(It.IsAny<string>())).ReturnsAsync((IdentityUser)null);

            accountRepository = new AccountRepository(mockSignInManager.Object, mockUserManager.Object, testContext, mockConfig.Object);

            // Action
            var result = await accountRepository.VerifyCredentials(fakeUserDto);


            // Assert
            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public async Task VerifyCredentialsReturnNullWhenUserNotSignedIn()
        {
            // Arrange
            var testContext = InMemoryHelper.GetTestDbContext(nameof(VerifyCredentialsReturnNullWhenUserNotSignedIn));
            await testContext.SaveChangesAsync();
            mockUserManager.Setup(m => m.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(new IdentityUser());
            mockSignInManager
                .Setup(sm =>
                    sm.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(SignInResult.Failed);

            accountRepository = new AccountRepository(mockSignInManager.Object, mockUserManager.Object, testContext, mockConfig.Object);

            // Action
            var result = await accountRepository.VerifyCredentials(fakeUserDto);


            // Assert
            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public async Task VerifyCredentialsReturnNewUserDto()
        {
            // Arrange
            var testContext = InMemoryHelper.GetTestDbContext(nameof(VerifyCredentialsReturnNewUserDto));
            await testContext.SaveChangesAsync();
            mockUserManager.Setup(m => m.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(new IdentityUser
            {
                Id = fakeUserDto.Id,
                UserName = fakeUserDto.Email
            });
            mockSignInManager
                .Setup(sm =>
                    sm.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(SignInResult.Success);

            accountRepository = new AccountRepository(mockSignInManager.Object, mockUserManager.Object, testContext, mockConfig.Object);

            // Action
            var result = await accountRepository.VerifyCredentials(fakeUserDto);


            // Assert
            Assert.AreEqual(fakeUserDto.Id, result.Id);
            Assert.AreEqual(fakeUserDto.Email, result.Email);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "User is already registered")]
        public async Task CreateUserReturnExceptionWhenUserIsFound()
        {
            // Arrange
            var testContext = InMemoryHelper.GetTestDbContext(nameof(CreateUserReturnExceptionWhenUserIsFound));
            await testContext.SaveChangesAsync();
            mockUserManager.Setup(m => m.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(new IdentityUser
            {
                Id = fakeUserDto.Id,
                UserName = fakeUserDto.Email,
                Email = fakeUserDto.Email
            });

            accountRepository = new AccountRepository(mockSignInManager.Object, mockUserManager.Object, testContext, mockConfig.Object);

            // Action
            var result = await accountRepository.CreateUser(fakeUserDto);
        }

        [TestMethod]
        public async Task CreateUserReturnNullWhenIdentityResultIsFailed()
        {
            // Arrange
            var testContext = InMemoryHelper.GetTestDbContext(nameof(CreateUserReturnNullWhenIdentityResultIsFailed));
            await testContext.SaveChangesAsync();

            mockUserManager.Setup(mgr => mgr.FindByNameAsync(It.IsAny<string>())).ReturnsAsync((IdentityUser?)null);
            mockUserManager.Setup(m => m.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed());

            accountRepository = new AccountRepository(mockSignInManager.Object, mockUserManager.Object, testContext, mockConfig.Object);

            // Action
            var result = await accountRepository.CreateUser(fakeUserDto);

            // Assert
            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public async Task CreateUserReturnUserDtoWhenIdentityResultIsSuccess()
        {
            // Arrange
            var testContext = InMemoryHelper.GetTestDbContext(nameof(CreateUserReturnUserDtoWhenIdentityResultIsSuccess));
            await testContext.SaveChangesAsync();

            mockUserManager.SetupSequence(m => m.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync((IdentityUser?)null)
                .ReturnsAsync(new IdentityUser
                {
                    Id = fakeUserDto.Id,
                    Email = fakeUserDto.Email,
                    UserName = fakeUserDto.Email
                });
            mockUserManager.Setup(m => m.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            accountRepository = new AccountRepository(mockSignInManager.Object, mockUserManager.Object, testContext, mockConfig.Object);

            // Action
            var result = await accountRepository.CreateUser(fakeUserDto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(UserDto));
            Assert.AreEqual(result.Id, fakeUserDto.Id);
        }
    }
}
