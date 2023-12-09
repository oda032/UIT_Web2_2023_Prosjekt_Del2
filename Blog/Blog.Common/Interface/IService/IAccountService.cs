using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Common.Model.Dto;

namespace Blog.Common.Interface.IService
{
    public interface IAccountService
    {
        Task<string> Login(UserDto userDto);
        Task<UserDto> Register(UserDto userDto);
        Task Logout();
        Task<IEnumerable<UserDto>> GetUsers();
    }
}
