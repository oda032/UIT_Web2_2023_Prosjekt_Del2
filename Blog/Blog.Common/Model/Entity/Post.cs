using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Identity;

namespace Blog.Common.Model.Entity
{
    public class Post
    {
        [Key]
        public int PostID { get; set; }
        [MinLength(5), MaxLength(200)]
        [Required]
        public string PostTitle { get; set; }
        [MaxLength(1000)]
        public string? PostDetails { get; set; }
        public int BlogID { get; set; }
        [ForeignKey("BlogID")]
        public virtual Blog Blog { get; set; }
        public List<Comment>? Comments { get; set; }
        public string PostOwnerID { get; set; }
        [ForeignKey("PostOwnerID")]
        public virtual IdentityUser PostOwner { get; set; }
        public List<PostTag>? PostTags { get; set; }
        public string? ObjectOwnerId { get; set; }
    }
}
