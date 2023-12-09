using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Common.Model.Dto;

namespace Blog.Common.Interface.IRepository
{
    public interface IAccountRepository
    {
        Task<UserDto> VerifyCredentials(UserDto user);

        string GenerateJwtToken(UserDto user);

        Task LogoutUser();

        Task<UserDto> CreateUser(UserDto user);
    }
}
