using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Common.Model.Dto
{
    public class UserDto
    {
        public string? Id { get; set; }
        public string Email { get; set; }
        public string? Passwd { get; set; }
        public string? Token { get; set; }
    }
}
