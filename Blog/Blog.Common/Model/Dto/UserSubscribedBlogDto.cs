using Blog.Common.Model.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Common.Model.Dto
{
    public class UserSubscribedBlogDto
    {
        public int? UserSubscribedBlogID { get; set; }
        public string? ApplicationUserID { get; set; }
        public int? BlogID { get; set; }
    }
}
