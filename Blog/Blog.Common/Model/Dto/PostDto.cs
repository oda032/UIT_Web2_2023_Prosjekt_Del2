using Blog.Common.Model.Entity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Blog.Common.Model.Dto
{
    public class PostDto
    {
        public int? PostID { get; set; }
        [MinLength(5), MaxLength(200)]
        [Required]
        public string PostTitle { get; set; }
        [MaxLength(1000)]
        public string? PostDetails { get; set; }
        public int BlogID { get; set; }
        public string? PostOwnerID { get; set; }
        public List<PostTagDto>? PostTags { get; set; }
        public string? ObjectOwnerId { get; set; }
        public string? PostOwnerName { get; set; }
    }
}
