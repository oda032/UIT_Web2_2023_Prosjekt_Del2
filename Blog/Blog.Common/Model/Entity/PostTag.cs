using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Blog.Common.Model.Entity
{
    public class PostTag
    {
        [Key]
        public int PostTagID { get; set; }
        public int PostID { get; set; }
        [ForeignKey("PostID")]
        [JsonIgnore]
        public virtual Post Post { get; set; }
        public int TagID { get; set; }
        [ForeignKey("TagID")]
        [JsonIgnore]
        public virtual Tag Tag { get; set; }
    }
}
