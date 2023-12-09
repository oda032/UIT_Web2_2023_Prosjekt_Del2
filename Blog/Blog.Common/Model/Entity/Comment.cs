using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Blog.Common.Model.Entity
{
    public class Comment
    {
        [Key]
        public int CommentID { get; set; }
        [MinLength(5), MaxLength(20)]
        [Required]
        public string CommentTitle { get; set; }
        [MaxLength(50)]
        public string? CommentDetails { get; set; }
        public string CommentOwnerID { get; set; }
        [ForeignKey("CommentOwnerID")]
        public virtual IdentityUser CommentOwner { get; set; }
        public int PostID { get; set; }
        [ForeignKey("PostID")]
        public virtual Post Post { get; set; }
        public int BlogID { get; set; }
        public string? ObjectOwnerId { get; set; }

        
    }
}
