using Blog.Common.Model.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Common.Model.ViewModel
{
    public class PostCreateModel
    {
        [MinLength(5), MaxLength(200)]
        [Required]
        public string PostTitle { get; set; }
        [MaxLength(1000)]
        public string? PostDetails { get; set; }
        public string? Tags { get; set; }
    }
}
