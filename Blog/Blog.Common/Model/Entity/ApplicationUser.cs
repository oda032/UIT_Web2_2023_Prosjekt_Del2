using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Common.Model.Entity
{
    public class ApplicationUser : IdentityUser
    {
        public List<UserSubscribedBlog>? UserSubscribedBlogs { get; set; }
    }
}
