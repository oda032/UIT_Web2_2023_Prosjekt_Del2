using Blog.Common.Model.Entity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Common.Model.Dto;

namespace Blog.Common.Model.Dto
{
    public class CommentDto
    {
        public int? CommentID { get; set; }
        [MinLength(5), MaxLength(20)]
        [Required]
        public string CommentTitle { get; set; }
        [MaxLength(50)]
        public string? CommentDetails { get; set; }
        public string? CommentOwnerID { get; set; }
        public int PostID { get; set; }
        public int BlogID { get; set; }
        public string? ObjectOwnerId { get; set; }
        public string? CommentOwnerName { get; set; }
    }
}