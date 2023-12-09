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

        [TestInitialize]
        public void Initialize()
        {
            mockConfig = new Mock<IConfiguration>();
            mockUserManager = UserManagerHelper.MockUserManager<IdentityUser>();
            mockSignInManager = UserManagerHelper.MockSignInManager<IdentityUser>(mockUserManager);
            mockSignInManager.Setup(m => m.SignOutAsync()).Returns(Task.CompletedTask);
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
    }
}
