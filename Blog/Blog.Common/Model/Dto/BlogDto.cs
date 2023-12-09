using Blog.Common.Enum;
using System.ComponentModel.DataAnnotations;

namespace Blog.Common.Model.Dto
{
    public class BlogDto
    {
        public int BlogID { get; set; }
        [MinLength(5), MaxLength(20)]
        [Required]
        public string BlogTitle { get; set; }
        [MaxLength(50)]
        public string? BlogDetails { get; set; }
        public string BlogOwnerID { get; set; }
        [Required]
        public BlogStatus BlogStatus { get; set; }
        public string? ObjectOwnerId { get; set; }
        public string? BlogOwnerName { get; set; }
    }
}
