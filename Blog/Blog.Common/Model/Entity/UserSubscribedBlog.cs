using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Common.Model.Entity
{
    public class UserSubscribedBlog
    {
        [Key]
        public int UserSubscribedBlogID { get; set; }
        public string ApplicationUserID { get; set; }
        [ForeignKey("ApplicationUserID")]
        public virtual ApplicationUser ApplicationUser { get; set; }
        public int BlogID { get; set; }
        [ForeignKey("BlogID")]
        public virtual Blog Blog { get; set; }
    }
}
