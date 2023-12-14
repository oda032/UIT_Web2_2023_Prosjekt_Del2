using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Blog.Test.Helpers
{
    public class UserManagerHelper
    {
        public static Mock<UserManager<TUser>> MockUserManager<TUser>() where TUser : class
        {
            var store = new Mock<Microsoft.AspNetCore.Identity.IUserStore<TUser>>();
            var mgr = new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);
            mgr.Object.UserValidators.Add(new UserValidator<TUser>());
            mgr.Object.PasswordValidators.Add(new PasswordValidator<TUser>());
            return mgr;
        }

        public static Mock<SignInManager<TUser>> MockSignInManager<TUser>(Mock<UserManager<TUser>> mgr) where TUser : class
        {
            var mockSignInManager = new Mock<SignInManager<TUser>>(mgr.Object,
                Mock.Of<IHttpContextAccessor>(),
                Mock.Of<IUserClaimsPrincipalFactory<TUser>>(),
                null, null, null, null);

            return mockSignInManager;
        }
    }
}
