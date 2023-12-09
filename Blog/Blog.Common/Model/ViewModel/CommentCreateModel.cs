using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Common.Model.ViewModel
{
    public class CommentCreateModel
    {
        [MinLength(5), MaxLength(20)]
        [Required]
        public string CommentTitle { get; set; }
        [MaxLength(50)]
        public string? CommentDetails { get; set; }
    }
}
