using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Blog.Common.Enum;
using Microsoft.AspNetCore.Identity;

namespace Blog.Common.Model.Entity
{
    public class Blog
    {
        [Key]
        public int BlogID { get; set; }
        [MinLength(5), MaxLength(20)]
        [Required]
        public string BlogTitle { get; set; }
        [MaxLength(50)]
        public string? BlogDetails { get; set; }
        public List<Post>? Posts { get; set; }
        public List<UserSubscribedBlog>? UserSubscribedBlogs { get; set; }
        public string BlogOwnerID { get; set; }
        [ForeignKey("BlogOwnerID")]
        public virtual IdentityUser BlogOwner { get; set; }
        [Required]
        public BlogStatus BlogStatus { get; set; }
        public string? ObjectOwnerId { get; set; }
    }
}
