using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Blog.Common.Model.Entity
{
    public class Tag
    {
        [Key]
        public int TagID { get; set; }
        [MaxLength(20)]
        [Required]
        public string TagName { get; set; }
        public List<PostTag>? PostTags { get; set; }
    }
}
